using KCV.Battle.Utils;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAdvancingWithDrawalDC : BaseAnimation
	{
		[SerializeField]
		private UIPanel _uiPanel;

		[SerializeField]
		private List<UIHexButton> _listHexBtns;

		[SerializeField]
		private List<Vector3> _listHexExBtnsPos4Sortie;

		[SerializeField]
		private List<Vector3> _listHexExBtnsPos4Rebellion;

		[SerializeField]
		private UIFleetInfos _uiFleetInfos;

		[SerializeField]
		[Button("SetHexBtnsPos4Sortie", "set hex buttons position for sortie battle.", new object[]
		{

		})]
		private int _nSetHexBtnsPos4Sortie;

		[Button("SetHexBtnsPos4Rebellion", "set hex buttons position for rebellion battle.", new object[]
		{

		})]
		[SerializeField]
		private int _nSetHexBtnsPos4Rebellion;

		private DelDecideAdvancingWithdrawalButton _delDecideAdvancingWithdrawalButton;

		private bool _isDecide;

		private bool _isInputPossible;

		private List<bool> _listIsBtn;

		private int _btnIndex;

		private Generics.BattleRootType _iType;

		public static ProdAdvancingWithDrawalDC Instantiate(ProdAdvancingWithDrawalDC prefab, Transform parent, Generics.BattleRootType iType)
		{
			ProdAdvancingWithDrawalDC prodAdvancingWithDrawalDC = UnityEngine.Object.Instantiate(prefab);
			prodAdvancingWithDrawalDC.transform.parent = parent;
			prodAdvancingWithDrawalDC.transform.localScaleZero();
			prodAdvancingWithDrawalDC.transform.localPositionZero();
			prodAdvancingWithDrawalDC.Init(iType);
			return prodAdvancingWithDrawalDC;
		}

		private bool Init(Generics.BattleRootType type)
		{
			_iType = type;
			_btnIndex = 0;
			if (_uiPanel == null)
			{
				_uiPanel = GetComponent<UIPanel>();
			}
			_uiPanel.depth = 70;
			_listHexBtns = new List<UIHexButton>();
			_listIsBtn = new List<bool>();
			foreach (int value in Enum.GetValues(typeof(AdvancingWithdrawalDCType)))
			{
				if (value != -1)
				{
					_listIsBtn.Add(item: false);
					_listHexBtns.Add(((Component)base.transform.FindChild($"{((AdvancingWithdrawalDCType)value).ToString()}Btn")).GetComponent<UIHexButton>());
					_listHexBtns[value].Init();
					_listHexBtns[value].SetIndex(value);
					_listHexBtns[value].uiButton.onClick = Util.CreateEventDelegateList(this, "DecideAdvancingWithDrawalBtn", _listHexBtns[value]);
					_listHexBtns[value].isColliderEnabled = true;
				}
			}
			_uiFleetInfos.Init(new List<ShipModel_BattleAll>(BattleTaskManager.GetBattleManager().Ships_f));
			_uiFleetInfos.widget.alpha = 0f;
			return true;
		}

		private void SetHexBtnsPos4Sortie()
		{
			int cnt = 0;
			_listHexBtns.ForEach(delegate(UIHexButton x)
			{
				x.transform.localPosition = _listHexExBtnsPos4Sortie[cnt];
				cnt++;
			});
		}

		private void SetHexBtnsPos4Rebellion()
		{
			int cnt = 0;
			_listHexBtns.ForEach(delegate(UIHexButton x)
			{
				x.transform.localPosition = _listHexExBtnsPos4Rebellion[cnt];
				cnt++;
			});
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiPanel);
			Mem.DelListSafe(ref _listHexBtns);
			Mem.DelListSafe(ref _listHexExBtnsPos4Sortie);
			Mem.DelListSafe(ref _listHexExBtnsPos4Rebellion);
			Mem.Del(ref _uiFleetInfos);
			Mem.Del(ref _nSetHexBtnsPos4Sortie);
			Mem.Del(ref _nSetHexBtnsPos4Rebellion);
			Mem.Del(ref _delDecideAdvancingWithdrawalButton);
			Mem.Del(ref _isDecide);
			Mem.Del(ref _isInputPossible);
			Mem.DelListSafe(ref _listIsBtn);
			Mem.Del(ref _btnIndex);
			Mem.Del(ref _isInputPossible);
		}

		public void Play(DelDecideAdvancingWithdrawalButton decideCallback)
		{
			_delDecideAdvancingWithdrawalButton = decideCallback;
			base.transform.localScaleOne();
			base.Init();
			if (_iType == Generics.BattleRootType.Rebellion)
			{
				SetHexBtnsPos4Rebellion();
			}
			else
			{
				SetHexBtnsPos4Sortie();
			}
			ShipModel_BattleAll shipModel_BattleAll = BattleTaskManager.GetBattleManager().Ships_f[0];
			if (shipModel_BattleAll.HasRecoverYouin())
			{
				_btnIndex = 1;
			}
			else if (shipModel_BattleAll.HasRecoverMegami())
			{
				_btnIndex = 2;
			}
			else
			{
				_btnIndex = 0;
			}
			if (shipModel_BattleAll.HasRecoverYouin())
			{
				_listIsBtn[1] = true;
			}
			else
			{
				_listHexBtns[1].uiButton.defaultColor = new Color(0.2f, 0.2f, 0.2f);
			}
			if (shipModel_BattleAll.HasRecoverMegami())
			{
				_listIsBtn[2] = true;
			}
			else
			{
				_listHexBtns[2].uiButton.defaultColor = new Color(0.2f, 0.2f, 0.2f);
			}
			if (BattleTaskManager.GetBattleManager().ChangeableDeck)
			{
				_listIsBtn[3] = BattleTaskManager.GetBattleManager().ChangeableDeck;
			}
			else
			{
				_listHexBtns[3].uiButton.defaultColor = new Color(0.2f, 0.2f, 0.2f);
			}
			_listIsBtn[0] = true;
			_listHexBtns.ForEach(delegate(UIHexButton x)
			{
				x.SetActive(isActive: true);
				x.Play(UIHexButton.AnimationList.HexButtonShow, delegate
				{
					_isInputPossible = true;
					SetAdvancingWithdrawalBtnState(_btnIndex);
				});
			});
			UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
			battleNavigation.SetNavigationInAdvancingWithDrawal();
			battleNavigation.Show(0.2f, null);
			_uiFleetInfos.Show();
		}

		public bool Run()
		{
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
			if (!_isDecide && _isInputPossible)
			{
				foreach (UIHexButton listHexBtn in _listHexBtns)
				{
					listHexBtn.Run();
				}
				if (keyControl.GetDown(KeyControl.KeyName.LEFT))
				{
					_btnIndex = _setButtonIndex(_btnIndex, isUp: false);
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					SetAdvancingWithdrawalBtnState(_btnIndex);
				}
				if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
				{
					_btnIndex = _setButtonIndex(_btnIndex, isUp: true);
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					SetAdvancingWithdrawalBtnState(_btnIndex);
				}
				if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					DecideAdvancingWithDrawalBtn(_listHexBtns[_btnIndex]);
				}
			}
			return !_isDecide;
		}

		private int _setButtonIndex(int nIndex, bool isUp)
		{
			int result = nIndex;
			if (!isUp)
			{
				for (int i = nIndex; i < 4; i++)
				{
					if (i != nIndex && _listIsBtn[i])
					{
						result = i;
						break;
					}
				}
			}
			else
			{
				for (int num = nIndex; num > -1; num--)
				{
					if (num != nIndex && _listIsBtn[num])
					{
						result = num;
						break;
					}
				}
			}
			return result;
		}

		private void setTextEnabled()
		{
			UISprite component = ((Component)_listHexBtns[1].transform.FindChild("Label/Text")).GetComponent<UISprite>();
			component.spriteName = ((!_listHexBtns[1].isFocus) ? "txt_yoin_off" : "txt_yoin_on");
			UISprite component2 = ((Component)_listHexBtns[2].transform.FindChild("Label/Text")).GetComponent<UISprite>();
			component2.spriteName = ((!_listHexBtns[2].isFocus) ? "txt_megami_off" : "txt_megami_on");
			UISprite component3 = ((Component)_listHexBtns[0].transform.FindChild("Label/Text")).GetComponent<UISprite>();
			component3.spriteName = ((!_listHexBtns[0].isFocus) ? "txt_escape_off" : "txt_escape_on");
			UISprite component4 = ((Component)_listHexBtns[3].transform.FindChild("Label/Text1")).GetComponent<UISprite>();
			UISprite component5 = ((Component)_listHexBtns[3].transform.FindChild("Label/Text2")).GetComponent<UISprite>();
			component4.spriteName = ((!_listHexBtns[3].isFocus) ? "txt_go_off" : "txt_go_on");
			component5.spriteName = ((!_listHexBtns[3].isFocus) ? "txt_kessen_off" : "txt_kessen_on");
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
			ShipModel_BattleAll shipModel_BattleAll = BattleTaskManager.GetBattleManager().Ships_f[0];
			if ((btn.index == 1 && !shipModel_BattleAll.HasRecoverYouin()) || (btn.index == 2 && !shipModel_BattleAll.HasRecoverMegami()))
			{
				return;
			}
			if (btn.index == 1 && _btnIndex != btn.index)
			{
				_btnIndex = btn.index;
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				SetAdvancingWithdrawalBtnState(_btnIndex);
			}
			else if (btn.index == 2 && _btnIndex != btn.index)
			{
				_btnIndex = btn.index;
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				SetAdvancingWithdrawalBtnState(_btnIndex);
			}
			else if (btn.index == 0 && _btnIndex != btn.index)
			{
				_btnIndex = btn.index;
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				SetAdvancingWithdrawalBtnState(_btnIndex);
			}
			else if (!_isDecide)
			{
				_isDecide = true;
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_036);
				_listHexBtns.ForEach(delegate(UIHexButton x)
				{
					x.isColliderEnabled = false;
				});
				SetAdvancingWithdrawalBtnState(btn.index);
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
