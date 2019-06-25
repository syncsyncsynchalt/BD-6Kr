using Common.Enum;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class UICircleHPGauge : BaseHPGauge
	{
		private int _shipNumber;

		private bool _isSmoll;

		private bool _isLight;

		private BattleHitStatus _hitType;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiForeground;

		[SerializeField]
		private UILabel _uiHPLabel;

		[SerializeField]
		private GameObject _uiDamageObj;

		[SerializeField]
		private UISprite[] _uiDamage;

		[SerializeField]
		private UISprite _uiShipNumber;

		[SerializeField]
		private Animation _anime;

		[SerializeField]
		private Animation _animeDamage;

		protected override void Awake()
		{
			Util.FindParentToChild(ref _uiBackground, base.transform, "Background");
			Util.FindParentToChild(ref _uiForeground, base.transform, "Foreground");
			Util.FindParentToChild(ref _uiShipNumber, base.transform, "ShipNumber");
			Util.FindParentToChild(ref _uiHPLabel, base.transform, "Hp");
			if (_uiDamageObj == null)
			{
				_uiDamageObj = base.transform.FindChild("DamageObj").gameObject;
			}
			_uiDamage = new UISprite[5];
			for (int i = 0; i < 5; i++)
			{
				Util.FindParentToChild(ref _uiDamage[i], _uiDamageObj.transform, "Damage" + (i + 1));
				_uiDamage[i].alpha = 0f;
			}
			if ((UnityEngine.Object)_anime == null)
			{
				_anime = ((Component)base.transform).GetComponent<Animation>();
			}
			if ((UnityEngine.Object)_animeDamage == null)
			{
				_animeDamage = ((Component)_uiDamageObj.transform).GetComponent<Animation>();
			}
			_uiForeground.type = UIBasicSprite.Type.Filled;
			_uiForeground.fillAmount = 1f;
			_uiShipNumber.SetActive(isActive: false);
			base.transform.localScale = Vector3.zero;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			_uiBackground = null;
			_uiForeground = null;
			_uiHPLabel = null;
			_uiDamageObj = null;
			_uiDamage = null;
			_anime = null;
			_animeDamage = null;
		}

		public void SetShipNumber(int num, bool isFriend, bool isTorpedo)
		{
		}

		public void SetTextureScale(Vector3 scale, bool isSmoll)
		{
			_isSmoll = isSmoll;
			base.transform.localScale = scale;
			if (_isSmoll)
			{
				UITexture component = ((Component)base.transform.FindChild("Frame1")).GetComponent<UITexture>();
				UITexture component2 = ((Component)base.transform.FindChild("Frame2")).GetComponent<UITexture>();
				string str = (!_isLight) ? string.Empty : "_bl";
				component.mainTexture = (Resources.Load("Textures/battle/Gauge/D/gaugeD_frame_m" + str) as Texture2D);
				component2.mainTexture = (Resources.Load("Textures/battle/Gauge/C/gaugeC_frame_m" + str) as Texture2D);
				_uiBackground.mainTexture = (Resources.Load("Textures/battle/Gauge/D/gaugeD_m" + str) as Texture2D);
				_uiForeground.mainTexture = (Resources.Load("Textures/battle/Gauge/D/gaugeD_base_m" + str) as Texture2D);
				component.MakePixelPerfect();
				component2.MakePixelPerfect();
				_uiBackground.MakePixelPerfect();
				_uiForeground.MakePixelPerfect();
				float x = 1f / scale.x;
				float y = 1f / scale.y;
				component.transform.localScale = new Vector3(x, y, 1f);
				component2.transform.localScale = new Vector3(x, y, 1f);
				_uiBackground.transform.localScale = new Vector3(x, y, 1f);
				_uiForeground.transform.localScale = new Vector3(x, y, 1f);
			}
		}

		public void SetTextureType(bool isLight)
		{
			_isLight = isLight;
			UITexture component = ((Component)base.transform.FindChild("Frame1")).GetComponent<UITexture>();
			((Component)base.transform.FindChild("Frame2")).GetComponent<UITexture>();
			UITexture component2 = ((Component)base.transform.FindChild("CriticallObj/CriticallFlash")).GetComponent<UITexture>();
			UISprite component3 = ((Component)component2.transform.FindChild("CriticallBack2")).GetComponent<UISprite>();
			if (_isLight)
			{
				UITexture uITexture = component2;
				Color color = component2.color;
				uITexture.color = new Color(1f, 0.3f, 0f, color.a);
				UISprite uISprite = component3;
				Color color2 = component3.color;
				uISprite.color = new Color(1f, 0.3f, 0f, color2.a);
				if (_isSmoll)
				{
					component.mainTexture = (Resources.Load("Textures/battle/Gauge/D/gaugeD_frame_m_bl") as Texture2D);
					component.alpha = 1f;
				}
				else
				{
					component.mainTexture = (Resources.Load("Textures/battle/Gauge/D/gaugeD_frame_bl") as Texture2D);
					component.alpha = 1f;
					component.material = null;
				}
			}
		}

		private void setDamageLabelPos()
		{
			int num = 0;
			int[] array = new int[5]
			{
				0,
				10,
				100,
				1000,
				10000
			};
			float[] array2 = new float[5]
			{
				-130f,
				-97.5f,
				-65f,
				-32.5f,
				0f
			};
			float[] array3 = new float[5]
			{
				-160f,
				-120f,
				-80f,
				-40f,
				0f
			};
			for (int i = 0; i < 5; i++)
			{
				if (_nDamage >= array[i])
				{
					num = i;
				}
			}
			int nDamage = _nDamage;
			int[] array4 = new int[5]
			{
				nDamage % 10,
				0,
				0,
				0,
				0
			};
			nDamage /= 10;
			array4[1] = nDamage % 10;
			nDamage /= 10;
			array4[2] = nDamage % 10;
			nDamage /= 10;
			array4[3] = nDamage % 10;
			nDamage /= 10;
			array4[4] = nDamage % 10;
			int num2 = nDamage / 10;
			for (int j = 0; j < 5; j++)
			{
				if (_nDamage >= array[j])
				{
					num = j;
				}
			}
			_uiDamageObj.transform.localPosition = ((_hitType != BattleHitStatus.Clitical) ? new Vector3(array2[num], 0f, 0f) : new Vector3(array3[num], 0f, 0f));
			for (int k = 0; k < 5; k++)
			{
				_uiDamage[k].SetActive(isActive: false);
			}
		}

		private void setDamageSprite(int value)
		{
			int[] array = new int[5];
			int[] array2 = new int[5]
			{
				0,
				10,
				100,
				1000,
				10000
			};
			int num = 0;
			array[0] = value % 10;
			int num2 = value / 10;
			array[1] = num2 % 10;
			num2 /= 10;
			array[2] = num2 % 10;
			num2 /= 10;
			array[3] = num2 % 10;
			num2 /= 10;
			array[4] = num2 % 10;
			int num3 = num2 / 10;
			for (int i = 0; i < 5; i++)
			{
				if (_nDamage >= array2[i])
				{
					num = i + 1;
				}
			}
			string arg = (_hitType != BattleHitStatus.Clitical) ? "txt_d" : "txt_c";
			for (int j = 0; j < num; j++)
			{
				_uiDamage[j].SetActive(isActive: true);
				_uiDamage[j].spriteName = arg + array[j];
				_uiDamage[j].MakePixelPerfect();
			}
		}

		public void SetDamagePosition(Vector3 vec)
		{
			_uiDamageObj.transform.localPosition = vec;
		}

		public Vector3 GetDamagePosition()
		{
			return _uiDamageObj.transform.localPosition;
		}

		public void SetHPGauge(int maxHP, int beforeHP, int afterHP, int damage, BattleHitStatus status, bool isFriend)
		{
			_uiHPLabel.textInt = beforeHP;
			_nMaxHP = maxHP;
			_nFromHP = beforeHP;
			_nToHP = ((afterHP > 0) ? afterHP : 0);
			_nDamage = ((damage < 100000) ? damage : 99999);
			_hitType = ((_nDamage <= 0) ? status : status);
			base.transform.localPosition = ((!isFriend) ? (Vector3.left * 200f) : (Vector3.right * 200f));
			if (_hitType == BattleHitStatus.Miss)
			{
				_nDamage = -1;
			}
			int now = (int)Math.Floor((float)_nFromHP);
			_uiHPLabel.textInt = _nFromHP;
			_uiForeground.color = Util.HpGaugeColor2(_nMaxHP, now);
			_uiHPLabel.color = Util.HpLabelColor(_nMaxHP, now);
			_uiForeground.fillAmount = Mathe.Rate(0f, _nMaxHP, _nFromHP);
			setDamageLabelPos();
		}

		public override void Play(Action callback)
		{
			base.transform.localScale = Vector3.one;
			_actCallback = callback;
			if (_nFromHP <= 0)
			{
				return;
			}
			if (_nDamage >= 0)
			{
				base.transform.LTValue(0f, _nDamage, 0.65f).setEase(LeanTweenType.easeOutExpo).setOnUpdate(delegate(float x)
				{
					int damageSprite = (int)Math.Round(x);
					setDamageSprite(damageSprite);
				})
					.setOnComplete(compGaugeDamage);
				_animeDamage.Stop();
				if (_hitType == BattleHitStatus.Clitical)
				{
					_animeDamage.Play("ShowDamageCriticall");
				}
				else
				{
					_animeDamage.Play("ShowDamageNormal");
				}
			}
			base.transform.LTValue(_nFromHP, _nToHP, 0.7f).setEase(LeanTweenType.easeOutExpo).setOnUpdate(delegate(float x)
			{
				int num = (int)Math.Round(x);
				_uiHPLabel.textInt = num;
				_uiForeground.fillAmount = Mathe.Rate(0f, _nMaxHP, x);
				_uiForeground.color = Util.HpGaugeColor2(_nMaxHP, num);
				_uiHPLabel.color = Util.HpLabelColor(_nMaxHP, num);
			})
				.setOnComplete(onAnimationFinished);
			switch (_hitType)
			{
			case BattleHitStatus.Normal:
				break;
			case BattleHitStatus.Clitical:
				PlayCriticall();
				break;
			case BattleHitStatus.Miss:
				PlayMiss();
				break;
			}
		}

		public void compGaugeDamage()
		{
			setDamageSprite(_nDamage);
		}

		public void Plays(Action callback)
		{
			_actCallback = callback;
			if (_nFromHP <= 0)
			{
				return;
			}
			base.transform.LTValue(_nFromHP, _nToHP, 0.634f).setEase(LeanTweenType.easeOutExpo).setOnUpdate(delegate(float x)
			{
				int num = (int)Math.Floor(x);
				_uiHPLabel.textInt = num;
				_uiForeground.fillAmount = Mathe.Rate(0f, _nMaxHP, x);
				_uiForeground.color = Util.HpGaugeColor2(_nMaxHP, num);
				_uiHPLabel.color = Util.HpLabelColor(_nMaxHP, num);
			})
				.setOnComplete(onAnimationFinished);
			if (_nDamage >= 0)
			{
				base.transform.LTValue(0f, _nDamage, 0.634f).setEase(LeanTweenType.easeOutExpo).setOnUpdate(delegate(float x)
				{
					int damageSprite = (int)Math.Floor(x);
					setDamageSprite(damageSprite);
				})
					.setOnComplete(compGaugeDamage);
				_animeDamage.Stop();
				if (_hitType == BattleHitStatus.Clitical)
				{
					_animeDamage.Play("ShowDamageCriticall");
				}
				else
				{
					_animeDamage.Play("ShowDamageNormal");
				}
			}
			switch (_hitType)
			{
			case BattleHitStatus.Normal:
				break;
			case BattleHitStatus.Clitical:
				PlayCriticall();
				break;
			case BattleHitStatus.Miss:
				PlayMiss();
				break;
			}
		}

		public void PlayMiss()
		{
			_anime.Stop();
			_anime.Play("ShowMissText");
		}

		public void PlayCriticall()
		{
			_anime.Stop();
			_anime.Play("ShowCriticallText");
		}
	}
}
