using local.models;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIPanel))]
	public class UIVeteransReportBonus : MonoBehaviour
	{
		[Serializable]
		private class EXP : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UILabel _uiEXPLabel;

			[SerializeField]
			private UILabel _uiEXPVal;

			[SerializeField]
			private UISprite _uiPlus;

			private int _nEXP;

			public Transform transform => _tra;

			public EXP(Transform obj)
			{
				_tra = obj;
				Util.FindParentToChild(ref _uiEXPLabel, transform, "EXPLabel");
				Util.FindParentToChild(ref _uiEXPVal, transform, "EXPVal");
				Util.FindParentToChild(ref _uiPlus, transform, "Plus");
			}

			public void Dispose()
			{
				Mem.Del(ref _tra);
				Mem.Del(ref _uiEXPLabel);
				Mem.Del(ref _uiEXPVal);
				if (_uiPlus != null)
				{
					_uiPlus.Clear();
				}
				Mem.Del(ref _uiPlus);
				Mem.Del(ref _nEXP);
			}

			public void SetEXP(ShipModel_BattleResult model)
			{
				_uiEXPVal.textInt = _nEXP;
				_nEXP = model.ExpFromBattle;
			}

			public void PlayUpdateEXP()
			{
				transform.LTValue(0f, _nEXP, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					_uiEXPVal.textInt = Convert.ToInt32(x);
				});
			}
		}

		[Serializable]
		private class SpecialVeterans : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UILabel _uiEXPLabel;

			[SerializeField]
			private UILabel _uiEXPVal;

			[SerializeField]
			private UISprite _uiItem;

			[SerializeField]
			private UISprite _uiLabel;

			[SerializeField]
			private UITexture _uiLine;

			[SerializeField]
			private UISprite _uiPlus;

			private int _nRate;

			private List<IReward> _listRewards;

			public Transform transform => _tra;

			public UIWidget widget => ((Component)transform).GetComponent<UIWidget>();

			public bool isReward => _nRate != 0 && _listRewards.Count != 0;

			public SpecialVeterans(Transform obj)
			{
				_tra = obj;
				_nRate = 0;
				_listRewards = new List<IReward>();
				Util.FindParentToChild(ref _uiEXPLabel, transform, "EXPLabel");
				Util.FindParentToChild(ref _uiEXPVal, transform, "EXPVal");
				Util.FindParentToChild(ref _uiItem, transform, "Item");
				Util.FindParentToChild(ref _uiLabel, transform, "Label");
				Util.FindParentToChild(ref _uiLine, transform, "Line");
				Util.FindParentToChild(ref _uiPlus, transform, "Plus");
			}

			public bool Init()
			{
				widget.alpha = 0f;
				_uiItem.SetActive(isActive: false);
				return true;
			}

			public bool Init(int nRate, List<IReward> models)
			{
				_nRate = nRate;
				_listRewards = models;
				if (nRate == 0 && models.Count == 0)
				{
					widget.alpha = 0f;
					return true;
				}
				_uiEXPVal.textInt = 0;
				if (models != null && models.Count != 0 && models is IReward_Useitem)
				{
					IReward_Useitem reward_Useitem = (IReward_Useitem)models[0];
					_uiItem.spriteName = string.Format("item_57", reward_Useitem.Id);
				}
				else
				{
					_uiItem.SetActive(isActive: false);
				}
				return true;
			}

			public void Dispose()
			{
				Mem.Del(ref _tra);
				Mem.Del(ref _uiEXPLabel);
				Mem.Del(ref _uiEXPVal);
				if (_uiItem != null)
				{
					_uiItem.Clear();
				}
				Mem.Del(ref _uiItem);
				if (_uiLabel != null)
				{
					_uiLabel.Clear();
				}
				Mem.Del(ref _uiLabel);
				Mem.Del(ref _uiLine);
				if (_uiPlus != null)
				{
					_uiPlus.Clear();
				}
				Mem.Del(ref _uiPlus);
				Mem.Del(ref _nRate);
				Mem.DelListSafe(ref _listRewards);
			}

			public void PlayDrawSpecialVeterans()
			{
				if (isReward)
				{
					transform.LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.easeInSine).setOnUpdate(delegate(float x)
					{
						widget.alpha = x;
					});
					_uiEXPVal.transform.LTValue(0f, _nRate, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
					{
						_uiEXPVal.textInt = Convert.ToInt32(x);
					});
				}
			}
		}

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UISprite _uiBaseEXPLabel;

		[SerializeField]
		private UISprite _uiBonusLabel;

		[SerializeField]
		private UITexture _uiBonusLine;

		[SerializeField]
		private UILabel _uiEXPLabel;

		[SerializeField]
		private UILabel _uiEXPValue;

		[SerializeField]
		private List<EXP> _listEXPs;

		[SerializeField]
		private SpecialVeterans _clsSpecialVeterans;

		[SerializeField]
		private float _fShoBonusPosX = 240f;

		private UIPanel _uiPanel;

		private BattleResultModel _clsResultModel;

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
		}

		private int baseEXP
		{
			get
			{
				return _uiEXPValue.textInt;
			}
			set
			{
				_uiEXPValue.textInt = value;
			}
		}

		public static UIVeteransReportBonus Instantiate(UIVeteransReportBonus prefab, Transform parent, Vector3 pos, BattleResultModel model, bool isPractice)
		{
			UIVeteransReportBonus uIVeteransReportBonus = UnityEngine.Object.Instantiate(prefab);
			uIVeteransReportBonus.transform.parent = parent;
			uIVeteransReportBonus.transform.localPosition = pos;
			uIVeteransReportBonus.transform.localScaleOne();
			uIVeteransReportBonus._clsResultModel = model;
			uIVeteransReportBonus._uiBaseEXPLabel.spriteName = ((!isPractice) ? "exp_txt2" : "exp_txt3");
			uIVeteransReportBonus.Init();
			return uIVeteransReportBonus;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiBaseEXPLabel);
			Mem.Del(ref _uiBonusLabel);
			Mem.Del(ref _uiBonusLine);
			Mem.Del(ref _uiEXPLabel);
			Mem.Del(ref _uiEXPValue);
			Mem.DelListSafe(ref _listEXPs);
			Mem.DelIDisposableSafe(ref _clsSpecialVeterans);
			Mem.Del(ref _fShoBonusPosX);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _clsResultModel);
		}

		private bool Init()
		{
			_uiEXPValue.textInt = _clsResultModel.BaseExp;
			int num = 0;
			ShipModel_BattleResult[] ships_f = _clsResultModel.Ships_f;
			foreach (ShipModel_BattleResult shipModel_BattleResult in ships_f)
			{
				if (shipModel_BattleResult == null)
				{
					_listEXPs[num].transform.SetActive(isActive: false);
					num++;
				}
				else
				{
					_listEXPs[num].SetEXP(shipModel_BattleResult);
					num++;
				}
			}
			_clsSpecialVeterans.Init();
			return true;
		}

		public void Show(Action callback)
		{
			Vector3 localPosition = base.transform.localPosition;
			localPosition.x = _fShoBonusPosX;
			base.transform.LTMoveLocal(localPosition, 0.5f).setEase(LeanTweenType.easeOutCubic).setOnComplete(callback)
				.setDelay(1f);
			base.transform.LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.easeOutCubic).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			})
				.setDelay(1f);
			PlayBonusEXP();
		}

		public void PlayBonusEXP()
		{
			base.transform.LTValue(0f, _clsResultModel.BaseExp, 0.5f).setEase(LeanTweenType.linear).setDelay(1f)
				.setOnUpdate(delegate(float x)
				{
					baseEXP = Convert.ToInt32(x);
				});
			_listEXPs.ForEach(delegate(EXP x)
			{
				x.PlayUpdateEXP();
			});
			Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(delegate
			{
				_clsSpecialVeterans.PlayDrawSpecialVeterans();
			});
		}
	}
}
