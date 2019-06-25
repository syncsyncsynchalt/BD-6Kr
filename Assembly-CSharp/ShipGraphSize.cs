using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipGraphSize : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int MstID;

		public int X;

		public int Y;
	}

	public List<Param> param = new List<Param>();
}
