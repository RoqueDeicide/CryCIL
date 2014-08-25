using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace CryEngine.CharacterCustomization
{
	public class CharacterAttachment
	{
		internal CharacterAttachment(CharacterAttachmentSlot slot, XElement element, bool child = false)
		{
			Element = element;
			Slot = slot;

			if (element == null)
			{
				Name = "None";
				ThumbnailPath = slot.Manager.InitParameters.EmptyThumbnailPath;
			}
			else
			{
				XAttribute slotAttachmentNameAttribute = element.Attribute("Name");
				if (slotAttachmentNameAttribute != null)
					Name = slotAttachmentNameAttribute.Value;

				XAttribute slotAttachmentThumbnailAttribute = element.Attribute("Thumbnail");
				if (slotAttachmentThumbnailAttribute != null)
					ThumbnailPath = slotAttachmentThumbnailAttribute.Value;

				XAttribute slotAttachmentTypeAttribute = element.Attribute("Type");
				if (slotAttachmentTypeAttribute != null)
					Type = slotAttachmentTypeAttribute.Value;

				XAttribute slotAttachmentBoneNameAttribute = element.Attribute("BoneName");
				if (slotAttachmentBoneNameAttribute != null)
					BoneName = slotAttachmentBoneNameAttribute.Value;

				XAttribute slotAttachmentObjectAttribute = element.Attribute("Binding");
				if (slotAttachmentObjectAttribute != null)
					Object = slotAttachmentObjectAttribute.Value;

				XAttribute slotAttachmentFlagsAttribute = element.Attribute("Flags");
				if (slotAttachmentFlagsAttribute != null)
					Flags = slotAttachmentFlagsAttribute.Value;

				XAttribute slotAttachmentPositionAttribute = element.Attribute("Position");
				if (slotAttachmentPositionAttribute != null)
					Position = slotAttachmentPositionAttribute.Value;

				XAttribute slotAttachmentRotationAttribute = element.Attribute("Rotation");
				if (slotAttachmentRotationAttribute != null)
					Rotation = slotAttachmentRotationAttribute.Value;

				Materials =
					element.Elements("Material")
					.Select(materialElement => new CharacterAttachmentMaterial(this, materialElement))
					.ToArray();
				Material = Materials.FirstOrDefault();

				if (!child)
				{
					List<CharacterAttachment> subCharacterAttachments = new List<CharacterAttachment>();

					foreach (XElement subAttachmentElement in element.Elements("SubAttachment"))
					{
						string subAttachmentSlotName = subAttachmentElement.Attribute("Slot").Value;

						CharacterAttachmentSlot subAttachmentSlot = Slot.SubAttachmentSlots.FirstOrDefault(x => x.Name == subAttachmentSlotName);
						if (subAttachmentSlot == null)
							throw new CustomizationConfigurationException(string.Format("Failed to find subattachment slot {0} for attachment {1} for primary slot {2}", subAttachmentSlotName, Name, Slot.Name));

						subCharacterAttachments.Add(new CharacterAttachment(subAttachmentSlot, subAttachmentElement, true));
					}

					SubAttachmentVariations = subCharacterAttachments.ToArray();
					SubAttachment = SubAttachmentVariations.FirstOrDefault();
				}

				if (slot.MirroredSlots != null)
				{
					MirroredChildren = new CharacterAttachment[slot.MirroredSlots.Length];
					for (int i = 0; i < slot.MirroredSlots.Length; i++)
					{
						CharacterAttachmentSlot mirroredSlot = slot.MirroredSlots.ElementAt(i);
						XElement mirroredAttachmentElement = element.Element(mirroredSlot.Name);
						if (mirroredAttachmentElement == null)
							throw new CustomizationConfigurationException(string.Format("Failed to get mirrored element from slot {0} and name {1}", slot.Name, mirroredSlot.Name));

						MirroredChildren[i] = new CharacterAttachment(mirroredSlot, mirroredAttachmentElement);
					}
				}
			}
		}

		private void WriteAttribute(XElement attachmentElement, XElement baseAttachmentElement, string name, object value)
		{
			if (value != null)
				attachmentElement.SetAttributeValue(name, value);
			else
			{
				XAttribute baseAttribute = baseAttachmentElement.Attribute(name);
				if (baseAttribute != null)
					value = baseAttribute.Value;

				attachmentElement.SetAttributeValue(name, value);
			}
		}

		public bool Write(XElement attachmentElement = null)
		{
			if (attachmentElement == null)
			{
				if (Slot.MirroredSlots != null && MirroredChildren != null)
				{
					foreach (CharacterAttachment mirroredAttachment in MirroredChildren)
					{
						Write(Slot.GetWriteableElement(mirroredAttachment.Slot.Name));
						mirroredAttachment.Write();
					}

					return true;
				}

				CharacterAttachment currentAttachment = Slot.Current;
				if (currentAttachment != null)
				{
					if (currentAttachment.SubAttachmentVariations != null)
					{
						foreach (CharacterAttachment subAttachment in currentAttachment.SubAttachmentVariations)
							subAttachment.Slot.Clear();
					}
				}

				attachmentElement = Slot.GetWriteableElement();
				if (attachmentElement == null)
					throw new CustomizationConfigurationException(string.Format("Failed to locate attachments for slot {0}!", Slot.Name));
			}

			string slotName = attachmentElement.Attribute("AName").Value;

			if (Slot.EmptyAttachment == this)
			{
				attachmentElement.RemoveAll();

				attachmentElement.SetAttributeValue("AName", slotName);
			}
			else
			{
				XElement baseAttachmentElement = Slot.Manager.GetAttachmentElements(Slot.Manager.BaseDefinition).FirstOrDefault(x => x.Attribute("AName").Value == slotName);

				WriteAttribute(attachmentElement, baseAttachmentElement, "Name", Name);

				WriteAttribute(attachmentElement, baseAttachmentElement, "Type", Type);
				WriteAttribute(attachmentElement, baseAttachmentElement, "BoneName", BoneName);

				WriteAttribute(attachmentElement, baseAttachmentElement, "Binding", Object);

				if (Material != null)
					Material.Save();

				string materialPath = Material != null ? Material.FilePath : null;
				WriteAttribute(attachmentElement, baseAttachmentElement, "Material", materialPath);

				WriteAttribute(attachmentElement, baseAttachmentElement, "Flags", Flags);

				WriteAttribute(attachmentElement, baseAttachmentElement, "Position", Position);
				WriteAttribute(attachmentElement, baseAttachmentElement, "Rotation", Rotation);

				if (SubAttachment != null)
					SubAttachment.Write();
			}

			return true;
		}

		public CharacterAttachmentMaterial RandomMaterial
		{
			get
			{
				if (Materials == null || Materials.Length == 0)
					return null;

				Random selector = new Random();
				int iRandom = selector.Next(Materials.Length);

				return Materials.ElementAt(iRandom);
			}
		}

		public CharacterAttachmentMaterial NextMaterial
		{
			get
			{
				if (Materials == null || Materials.Length == 0)
					return null;

				CharacterAttachmentMaterial currentMaterial = Material;
				CharacterAttachmentMaterial nextMaterial = null;

				for (int i = 0; i < Materials.Length; i++)
				{
					CharacterAttachmentMaterial material = Materials.ElementAt(i);

					if (material != currentMaterial) continue;
					nextMaterial =
						i < this.Materials.Length - 1
							? this.Materials.ElementAt(i + 1)
							: this.Materials.ElementAt(0);

					break;
				}

				return nextMaterial;
			}
		}

		public CharacterAttachment RandomSubAttachment
		{
			get
			{
				if (SubAttachmentVariations == null || SubAttachmentVariations.Length == 0)
					return SubAttachment;

				Random selector = new Random();

				return SubAttachmentVariations.ElementAt(selector.Next(SubAttachmentVariations.Length));
			}
		}

		public CharacterAttachmentSlot Slot { get; set; }

		public string Name { get; set; }

		/// <summary>
		/// Path to this attachment's preview image, relative to the game directory.
		/// </summary>
		public string ThumbnailPath { get; set; }

		public string Type { get; set; }
		public string BoneName { get; set; }

		public string Object { get; set; }

		public CharacterAttachmentMaterial[] Materials { get; set; }
		public CharacterAttachmentMaterial Material { get; set; }

		public string Flags { get; set; }

		public string Position { get; set; }
		public string Rotation { get; set; }

		public CharacterAttachment[] SubAttachmentVariations { get; set; }
		public CharacterAttachment SubAttachment { get; set; }

		// Only used when mirroring
		public CharacterAttachment[] MirroredChildren { get; set; }

		internal XElement Element { get; private set; }
	}
}