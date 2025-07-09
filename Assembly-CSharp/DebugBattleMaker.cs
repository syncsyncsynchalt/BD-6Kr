using Server_Common.Formats.Battle;
using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class DebugBattleMaker
{
	public static string currentDir = "//" + Application.streamingAssetsPath + "/Xml/DebugDatas/";

	public static void SerializeBattleStart()
	{
	}

	public static bool SerializeDayBattle(AllBattleFmt fmt)
	{
		if (fmt == null)
		{
			return false;
		}
		string fileName = currentDir + "DayBattleFmt.xml";
		try
		{
			XmlSerializer serializer = new XmlSerializer(typeof(AllBattleFmt));
			return writeBattleFmt(fileName, serializer, fmt);
		}
		catch (Exception e)
		{
			Console.WriteLine($"SerializeDayBattle error: {e.Message}");
			if (e.InnerException != null)
			{
				Console.WriteLine($"Inner exception: {e.InnerException.Message}");
			}
			Console.WriteLine($"Stack trace: {e.StackTrace}");
			return false;
		}
	}

	public static bool SerializeNightBattle(AllBattleFmt fmt)
	{
		if (fmt == null)
		{
			return false;
		}
		string fileName = currentDir + "NightBattleFmt.xml";
		try
		{
			XmlSerializer serializer = new XmlSerializer(typeof(AllBattleFmt));
			return writeBattleFmt(fileName, serializer, fmt);
		}
		catch (Exception e)
		{
			Console.WriteLine($"SerializeNightBattle error: {e.Message}");
			if (e.InnerException != null)
			{
				Console.WriteLine($"Inner exception: {e.InnerException.Message}");
			}
			Console.WriteLine($"Stack trace: {e.StackTrace}");
			return false;
		}
	}

	public static bool SerializeBattleResult(BattleResultFmt fmt)
	{
		if (fmt == null)
		{
			return false;
		}
		string fileName = currentDir + "BattleResultFmt.xml";
		try
		{
			XmlSerializer serializer = new XmlSerializer(typeof(BattleResultFmt));
			return writeBattleFmt(fileName, serializer, fmt);
		}
		catch (Exception e)
		{
			Console.WriteLine($"SerializeBattleResult error: {e.Message}");
			if (e.InnerException != null)
			{
				Console.WriteLine($"Inner exception: {e.InnerException.Message}");
			}
			Console.WriteLine($"Stack trace: {e.StackTrace}");
			return false;
		}
	}

	public static void LoadBattleData(out AllBattleFmt day, out AllBattleFmt night, out BattleResultFmt result)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(AllBattleFmt));
		day = null;
		if (File.Exists(currentDir + "DayBattleFmt.xml"))
		{
			StreamReader streamReader = new StreamReader(currentDir + "DayBattleFmt.xml");
			day = (AllBattleFmt)xmlSerializer.Deserialize(streamReader);
			streamReader.Close();
		}
		night = null;
		if (File.Exists(currentDir + "NightBattleFmt.xml"))
		{
			StreamReader streamReader2 = new StreamReader(currentDir + "NightBattleFmt.xml");
			night = (AllBattleFmt)xmlSerializer.Deserialize(streamReader2);
			streamReader2.Close();
		}
		result = null;
		xmlSerializer = new XmlSerializer(typeof(BattleResultFmt));
		if (File.Exists(currentDir + "BattleResultFmt.xml"))
		{
			StreamReader streamReader3 = new StreamReader(currentDir + "BattleResultFmt.xml");
			result = (BattleResultFmt)xmlSerializer.Deserialize(streamReader3);
			streamReader3.Close();
		}
	}

	private static bool writeBattleFmt(string fileName, XmlSerializer serializer, object data)
	{
		try
		{
			// ディレクトリが存在しない場合は作成
			string directory = Path.GetDirectoryName(fileName);
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			using (StreamWriter writer = new StreamWriter(fileName))
			{
				serializer.Serialize(writer, data);
			}
			return true;
		}
		catch (Exception ex)
		{
			Debug.LogError($"XMLシリアライズエラー: {ex.Message}");
			Debug.LogError($"スタックトレース: {ex.StackTrace}");
			return false;
		}
	}
}
