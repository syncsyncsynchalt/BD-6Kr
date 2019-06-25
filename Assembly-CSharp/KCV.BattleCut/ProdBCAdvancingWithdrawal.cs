using Common.Enum;
using KCV.Battle.Utils;
using KCV.Generic;
using local.managers;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdBCAdvancingWithdrawal : MonoBehaviour
	{
		[SerializeField]
		private List<Vector3> _listAWPos4Sortie;

		[SerializeField]
		private List<Vector3> _listAWPos4Rebellion;

		[SerializeField]
		private List<UILabelButton> _listLabelButton;

		private bool _isInputPossible;

		private UIPanel _uiPanel;

		private Generics.BattleRootType _iRootType;

		private AdvancingWithdrawalType _iSelectType;

		private Action<AdvancingWithdrawalType> _actCallback;

		private List<bool> _listEnabledBtn;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		private int maxIndex
		{
			get
			{
				int result = 0;
				switch (_iRootType)
				{
				case Generics.BattleRootType.SortieMap:
					result = 1;
					break;
				case Generics.BattleRootType.Rebellion:
				{
					BattleManager battleManager = BattleCutManager.GetBattleManager();
					result = ((!battleManager.ChangeableDeck) ? 1 : 2);
					break;
				}
				}
				return result;
			}
		}

		public static ProdBCAdvancingWithdrawal Instantiate(ProdBCAdvancingWithdrawal prefab, Transform parent, Generics.BattleRootType iType)
		{
			ProdBCAdvancingWithdrawal prodBCAdvancingWithdrawal = UnityEngine.Object.Instantiate(prefab);
			prodBCAdvancingWithdrawal.transform.parent = parent;
			prodBCAdvancingWithdrawal.transform.localPositionZero();
			prodBCAdvancingWithdrawal.transform.localScaleOne();
			prodBCAdvancingWithdrawal.Init(iType);
			return prodBCAdvancingWithdrawal;
		}

		private void OnDestroy()
		{
			Mem.DelListSafe(ref _listEnabledBtn);
			Mem.DelListSafe(ref _listLabelButton);
			Mem.DelListSafe(ref _listAWPos4Sortie);
			Mem.DelListSafe(ref _listAWPos4Rebellion);
			Mem.Del(ref _isInputPossible);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _iRootType);
			Mem.Del(ref _iSelectType);
			Mem.Del(ref _actCallback);
		}

		private bool Init(Generics.BattleRootType iType)
		{
			_isInputPossible = false;
			panel.alpha = 0f;
			_iRootType = iType;
			_iSelectType = AdvancingWithdrawalType.Withdrawal;
			SetEnabledBtns(iType);
			SetLabelPos(iType);
			int cnt = 0;
			_listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.Init(cnt, _listEnabledBtn[cnt], KCVColor.ConvertColor(110f, 110f, 110f, 255f), KCVColor.ConvertColor(110f, 110f, 110f, 128f));
				x.isFocus = false;
				x.toggle.group = 1;
				x.toggle.enabled = false;
				x.toggle.onDecide = delegate
				{
					DecideAdvancingWithDrawal();
				};
				x.toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", (AdvancingWithdrawalType)x.index);
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

		private void SetEnabledBtns(Generics.BattleRootType iType)
		{
			_listEnabledBtn = new List<bool>();
			switch (iType)
			{
			case Generics.BattleRootType.SortieMap:
				_listEnabledBtn.Add(item: true);
				_listEnabledBtn.Add(item: true);
				_listEnabledBtn.Add(item: false);
				break;
			case Generics.BattleRootType.Rebellion:
				_listEnabledBtn.Add(item: true);
				_listEnabledBtn.Add(BattleCutManager.GetBattleManager().Ships_f[0].DmgStateEnd != DamageState_Battle.Taiha);
				_listEnabledBtn.Add(BattleCutManager.GetBattleManager().ChangeableDeck);
				break;
			}
		}

		public void Play(Action<AdvancingWithdrawalType> onDecideCallback)
		{
			_actCallback = onDecideCallback;
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
				DecideAdvancingWithDrawal();
			}
			return true;
		}

		private void PreparaNext(bool isFoward)
		{
			AdvancingWithdrawalType iSelectType = _iSelectType;
			_iSelectType = (AdvancingWithdrawalType)Mathe.NextElement((int)_iSelectType, 0, maxIndex, isFoward, (int x) => _listLabelButton[x].isValid);
			if (iSelectType != _iSelectType)
			{
				ChangeFocus(_iSelectType);
			}
		}

		private void ChangeFocus(AdvancingWithdrawalType iType)
		{
			_listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.isFocus = ((x.index == (int)iType) ? true : false);
			});
		}

		private LTDescr Show()
		{
			return panel.transform.LTValue(0f, 1f, Defines.PHASE_FADE_TIME).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
		}

		private LTDescr Hide()
		{
			return panel.transform.LTValue(1f, 0f, Defines.PHASE_FADE_TIME).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
		}

		private void OnActive(AdvancingWithdrawalType nIndex)
		{
			if (_iSelectType != nIndex)
			{
				_iSelectType = nIndex;
				ChangeFocus(_iSelectType);
			}
		}

		private void DecideAdvancingWithDrawal()
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.Hide(Defines.PHASE_FADE_TIME, null);
			_isInputPossible = false;
			_listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.toggle.enabled = false;
			});
			Hide().setOnComplete((Action)delegate
			{
				Dlg.Call(ref _actCallback, _iSelectType);
			});
		}
	}
}
