using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using local.models;
using Server_Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Checker
{
	public class BattleCutInCheck : MonoBehaviour
	{
		public ProdTranscendenceCutIn prodTranscendenceCutIn;

		public int mstId = 1;

		private ShipModelMst _clsCurrentShipMst;

		public bool isDamaged;

		public BattleAttackKind battleAttackKind;

		public Vector3 offset = Vector3.zero;

		[Button("SwitchIsDamaged", "SwitchIsDamaged", new object[]
		{

		})]
		public int Damaged;

		private Dictionary<int, Mst_ship> _dicMstShip;

		private int MIN_SHIP_ID => (from order in Mst_DataManager.Instance.Mst_ship
									orderby order.Value.Id
									select order).First().Value.Id;

		private int MAX_SHIP_ID => Mst_DataManager.Instance.Mst_ship.Max((KeyValuePair<int, Mst_ship> order) => order.Value.Id);

		private void Start()
		{
			_dicMstShip = Mst_DataManager.Instance.Mst_ship;
			mstId = MIN_SHIP_ID;
			_clsCurrentShipMst = new ShipModelMst(mstId);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				PlayTranscendenceCutIn();
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				PlayOffsetPlayTranscendenceCutIn();
			}
			if (Input.GetKeyDown(KeyCode.F))
			{
				SwitchIsDamaged();
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				SubMstID();
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				AddMstID();
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				Sub10MstID();
			}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				Add10MstID();
			}
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				SubBattleAttackKind();
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				AddBattleAttackKind();
			}
			if (Input.GetKeyDown(KeyCode.A))
			{
				SubOffSetX();
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				AddOffSetX();
			}
			if (Input.GetKeyDown(KeyCode.S))
			{
				SubOffSetY();
			}
			if (Input.GetKeyDown(KeyCode.W))
			{
				AddOffSetY();
			}
			if (Input.GetKey(KeyCode.J))
			{
				SubOffSetX();
			}
			if (Input.GetKey(KeyCode.L))
			{
				AddOffSetX();
			}
			if (Input.GetKey(KeyCode.K))
			{
				SubOffSetY();
			}
			if (Input.GetKey(KeyCode.I))
			{
				AddOffSetY();
			}
			if (Input.GetKeyDown(KeyCode.Z))
			{
				SubOffSetZ();
			}
			if (Input.GetKeyDown(KeyCode.X))
			{
				AddOffSetZ();
			}
			if (Input.GetKeyDown(KeyCode.R))
			{
				offset = Vector3.zero;
			}
			if (Input.GetKeyDown(KeyCode.T))
			{
				GetOffs();
			}
			if (Input.GetKeyDown(KeyCode.V))
			{
				OffsetCopy();
			}
		}

		private void OnGUI()
		{
			GUILayout.BeginArea(new Rect(Screen.width / 2, Screen.height / 2, Screen.width / 2, Screen.height / 2));
			GUILayout.BeginVertical("box", new GUILayoutOption[0]);
			if (GUILayout.Button("Play(Enter)", new GUILayoutOption[1]
			{
				GUILayout.MinWidth(200f)
			}))
			{
				PlayTranscendenceCutIn();
			}
			if (GUILayout.Button("PlayOffs(Space)", new GUILayoutOption[1]
			{
				GUILayout.MinWidth(200f)
			}))
			{
				PlayOffsetPlayTranscendenceCutIn();
			}
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			if (GUILayout.Button("-(1)", new GUILayoutOption[1]
			{
				GUILayout.MinWidth(20f)
			}))
			{
				SubBattleAttackKind();
			}
			if (GUILayout.Button("+(2)", new GUILayoutOption[1]
			{
				GUILayout.MinWidth(20f)
			}))
			{
				AddBattleAttackKind();
			}
			GUILayout.Label($"{battleAttackKind}", new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			if (GUILayout.Button("-(←)", new GUILayoutOption[1]
			{
				GUILayout.MinWidth(20f)
			}))
			{
				SubMstID();
			}
			if (GUILayout.Button("+(→)", new GUILayoutOption[1]
			{
				GUILayout.MinWidth(20f)
			}))
			{
				AddMstID();
			}
			GUILayout.Label(string.Format("[{1}]{0}", mstId, (!_dicMstShip.ContainsKey(mstId)) ? string.Empty : _dicMstShip[mstId].Name), new GUILayoutOption[0]);
			GUILayout.EndHorizontal();


			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			int num = Mst_DataManager.Instance.Mst_ship_resources.ContainsKey(mstId) ? Mst_DataManager.Instance.Mst_ship_resources[mstId].Standing_id : 0;
			if (num > 500)
			{
			}
			GUILayout.Label($"ID:{mstId} StandingID:{num}", new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			int num2 = Mst_DataManager.Instance.Mst_ship_resources.ContainsKey(mstId) ? Mst_DataManager.Instance.Mst_ship_resources[mstId].Standing_id : 0;
			bool isFriend = (num2 <= 500) ? true : false;
			try
			{
				Vector3 shipOffsPos = ShipUtils.GetShipOffsPos(num2, isFriend, isDamaged: false, MstShipGraphColumn.CutIn);
				Vector3 shipOffsPos2 = ShipUtils.GetShipOffsPos(num2, isFriend, isDamaged: true, MstShipGraphColumn.CutIn);
				GUILayout.Label($"[主主主左上]\n通常:{shipOffsPos}\nダメージ:{shipOffsPos2}", new GUILayoutOption[0]);
				Vector3 shipOffsPos3 = ShipUtils.GetShipOffsPos(num2, isFriend, isDamaged: false, MstShipGraphColumn.CutInSp1);
				Vector3 shipOffsPos4 = ShipUtils.GetShipOffsPos(num2, isFriend, isDamaged: true, MstShipGraphColumn.CutInSp1);
				GUILayout.Label($"[雷雷中心]\n通常:{shipOffsPos3}\nダメージ:{shipOffsPos4}", new GUILayoutOption[0]);
			}
			catch
			{
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			GUILayout.Label($"可変オフセット:{offset}", new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			if (GUILayout.Button(string.Format("ダメージ状態:{0}(D)", (!isDamaged) ? "通常" : "ダメージ"), new GUILayoutOption[0]))
			{
				SwitchIsDamaged();
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}

		private void GetOffs()
		{
			offset = ShipUtils.GetShipOffsPos(mstId, mstId < 500, isDamaged, MstShipGraphColumn.CutIn);
		}

		private void PlayTranscendenceCutIn()
		{
		}

		private void PlayOffsetPlayTranscendenceCutIn()
		{
		}

		private void SwitchIsDamaged()
		{
			isDamaged = !isDamaged;
		}

		private void AddMstID()
		{
			mstId++;
			mstId = Mathe.MinMax2(mstId, MIN_SHIP_ID, MAX_SHIP_ID);
			PlayOffsetPlayTranscendenceCutIn();
		}

		private void Add10MstID()
		{
			mstId += 10;
			mstId = Mathe.MinMax2(mstId, MIN_SHIP_ID, MAX_SHIP_ID);
			PlayOffsetPlayTranscendenceCutIn();
		}

		private void SubMstID()
		{
			mstId--;
			mstId = Mathe.MinMax2(mstId, MIN_SHIP_ID, MAX_SHIP_ID);
			PlayOffsetPlayTranscendenceCutIn();
		}

		private void Sub10MstID()
		{
			mstId -= 10;
			mstId = Mathe.MinMax2(mstId, MIN_SHIP_ID, MAX_SHIP_ID);
			PlayOffsetPlayTranscendenceCutIn();
		}

		private void AddBattleAttackKind()
		{
			battleAttackKind++;
		}

		private void SubBattleAttackKind()
		{
			battleAttackKind--;
		}

		private void AddOffSetX()
		{
			offset.x += 1f;
			PlayOffsetPlayTranscendenceCutIn();
		}

		private void SubOffSetX()
		{
			offset.x -= 1f;
			PlayOffsetPlayTranscendenceCutIn();
		}

		private void AddOffSetY()
		{
			offset.y += 1f;
			PlayOffsetPlayTranscendenceCutIn();
		}

		private void SubOffSetY()
		{
			offset.y -= 1f;
			PlayOffsetPlayTranscendenceCutIn();
		}

		private void AddOffSetZ()
		{
			offset.z += 1f;
		}

		private void SubOffSetZ()
		{
			offset.z -= 1f;
		}

		private void OffsetCopy()
		{
		}
	}
}
