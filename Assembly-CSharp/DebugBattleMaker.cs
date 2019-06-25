using Server_Common.Formats.Battle;
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
		XmlSerializer serializer = new XmlSerializer(typeof(AllBattleFmt));
		return writeBattleFmt(fileName, serializer, fmt);
	}

	public static bool SerializeNightBattle(AllBattleFmt fmt)
	{
		if (fmt == null)
		{
			return false;
		}
		string fileName = currentDir + "NightBattleFmt.xml";
		XmlSerializer serializer = new XmlSerializer(typeof(AllBattleFmt));
		return writeBattleFmt(fileName, serializer, fmt);
	}

	public static bool SerializeBattleResult(BattleResultFmt fmt)
	{
		if (fmt == null)
		{
			return false;
		}
		string fileName = currentDir + "BattleResultFmt.xml";
		XmlSerializer serializer = new XmlSerializer(typeof(BattleResultFmt));
		return writeBattleFmt(fileName, serializer, fmt);
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
		return true;
	}
}
