using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using CryEngine;

namespace CryEngine.CharacterCustomization
{
    public class CharacterAttachmentSlot
    {
        internal CharacterAttachmentSlot(CustomizationManager manager, XElement element)
        {
            Manager = manager;
            Element = element;

            Name = element.Attribute("name").Value;

            var uiNameAttribute = element.Attribute("uiName");
            if(uiNameAttribute != null)
                UIName = uiNameAttribute.Value;

            var categoryAttribute = element.Attribute("category");
            if (categoryAttribute != null)
                Category = categoryAttribute.Value;

            var allowNoneAttribute = element.Attribute("allowNone");
            if(allowNoneAttribute != null)
                CanBeEmpty = allowNoneAttribute.Value == "1";

			if(CanBeEmpty)
				EmptyAttachment = new CharacterAttachment(this, null);

            var hiddenAttribute = element.Attribute("hidden");
            if(hiddenAttribute != null)
                Hidden = hiddenAttribute.Value == "1";

			var subSlotElements = element.Elements("AttachmentSlot");
			if (subSlotElements.Count() > 0)
			{
				SubAttachmentSlots = new CharacterAttachmentSlot[subSlotElements.Count()];
				for(int i = 0; i < subSlotElements.Count(); i++)
				{
					var subSlotElement = subSlotElements.ElementAt(i);

					SubAttachmentSlots[i] = new CharacterAttachmentSlot(manager, subSlotElement);
				}
			}

			var mirroredSlotElements = element.Elements("MirroredSlot");
			if (mirroredSlotElements.Count() > 0)
			{
				MirroredSlots = new CharacterAttachmentSlot[mirroredSlotElements.Count()];
				for (int i = 0; i < mirroredSlotElements.Count(); i++)
				{
					var mirroredSlotElement = mirroredSlotElements.ElementAt(i);

					MirroredSlots[i] = new CharacterAttachmentSlot(manager, mirroredSlotElement);
				}
			}

			// Set up brands
			var slotBrandElements = element.Elements("Brand");

			int numBrands = slotBrandElements.Count();
			Brands = new CharacterAttachmentBrand[numBrands];

			for(int i = 0; i < numBrands; i++)
			{
				var brandElement = slotBrandElements.ElementAt(i);

				Brands[i] = new CharacterAttachmentBrand(this, brandElement);
			}
        }

        /// <summary>
        /// Clears the currently active attachment for this slot.
        /// </summary>
        public void Clear()
        {
			GetWriteableElement().SetAttributeValue("Binding", null);
        }

		public CharacterAttachment EmptyAttachment { get; set; }

		public CharacterAttachmentBrand GetBrand(string name)
		{
			if (Brands != null)
			{
				foreach (var brand in Brands)
				{
					if (brand.Name == name)
						return brand;
				}
			}

			return null;
		}

        public CharacterAttachment GetAttachment(string name)
        {
			if (Brands != null)
			{
				foreach (var brand in Brands)
				{
					foreach (var attachment in brand.Attachments)
					{
						if (attachment.Name == name)
							return attachment;
					}
				}
			}

			if (name == "None")
				return EmptyAttachment;

            return null;
        }

		public XElement GetWriteableElement(string name = null)
		{
			if (name == null)
				name = Name;

            return Manager.GetAttachmentElements(Manager.CharacterDefinition).FirstOrDefault(x => 
                {
                    var aNameAttribute = x.Attribute("AName");
                    if (aNameAttribute == null)
                        return false;

                    return aNameAttribute.Value == name;
                });
		}

        public string Name { get; set; }

        public string UIName { get; set; }

        public string Category { get; set; }

		/// <summary>
		/// Brands containing attachments.
		/// </summary>
		public CharacterAttachmentBrand[] Brands { get; set; }

        /// <summary>
        /// Gets the currently active attachment for this slot.
        /// </summary>
        public CharacterAttachment Current
        {
            get
            {
                var attachmentList = Manager.CharacterDefinition.Element("CharacterDefinition").Element("AttachmentList");

                var attachmentElements = attachmentList.Elements("Attachment");
				var attachmentElement = attachmentElements.FirstOrDefault(x => 
                    {
                        var aNameAttribute = x.Attribute("AName");
                        if (aNameAttribute == null)
                            return false;

                        return aNameAttribute.Value == Name;
                    });

                if (attachmentElement == null)
                    return null;

				foreach (var brand in Brands)
				{
					foreach (var attachment in brand.Attachments)
					{
						var nameAttribute = attachmentElement.Attribute("Name");
						if (nameAttribute == null)
							continue;

						if (attachment.Name == nameAttribute.Value)
							return attachment;
					}
				}

                return null;
            }
        }

        /// <summary>
        /// Gets a random attachment for this slot.
        /// </summary>
        /// <returns>The randomed attachment, possibly null if <see cref="CanBeEmpty"/> is set to true.</returns>
        public CharacterAttachment RandomAttachment
        {
            get
            {
                var selector = new Random();

				var iRandom = selector.Next(Brands.Length);

				var brand = Brands[iRandom];

				iRandom = selector.Next(CanBeEmpty ? -1 : 0, brand.Attachments.Length);

                if (iRandom != -1)
					return brand.Attachments.ElementAt(iRandom);
                else
                    return EmptyAttachment;
            }
        }

		public CharacterAttachmentSlot[] SubAttachmentSlots { get; set; }

		public CharacterAttachmentSlot[] MirroredSlots { get; set; }

        internal XElement Element { get; private set; }

        public bool CanBeEmpty { get; set; }

        public bool Hidden { get; set; }

        public CustomizationManager Manager { get; set; }
    }
}
