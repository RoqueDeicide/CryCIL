using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using CryEngine;
using CryEngine.Utilities;

namespace CryEngine.CharacterCustomization
{
	public class CharacterAttachmentMaterial
	{
		internal CharacterAttachmentMaterial(CharacterAttachment attachment, XElement element, CharacterAttachmentMaterial parentMaterial = null)
		{
			Element = element;
			Attachment = attachment;

			ParentMaterial = parentMaterial;

			var subMaterialElements = element.Elements("Submaterial");
			Submaterials = new CharacterAttachmentMaterial[subMaterialElements.Count()];

			for(var i = 0; i < subMaterialElements.Count(); i++)
			{
				var subMaterialElement = subMaterialElements.ElementAt(i);

				Submaterials[i] = new CharacterAttachmentMaterial(attachment, subMaterialElement, this);
			}

			if (parentMaterial == null)
			{
				var pathAttribute = element.Attribute("path");
				if (pathAttribute != null)
					BaseFilePath = pathAttribute.Value;
			}
			else
			{
				var nameAttribute = element.Attribute("name");
				if (nameAttribute != null)
					BaseFilePath = nameAttribute.Value;
			}

			var colorModifierElement = element.Element("ColorModifier");
			if (colorModifierElement != null)
			{
				var redAttribute = colorModifierElement.Attribute("red");
				if (redAttribute != null)
					ColorRed = ParseColor(redAttribute.Value);

				var greenAttribute = colorModifierElement.Attribute("green");
				if (greenAttribute != null)
					ColorGreen = ParseColor(greenAttribute.Value);

				var blueAttribute = colorModifierElement.Attribute("blue");
				if (blueAttribute != null)
					ColorBlue = ParseColor(blueAttribute.Value);

				var alphaAttribute = colorModifierElement.Attribute("alpha");
				if (alphaAttribute != null)
					ColorAlpha = ParseColor(alphaAttribute.Value);
			}

			var diffuseElement = element.Element("Diffuse");
			DiffuseColor = UnusedMarker.Vec3;

			if (diffuseElement != null)
			{
				var texPathAttribute = diffuseElement.Attribute("path");
				if (texPathAttribute != null)
					DiffuseTexture = texPathAttribute.Value;

				var colorAttribute = diffuseElement.Attribute("color");
				if (colorAttribute != null)
					DiffuseColor = ParseColor(colorAttribute.Value);
			}

			var specularElement = element.Element("Specular");
			SpecularColor = UnusedMarker.Vec3;

			if (specularElement != null)
			{
				var texPathAttribute = specularElement.Attribute("path");
				if (texPathAttribute != null)
					SpecularTexture = texPathAttribute.Value;

				var colorAttribute = specularElement.Attribute("color");
				if (colorAttribute != null)
					SpecularColor = ParseColor(colorAttribute.Value);
			}

			var bumpmapElement = element.Element("Bumpmap");
			if (bumpmapElement != null)
			{
				var texPathAttribute = bumpmapElement.Attribute("path");
				if (texPathAttribute != null)
					BumpmapTexture = texPathAttribute.Value;
			}

			var customTexElement = element.Element("Custom");
			if (customTexElement != null)
			{
				var texPathAttribute = customTexElement.Attribute("path");
				if (texPathAttribute != null)
					CustomTexture = texPathAttribute.Value;
			}
		}

		Vec3 ParseColor(string colorString)
		{
			Vec3 color = Vec3.Parse(colorString);

			return new Vec3((float)Math.Pow(color.X / 255, 2.2), (float)Math.Pow(color.Y / 255, 2.2), (float)Math.Pow(color.Z / 255, 2.2));
		}

		bool UpdateMaterialElement(XElement materialElement, CharacterAttachmentMaterial material)
		{
            Debug.LogAlways("Updating material element for attachment {0} in slot {1}", material.Attachment.Name, material.Attachment.Slot.Name);
			var modifiedMaterial = false;

			var genMaskAttribute = materialElement.Attribute("StringGenMask");
			if (genMaskAttribute != null && genMaskAttribute.Value.Contains("%COLORMASKING"))
			{
                Debug.LogAlways("Writing color mask");
				var publicParamsElement = materialElement.Element("PublicParams");

				publicParamsElement.SetAttributeValue("ColorMaskR", material.ColorRed.ToString());
				publicParamsElement.SetAttributeValue("ColorMaskG", material.ColorGreen.ToString());
				publicParamsElement.SetAttributeValue("ColorMaskB", material.ColorBlue.ToString());
				publicParamsElement.SetAttributeValue("ColorMaskA", material.ColorAlpha.ToString());

				modifiedMaterial = true;
			}

			var texturesElement = materialElement.Element("Textures");
			if (texturesElement != null)
			{
				if (WriteTexture(texturesElement, "Diffuse", material.DiffuseTexture)
					|| WriteTexture(texturesElement, "Specular", material.SpecularTexture)
					|| WriteTexture(texturesElement, "Bumpmap", material.BumpmapTexture)
					|| WriteTexture(texturesElement, "Custom", material.CustomTexture))
				{
					modifiedMaterial = true;
				}
			}

			if (!UnusedMarker.IsUnused(DiffuseColor))
				materialElement.SetAttributeValue("Diffuse", DiffuseColor.ToString());

			if (!UnusedMarker.IsUnused(SpecularColor))
				materialElement.SetAttributeValue("Specular", SpecularColor.ToString());

			return modifiedMaterial;
		}

		public void Save()
		{
			var basePath = Path.Combine(CryPak.GameFolder, BaseFilePath);
			if (!File.Exists(basePath + ".mtl"))
				throw new CustomizationConfigurationException(string.Format("Could not save modified material, base {0} did not exist.", basePath));

			var materialDocument = XDocument.Load(basePath + ".mtl");

			var materialElement = materialDocument.Element("Material");

			// Store boolean to determine whether we need to save to an alternate location.
			bool modifiedMaterial = false;

			if (Submaterials.Length == 0)
				modifiedMaterial = UpdateMaterialElement(materialElement, this);
			else
			{
				var subMaterialsElement = materialElement.Element("SubMaterials");

				var subMaterialElements = subMaterialsElement.Elements("Material");

				for (var i = 0; i < Submaterials.Length; i++)
				{
					var subMaterial = Submaterials.ElementAt(i);

					var modifiedSubMaterial = UpdateMaterialElement(subMaterialElements.FirstOrDefault(x => 
						{
							var nameAttribute = x.Attribute("Name");

							if(nameAttribute != null)
								return nameAttribute.Value == subMaterial.BaseFilePath;

							return false;
						}), subMaterial);

					if (modifiedSubMaterial)
						modifiedMaterial = modifiedSubMaterial;
				}
			}

			if (modifiedMaterial)
			{
				if (string.IsNullOrEmpty(FilePath))
				{
					var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
					var stringChars = new char[16];

					for (int i = 0; i < stringChars.Length; i++)
						stringChars[i] = chars[CustomizationManager.Selector.Next(chars.Length)];

					var fileName = new string(stringChars);

                    FilePath = Path.Combine(Attachment.Slot.Manager.InitParameters.TempDirectory, "Materials", Attachment.Slot.Name, Attachment.Name ?? "unknown", fileName);
				}

				var fullFilePath = CryPak.AdjustFileName(FilePath, PathResolutionRules.RealPath | PathResolutionRules.ForWriting) + ".mtl";

				if (!File.Exists(fullFilePath))
				{
					var directory = new DirectoryInfo(Path.GetDirectoryName(fullFilePath));
					while (!directory.Exists)
					{
						Directory.CreateDirectory(directory.FullName);

						directory = Directory.GetParent(directory.FullName);
					}

					var file = File.Create(fullFilePath);
					file.Close();
				}

				materialDocument.Save(fullFilePath);
			}
			else
				FilePath = BaseFilePath;
		}

		bool WriteTexture(XElement texturesElement, string textureType, string texturePath)
		{
			if (texturePath == null)
				return false;

			var element = texturesElement.Elements("Texture").FirstOrDefault(x => x.Attribute("Map").Value == textureType);
			if (element == null)
			{
				element = new XElement("Texture");

				element.SetAttributeValue("Map", textureType);
				element.SetAttributeValue("File", texturePath);

				texturesElement.SetElementValue("Texture", element);
			}
			else
				element.SetAttributeValue("File", texturePath);

			return true;
		}

		public Vec3 ColorRed { get; set; }
		public Vec3 ColorGreen { get; set; }
		public Vec3 ColorBlue { get; set; }
		public Vec3 ColorAlpha { get; set; }

		public string DiffuseTexture { get; set; }
		public string SpecularTexture { get; set; }
		public string BumpmapTexture { get; set; }
		public string CustomTexture { get; set; }

		public Vec3 DiffuseColor { get; set; }
		public Vec3 SpecularColor { get; set; }

		/// <summary>
		/// Path to the mtl file.
		/// </summary>
		public string FilePath { get; set; }
		/// <summary>
		/// Path to the mtl file this material is based on.
		/// </summary>
		public string BaseFilePath { get; set; }

		public CharacterAttachmentMaterial[] Submaterials { get; set; }
		public CharacterAttachmentMaterial ParentMaterial { get; set; }

		public CharacterAttachment Attachment { get; set; }

		public XElement Element { get; set; }
	}
}
