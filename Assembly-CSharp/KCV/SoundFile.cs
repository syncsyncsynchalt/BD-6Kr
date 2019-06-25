using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	public static class SoundFile
	{
		private const string BGM_FILE_PATH = "Sounds/BGM/{0}";

		private const string SE_FILE_PATH = "Sounds/SE/{0}";

		private static Dictionary<SEFIleInfos, AudioClip> _dicSEFileDictionary = new Dictionary<SEFIleInfos, AudioClip>();

		public static AudioClip LoadSE(SEFIleInfos file)
		{
			if (_dicSEFileDictionary.ContainsKey(file))
			{
				if ((Object)_dicSEFileDictionary[file] != null)
				{
					return _dicSEFileDictionary[file];
				}
				AudioClip val = Resources.Load($"Sounds/SE/{file.SEFileName()}") as AudioClip;
				if ((Object)val == null)
				{
					return null;
				}
				_dicSEFileDictionary[file] = val;
			}
			else
			{
				string text = $"Sounds/SE/{file.SEFileName()}";
				AudioClip val = Resources.Load($"Sounds/SE/{file.SEFileName()}") as AudioClip;
				if ((Object)val == null)
				{
					return null;
				}
				_dicSEFileDictionary.Add(file, val);
			}
			return _dicSEFileDictionary[file];
		}

		public static void ClearAllSE()
		{
			_dicSEFileDictionary.Clear();
		}

		public static AudioClip LoadBGM(BGMFileInfos file)
		{
			return Resources.Load($"Sounds/BGM/{file.BGMFileName()}") as AudioClip;
		}

		public static AudioClip LoadBGM(int bgmNum)
		{
			return Resources.Load($"Sounds/BGM/{bgmNum}") as AudioClip;
		}
	}
}
