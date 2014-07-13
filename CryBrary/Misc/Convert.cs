namespace CryEngine
{
	public static class Convert
	{
		public static object FromString(EditorPropertyType type, string value)
		{
			if (type == EditorPropertyType.String)
				return value;
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (value == null)
				throw new System.ArgumentNullException("value");
			if (value.Length < 1)
				throw new System.ArgumentException("value string was empty");
#endif

			switch (type)
			{
				case EditorPropertyType.Bool:
					{
						if (value == "0")
							value = "false";
						else if (value == "1")
							value = "true";

						return bool.Parse(value);
					}
				case EditorPropertyType.Int:
					return int.Parse(value);
				case EditorPropertyType.Float:
					return float.Parse(value);
				case EditorPropertyType.Vec3:
					return Vector3.Parse(value);
			}

			return null;
		}
	}
}