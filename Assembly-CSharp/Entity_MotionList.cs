using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity_MotionList : ScriptableObject
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

		public int Motion1;

		public int Motion2;

		public int Motion3;

		public int Motion4;
	}

	public List<Sheet> sheets = new List<Sheet>();
}
