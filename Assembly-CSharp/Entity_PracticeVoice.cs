using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity_PracticeVoice : ScriptableObject
{
	[Serializable]
	public class Sheet
	{
		public string name = string.Empty;

		public List<Param> list = new List<Param>();
	}

	[Serializable]
	public class Param
	{
		public int MstID;

		public string Name;

		public int VoiceNo;
	}

	public List<Sheet> sheets = new List<Sheet>();
}
