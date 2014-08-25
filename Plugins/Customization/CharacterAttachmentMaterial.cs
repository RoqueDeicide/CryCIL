using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using CryEngine;
using CryEngine.Mathematics;
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

			List<XElement> subMaterialElements = element.Elements("Submaterial").ToList();
			Submaterials = new CharacterAttachmentMaterial[subMaterialElements.Count];

			for (int i = 0; i < subMaterialElements.Count(); i++)
			{
				XElement subMaterialElement = subMaterialElements[i];

				Submaterials[i] = new CharacterAttachmentMaterial(attachment, subMaterialElement, this);
			}

			if (parentMaterial == null)
			{
				XAttribute pathAttribute = element.Attribute("path");
				if (pathAttribute != null)
					BaseFilePath = pathAttribute.Value;
			}
			else
			{
				XAttribute nameAttribute = element.Attribute("name");
				if (nameAttribute != null)
					BaseFilePath = nameAttribute.Value;
			}

			XElement colorModifierElement = element.Element("ColorModifier");
			if (colorModifierElement != null)
			{
				XAttribute redAttribute = colorModifierElement.Attribute("red");
				if (redAttribute != null)
					ColorRed = ParseColor(redAttribute.Value);

				XAttribute greenAttribute = colorModifierElement.Attribute("green");
				if (greenAttribute != null)
					ColorGreen = ParseColor(greenAttribute.Value);

				XAttribute blueAttribute = colorModifierElement.Attribute("blue");
				if (blueAttribute != null)
					ColorBlue = ParseColor(blueAttribute.Value);

				XAttribute alphaAttribute = colorModifierElement.Attribute("alpha");
				if (alphaAttribute != null)
					ColorAlpha = ParseColor(alphaAttribute.Value);
			}

			XElement diffuseElement = element.Element("Diffuse");
			DiffuseColor = UnusedMarker.Vector3;

			if (diffuseElement != null)
			{
				XAttribute texPathAttribute = diffuseElement.Attribute("path");
				if (texPathAttribute != null)
					DiffuseTexture = texPathAttribute.Value;

				XAttribute colorAttribute = diffuseElement.Attribute("color");
				if (colorAttribute != null)
					DiffuseColor = ParseColor(colorAttribute.Value);
			}

			XElement specularElement = element.Element("Specular");
			SpecularColor = UnusedMarker.Vector3;

			if (specularElement != null)
			{
				XAttribute texPathAttribute = specularElement.Attribute("path");
				if (texPathAttribute != null)
					SpecularTexture = texPathAttribute.Value;

				XAttribute colorAttribute = specularElement.Attribute("color");
				if (colorAttribute != null)
					SpecularColor = ParseColor(colorAttribute.Value);
			}

			XElement bumpmapElement = element.Element("Bumpmap");
			if (bumpmapElement != null)
			{
				XAttribute texPathAttribute = bumpmapElement.Attribute("path");
				if (texPathAttribute != null)
					BumpmapTexture = texPathAttribute.Value;
			}

			XElement customTexElement = element.Element("Custom");
			if (customTexElement != null)
			{
				XAttribute texPathAttribute = customTexElement.Attribute("path");
				if (texPathAttribute != null)
					CustomTexture = texPathAttribute.Value;
			}
		}

		private Vector3 ParseColor(string colorString)
		{
			Vector3 color = Vector3.Parse(colorString);

			return new Vector3((float)Math.Pow(color.X / 255, 2.2), (float)Math.Pow(color.Y / 255, 2.2), (float)Math.Pow(color.Z / 255, 2.2));
		}

		private bool UpdateMaterialElement(XElement materialElement, CharacterAttachmentMaterial material)
		{
			Debug.LogAlways("Updating material element for attachment {0} in slot {1}", material.Attachment.Name, material.Attachment.Slot.Name);
			bool modifiedMaterial = false;

			XAttribute genMaskAttribute = materialElement.Attribute("StringGenMask");
			if (genMaskAttribute != null && genMaskAttribute.Value.Contains("%COLORMASKING"))
			{
				Debug.LogAlways("Writing color mask");
				XElement publicParamsElement = materialElement.Element("PublicParams");

				publicParamsElement.SetAttributeValue("ColorMaskR", material.ColorRed.ToString());
				publicParamsElement.SetAttributeValue("ColorMaskG", material.ColorGreen.ToString());
				publicParamsElement.SetAttributeValue("ColorMaskB", material.ColorBlue.ToString());
				publicParamsElement.SetAttributeValue("ColorMaskA", material.ColorAlpha.ToString());

				modifiedMaterial = true;
			}

			XElement texturesElement = materialElement.Element("Textures");
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
			string basePath = Path.Combine(CryPak.GameFolder, BaseFilePath);
			if (!File.Exists(basePath + ".mtl"))
				throw new CustomizationConfigurationException(string.Format("Could not save modified material, base {0} did not exist.", basePath));

			XDocument materialDocument = XDocument.Load(basePath + ".mtl");

			XElement materialElement = materialDocument.Element("Material");
			if (materialElement == null)
			{
				throw new CustomizationConfigurationException("Could not save modified material, base file is not properly configured.");
			}
			// Store boolean to determine whether we need to save to an alternate location.
			bool modifiedMaterial = false;

			if (Submaterials.Length == 0)
				modifiedMaterial = UpdateMaterialElement(materialElement, this);
			else
			{
				XElement subMaterialsElement = materialElement.Element("SubMaterials");
				if (subMaterialsElement == null)
				{
					throw new CustomizationConfigurationException("Could not save modified material, base file is not properly configured.");
				}
				List<XElement> subMaterialElements = subMaterialsElement.Elements("Material").ToList();

				for (int i = 0; i < Submaterials.Length; i++)
				{
					CharacterAttachmentMaterial subMaterial = Submaterials.ElementAt(i);

					bool modifiedSubMaterial = UpdateMaterialElement(subMaterialElements.Find(x =>
						{
							XAttribute nameAttribute = x.Attribute("Name");

							if (nameAttribute != null)
								return nameAttribute.Value == subMaterial.BaseFilePath;

							return false;
						}), subMaterial);

					modifiedMaterial = modifiedSubMaterial;
				}
			}

			if (modifiedMaterial)
			{
				if (string.IsNullOrEmpty(FilePath))
				{
					const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
					char[] stringChars = new char[16];

					for (int i = 0; i < stringChars.Length; i++)
						stringChars[i] = chars[CustomizationManager.Selector.Next(chars.Length)];

					string fileName = new string(stringChars);

					FilePath = Path.Combine(Attachment.Slot.Manager.InitParameters.TempDirectory, "Materials", Attachment.Slot.Name, Attachment.Name ?? "unknown", fileName);
				}

				string fullFilePath = CryPak.AdjustFileName(FilePath, PathResolutionRules.RealPath | PathResolutionRules.ForWriting) + ".mtl";

				if (!File.Exists(fullFilePath))
				{
// ReSharper disable AssignNullToNotNullAttribute
					DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(fullFilePath));
// ReSharper restore AssignNullToNotNullAttribute
					while (!directory.Exists)
					{
						Directory.CreateDirectory(directory.FullName);

						directory = Directory.GetParent(directory.FullName);
					}

					FileStream file = File.Create(fullFilePath);
					file.Close();
				}

				materialDocument.Save(fullFilePath);
			}
			else
				FilePath = BaseFilePath;
		}

		private static bool WriteTexture(XElement texturesElement, string textureType, string texturePath)
		{
			if (texturePath == null)
				return false;

			XElement element = texturesElement.Elements("Texture").FirstOrDefault(x => x.Attribute("Map").Value == textureType);
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

		public Vector3 ColorRed { get; set; }
		public Vector3 ColorGreen { get; set; }
		public Vector3 ColorBlue { get; set; }
		public Vector3 ColorAlpha { get; set; }

		public string DiffuseTexture { get; set; }
		public string SpecularTexture { get; set; }
		public string BumpmapTexture { get; set; }
		public string CustomTexture { get; set; }

		public Vector3 DiffuseColor { get; set; }
		public Vector3 SpecularColor { get; set; }

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