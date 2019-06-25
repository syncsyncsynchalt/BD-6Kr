using Common.Enum;
using KCV.Battle.Utils;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdBufferEffect : BaseAnimation
	{
		[Serializable]
		private struct BufferLabel : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UILabel _uiBufferLabel;

			[SerializeField]
			private UITexture _uiSeparator;

			public Transform transform => _tra;

			public UILabel bufferLabel => _uiBufferLabel;

			public UITexture separator => _uiSeparator;

			public BufferLabel(Transform trans, UILabel label, UITexture separator)
			{
				_tra = trans;
				_uiBufferLabel = label;
				_uiSeparator = separator;
			}

			public bool Init(EffectModel model)
			{
				SetLabel(model.Command);
				return true;
			}

			public void Dispose()
			{
				Mem.Del(ref _tra);
				Mem.Del(ref _uiBufferLabel);
				Mem.Del(ref _uiSeparator);
			}

			public void Clear()
			{
				_uiBufferLabel.text = string.Empty;
			}

			private void SetLabel(BattleCommand iCommand)
			{
				string text = string.Empty;
				switch (iCommand)
				{
				case BattleCommand.Sekkin:
					text = "接近";
					break;
				case BattleCommand.Hougeki:
					text = "砲撃";
					break;
				case BattleCommand.Raigeki:
					text = "魚雷戦用意";
					break;
				case BattleCommand.Ridatu:
					text = "離脱";
					break;
				case BattleCommand.Taisen:
					text = "対潜";
					break;
				case BattleCommand.Kaihi:
					text = "回避";
					break;
				case BattleCommand.Kouku:
					text = "航空";
					break;
				case BattleCommand.Totugeki:
					text = "突撃";
					break;
				case BattleCommand.Tousha:
					text = "統射";
					break;
				}
				_uiBufferLabel.text = text;
			}
		}

		[Serializable]
		private class BufferEffect : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UILabel _uiEffectType;

			[SerializeField]
			private UITexture _uiSeparator;

			[SerializeField]
			private UILabel _uiValue;

			private BufferCorrection _strCorrection;

			public Transform transform => _tra;

			private int correctionFactor
			{
				set
				{
					string arg = (Mathf.Sign(value) != -1f) ? "+" : string.Empty;
					_uiValue.text = $"{arg}{value}%";
				}
			}

			public BufferEffect(UILabel effectType, UITexture separator, UILabel value, BufferCorrection correction)
			{
				_uiEffectType = effectType;
				_uiSeparator = separator;
				_uiValue = value;
				_strCorrection = correction;
			}

			public bool Init(BufferCorrection correction)
			{
				_strCorrection = correction;
				correctionFactor = 0;
				SetEffectTypeLabel(_strCorrection.type);
				return true;
			}

			public void Dispose()
			{
				Mem.Del(ref _tra);
				Mem.Del(ref _uiEffectType);
				Mem.Del(ref _uiSeparator);
				Mem.Del(ref _uiValue);
				_strCorrection.Dispose();
				Mem.Del(ref _strCorrection);
			}

			public void Clear()
			{
				correctionFactor = 0;
			}

			public LTDescr PlayCntUp(float time)
			{
				return _uiValue.transform.LTValue(0f, _strCorrection.collectionFactor, time).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					correctionFactor = Convert.ToInt32(x);
				});
			}

			private void SetEffectTypeLabel(BufferCorrectionType iType)
			{
				string text = string.Empty;
				switch (iType)
				{
				case BufferCorrectionType.AttackHitFactor:
					text = "攻撃命中率補正";
					break;
				case BufferCorrectionType.HitAvoianceFactor:
					text = "被弾回避率補正";
					break;
				case BufferCorrectionType.TorpedoHitFactor:
					text = "雷撃命中率補正";
					break;
				}
				_uiEffectType.text = text;
			}
		}

		[SerializeField]
		private BufferLabel _strBufferLabel = default(BufferLabel);

		[SerializeField]
		private List<BufferEffect> _listBufferEffects = new List<BufferEffect>();

		[SerializeField]
		private UILabel _uiWithdrawalAcceptance;

		private UIPanel _uiPanel;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static ProdBufferEffect Instantiate(ProdBufferEffect prefab, Transform parent)
		{
			ProdBufferEffect prodBufferEffect = UnityEngine.Object.Instantiate(prefab);
			prodBufferEffect.transform.parent = parent;
			prodBufferEffect.transform.localScaleOne();
			prodBufferEffect.transform.localPosition = new Vector3(-420f, -80f, 0f);
			prodBufferEffect.Init();
			return prodBufferEffect;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.DelIDisposableSafe(ref _strBufferLabel);
			_listBufferEffects.ForEach(delegate(BufferEffect x)
			{
				x.Dispose();
			});
			Mem.DelListSafe(ref _listBufferEffects);
			Mem.Del(ref _uiWithdrawalAcceptance);
			Mem.Del(ref _uiPanel);
		}

		public override bool Init()
		{
			base.Init();
			panel.alpha = 0f;
			panel.widgetsAreStatic = true;
			return true;
		}

		public void SetEffectData(EffectModel model)
		{
			_strBufferLabel.Init(model);
			SetCorrectionData(model);
			SetPosition(model.Command);
			_uiWithdrawalAcceptance.text = ((model.Command != BattleCommand.Ridatu) ? string.Empty : ((!model.Withdrawal) ? "失敗" : "成功"));
		}

		private void SetCorrectionData(EffectModel model)
		{
			_listBufferEffects[0].Init(new BufferCorrection(BufferCorrectionType.AttackHitFactor, model.MeichuBuff));
			_listBufferEffects[1].Init(new BufferCorrection(BufferCorrectionType.HitAvoianceFactor, model.KaihiBuff));
			_listBufferEffects[2].Init(new BufferCorrection(BufferCorrectionType.TorpedoHitFactor, model.RaiMeichuBuff));
		}

		private void SetPosition(BattleCommand iCommand)
		{
			switch (iCommand)
			{
			case BattleCommand.Hougeki:
			case BattleCommand.Taisen:
			case BattleCommand.Kouku:
				break;
			case BattleCommand.Sekkin:
				_strBufferLabel.transform.localPosition = Vector3.zero;
				_listBufferEffects[0].transform.localPosition = Vector3.down * 75f;
				_listBufferEffects[1].transform.localPosition = Vector3.left * 600f;
				_listBufferEffects[2].transform.localPosition = Vector3.left * 600f;
				_uiWithdrawalAcceptance.transform.localPosition = Vector3.left * 600f;
				break;
			case BattleCommand.Raigeki:
				_strBufferLabel.transform.localPosition = Vector3.zero;
				_listBufferEffects[0].transform.localPosition = Vector3.left * 600f;
				_listBufferEffects[1].transform.localPosition = Vector3.left * 600f;
				_listBufferEffects[2].transform.localPosition = Vector3.down * 75f;
				_uiWithdrawalAcceptance.transform.localPosition = Vector3.left * 600f;
				break;
			case BattleCommand.Ridatu:
			{
				_strBufferLabel.transform.localPosition = Vector3.zero;
				Transform transform = _listBufferEffects[0].transform;
				Vector3 localPosition = Vector3.left * 600f;
				_listBufferEffects[1].transform.localPosition = localPosition;
				transform.localPosition = localPosition;
				_listBufferEffects[2].transform.localPosition = Vector3.left * 600f;
				_uiWithdrawalAcceptance.transform.localPosition = Vector3.down * 85f;
				break;
			}
			case BattleCommand.Kaihi:
				_strBufferLabel.transform.localPosition = Vector3.zero;
				_listBufferEffects[0].transform.localPosition = Vector3.left * 600f;
				_listBufferEffects[1].transform.localPosition = Vector3.down * 75f;
				_listBufferEffects[2].transform.localPosition = Vector3.left * 600f;
				_uiWithdrawalAcceptance.transform.localPosition = Vector3.left * 600f;
				break;
			case BattleCommand.Totugeki:
				_strBufferLabel.transform.localPosition = Vector3.up * 135f;
				_listBufferEffects[0].transform.localPosition = Vector3.up * 60f;
				_listBufferEffects[1].transform.localPosition = Vector3.down * 75f;
				_listBufferEffects[2].transform.localPosition = Vector3.left * 600f;
				_uiWithdrawalAcceptance.transform.localPosition = Vector3.left * 600f;
				break;
			case BattleCommand.Tousha:
				_strBufferLabel.transform.localPosition = Vector3.zero;
				_listBufferEffects[0].transform.localPosition = Vector3.down * 75f;
				_listBufferEffects[1].transform.localPosition = Vector3.left * 600f;
				_listBufferEffects[2].transform.localPosition = Vector3.left * 600f;
				_uiWithdrawalAcceptance.transform.localPosition = Vector3.left * 600f;
				break;
			}
		}

		public override void Play(Action callback)
		{
			panel.widgetsAreStatic = false;
			panel.alpha = 0.01f;
			_actCallback = callback;
			PlayCountUp();
			base.animation.Play();
		}

		private void PlayCountUp()
		{
			_listBufferEffects.ForEach(delegate(BufferEffect x)
			{
				x.PlayCntUp(0.75f);
			});
		}

		protected override void onAnimationFinished()
		{
			panel.alpha = 0f;
			_strBufferLabel.Clear();
			panel.widgetsAreStatic = true;
			base.onAnimationFinished();
		}
	}
}
