using KCV.Generic;
using local.utils;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class ProdBCWithdrawalDecision : MonoBehaviour
	{
		[SerializeField]
		private List<UILabelButton> _listLabelButton;

		private int _nIndex;

		private bool _isInputPossible;

		private UIPanel _uiPanel;

		private Action<int> _actCallback;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public int index => _nIndex;

		public static ProdBCWithdrawalDecision Instantiate(ProdBCWithdrawalDecision prefab, Transform parent)
		{
			ProdBCWithdrawalDecision prodBCWithdrawalDecision = UnityEngine.Object.Instantiate(prefab);
			prodBCWithdrawalDecision.transform.parent = parent;
			prodBCWithdrawalDecision.transform.localPositionZero();
			prodBCWithdrawalDecision.transform.localScaleOne();
			prodBCWithdrawalDecision.Init();
			return prodBCWithdrawalDecision;
		}

		private void OnDestroy()
		{
			Mem.DelListSafe(ref _listLabelButton);
			Mem.Del(ref _nIndex);
			Mem.Del(ref _isInputPossible);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _actCallback);
		}

		private bool Init()
		{
			panel.alpha = 0f;
			_nIndex = 0;
			int cnt = 0;
			_listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.Init(cnt, isValid: true, KCVColor.ConvertColor(110f, 110f, 110f, 255f));
				x.isFocus = false;
				x.toggle.group = 10;
				x.toggle.enabled = false;
				x.toggle.onDecide = delegate
				{
					OnDecide(x.index);
				};
				x.toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", x.index);
				if (x.index == 0)
				{
					x.toggle.startsActive = true;
				}
				cnt++;
			});
			_isInputPossible = false;
			return true;
		}

		public ProdBCWithdrawalDecision Play(Action<int> onFinished)
		{
			_actCallback = onFinished;
			Observable.FromCoroutine(PlayShowAnim).Subscribe(delegate
			{
				KeyControl keyControl = BattleCutManager.GetKeyControl();
				keyControl.IsRun = true;
				ChangeFocus(_nIndex);
				_listLabelButton.ForEach(delegate(UILabelButton x)
				{
					x.toggle.enabled = true;
				});
				_isInputPossible = true;
			}).AddTo(base.gameObject);
			return this;
		}

		private IEnumerator PlayShowAnim()
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.SetNavigationInWithdrawalDecision();
			bool isWait = true;
			TrophyUtil.Unlock_At_SCutBattle();
			BattleCutManager.GetStateBattle().prodBCBattle.SetResultHPModeToWithdrawal(-74.86f);
			navigation.Show(0.35f, null);
			Show(delegate
			{
				isWait = false;
			});
			while (isWait)
			{
				yield return null;
			}
		}

		public bool Run()
		{
			KeyControl keyControl = BattleCutManager.GetKeyControl();
			if (_isInputPossible)
			{
				if (keyControl.GetDown(KeyControl.KeyName.LEFT))
				{
					PreparaNext(isFoward: false);
				}
				else if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
				{
					PreparaNext(isFoward: true);
				}
				if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					OnDecide(_nIndex);
					return true;
				}
			}
			return true;
		}

		private void Show(Action onFinished)
		{
			LeanTween.value(base.gameObject, 0f, 1f, 0.35f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					Dlg.Call(ref onFinished);
				});
		}

		private void Hide(Action onFinished)
		{
			LeanTween.value(base.gameObject, 1f, 0f, Defines.PHASE_FADE_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					Dlg.Call(ref onFinished);
				});
		}

		private void PreparaNext(bool isFoward)
		{
			int nIndex = _nIndex;
			_nIndex = Mathe.NextElement(_nIndex, 0, 1, isFoward);
			if (nIndex != _nIndex)
			{
				ChangeFocus(_nIndex);
			}
		}

		private void ChangeFocus(int nIndex)
		{
			_listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.isFocus = ((x.index == nIndex) ? true : false);
			});
		}

		private void OnActive(int nIndex)
		{
			if (_nIndex != nIndex)
			{
				_nIndex = nIndex;
				ChangeFocus(_nIndex);
			}
		}

		private void OnDecide(int nIndex)
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			_nIndex = nIndex;
			_isInputPossible = false;
			_listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.toggle.enabled = false;
			});
			Hide(delegate
			{
				if (_actCallback != null)
				{
					_actCallback(_nIndex);
				}
			});
			navigation.Hide(Defines.PHASE_FADE_TIME, null);
			BattleCutManager.GetStateBattle().prodBCBattle.Hide(Defines.PHASE_FADE_TIME);
		}
	}
}
