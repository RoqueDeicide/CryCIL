using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine.CharacterCustomization
{
	public struct CustomizationInitializationParameters
	{
		public static CustomizationInitializationParameters GetDefault()
		{
			var initParams = new CustomizationInitializationParameters();

			initParams.CharacterDefinitionLocation = "%USER%/Cosmetics/Definitions/MyCharacter.cdf";
			initParams.BaseCharacterDefinition = "Scripts/Config/base.cdf";

			initParams.AvailableAttachmentsDirectory = "Scripts/Config/Attachments/";

			initParams.EmptyThumbnailPath = "Objects/Characters/Attachments/thumb_empty.dds";

			initParams.TempDirectory = "%USER%/Cosmetics/";

			return initParams;
		}

		public string CharacterDefinitionLocation;
		public string BaseCharacterDefinition;

		public string AvailableAttachmentsDirectory;

		public string EmptyThumbnailPath;

		public string TempDirectory;
	}
}