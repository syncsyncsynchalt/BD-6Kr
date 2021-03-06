using System;

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class PlayerPrefs
	{
		private static bool TrySetInt(string key, int value) { throw new NotImplementedException("なにこれ"); }

		private static bool TrySetFloat(string key, float value) { throw new NotImplementedException("なにこれ"); }

		private static bool TrySetSetString(string key, string value) { throw new NotImplementedException("なにこれ"); }

		public static void SetInt(string key, int value)
		{
			if (!TrySetInt(key, value))
			{
				throw new PlayerPrefsException("Could not store preference value");
			}
		}

		public static int GetInt(string key, [DefaultValue("0")] int defaultValue) {

            // TODO: ちゃんとする
            var playerPrefs = new Dictionary<string, int>() {
                { "VolumeBGM", 50 },
                { "VolumeSE", 50 },
                { "VolumeVoice", 50 },
                { "GuideDisplay", 0 },
            };

            if (!playerPrefs.ContainsKey(key)) { throw new NotImplementedException("なにこれ"); }

            return playerPrefs[key];
        }

		[ExcludeFromDocs]
		public static int GetInt(string key)
		{
			int defaultValue = 0;
			return GetInt(key, defaultValue);
		}

		public static void SetFloat(string key, float value)
		{
			if (!TrySetFloat(key, value))
			{
				throw new PlayerPrefsException("Could not store preference value");
			}
		}

		public static float GetFloat(string key, [DefaultValue("0.0F")] float defaultValue) { throw new NotImplementedException("なにこれ"); }

		[ExcludeFromDocs]
		public static float GetFloat(string key)
		{
			float defaultValue = 0f;
			return GetFloat(key, defaultValue);
		}

		public static void SetString(string key, string value)
		{
			if (!TrySetSetString(key, value))
			{
				throw new PlayerPrefsException("Could not store preference value");
			}
		}

		public static string GetString(string key, [DefaultValue("\"\"")] string defaultValue) { throw new NotImplementedException("なにこれ"); }

		[ExcludeFromDocs]
		public static string GetString(string key)
		{
			string empty = string.Empty;
			return GetString(key, empty);
		}

		public static bool HasKey(string key) { throw new NotImplementedException("なにこれ"); }

		public static void DeleteKey(string key) { throw new NotImplementedException("なにこれ"); }

		public static void DeleteAll() { throw new NotImplementedException("なにこれ"); }

		public static void Save() { throw new NotImplementedException("なにこれ"); }
	}
}
