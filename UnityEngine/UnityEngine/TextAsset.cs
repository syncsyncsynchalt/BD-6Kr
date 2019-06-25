using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public class TextAsset : Object
	{
		public string text
		{
			get;
		}

		public byte[] bytes
		{
			get;
		}

		public override string ToString()
		{
			return text;
		}
	}
}
