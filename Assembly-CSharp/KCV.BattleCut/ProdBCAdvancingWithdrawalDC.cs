using Common.Enum;
using KCV.Battle.Utils;
using KCV.Generic;
using local.models;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.BattleCut
{
	public class ProdBCAdvancingWithdrawalDC : MonoBehaviour
	{
		[SerializeField]
		private List<Vector3> _listAWPos4Sortie;

		[SerializeField]
		private List<Vector3> _listAWPos4Rebellion;

		[SerializeField]
		private List<UILabelButton> _listLabelButton;

		[Button("SetLabelPos", "set lavbels position to sortiemap battle.", new object[]
		{
			Generics.BattleRootType.SortieMap
		})]
		[SerializeField]
		private int _nSetLabelPos2SortieMap;

		[SerializeField]
		[Button("SetLabelPos", "set lavbels position to sortiemap battle.", new object[]
		{
			Generics.BattleRootType.Rebellion
		})]
		private int _nSetLabelPos2Rebellion;

		private UIPanel _uiPanel;

		private ShipModel_BattleAll _clsShipModel;

		private AdvancingWithdrawalDCType _iSelectType;

		private bool _isInputPossible;

		private Action<AdvancingWithdrawalDCType, ShipRecoveryType> _actOnDecide;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static ProdBCAdvancingWithdrawalDC Instantiate(ProdBCAdvancingWithdrawalDC prefab, Transform parent, ShipModel_BattleAll flagShip, Generics.BattleRootType iRootType)
		{
			ProdBCAdvancingWithdrawalDC prodBCAdvancingWithdrawalDC = UnityEngine.Object.Instantiate(prefab);
			prodBCAdvancingWithdrawalDC.transform.parent = parent;
			prodBCAdvancingWithdrawalDC.transform.localScaleOne();
			prodBCAdvancingWithdrawalDC.transform.localPositionZero();
			prodBCAdvancingWithdrawalDC.Init(flagShip, iRootType);
			return prodBCAdvancingWithdrawalDC;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _clsShipModel);
			Mem.DelListSafe(ref _listLabelButton);
			Mem.Del(ref _iSelectType);
			Mem.Del(ref _isInputPossible);
			Mem.Del(ref _actOnDecide);
		}

		private bool Init(ShipModel_BattleAll flagShip, Generics.BattleRootType iRootType)
		{
			_clsShipModel = flagShip;
			panel.alpha = 0f;
			SetLabelPos(iRootType);
			_iSelectType = AdvancingWithdrawalDCType.Withdrawal;
			int cnt = 0;
			_listLabelButton.ForEach(delegate(UILabelButton x)
			{
				bool isValid = true;
				if (cnt == 2)
				{
					isValid = (flagShip.HasRecoverMegami() ? true : false);
				}
				else if (cnt == 1)
				{
					isValid = (flagShip.HasRecoverYouin() ? true : false);
				}
				else if (cnt == 3)
				{
					isValid = BattleCutManager.GetBattleManager().ChangeableDeck;
				}
				x.Init(cnt, isValid, KCVColor.ConvertColor(110f, 110f, 110f, 255f), KCVColor.ConvertColor(110f, 110f, 110f, 128f));
				x.isFocus = false;
				x.toggle.group = 20;
				x.toggle.enabled = false;
				x.toggle.onDecide = delegate
				{
					Decide();
				};
				x.toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", (AdvancingWithdrawalDCType)x.index);
				if (x.index == 0)
				{
					x.toggle.startsActive = true;
				}
				cnt++;
			});
			ChangeFocus(_iSelectType);
			return true;
		}

		private void SetLabelPos(Generics.BattleRootType iType)
		{
			int cnt = 0;
			_listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.transform.localPosition = ((iType != Generics.BattleRootType.Rebellion) ? _listAWPos4Sortie[cnt] : _listAWPos4Rebellion[cnt]);
				cnt++;
			});
		}

		public void Play(Action<AdvancingWithdrawalDCType, ShipRecoveryType> onDecide)
		{
			_actOnDecide = onDecide;
			BattleCutManager.GetStateBattle().prodBCBattle.setResultHPModeAdvancingWithdrawal(-74.86f);
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.SetNavigationInAdvancingWithdrawal();
			navigation.Show(Defines.PHASE_FADE_TIME, null);
			Show().setOnComplete((Action)delegate
			{
				_isInputPossible = true;
				_listLabelButton.ForEach(delegate(UILabelButton x)
				{
					x.toggle.enabled = (x.isValid ? true : false);
				});
			});
		}

		public bool Run()
		{
			if (!_isInputPossible)
			{
				return true;
			}
			KeyControl keyControl = BattleCutManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.LEFT))
			{
				PreparaNext(isFoward: true);
			}
			else if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
			{
				PreparaNext(isFoward: false);
			}
			else if (keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				Decide();
			}
			return true;
		}

		private void ChangeFocus(AdvancingWithdrawalDCType iType)
		{
			_listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.isFocus = ((x.index == (int)iType) ? true : false);
			});
		}

		private void PreparaNext(bool isFoward)
		{
			AdvancingWithdrawalDCType iSelectType = _iSelectType;
			_iSelectType = (AdvancingWithdrawalDCType)Mathe.NextElement((int)_iSelectType, 0, 2, isFoward, (int x) => _listLabelButton[x].isValid);
			if (iSelectType != _iSelectType)
			{
				ChangeFocus(_iSelectType);
			}
		}

		private LTDescr Show()
		{
			return panel.transform.LTValue(panel.alpha, 1f, Defines.PHASE_FADE_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
		}

		private LTDescr Hide()
		{
			return panel.transform.LTValue(panel.alpha, 0f, Defines.PHASE_FADE_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
		}

		private void OnActive(AdvancingWithdrawalDCType iType)
		{
			if (_iSelectType != iType)
			{
				_iSelectType = iType;
				ChangeFocus(_iSelectType);
			}
		}

		public void Decide()
		{
			_isInputPossible = false;
			_listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.toggle.enabled = false;
			});
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.Hide(Defines.PHASE_FADE_TIME, null);
			ShipRecoveryType type = BattleUtils.GetShipRecoveryType(_iSelectType);
			Hide().setOnComplete((Action)delegate
			{
				Dlg.Call(ref _actOnDecide, _iSelectType, type);
			});
		}
	}
}
