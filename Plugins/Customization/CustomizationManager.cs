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
            var writeableCdfPath = CryPak.AdjustFileName(initParams.CharacterDefinitionLocation, PathResolutionRules.RealPath | PathResolutionRules.ForWriting);

            var baseCdfPath = Path.Combine(CryPak.GameFolder, initParams.BaseCharacterDefinition);
            BaseDefinition = XDocument.Load(baseCdfPath);

            if (File.Exists(writeableCdfPath))
                CharacterDefinition = XDocument.Load(writeableCdfPath);
            else
            {
                var directory = new DirectoryInfo(Path.GetDirectoryName(writeableCdfPath));
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

		void Initialize()
        {
            var slots = new List<CharacterAttachmentSlot>();
            Slots = slots;

            foreach (var file in Directory.EnumerateFiles(Path.Combine(CryPak.GameFolder, InitParameters.AvailableAttachmentsDirectory), "*.xml"))
			{
				var xDocument = XDocument.Load(file);
				if (xDocument == null)
					continue;

				var attachmentSlotElement = xDocument.Element("AttachmentSlot");
				if (attachmentSlotElement == null)
					continue;

				slots.Add(new CharacterAttachmentSlot(this, attachmentSlotElement));
			}
        }

        public CharacterAttachmentSlot GetSlot(string slotName)
        {
            foreach (var slot in Slots)
            {
                if (slot.Name.Equals(slotName, StringComparison.CurrentCultureIgnoreCase))
                    return slot;
            }

            return null;
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
            var attachmentList = definitionDocument.Element("CharacterDefinition").Element("AttachmentList");
            return attachmentList.Elements("Attachment");
        }

        public CustomizationInitializationParameters InitParameters { get; set; }

        internal static Random Selector = new Random();
    }
}
