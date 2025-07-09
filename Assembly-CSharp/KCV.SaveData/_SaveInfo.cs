using System;
using UnityEngine;

namespace KCV.SaveData
{
	public class _SaveInfo : MonoBehaviour
	{
		[Serializable]
		private class DisplayInfo
		{
			private GameObject _objDisplayInfo;

			private UITexture _uiBackground;

			private UILabel _uiName;

			private UILabel _uiLv;

			private UILabel _uiFleetNum;

			private UILabel _uiShipNum;

			private UILabel _uiCheatsArea;

			private UILabel _uiArea;

			private UILabel _uiTurn;

			private UILabel _uiDifficlty;

			private UILabel _uiTactics;

			public DisplayInfo(Transform parent, string objName)
			{
				Util.FindParentToChild(ref _objDisplayInfo, parent, objName);
				Util.FindParentToChild(ref _uiBackground, _objDisplayInfo.transform, "bg");
				Util.FindParentToChild(ref _uiName, _objDisplayInfo.transform, "Name");
				Util.FindParentToChild(ref _uiLv, _objDisplayInfo.transform, "Lv");
				Util.FindParentToChild(ref _uiFleetNum, _objDisplayInfo.transform, "FleetNum");
				Util.FindParentToChild(ref _uiShipNum, _objDisplayInfo.transform, "ShipNum");
				Util.FindParentToChild(ref _uiCheatsArea, _objDisplayInfo.transform, "CheatsArea");
				Util.FindParentToChild(ref _uiArea, _objDisplayInfo.transform, "Area");
				Util.FindParentToChild(ref _uiTurn, _objDisplayInfo.transform, "Turn");
				Util.FindParentToChild(ref _uiDifficlty, _objDisplayInfo.transform, "Difficlty");
				Util.FindParentToChild(ref _uiTactics, _objDisplayInfo.transform, "Tactics");
			}

			public void SetDisplayData()
			{
				_uiName.text = string.Format("{0}", "だみはらだみこだみはらだ");
				_uiLv.text = $"司令部LV：{999}";
				_uiFleetNum.text = $"保有艦隊数：{999}";
				_uiShipNum.text = $"艦娘保有数：{999}";
				_uiCheatsArea.text = $"攻略海域数：{999}";
				_uiArea.text = string.Format("現\u3000在\u3000地：{0}", "鎮守府海域");
				_uiTurn.text = string.Format("現在ターン：{0}", "零の年 睦月17日");
				_uiTactics.text = string.Format("遂行中作戦：{0}", "第17海域あ号作戦");
				_uiDifficlty.text = string.Format("難\u3000易\u3000度：{0}", "ナイトメアモード");
			}

			public void Release()
			{
				_objDisplayInfo = null;
				_uiBackground = null;
				_uiName = null;
				_uiLv = null;
				_uiFleetNum = null;
				_uiShipNum = null;
				_uiCheatsArea = null;
				_uiArea = null;
				_uiDifficlty = null;
				_uiTactics = null;
			}
		}

		private UITexture _uiThumb;

		private DisplayInfo _uiDisplayInfo;

		private void Awake()
		{
			Util.FindParentToChild(ref _uiThumb, base.transform, "Thumbnail/Thumb");
			_uiDisplayInfo = new DisplayInfo(base.transform, "DisplayInfo");
			_uiDisplayInfo.SetDisplayData();
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void OnDisable()
		{
			_uiThumb = null;
			_uiDisplayInfo = null;
		}

		public void SetSaveData()
		{
			_uiDisplayInfo.SetDisplayData();
		}
	}
}
