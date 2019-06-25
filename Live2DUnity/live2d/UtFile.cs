using System;
using System.IO;
using UnityEngine;

namespace live2d
{
	public class UtFile
	{
		public static byte[] loadFile(string path)
		{
			try
			{
				FileStream input = File.OpenRead(path);
				BinaryReader binaryReader = new BinaryReader(input);
				byte[] result = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
				binaryReader.Close();
				return result;
			}
			catch (Exception)
			{
				TextAsset textAsset = (TextAsset)Resources.Load(path, typeof(TextAsset));
				return textAsset.bytes;
			}
		}

		public static Stream convertStream(byte[] bytes)
		{
			return new MemoryStream(bytes);
		}

		public static byte[] load(Stream stream)
		{
			int num = (int)stream.Length;
			byte[] array = new byte[num];
			stream.Read(array, 0, num);
			return array;
		}
	}
}
