using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdAdvancingWithDrawalSelect : BaseAnimation
	{
		[SerializeField]
		private List<UIAdvancingWithDrawalButton> _listHexExBtns;

		[SerializeField]
		private List<Vector3> _listHexExBtnsPos4Sortie;

		[SerializeField]
		private List<Vector3> _listHexExBtnsPos4Rebellion;

		[SerializeField]
		private UIFleetInfos _uiFleetInfos;

		[SerializeField]
		private int _nToggleGroup = 10;

		[Button("SetHexBtnsPos4Sortie", "set hex buttons position for sortie battle.", new object[]
		{

		})]
		[SerializeField]
		private int _nSetHexBtnsPos4Sortie;

		[Button("SetHexBtnsPos4Rebellion", "set hex buttons position for rebellion battle.", new object[]
		{

		})]
		[SerializeField]
		private int _nSetHexBtnsPos4Rebellion;

		private UIPanel _uiPanel;

		private int _nIndex;

		private bool _isDecide;

		private bool _isInputPossible;

		private List<bool> _listEnabledBtn;

		private Generics.BattleRootType _iRootType;

		private DelDecideHexButtonEx _delDecideAdvancingWithdrawalButton;

		public UIPanel panel
		{
			get
			{
				if (_uiPanel == null)
				{
					_uiPanel = GetComponent<UIPanel>();
				}
				return _uiPanel;
			}
			private set
			{
				_uiPanel = value;
			}
		}

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
					BattleManager battleManager = BattleTaskManager.GetBattleManager();
					result = ((!battleManager.ChangeableDeck) ? 1 : 2);
					break;
				}
				}
				return result;
			}
		}

		private bool isAdvancindPrimaryEnabled => BattleTaskManager.GetBattleManager().ChangeableDeck;

		private bool isAdvancindEnabled => BattleTaskManager.GetBattleManager().Ships_f[0].DmgStateEnd != DamageState_Battle.Taiha;

		public static ProdAdvancingWithDrawalSelect Instantiate(ProdAdvancingWithDrawalSelect prefab, Transform parent, Generics.BattleRootType iType)
		{
			ProdAdvancingWithDrawalSelect prodAdvancingWithDrawalSelect = UnityEngine.Object.Instantiate(prefab);
			prodAdvancingWithDrawalSelect.transform.parent = parent;
			prodAdvancingWithDrawalSelect.transform.localScaleZero();
			prodAdvancingWithDrawalSelect.transform.localPositionZero();
			prodAdvancingWithDrawalSelect._iRootType = iType;
			prodAdvancingWithDrawalSelect.Init();
			return prodAdvancingWithDrawalSelect;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.DelListSafe(ref _listHexExBtns);
			Mem.DelListSafe(ref _listHexExBtnsPos4Sortie);
			Mem.DelListSafe(ref _listHexExBtnsPos4Rebellion);
			Mem.Del(ref _uiFleetInfos);
			Mem.Del(ref _nToggleGroup);
			Mem.Del(ref _nSetHexBtnsPos4Sortie);
			Mem.Del(ref _nSetHexBtnsPos4Rebellion);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _nIndex);
			Mem.Del(ref _isDecide);
			Mem.Del(ref _isInputPossible);
			Mem.DelListSafe(ref _listEnabledBtn);
			Mem.Del(ref _iRootType);
			Mem.Del(ref _delDecideAdvancingWithdrawalButton);
		}

		private new bool Init()
		{
			panel.depth = 70;
			_nIndex = 0;
			_listEnabledBtn = new List<bool>();
			if (_listHexExBtns == null)
			{
				_listHexExBtns = new List<UIAdvancingWithDrawalButton>();
			}
			int cnt = 0;
			_listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton x)
			{
				x.Init(cnt, isColliderEnabled: false, 0, (Action)delegate
				{
					DecideAdvancingWithDrawalBtn(x);
				});
				x.toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", x.index);
				_listEnabledBtn.Add(item: true);
				cnt++;
			});
			if (_iRootType == Generics.BattleRootType.Rebellion)
			{
				SetHexBtnsPos4Rebellion();
			}
			else
			{
				SetHexBtnsPos4Sortie();
			}
			_uiFleetInfos.Init(new List<ShipModel_BattleAll>(BattleTaskManager.GetBattleManager().Ships_f));
			_uiFleetInfos.widget.alpha = 0f;
			return true;
		}

		private void SetHexBtnsPos4Sortie()
		{
			int cnt = 0;
			_listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton x)
			{
				x.transform.localPosition = _listHexExBtnsPos4Sortie[cnt];
				cnt++;
			});
		}

		private void SetHexBtnsPos4Rebellion()
		{
			int cnt = 0;
			_listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton x)
			{
				x.transform.localPosition = _listHexExBtnsPos4Rebellion[cnt];
				cnt++;
			});
		}

		public void Play(DelDecideHexButtonEx decideCallback)
		{
			base.Init();
			_delDecideAdvancingWithdrawalButton = decideCallback;
			base.transform.localScaleOne();
			if (_iRootType == Generics.BattleRootType.SortieMap)
			{
				ShowHexButtons2Sortie();
			}
			else
			{
				ShowHexButtons2Rebellion();
			}
			_uiFleetInfos.Show();
			UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
			battleNavigation.SetNavigationInAdvancingWithDrawal();
			battleNavigation.Show(0.2f, null);
			battleNavigation.panel.depth = panel.depth + 1;
		}

		private void ShowHexButtons2Sortie()
		{
			_listEnabledBtn[2] = false;
			_listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton x)
			{
				ProdAdvancingWithDrawalSelect prodAdvancingWithDrawalSelect = this;
				x.SetActive(isActive: true);
				x.Play(delegate
				{
					if (x.index == 0)
					{
						prodAdvancingWithDrawalSelect._listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton y)
						{
							y.toggle.group = 10;
						});
						KeyControl keyControl = BattleTaskManager.GetKeyControl();
						keyControl.reset(0, prodAdvancingWithDrawalSelect.maxIndex);
						keyControl.setChangeValue(0f, -1f, 0f, 1f);
						keyControl.Index = 0;
						x.isFocus = true;
						keyControl.isLoopIndex = false;
						prodAdvancingWithDrawalSelect._isInputPossible = true;
					}
					else
					{
						x.isFocus = false;
					}
					if (x.index == 2)
					{
						x.isColliderEnabled = prodAdvancingWithDrawalSelect.isAdvancindPrimaryEnabled;
					}
					x.isColliderEnabled = true;
				});
			});
		}

		private void ShowHexButtons2Rebellion()
		{
			_listEnabledBtn[1] = isAdvancindEnabled;
			_listEnabledBtn[2] = isAdvancindPrimaryEnabled;
			_listHexExBtns[0].SetActive(isActive: true);
			_listHexExBtns[0].Play(null);
			_listHexExBtns[1].SetActive(isActive: true);
			_listHexExBtns[1].Play(delegate
			{
				_listHexExBtns[0].isFocus = true;
				_listHexExBtns[1].isFocus = false;
				_listHexExBtns[2].SetActive(isActive: true);
				_listHexExBtns[2].Play(delegate
				{
					_listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton x)
					{
						x.isFocus = false;
						x.toggle.group = 10;
					});
					KeyControl keyControl = BattleTaskManager.GetKeyControl();
					keyControl.reset(0, maxIndex);
					keyControl.setChangeValue(0f, -1f, 0f, 1f);
					keyControl.Index = 0;
					keyControl.isLoopIndex = false;
					_isInputPossible = true;
					_listHexExBtns[0].isFocus = true;
					_listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton x)
					{
						x.isColliderEnabled = _listEnabledBtn[x.index];
					});
				});
			});
		}

		public bool Run()
		{
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
			if (!_isDecide && _isInputPossible)
			{
				if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
				{
					PreparaNext(isFoward: false);
				}
				if (keyControl.GetDown(KeyControl.KeyName.LEFT))
				{
					PreparaNext(isFoward: true);
				}
				if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					_listHexExBtns[_nIndex].OnDecide();
					return false;
				}
			}
			return !_isDecide;
		}

		private void PreparaNext(bool isFoward)
		{
			int nIndex = _nIndex;
			_nIndex = Mathe.NextElement(_nIndex, 0, maxIndex, isFoward, (int x) => _listEnabledBtn[x]);
			if (nIndex != _nIndex)
			{
				ChangeFocus(_nIndex);
			}
		}

		private void ChangeFocus(int nIndex)
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			_listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton x)
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

		private void DecideAdvancingWithDrawalBtn(UIHexButtonEx btn)
		{
			if (!_isDecide)
			{
				_isDecide = true;
				KeyControl keyControl = BattleTaskManager.GetKeyControl();
				keyControl.Index = btn.index;
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_036);
				_listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton x)
				{
					x.isColliderEnabled = false;
				});
				UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
				battleNavigation.Hide(0.2f, null);
				if (_delDecideAdvancingWithdrawalButton != null)
				{
					_delDecideAdvancingWithdrawalButton(btn);
				}
			}
		}
	}
}
