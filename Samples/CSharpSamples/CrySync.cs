using CryCil.Engine.Data;
using CryCil.Geometry;

namespace CSharpSamples
{
	/// <summary>
	/// Represents an example of synchronization
	/// </summary>
	public class SyncObject : ISynchronizable
	{
		private string text;
		private Quaternion quat;
		private long num;
		/// <inheritdoc/>
		public void Sync(CrySync s)
		{
			// Generally when writing optional groups their condition is based on the state of the
			// serialized object. This means that we will have a problem on the receiving end, since the
			// state of the receiver cannot be used to determine whether the group was written.

			// Generally this is solved by making sure "true" is passed as a condition when reading:

			// Standard CryEngine way:

			if (s.Writing)
			{
				if (s.BeginOptionalGroup("group1", this.Condition1()))
				{
					// This code won't be reached when underlying implementation doesn't support optional
					// groups or condition is false.
					s.Sync("text", ref this.text);
					s.EndGroup();
				}
			}
			else
			{
				if (s.BeginOptionalGroup("group1", true))
				{
					// This code won't be reached when underlying implementation doesn't support optional
					// groups or the group wasn't written before.
					s.Sync("text", ref this.text);
					s.EndGroup();
				}
			}

			// Above method is robust, doesn't involve serializing extra data, but it requires a lot of
			// code duplication since you have to write the code for both writing and reading the data.
			// This does, however allow special coding for writing and reading.

			if (s.BeginOptionalGroup("group2", s.Reading || this.Condition2()))
			{
				// This is an equivalent of the above code with less duplication, but specialization of the
				// code may bloat it a little.
				s.Sync("quat", ref this.quat);
				s.EndGroup();
			}

			// Also presence of the optional group can be defined by global state, which works for some
			// static stuff and is very simple.

			if (s.BeginOptionalGroup("group3", this.Condition3()))
			{
				// This code will be reached when the global state allows it.
				s.Sync("num", ref this.num);
				s.EndGroup();
			}
		}
		private bool Condition1()
		{
			// Put your logic here.
			return true;
		}
		private bool Condition2()
		{
			// Put your logic here.
			return true;
		}
		private bool Condition3()
		{
			// Put your logic here.
			return true;
		}
	}
}