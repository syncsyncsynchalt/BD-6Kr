using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIRepairShip : MonoBehaviour
	{
		[Serializable]
		private class Lines
		{
			private Transform _traLines;

			private UISprite _uiBG;

			private UILabel _uiLabel;

			public Lines(Transform parent, string objName)
			{
				Util.FindParentToChild(ref _traLines, parent, objName);
				Util.FindParentToChild(ref _uiBG, _traLines, "BG");
				Util.FindParentToChild(ref _uiLabel, _traLines, "Label");
				_uiLabel.text = string.Empty;
			}

			public void SetLines(string lines)
			{
				_uiLabel.text = lines;
			}
		}

		public static readonly string LINES_WELCOME = "[000000]\u3000提督、明石の工廠へようこそ！\n\u3000どの装備の改修を試みますか？[-]";

		public static readonly string LINES_CON_REVAMPSLOTITEM = "[" + Generics.BBCodeColor.kGreen + "]\u3000{0}[-]\n\u3000[000000]を、改修しますね！[-]";

		public static readonly string LINES_SUCCESS = "[000000]\u3000改修に成功しました。\n";

		public static readonly string LINES_FAILURE = "[000000]\u3000改修に失敗しました。\n";

		public static readonly string LINES_LIST = "[000000]\u3000一覧から選択してください。\n";

		private UITexture _uiShip;

		private UITexture _uiEyes;

		private Lines _uiLines;

		private void Awake()
		{
			Util.FindParentToChild(ref _uiShip, base.transform, "RepairShip/Ship");
			Util.FindParentToChild(ref _uiEyes, base.transform, "RepairShip/Eye");
			_uiLines = new Lines(base.transform, "Lines");
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void OnDestroy()
		{
			_uiShip = null;
			_uiEyes = null;
		}

		public void SetRepairShip(ShipModel model)
		{
			Debug.Log($"[ID:{model.MstId}]{model.Name}");
		}

		public void SetLines(string lines)
		{
			_uiLines.SetLines(lines);
		}

		public void SetLines(RevampValidationResult iResult, RevampRecipeDetailModel model)
		{
			if (model != null)
			{
				string str = "[000000]";
				switch (iResult)
				{
				case RevampValidationResult.Max_Level:
					str += $"[FF0000]\u3000現在、選択された装備[-]\n";
					str += $"[1DBDC0]\u3000{model.Slotitem.Name}[-]\n";
					str += $"[FF0000]\u3000は、これ以上の改修ができません。[-]";
					break;
				case RevampValidationResult.Lock:
					str += $"[1DBDC0]\u3000{model.Slotitem.Name}[-]\nを改修しますね！\n\n";
					str += $"[FF0000]\u3000この装備を改修するには\n\u3000同装備のロック解除が必要です。[-]";
					break;
				case RevampValidationResult.Less_Slotitem_No_Lock:
					str += $"[1DBDC0]\u3000{model.Slotitem.Name}[-]\nを改修しますね！\n\n";
					str += $"[FF0000]\u3000この改修に必要となる\n(無改修)\n[-]";
					str += $"[1DBDC0]\u3000{model.Slotitem.Name}x{model.RequiredSlotitemCount}[-]";
					str += $"[FF0000]\u3000が足りません。[-]";
					break;
				case RevampValidationResult.Less_Slotitem:
					str += $"[1DBDC0]\u3000{model.Slotitem.Name}[-]\nを改修しますね！\n\n";
					str += $"[FF0000]\u3000この改修に必要となる\n(無改修)\n[-]";
					str += $"[1DBDC0]\u3000{model.Slotitem.Name}×{model.RequiredSlotitemCount}[-]";
					str += $"[FF0000]\u3000が足りません。[-]";
					break;
				case RevampValidationResult.OK:
					str += $"[1DBDC0]\u3000{model.Slotitem.Name}[-]\nを改修しますね！[-]\n\n";
					str += $"[000000]\u3000この改修には、無改修の\n[1DBDC0]{model.Slotitem.Name}×{model.RequiredSlotitemCount}[-]\nが必要です。[-]";
					str += $"[000000]\u3000(※改修で消費します)[-]";
					break;
				case RevampValidationResult.Less_Fuel:
				case RevampValidationResult.Less_Ammo:
				case RevampValidationResult.Less_Steel:
				case RevampValidationResult.Less_Baux:
				case RevampValidationResult.Less_Devkit:
				case RevampValidationResult.Less_Revkit:
					str += $"[FF0000]\u3000資材が足りません。";
					break;
				}
				str += "[-]";
				_uiLines.SetLines(str);
			}
		}
	}
}
