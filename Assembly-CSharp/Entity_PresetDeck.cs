using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity_PresetDeck : ScriptableObject
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
		public int No;

		public string Name;

		public string[] PresetShip;
	}

	public List<Sheet> sheets = new List<Sheet>();
}
