using KCV.Battle.Utils;
using KCV.BattleCut;
using KCV.Utils;
using local.models;
using Server_Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdEscortShipEvacuation : BaseAnimation
	{
		public enum HexType
		{
			None = -1,
			Back,
			Next
		}

		[SerializeField]
		private UIPanel _uiPanel;

		[SerializeField]
		private UITexture[] _uiShip;

		[SerializeField]
		private List<UIHexButton> _listHexBtns;

		[SerializeField]
		private UILabel _uiLabel1;

		[SerializeField]
		private UILabel _uiLabel2;

		[SerializeField]
		private ParticleSystem _uiSmoke;

		private int _debugIndex;

		private int _btnIndex;

		private bool _isDecide;

		private bool _isInputPossible;

		private bool _isBattleCut;

		private List<bool> _listIsBtn;

		private KeyControl _clsInput;

		private ShipModel[] _shipModels;

		private DelDecideAdvancingWithdrawalButton _delDecideAdvancingWithdrawalButton;

		public static ProdEscortShipEvacuation Instantiate(ProdEscortShipEvacuation prefab, Transform parent, KeyControl input, ShipModel[] escapeCandidate, bool isBattleCut)
		{
			ProdEscortShipEvacuation prodEscortShipEvacuation = UnityEngine.Object.Instantiate(prefab);
			prodEscortShipEvacuation.transform.parent = parent;
			prodEscortShipEvacuation.transform.localScaleOne();
			prodEscortShipEvacuation.transform.localPositionZero();
			prodEscortShipEvacuation._clsInput = input;
			prodEscortShipEvacuation._shipModels = escapeCandidate;
			prodEscortShipEvacuation._isBattleCut = isBattleCut;
			return prodEscortShipEvacuation;
		}

		private new void OnDestroy()
		{
			Mem.Del(ref _uiPanel);
			Mem.DelArySafe(ref _uiShip);
			Mem.Del(ref _uiLabel1);
			Mem.Del(ref _uiLabel2);
			Mem.Del(ref _uiSmoke);
			Mem.DelListSafe(ref _listHexBtns);
			Mem.DelListSafe(ref _listIsBtn);
			Mem.Del(ref _clsInput);
			Mem.DelArySafe(ref _shipModels);
			Mem.Del(ref _delDecideAdvancingWithdrawalButton);
		}

		public new void Init()
		{
			_debugIndex = 0;
			_btnIndex = 0;
			_isDecide = false;
			_isInputPossible = false;
			if (_uiPanel == null)
			{
				_uiPanel = GetComponent<UIPanel>();
			}
			Util.FindParentToChild(ref _uiLabel1, base.transform, "Label1");
			Util.FindParentToChild(ref _uiLabel2, base.transform, "Label2");
			Util.FindParentToChild<ParticleSystem>(ref _uiSmoke, base.transform, "Smoke");
			_uiPanel.depth = 70;
			((Component)_uiSmoke).SetActive(isActive: false);
			_uiShip = new UITexture[2];
			for (int i = 0; i < 2; i++)
			{
				Util.FindParentToChild(ref _uiShip[i], base.transform, "ShipObj" + (i + 1) + "/Ship");
			}
			_listHexBtns = new List<UIHexButton>();
			_listIsBtn = new List<bool>();
			foreach (int value in Enum.GetValues(typeof(HexType)))
			{
				if (value != -1)
				{
					_listIsBtn.Add(item: false);
					_listHexBtns.Add(((Component)base.transform.FindChild($"{((HexType)value).ToString()}Btn")).GetComponent<UIHexButton>());
					_listHexBtns[value].Init();
					_listHexBtns[value].SetIndex(value);
					_listHexBtns[value].uiButton.onClick = Util.CreateEventDelegateList(this, "DecideAdvancingWithDrawalBtn", _listHexBtns[value]);
					_listHexBtns[value].isColliderEnabled = true;
				}
			}
		}

		public void Play(DelDecideAdvancingWithdrawalButton decideCallback)
		{
			_delDecideAdvancingWithdrawalButton = decideCallback;
			base.Init();
			_setShipTexture();
			_setLabel();
			_btnIndex = 0;
			_listIsBtn[0] = true;
			((Component)_uiSmoke).SetActive(isActive: true);
			_uiSmoke.Play();
			_listHexBtns.ForEach(delegate(UIHexButton x)
			{
				x.SetActive(isActive: true);
				x.Play(UIHexButton.AnimationList.HexButtonShow, delegate
				{
					if (_isBattleCut)
					{
						UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
						navigation.SetNavigationInEscortShipEvacuation();
						navigation.Show(0.2f, null);
					}
					else
					{
						UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
						battleNavigation.SetNavigationInEscortShipEvacuation();
						battleNavigation.Show(0.2f, null);
					}
					_isInputPossible = true;
					SetAdvancingWithdrawalBtnState(_btnIndex);
				});
			});
		}

		public bool Run()
		{
			if (!_isDecide && _isInputPossible)
			{
				foreach (UIHexButton listHexBtn in _listHexBtns)
				{
					listHexBtn.Run();
				}
				if (_clsInput.keyState[14].down)
				{
					_btnIndex = _setButtonIndex(isUp: true);
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					SetAdvancingWithdrawalBtnState(_btnIndex);
				}
				else if (_clsInput.keyState[10].down)
				{
					_btnIndex = _setButtonIndex(isUp: false);
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					SetAdvancingWithdrawalBtnState(_btnIndex);
				}
				else if (_clsInput.GetDown(KeyControl.KeyName.MARU))
				{
					DecideAdvancingWithDrawalBtn(_listHexBtns[_btnIndex]);
				}
			}
			return !_isDecide;
		}

		private void _setShipTexture()
		{
			for (int i = 0; i < 2; i++)
			{
				int num = (i == 0) ? 1 : 0;
				bool flag = (i == 0) ? true : false;
				if (_shipModels[num] != null)
				{
					_uiShip[i].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(_shipModels[num].GetGraphicsMstId(), flag);
					_uiShip[i].MakePixelPerfect();
					_uiShip[i].transform.localPosition = Util.Poi2Vec(new ShipOffset(_shipModels[num].GetGraphicsMstId()).GetShipDisplayCenter(flag));
				}
			}
		}

		private void _setLabel()
		{
			if (_shipModels[0] != null && _shipModels[1] != null)
			{
				_uiLabel1.text = _shipModels[1].Name + " Lv" + _shipModels[1].Level + "が大きく損傷しています。";
				_uiLabel2.text = "随伴艦の" + _shipModels[0].Name + "を護衛につけて戦場から退避させますか？";
				_uiLabel1.MakePixelPerfect();
				_uiLabel2.MakePixelPerfect();
			}
		}

		private void _debugShipTexture()
		{
			if (Mst_DataManager.Instance.Mst_shipgraph.ContainsKey(_debugIndex))
			{
				ShipModelMst shipModelMst = new ShipModelMst(_debugIndex);
				_uiShip[0].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(_debugIndex, isDamaged: true);
				_uiShip[0].MakePixelPerfect();
				_uiShip[0].transform.localPosition = Util.Poi2Vec(new ShipOffset(_debugIndex).GetShipDisplayCenter(damaged: true));
				_uiShip[1].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(_debugIndex, isDamaged: false);
				_uiShip[1].MakePixelPerfect();
				_uiShip[1].transform.localPosition = Util.Poi2Vec(new ShipOffset(_debugIndex).GetShipDisplayCenter(damaged: false));
				_uiLabel1.text = shipModelMst.Name + " Lv" + 100 + "が大きく損傷しています。";
				_uiLabel2.text = "随伴艦の" + shipModelMst.Name + "を護衛につけて戦場から退避させますか？";
				_uiLabel1.MakePixelPerfect();
				_uiLabel2.MakePixelPerfect();
			}
		}

		private int _setButtonIndex(bool isUp)
		{
			return (!isUp) ? 1 : 0;
		}

		private void setTextEnabled()
		{
			UISprite component = ((Component)_listHexBtns[0].transform.FindChild("Label/Text")).GetComponent<UISprite>();
			component.spriteName = ((!_listHexBtns[0].isFocus) ? "txt_shelter_off" : "txt_shelter_on");
			UISprite component2 = ((Component)_listHexBtns[1].transform.FindChild("Label/Text")).GetComponent<UISprite>();
			component2.spriteName = ((!_listHexBtns[1].isFocus) ? "txt_continue_off" : "txt_continue_on");
		}

		private void SetAdvancingWithdrawalBtnState(int nIndex)
		{
			_listHexBtns.ForEach(delegate(UIHexButton x)
			{
				if (x.index == nIndex)
				{
					x.isFocus = true;
				}
				else
				{
					x.isFocus = false;
				}
				x.SetFocusAnimation();
			});
			setTextEnabled();
		}

		private void DecideAdvancingWithDrawalBtn(UIHexButton btn)
		{
			if (!_isDecide)
			{
				_isDecide = true;
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_036);
				_listHexBtns.ForEach(delegate(UIHexButton x)
				{
					x.isColliderEnabled = false;
				});
				SetAdvancingWithdrawalBtnState(btn.index);
				if (_isBattleCut)
				{
					UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
					navigation.Hide(0.2f, null);
				}
				else
				{
					UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
					battleNavigation.Hide(0.2f, null);
				}
				if (_delDecideAdvancingWithdrawalButton != null)
				{
					_delDecideAdvancingWithdrawalButton(btn);
				}
			}
		}
	}
}
