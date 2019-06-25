using local.managers;
using local.models;
using LT.Tweening;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIWidget))]
	public class BtlCut_HPBar : MonoBehaviour
	{
		[SerializeField]
		private UISlider _uiHPSlider;

		[SerializeField]
		private UILabel _uiHPLabel;

		[SerializeField]
		private UISprite _uiShipIcon;

		[SerializeField]
		private UISprite _uiEscapeIcon;

		[SerializeField]
		private UISprite _uiRepairIcon;

		private int _nShipType;

		private UIWidget _uiWidget;

		private bool _isBattleCut = true;

		public HPData hpData
		{
			get;
			private set;
		}

		public int shipType
		{
			get
			{
				return _nShipType;
			}
			private set
			{
				_nShipType = value;
				int num = (value != 9) ? _nShipType : 8;
				_uiShipIcon.spriteName = $"icon_ship{num}";
				_uiShipIcon.MakePixelPerfect();
			}
		}

		public UIWidget widget => this.GetComponentThis(ref _uiWidget);

		public static BtlCut_HPBar Instantiate(BtlCut_HPBar prefab, Transform parent, ShipModel_BattleAll model, bool isAfter, BattleManager manager)
		{
			BtlCut_HPBar btlCut_HPBar = Object.Instantiate(prefab);
			btlCut_HPBar.transform.parent = parent;
			btlCut_HPBar.transform.localScaleOne();
			btlCut_HPBar.transform.localPositionZero();
			btlCut_HPBar._isBattleCut = false;
			if (isAfter)
			{
				btlCut_HPBar.SetHpBarAfter(model, manager);
			}
			else
			{
				btlCut_HPBar.SetHpBar(model);
			}
			return btlCut_HPBar;
		}

		private void Awake()
		{
			_uiHPSlider.value = 1f;
			_isBattleCut = true;
			if (_uiEscapeIcon != null)
			{
				_uiEscapeIcon.SetActive(isActive: false);
			}
			if (_uiRepairIcon != null)
			{
				_uiRepairIcon.SetActive(isActive: false);
			}
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiHPSlider);
			Mem.Del(ref _uiShipIcon);
			Mem.Del(ref _nShipType);
			Mem.Del(ref _uiEscapeIcon);
			Mem.Del(ref _uiRepairIcon);
		}

		private void SetHpBar(HPData data, int shipType)
		{
			hpData = data;
			if (shipType > 0)
			{
				this.shipType = shipType;
			}
			_uiHPSlider.value = Mathe.Rate(0f, data.maxHP, data.nowHP);
			_uiHPSlider.foregroundWidget.color = Util.HpGaugeColor2(hpData.maxHP, data.nowHP);
			if (_uiHPLabel != null)
			{
				_uiHPLabel.text = $"{data.nowHP}/{data.maxHP}";
			}
		}

		public void SetHpBar(ShipModel_BattleAll model)
		{
			SetHpBar(new HPData(model.MaxHp, model.HpPhaseStart), (!model.IsFriend()) ? (-1) : model.ShipType);
			if (model.IsFriend())
			{
				_uiEscapeIcon.SetActive(model.IsEscape());
			}
			if (_uiRepairIcon != null)
			{
				_uiRepairIcon.SetActive(isActive: false);
			}
		}

		public void SetHpBarAfter(ShipModel_BattleAll model, BattleManager manager)
		{
			SetHpBar(new HPData(model.MaxHp, model.HpEnd), (!model.IsFriend()) ? (-1) : model.ShipType);
			if (model.IsFriend())
			{
				_uiEscapeIcon.SetActive(model.IsEscape());
			}
			if (manager.IsUseRecoverySlotitem(model.TmpId) != 0 && _uiRepairIcon != null)
			{
				_uiRepairIcon.spriteName = ((!_isBattleCut) ? "fuki2_set" : "fuki_set");
				_uiRepairIcon.SetActive(isActive: true);
			}
		}

		public void Play()
		{
			hpData.attackCnt--;
			hpData.nextHP = hpData.nowHP - hpData.oneAttackDamage[hpData.attackCnt];
			UpdateDamage();
		}

		private void UpdateDamage()
		{
			LeanTween.value(base.gameObject, _uiHPSlider.value, Mathe.Rate(0f, hpData.maxHP, hpData.nextHP), 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiHPSlider.value = x;
			});
			base.transform.LTValue(hpData.nowHP, hpData.nextHP, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				if (_uiHPLabel != null)
				{
					_uiHPLabel.text = $"{(int)x}/{hpData.maxHP}";
				}
				_uiHPSlider.foregroundWidget.color = Util.HpGaugeColor2(hpData.maxHP, (int)x);
			});
			hpData.nowHP = hpData.nextHP;
		}

		public void Hide()
		{
			_uiHPSlider.SetActive(isActive: false);
			if (_uiShipIcon != null)
			{
				_uiShipIcon.SetActive(isActive: false);
			}
			if (_uiHPLabel != null)
			{
				_uiHPLabel.SetActive(isActive: false);
			}
		}

		public void SetHPLabelColor(Color color)
		{
			if (_uiHPLabel != null)
			{
				_uiHPLabel.color = color;
			}
		}
	}
}
