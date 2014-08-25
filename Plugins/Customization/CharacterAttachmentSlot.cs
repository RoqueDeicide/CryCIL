using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using CryEngine;
using CryEngine.Extensions;

namespace CryEngine.CharacterCustomization
{
	public class CharacterAttachmentSlot
	{
		internal CharacterAttachmentSlot(CustomizationManager manager, XElement element)
		{
			Manager = manager;
			Element = element;

			Name = element.Attribute("name").Value;

			XAttribute uiNameAttribute = element.Attribute("uiName");
			if (uiNameAttribute != null)
				UIName = uiNameAttribute.Value;

			XAttribute categoryAttribute = element.Attribute("category");
			if (categoryAttribute != null)
				Category = categoryAttribute.Value;

			XAttribute allowNoneAttribute = element.Attribute("allowNone");
			if (allowNoneAttribute != null)
				CanBeEmpty = allowNoneAttribute.Value == "1";

			if (CanBeEmpty)
				EmptyAttachment = new CharacterAttachment(this, null);

			XAttribute hiddenAttribute = element.Attribute("hidden");
			if (hiddenAttribute != null)
				Hidden = hiddenAttribute.Value == "1";

			List<XElement> subSlotElements = element.Elements("AttachmentSlot").ToList();
			if (subSlotElements.Count != 0)
			{
				SubAttachmentSlots = new CharacterAttachmentSlot[subSlotElements.Count()];
				for (int i = 0; i < subSlotElements.Count(); i++)
				{
					XElement subSlotElement = subSlotElements.ElementAt(i);

					SubAttachmentSlots[i] = new CharacterAttachmentSlot(manager, subSlotElement);
				}
			}

			List<XElement> mirroredSlotElements = element.Elements("MirroredSlot").ToList();
			if (mirroredSlotElements.Count != 0)
			{
				MirroredSlots = new CharacterAttachmentSlot[mirroredSlotElements.Count()];
				for (int i = 0; i < mirroredSlotElements.Count(); i++)
				{
					XElement mirroredSlotElement = mirroredSlotElements[i];

					MirroredSlots[i] = new CharacterAttachmentSlot(manager, mirroredSlotElement);
				}
			}

			// Set up brands
			List<XElement> slotBrandElements = element.Elements("Brand").ToList();

			int numBrands = slotBrandElements.Count;
			Brands = new CharacterAttachmentBrand[numBrands];

			for (int i = 0; i < numBrands; i++)
			{
				XElement brandElement = slotBrandElements[i];

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
			return this.Brands == null ? null : this.Brands.FirstOrDefault(brand => brand.Name == name);
		}

		public CharacterAttachment GetAttachment(string name)
		{

			if (name == "None")
				return EmptyAttachment;
			return
				this.Brands != null
				? this.Brands.Select(x=>x.Attachments.First(y => y.Name == name)).FirstOrDefault()
				: null;
		}

		public XElement GetWriteableElement(string name = null)
		{
			if (name == null)
				name = Name;

			return Manager.GetAttachmentElements(Manager.CharacterDefinition).FirstOrDefault(x =>
				{
					XAttribute aNameAttribute = x.Attribute("AName");
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
				XElement attachmentList = Manager.CharacterDefinition.Element("CharacterDefinition").Element("AttachmentList");

				IEnumerable<XElement> attachmentElements = attachmentList.Elements("Attachment");
				XElement attachmentElement = attachmentElements.FirstOrDefault(x =>
					{
						XAttribute aNameAttribute = x.Attribute("AName");
						if (aNameAttribute == null)
							return false;

						return aNameAttribute.Value == Name;
					});

				if (attachmentElement == null)
					return null;

				return
				(
					from brand in this.Brands
					from attachment in brand.Attachments
						let nameAttribute = attachmentElement.Attribute("Name")
					where nameAttribute != null
					where attachment.Name == nameAttribute.Value
					select attachment
				).FirstOrDefault();
			}
		}

		/// <summary>
		/// Gets a random attachment for this slot.
		/// </summary>
		/// <returns>
		/// The randomed attachment, possibly null if <see cref="CanBeEmpty" /> is set to true.
		/// </returns>
		public CharacterAttachment RandomAttachment
		{
			get
			{
				Random selector = new Random();

				int iRandom = selector.Next(Brands.Length);

				CharacterAttachmentBrand brand = Brands[iRandom];

				iRandom = selector.Next(CanBeEmpty ? -1 : 0, brand.Attachments.Length);

				return
					iRandom != -1
					? brand.Attachments.ElementAt(iRandom)
					: this.EmptyAttachment;
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