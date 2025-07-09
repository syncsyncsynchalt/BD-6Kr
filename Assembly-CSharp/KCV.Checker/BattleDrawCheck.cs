using KCV.Battle;
using local.models;
using Server_Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Checker
{
	public class BattleDrawCheck : MonoBehaviour
	{
		public enum POG
		{
			注視点,
			特殊注視点,
			演習注視点
		}

		public enum Mode
		{
			ビュワー,
			編集
		}

		private ShipModelMst _clsCurrentShipMst;

		public UIBattleShip uiBattleShip;

		public BattleFieldCamera fieldCamera;

		public GameObject latticePattern;

		public Transform pointOfGazeObj;

		public int shipID;

		public bool isDamaged;

		public bool isInformationOpen = true;

		public POG pog;

		public Mode mode;

		private int MIN_SHIP_ID => (from order in Mst_DataManager.Instance.Mst_ship
									orderby order.Value.Id
									select order).First().Value.Id;

		private int MAX_SHIP_ID => Mst_DataManager.Instance.Mst_ship.Max((KeyValuePair<int, Mst_ship> order) => order.Value.Id);

		private void Awake()
		{
			BattleField componentInChildren = base.transform.GetComponentInChildren<BattleField>();
			if (uiBattleShip != null)
			{
				uiBattleShip = Util.Instantiate(uiBattleShip.gameObject, componentInChildren.gameObject).GetComponent<UIBattleShip>();
				uiBattleShip.transform.position = Vector3.zero;
				uiBattleShip.billboard.billboardTarget = fieldCamera.transform;
			}
			if (fieldCamera != null)
			{
				fieldCamera.cullingMask = GetDefaultLayers();
			}
		}

		private void Start()
		{
			shipID = MIN_SHIP_ID;
			_clsCurrentShipMst = new ShipModelMst(shipID);
			Debug.Log(string.Empty + MIN_SHIP_ID + "|" + MAX_SHIP_ID);
			setShipID(shipID);
		}

		private void Update()
		{
			switch (mode)
			{
				case Mode.ビュワー:
					if (Input.GetKeyDown(KeyCode.LeftArrow))
					{
						setShipID(shipID - 1);
					}
					else if (Input.GetKeyDown(KeyCode.RightArrow))
					{
						setShipID(shipID + 1);
					}
					else if (Input.GetKeyDown(KeyCode.UpArrow))
					{
						setShipID(shipID + 10);
					}
					else if (Input.GetKeyDown(KeyCode.DownArrow))
					{
						setShipID(shipID - 10);
					}
					break;
				case Mode.編集:
					if (Input.GetKeyDown(KeyCode.R))
					{
						UpdateShip();
						focusCamera();
					}
					switch (pog)
					{
						case POG.注視点:
							if (Input.GetKeyDown(KeyCode.LeftArrow))
							{
								setShipLocalPointOfGaze(Vector3.left);
							}
							else if (Input.GetKeyDown(KeyCode.RightArrow))
							{
								setShipLocalPointOfGaze(Vector3.right);
							}
							else if (Input.GetKeyDown(KeyCode.UpArrow))
							{
								setShipLocalPointOfGaze(Vector3.up);
							}
							else if (Input.GetKeyDown(KeyCode.DownArrow))
							{
								setShipLocalPointOfGaze(Vector3.down);
							}
							if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftArrow))
							{
								setShipLocalPointOfGaze(Vector3.left * 10f);
							}
							if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightArrow))
							{
								setShipLocalPointOfGaze(Vector3.right * 10f);
							}
							if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.UpArrow))
							{
								setShipLocalPointOfGaze(Vector3.up * 10f);
							}
							if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.DownArrow))
							{
								setShipLocalPointOfGaze(Vector3.down * 10f);
							}
							break;
						case POG.特殊注視点:
							if (Input.GetKeyDown(KeyCode.LeftArrow))
							{
								setShipLocalSPPointOfGaze(Vector3.left);
							}
							else if (Input.GetKeyDown(KeyCode.RightArrow))
							{
								setShipLocalSPPointOfGaze(Vector3.right);
							}
							else if (Input.GetKeyDown(KeyCode.UpArrow))
							{
								setShipLocalSPPointOfGaze(Vector3.up);
							}
							else if (Input.GetKeyDown(KeyCode.DownArrow))
							{
								setShipLocalSPPointOfGaze(Vector3.down);
							}
							if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftArrow))
							{
								setShipLocalSPPointOfGaze(Vector3.left * 10f);
							}
							if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightArrow))
							{
								setShipLocalSPPointOfGaze(Vector3.right * 10f);
							}
							if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.UpArrow))
							{
								setShipLocalSPPointOfGaze(Vector3.up * 10f);
							}
							if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.DownArrow))
							{
								setShipLocalSPPointOfGaze(Vector3.down * 10f);
							}
							break;
						case POG.演習注視点:
							if (Input.GetKeyDown(KeyCode.LeftArrow))
							{
								setShipLocalSPPointOfGaze(Vector3.left);
							}
							else if (Input.GetKeyDown(KeyCode.RightArrow))
							{
								setShipLocalSPPointOfGaze(Vector3.right);
							}
							else if (Input.GetKeyDown(KeyCode.UpArrow))
							{
								setShipLocalSPPointOfGaze(Vector3.up);
							}
							else if (Input.GetKeyDown(KeyCode.DownArrow))
							{
								setShipLocalSPPointOfGaze(Vector3.down);
							}
							if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftArrow))
							{
								setShipLocalSPPointOfGaze(Vector3.left * 10f);
							}
							if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightArrow))
							{
								setShipLocalSPPointOfGaze(Vector3.right * 10f);
							}
							if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.UpArrow))
							{
								setShipLocalSPPointOfGaze(Vector3.up * 10f);
							}
							if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.DownArrow))
							{
								setShipLocalSPPointOfGaze(Vector3.down * 10f);
							}
							break;
					}
					break;
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				changeDamage();
			}
			if (Input.GetKeyDown(KeyCode.F))
			{
				focusCamera();
			}
			if (Input.GetKeyDown(KeyCode.P))
			{
				changePog();
			}
			if (Input.GetKeyDown(KeyCode.E))
			{
				changeMode();
			}
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				setShipID(1);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				setShipID(501);
			}
			if (Input.GetKeyDown(KeyCode.O))
			{
				isInformationOpen = !isInformationOpen;
			}
			if (Input.GetKeyDown(KeyCode.A))
			{
				latticePatternActive();
			}
		}

		private void FixedUpdate()
		{
			if (pointOfGazeObj.position != fieldCamera.pointOfGaze)
			{
				pointOfGazeObj.position = fieldCamera.pointOfGaze;
			}
		}

		private void UpdateShip()
		{
			if (_clsCurrentShipMst != null)
			{
				UpdateShipTexture();
				UpdateShipFootOffs();
				UpdateShipPog();
				UpdateShipSPPog();
				UpdateShipScaleMag();
			}
			else
			{
				uiBattleShip.object3D.mainTexture = null;
			}
		}

		private void UpdateShipTexture()
		{
			int texNum = (!isDamaged) ? 9 : 10;
			uiBattleShip.object3D.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(_clsCurrentShipMst.GetGraphicsMstId(), texNum);
			uiBattleShip.object3D.MakePixelPerfect();
		}

		private void UpdateShipFootOffs()
		{
			Vector3 vector;
			try
			{
				vector = Util.Poi2Vec(_clsCurrentShipMst.Offsets.GetFoot_InBattle(isDamaged));
				Debug.Log($"足元位置:{vector}");
			}
			catch
			{
				vector = Vector3.zero;
			}
			uiBattleShip.object3D.transform.localPosition = vector;
		}

		private void UpdateShipPog()
		{
			Vector3 vector;
			try
			{
				vector = Util.Poi2Vec(_clsCurrentShipMst.Offsets.GetPog_InBattle(isDamaged));
				Debug.Log($"注視点:{vector}");
			}
			catch
			{
				vector = Vector3.zero;
			}
			Transform transform = uiBattleShip.transform.FindChild("POG");
			transform.transform.localPosition = vector;
		}

		private void UpdateShipSPPog()
		{
			Vector3 vector;
			try
			{
				vector = ((pog != POG.特殊注視点) ? Util.Poi2Vec(_clsCurrentShipMst.Offsets.GetPogSpEnsyu_InBattle(isDamaged)) : Util.Poi2Vec(_clsCurrentShipMst.Offsets.GetPogSp_InBattle(isDamaged)));
				Debug.Log($"特殊注視点:{vector} - {pog}");
			}
			catch
			{
				vector = Vector3.zero;
			}
			Transform transform = uiBattleShip.transform.FindChild("SPPog");
			transform.transform.localPosition = vector;
		}

		private void UpdateShipScaleMag()
		{
			float d;
			try
			{
				d = (float)Mst_DataManager.Instance.Mst_shipgraphbattle[_clsCurrentShipMst.GetGraphicsMstId()].Scale_mag;
			}
			catch
			{
				d = 1f;
			}
			Transform transform = uiBattleShip.transform.Find("ShipAnchor");
			transform.transform.localScale = Vector3.one * d;
		}

		private void changePog()
		{
			if (pog == POG.注視点)
			{
				pog = POG.特殊注視点;
			}
			else if (pog == POG.特殊注視点)
			{
				pog = POG.演習注視点;
			}
			else
			{
				pog = POG.注視点;
			}
			UpdateShip();
			focusCamera();
		}

		private void changeMode()
		{
			if (mode == Mode.ビュワー)
			{
				mode = Mode.編集;
			}
			else
			{
				mode = Mode.ビュワー;
			}
		}

		private void changeDamage()
		{
			isDamaged = !isDamaged;
			UpdateShip();
			focusCamera();
		}

		private void focusCamera()
		{
			if (!(fieldCamera != null))
			{
				return;
			}
			for (int i = 0; i < 3; i++)
			{
				switch (pog)
				{
					case POG.注視点:
						fieldCamera.transform.position = calcCamTargetPosToPog();
						fieldCamera.LookAt(uiBattleShip.pointOfGaze);
						break;
					case POG.特殊注視点:
						fieldCamera.transform.position = calcCamTargetPosToSPPog();
						fieldCamera.LookAt(uiBattleShip.spPointOfGaze);
						break;
					case POG.演習注視点:
						fieldCamera.transform.position = calcCamTargetPosToSPPog();
						fieldCamera.LookAt(uiBattleShip.spPointOfGaze);
						break;
				}
			}
		}

		public Generics.Layers GetDefaultLayers()
		{
			return Generics.Layers.TransparentFX | Generics.Layers.Water | Generics.Layers.Background | Generics.Layers.ShipGirl | Generics.Layers.Effects;
		}

		private Vector3 calcCamTargetPosToPog()
		{
			Vector3 vector = Mathe.NormalizeDirection(uiBattleShip.pointOfGaze, fieldCamera.eyePosition) * 10f;
			Vector3 pointOfGaze = uiBattleShip.pointOfGaze;
			float x = pointOfGaze.x + vector.x;
			Vector3 pointOfGaze2 = uiBattleShip.pointOfGaze;
			float y = pointOfGaze2.y;
			Vector3 pointOfGaze3 = uiBattleShip.pointOfGaze;
			return new Vector3(x, y, pointOfGaze3.z + vector.z);
		}

		private Vector3 calcCamTargetPosToSPPog()
		{
			Vector3 vector = Mathe.NormalizeDirection(uiBattleShip.spPointOfGaze, fieldCamera.eyePosition) * 10f;
			Vector3 spPointOfGaze = uiBattleShip.spPointOfGaze;
			float x = spPointOfGaze.x + vector.x;
			Vector3 spPointOfGaze2 = uiBattleShip.spPointOfGaze;
			float y = spPointOfGaze2.y;
			Vector3 spPointOfGaze3 = uiBattleShip.spPointOfGaze;
			return new Vector3(x, y, spPointOfGaze3.z + vector.z);
		}

		private void setShipID(int id)
		{
			shipID = Mathe.MinMax2(id, MIN_SHIP_ID, MAX_SHIP_ID);
			try
			{
				_clsCurrentShipMst = new ShipModelMst(shipID);
			}
			catch
			{
			}
			UpdateShip();
			focusCamera();
		}

		private void setShipLocalPointOfGaze(Vector3 dir)
		{
			uiBattleShip.localPointOfGaze += dir;
			focusCamera();
		}

		private void setShipLocalSPPointOfGaze(Vector3 dir)
		{
			uiBattleShip.localSPPointOfGaze += dir;
			focusCamera();
		}

		private void positionCopy()
		{
			switch (pog)
			{
				case POG.注視点:
					Debug.Log(string.Format("注視点[{1}]:{0}", uiBattleShip.localPointOfGaze, (!isDamaged) ? "通常" : "ダメージ"));
					break;
				case POG.特殊注視点:
					Debug.Log(string.Format("特殊注視点注視点[{1}]:{0}", uiBattleShip.localSPPointOfGaze, (!isDamaged) ? "通常" : "ダメージ"));
					break;
				case POG.演習注視点:
					Debug.Log(string.Format("演習特殊注視点[{1}]:{0}", uiBattleShip.localSPPointOfGaze, (!isDamaged) ? "通常" : "ダメージ"));
					break;
			}
		}

		private void footPositionCopy()
		{
			Debug.Log(string.Format("足元座標[{1}]:{0}", uiBattleShip.object3D.transform.localPosition, (!isDamaged) ? "通常" : "ダメージ"));
		}

		private Mst_ship getMstShip(int mstId)
		{
			if (Mst_DataManager.Instance.Mst_ship.ContainsKey(mstId))
			{
				return Mst_DataManager.Instance.Mst_ship[mstId];
			}
			return null;
		}

		private void latticePatternActive()
		{
			latticePattern.SetActive(!latticePattern.activeSelf);
		}

		private void OnGUI()
		{
			GUILayout.BeginVertical("box", new GUILayoutOption[0]);
			GUILayout.Label("[BattleDrawCheck Information]", new GUILayoutOption[0]);
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			GUILayout.Label($"モード:{mode}", new GUILayoutOption[0]);
			if (GUILayout.Button("モード変更(E)", new GUILayoutOption[0]))
			{
				changeMode();
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			GUILayout.Label("注視点状態:" + pog.ToString(), new GUILayoutOption[0]);
			if (GUILayout.Button("注視点変更(P)", new GUILayoutOption[0]))
			{
				changePog();
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			GUILayout.Label(string.Format("ダメージ状態:{0}", (!isDamaged) ? "通常" : "ダメージ"), new GUILayoutOption[0]);
			if (GUILayout.Button("ダメージ状態変更(D)", new GUILayoutOption[0]))
			{
				changeDamage();
			}
			GUILayout.EndHorizontal();
			GUILayout.Label("艦ID:" + shipID, new GUILayoutOption[0]);
			DrawCurrentInfo();
			DrawCurrentPog();
			isInformationOpen = GUILayout.Toggle(isInformationOpen, "Open Settings.(O)", new GUILayoutOption[0]);
			if (isInformationOpen)
			{
				GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
				switch (mode)
				{
					case Mode.ビュワー:
						GUILayout.Label($"艦ID", new GUILayoutOption[0]);
						if (GUILayout.Button("-1(←)", new GUILayoutOption[0]))
						{
							setShipID(shipID - 1);
						}
						else if (GUILayout.Button("+1(→)", new GUILayoutOption[0]))
						{
							setShipID(shipID + 1);
						}
						else if (GUILayout.Button("-10(↓)", new GUILayoutOption[0]))
						{
							setShipID(shipID - 10);
						}
						else if (GUILayout.Button("+10(↑)", new GUILayoutOption[0]))
						{
							setShipID(shipID + 10);
						}
						break;
					case Mode.編集:
						switch (pog)
						{
							case POG.注視点:
								GUILayout.BeginVertical(new GUILayoutOption[0]);
								GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
								if (GUILayout.Button("-1(←)", new GUILayoutOption[0]))
								{
									setShipLocalPointOfGaze(Vector3.left);
								}
								else if (GUILayout.Button("+1(→)", new GUILayoutOption[0]))
								{
									setShipLocalPointOfGaze(Vector3.right);
								}
								else if (GUILayout.Button("-1(↓)", new GUILayoutOption[0]))
								{
									setShipLocalPointOfGaze(Vector3.up);
								}
								else if (GUILayout.Button("+1(↑)", new GUILayoutOption[0]))
								{
									setShipLocalPointOfGaze(Vector3.down);
								}
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
								if (GUILayout.Button("-10(Sf+←)", new GUILayoutOption[0]))
								{
									setShipLocalPointOfGaze(Vector3.left * 10f);
								}
								if (GUILayout.Button("+10(Sf+→)", new GUILayoutOption[0]))
								{
									setShipLocalPointOfGaze(Vector3.right * 10f);
								}
								if (GUILayout.Button("-10(Sf+↓)", new GUILayoutOption[0]))
								{
									setShipLocalPointOfGaze(Vector3.down * 10f);
								}
								if (GUILayout.Button("+10(Sf+↑)", new GUILayoutOption[0]))
								{
									setShipLocalPointOfGaze(Vector3.up * 10f);
								}
								GUILayout.EndHorizontal();
								GUILayout.EndVertical();
								break;
							case POG.特殊注視点:
								GUILayout.BeginVertical(new GUILayoutOption[0]);
								GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
								if (GUILayout.Button("-1(←)", new GUILayoutOption[0]))
								{
									setShipLocalSPPointOfGaze(Vector3.left);
								}
								else if (GUILayout.Button("+1(→)", new GUILayoutOption[0]))
								{
									setShipLocalSPPointOfGaze(Vector3.right);
								}
								else if (GUILayout.Button("-1(↓)", new GUILayoutOption[0]))
								{
									setShipLocalSPPointOfGaze(Vector3.up);
								}
								else if (GUILayout.Button("+1(↑)", new GUILayoutOption[0]))
								{
									setShipLocalSPPointOfGaze(Vector3.down);
								}
								GUILayout.EndHorizontal();


								GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
								if (GUILayout.Button("-10(Sf+←)", new GUILayoutOption[0]))
								{
									setShipLocalSPPointOfGaze(Vector3.left * 10f);
								}
								if (GUILayout.Button("+10(Sf+→)", new GUILayoutOption[0]))
								{
									setShipLocalSPPointOfGaze(Vector3.right * 10f);
								}
								if (GUILayout.Button("-10(Sf+↓)", new GUILayoutOption[0]))
								{
									setShipLocalSPPointOfGaze(Vector3.down * 10f);
								}
								if (GUILayout.Button("+10(Sf+↑)", new GUILayoutOption[0]))
								{
									setShipLocalSPPointOfGaze(Vector3.up * 10f);
								}
								GUILayout.EndHorizontal();
								GUILayout.EndVertical();
								break;
						}
						break;
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
				GUILayout.Label($"味方/敵切り替え", new GUILayoutOption[0]);
				if (GUILayout.Button("味方艦(1)", new GUILayoutOption[0]))
				{
					setShipID(1);
				}
				else if (GUILayout.Button("敵艦(2)", new GUILayoutOption[0]))
				{
					setShipID(501);
				}
				GUILayout.EndHorizontal();
				if (GUILayout.Button($"フォーカス[{pog.ToString()}](F)", new GUILayoutOption[0]))
				{
					focusCamera();
				}
				if (GUILayout.Button($"格子表示切り替え(A)", new GUILayoutOption[0]))
				{
					latticePatternActive();
				}
				if (GUILayout.Button($"座標コピー[{pog.ToString()}](C)", new GUILayoutOption[0]))
				{
					positionCopy();
				}
			}
			GUILayout.EndVertical();
		}

		private void DrawCurrentInfo()
		{
			try
			{
				_clsCurrentShipMst = new ShipModelMst(shipID);
				GUILayout.Label($"[{_clsCurrentShipMst.Name}({_clsCurrentShipMst.Yomi})]艦ID:{_clsCurrentShipMst.MstId} GraphicID:{_clsCurrentShipMst.GetGraphicsMstId()}", new GUILayoutOption[0]);
			}
			catch
			{
			}
		}

		private void DrawCurrentPog()
		{
			try
			{
				GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
				GUILayout.Label(string.Format("[足元座標({0})]\n{1}", (!isDamaged) ? "通常" : "大破", Util.Poi2Vec(_clsCurrentShipMst.Offsets.GetFoot_InBattle(isDamaged))), new GUILayoutOption[0]);
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
				GUILayout.BeginVertical("box", new GUILayoutOption[0]);
				GUILayout.Label(string.Format("[注視点{0}]\nMst:{1}\n---Edit---\nW:{2}\nL:{3}", (!isDamaged) ? "通常" : "大破", Util.Poi2Vec(_clsCurrentShipMst.Offsets.GetPog_InBattle(isDamaged)), uiBattleShip.pointOfGaze, uiBattleShip.localPointOfGaze), new GUILayoutOption[0]);
				GUILayout.EndVertical();
				GUILayout.BeginVertical("box", new GUILayoutOption[0]);
				GUILayout.Label(string.Format("[特殊注視点{0}]\nMst:{1}\n---Edit---\nW:{2}\nL:{3}", (!isDamaged) ? "通常" : "大破", Util.Poi2Vec(_clsCurrentShipMst.Offsets.GetPogSp_InBattle(isDamaged)), uiBattleShip.spPointOfGaze, uiBattleShip.localSPPointOfGaze), new GUILayoutOption[0]);
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
			}
			catch
			{
			}
		}
	}
}
