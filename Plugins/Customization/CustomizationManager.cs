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
	public class CustomizationManager
	{
		public CustomizationManager(CustomizationInitializationParameters initParams)
		{
			string writeableCdfPath = CryPak.AdjustFileName(initParams.CharacterDefinitionLocation, PathResolutionRules.RealPath | PathResolutionRules.ForWriting);

			string baseCdfPath = Path.Combine(CryPak.GameFolder, initParams.BaseCharacterDefinition);
			BaseDefinition = XDocument.Load(baseCdfPath);

			if (File.Exists(writeableCdfPath))
				CharacterDefinition = XDocument.Load(writeableCdfPath);
			else
			{
// ReSharper disable AssignNullToNotNullAttribute
				DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(writeableCdfPath));
// ReSharper restore AssignNullToNotNullAttribute
				while (!directory.Exists)
				{
					Directory.CreateDirectory(directory.FullName);

					directory = Directory.GetParent(directory.FullName);
				}

				File.Copy(baseCdfPath, writeableCdfPath);

				CharacterDefinition = XDocument.Load(writeableCdfPath);
			}

			InitParameters = initParams;

			Initialize();
		}

		private void Initialize()
		{
			List<CharacterAttachmentSlot> slots = new List<CharacterAttachmentSlot>();
			Slots = slots;

			slots.AddRange
			(
				from file in Directory.EnumerateFiles
				(
					Path.Combine(CryPak.GameFolder, this.InitParameters.AvailableAttachmentsDirectory),
					"*.xml"
				)
				select XDocument.Load(file) into xDocument
				where xDocument != null
				select xDocument.Element("AttachmentSlot") into attachmentSlotElement
				where attachmentSlotElement != null
				select new CharacterAttachmentSlot(this, attachmentSlotElement)
			);
		}

		public CharacterAttachmentSlot GetSlot(string slotName)
		{
			return
				this.Slots.FirstOrDefault(slot => slot.Name.Equals(slotName, StringComparison.CurrentCultureIgnoreCase));
		}

		public void Save()
		{
			CharacterDefinition.Save(CryPak.AdjustFileName(InitParameters.CharacterDefinitionLocation, PathResolutionRules.RealPath | PathResolutionRules.ForWriting));
		}

		public IEnumerable<CharacterAttachmentSlot> Slots { get; set; }

		internal XDocument CharacterDefinition { get; set; }
		internal XDocument BaseDefinition { get; set; }

		internal IEnumerable<XElement> GetAttachmentElements(XDocument definitionDocument)
		{
			XElement attachmentList = definitionDocument.Element("CharacterDefinition").Element("AttachmentList");
			return attachmentList.Elements("Attachment");
		}

		public CustomizationInitializationParameters InitParameters { get; set; }

		internal static Random Selector = new Random();
	}
}