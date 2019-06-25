using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Startup
{
	public class Defines : IDisposable
	{
		public const string HEADER_MESSAGE_ADMIRAL_NAME = "提督名入力";

		public const string HEADER_MESSAGE_STARTER_SELECT = "ゲ\u30fcム開始";

		public const string HEADER_MESSAGE_FIRST_SHIP = "初期艦選択";

		public const string HEADER_MESSAGE_TUTORIAL = "チュ\u30fcトリアル";

		public const float STARTER_PARTNER_SWIPE_RANGE = 0.15f;

		public const int PARTNERSHIP_SELECT_VOICE_NUM = 26;

		private static List<List<int>> _listStarterShipsID;

		private static List<List<Vector3>> _listStarterTextSize;

		public static List<List<int>> STARTER_PARTNER_SHIPS_ID => _listStarterShipsID;

		public static List<List<Vector3>> STARTER_PARTNER_TEXT_SIZE => _listStarterTextSize;

		public Defines()
		{
			_listStarterShipsID = new List<List<int>>
			{
				new List<int>
				{
					54,
					55,
					56
				},
				new List<int>
				{
					9,
					33,
					37,
					46,
					94,
					1,
					43,
					96
				}
			};
			_listStarterTextSize = new List<List<Vector3>>
			{
				new List<Vector3>
				{
					new Vector3(816f, 350f, 0f),
					new Vector3(760f, 370f, 0f),
					new Vector3(758f, 368f, 0f)
				},
				new List<Vector3>
				{
					new Vector3(770f, 348f, 0f),
					new Vector3(766f, 352f, 0f),
					new Vector3(804f, 368f, 0f),
					new Vector3(762f, 368f, 0f),
					new Vector3(764f, 368f, 0f),
					new Vector3(666f, 380f, 0f),
					new Vector3(712f, 382f, 0f),
					new Vector3(784f, 352f, 0f)
				}
			};
		}

		public void Dispose()
		{
			Mem.DelListSafe(ref _listStarterShipsID);
			Mem.DelListSafe(ref _listStarterTextSize);
		}
	}
}
