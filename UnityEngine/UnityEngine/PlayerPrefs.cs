using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using UnityEngine.Internal;

namespace UnityEngine;

public sealed class PlayerPrefs
{
	private static Dictionary<string, object> _prefs = new Dictionary<string, object>();
	private static readonly string _prefsFile = "unity_player_prefs.json";
	private static bool _loaded = false;
	private static readonly object _lock = new object();

	static PlayerPrefs()
	{
		LoadPrefs();
	}

	private static bool TrySetInt(string key, int value)
	{
		try
		{
			EnsureLoaded();
			lock (_lock)
			{
				_prefs[key] = value;
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	private static bool TrySetFloat(string key, float value)
	{
		try
		{
			EnsureLoaded();
			lock (_lock)
			{
				_prefs[key] = value;
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	private static bool TrySetSetString(string key, string value)
	{
		try
		{
			EnsureLoaded();
			lock (_lock)
			{
				_prefs[key] = value;
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static void SetInt(string key, int value)
	{
		if (!TrySetInt(key, value))
		{
			throw new PlayerPrefsException("Could not store preference value");
		}
	}

	public static int GetInt(string key, [DefaultValue("0")] int defaultValue = 0)
	{
		EnsureLoaded();
		lock (_lock)
		{
			if (_prefs.TryGetValue(key, out object value))
			{
				if (value is int intValue)
					return intValue;
				if (value is JsonElement element && element.ValueKind == JsonValueKind.Number)
					return element.GetInt32();
			}
		}
		return defaultValue;
	}

	[ExcludeFromDocs]
	public static int GetInt(string key)
	{
		return GetInt(key, 0);
	}

	public static void SetFloat(string key, float value)
	{
		if (!TrySetFloat(key, value))
		{
			throw new PlayerPrefsException("Could not store preference value");
		}
	}

	public static float GetFloat(string key, [DefaultValue("0.0F")] float defaultValue = 0.0f)
	{
		EnsureLoaded();
		lock (_lock)
		{
			if (_prefs.TryGetValue(key, out object value))
			{
				if (value is float floatValue)
					return floatValue;
				if (value is JsonElement element && element.ValueKind == JsonValueKind.Number)
					return element.GetSingle();
			}
		}
		return defaultValue;
	}

	[ExcludeFromDocs]
	public static float GetFloat(string key)
	{
		return GetFloat(key, 0f);
	}

	public static void SetString(string key, string value)
	{
		if (!TrySetSetString(key, value))
		{
			throw new PlayerPrefsException("Could not store preference value");
		}
	}

	public static string GetString(string key, [DefaultValue("\"\"")] string defaultValue = "")
	{
		EnsureLoaded();
		lock (_lock)
		{
			if (_prefs.TryGetValue(key, out object value))
			{
				if (value is string stringValue)
					return stringValue;
				if (value is JsonElement element && element.ValueKind == JsonValueKind.String)
					return element.GetString();
			}
		}
		return defaultValue;
	}

	[ExcludeFromDocs]
	public static string GetString(string key)
	{
		return GetString(key, string.Empty);
	}

	public static bool HasKey(string key)
	{
		EnsureLoaded();
		lock (_lock)
		{
			return _prefs.ContainsKey(key);
		}
	}

	public static void DeleteKey(string key)
	{
		EnsureLoaded();
		lock (_lock)
		{
			_prefs.Remove(key);
		}
	}

	public static void DeleteAll()
	{
		lock (_lock)
		{
			_prefs.Clear();
		}
	}

	public static void Save()
	{
		try
		{
			lock (_lock)
			{
				var json = JsonSerializer.Serialize(_prefs, new JsonSerializerOptions
				{
					WriteIndented = true
				});
				File.WriteAllText(_prefsFile, json);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"PlayerPrefs保存エラー: {ex.Message}");
		}
	}

	private static void LoadPrefs()
	{
		try
		{
			if (File.Exists(_prefsFile))
			{
				var json = File.ReadAllText(_prefsFile);
				lock (_lock)
				{
					_prefs = JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
				}
			}
			_loaded = true;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"PlayerPrefs読み込みエラー: {ex.Message}");
			lock (_lock)
			{
				_prefs = new Dictionary<string, object>();
			}
			_loaded = true;
		}
	}

	private static void EnsureLoaded()
	{
		if (!_loaded)
		{
			LoadPrefs();
		}
	}
}
