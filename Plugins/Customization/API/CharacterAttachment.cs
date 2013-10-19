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
				var slotAttachmentNameAttribute = element.Attribute("Name");
				if (slotAttachmentNameAttribute != null)
					Name = slotAttachmentNameAttribute.Value;

				var slotAttachmentThumbnailAttribute = element.Attribute("Thumbnail");
				if (slotAttachmentThumbnailAttribute != null)
					ThumbnailPath = slotAttachmentThumbnailAttribute.Value;

				var slotAttachmentTypeAttribute = element.Attribute("Type");
				if (slotAttachmentTypeAttribute != null)
					Type = slotAttachmentTypeAttribute.Value;

				var slotAttachmentBoneNameAttribute = element.Attribute("BoneName");
				if (slotAttachmentBoneNameAttribute != null)
					BoneName = slotAttachmentBoneNameAttribute.Value;

				var slotAttachmentObjectAttribute = element.Attribute("Binding");
				if (slotAttachmentObjectAttribute != null)
					Object = slotAttachmentObjectAttribute.Value;

				var slotAttachmentFlagsAttribute = element.Attribute("Flags");
				if (slotAttachmentFlagsAttribute != null)
					Flags = slotAttachmentFlagsAttribute.Value;

				var slotAttachmentPositionAttribute = element.Attribute("Position");
				if (slotAttachmentPositionAttribute != null)
					Position = slotAttachmentPositionAttribute.Value;

				var slotAttachmentRotationAttribute = element.Attribute("Rotation");
				if (slotAttachmentRotationAttribute != null)
					Rotation = slotAttachmentRotationAttribute.Value;

				var slotAttachmentMaterials = new List<CharacterAttachmentMaterial>();

				foreach (var materialElement in element.Elements("Material"))
					slotAttachmentMaterials.Add(new CharacterAttachmentMaterial(this, materialElement));

				Materials = slotAttachmentMaterials.ToArray();
				Material = Materials.FirstOrDefault();

				if (!child)
				{
					var subCharacterAttachments = new List<CharacterAttachment>();

					foreach (var subAttachmentElement in element.Elements("SubAttachment"))
					{
						var subAttachmentSlotName = subAttachmentElement.Attribute("Slot").Value;

						var subAttachmentSlot = Slot.SubAttachmentSlots.FirstOrDefault(x => x.Name == subAttachmentSlotName);
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
						var mirroredSlot = slot.MirroredSlots.ElementAt(i);
						var mirroredAttachmentElement = element.Element(mirroredSlot.Name);
						if (mirroredAttachmentElement == null)
							throw new CustomizationConfigurationException(string.Format("Failed to get mirrored element from slot {0} and name {1}", slot.Name, mirroredSlot.Name));

						MirroredChildren[i] = new CharacterAttachment(mirroredSlot, mirroredAttachmentElement);
					}
				}
			}
		}

        void WriteAttribute(XElement attachmentElement, XElement baseAttachmentElement, string name, object value)
        {
            if (value != null)
                attachmentElement.SetAttributeValue(name, value);
            else
            {
                var baseAttribute = baseAttachmentElement.Attribute(name);
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
                    foreach (var mirroredAttachment in MirroredChildren)
                    {
                        Write(Slot.GetWriteableElement(mirroredAttachment.Slot.Name));
                        mirroredAttachment.Write(null);
                    }

                    return true;
                }

                var currentAttachment = Slot.Current;
                if (currentAttachment != null)
                {
                    if (currentAttachment.SubAttachmentVariations != null)
                    {
                        foreach (var subAttachment in currentAttachment.SubAttachmentVariations)
                            subAttachment.Slot.Clear();
                    }
                }

                attachmentElement = Slot.GetWriteableElement();
                if (attachmentElement == null)
                    throw new CustomizationConfigurationException(string.Format("Failed to locate attachments for slot {0}!", Slot.Name));
            }

            var slotName = attachmentElement.Attribute("AName").Value;

            if (Slot.EmptyAttachment == this)
            {
                attachmentElement.RemoveAll();

                attachmentElement.SetAttributeValue("AName", slotName);
            }
            else
            {
                var baseAttachmentElement = Slot.Manager.GetAttachmentElements(Slot.Manager.BaseDefinition).FirstOrDefault(x => x.Attribute("AName").Value == slotName);

                WriteAttribute(attachmentElement, baseAttachmentElement, "Name", Name);

                WriteAttribute(attachmentElement, baseAttachmentElement, "Type", Type);
                WriteAttribute(attachmentElement, baseAttachmentElement, "BoneName", BoneName);

                WriteAttribute(attachmentElement, baseAttachmentElement, "Binding", Object);

                if (Material != null)
                    Material.Save();

                var materialPath = Material != null ? Material.FilePath : null;
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

				var selector = new Random();
				var iRandom = selector.Next(Materials.Length);

				return Materials.ElementAt(iRandom);
			}
		}

        public CharacterAttachmentMaterial NextMaterial
        {
            get
            {
                if (Materials == null || Materials.Length == 0)
                    return null;

                var currentMaterial = Material;
                CharacterAttachmentMaterial nextMaterial = null;

                for(var i = 0; i < Materials.Length; i++)
                {
                    var material = Materials.ElementAt(i);

                    if (material == currentMaterial)
                    {
                        if (i < Materials.Length - 1)
                            nextMaterial = Materials.ElementAt(i + 1);
                        else
                            nextMaterial = Materials.ElementAt(0);

                        break;
                    }
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

				var selector = new Random();

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