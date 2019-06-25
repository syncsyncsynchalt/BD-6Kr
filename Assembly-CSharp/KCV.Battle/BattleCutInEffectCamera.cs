using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(GlowEffect))]
	[RequireComponent(typeof(MotionBlur))]
	public class BattleCutInEffectCamera : BaseCamera
	{
		private GlowEffect _clsGlowEffect;

		private MotionBlur _clsMotionBlur;

		private Blur _clsBlur;

		public GlowEffect glowEffect
		{
			get
			{
				if (_clsGlowEffect == null)
				{
					_clsGlowEffect = GetComponent<GlowEffect>();
				}
				return _clsGlowEffect;
			}
		}

		public MotionBlur motionBlur
		{
			get
			{
				if (_clsMotionBlur == null)
				{
					_clsMotionBlur = GetComponent<MotionBlur>();
				}
				if (_clsMotionBlur.shader == null)
				{
					_clsMotionBlur.shader = Shader.Find("Hidden/MotionBlur");
				}
				return _clsMotionBlur;
			}
		}

		public Blur blur
		{
			get
			{
				if (_clsBlur == null)
				{
					_clsBlur = GetComponent<Blur>();
				}
				return _clsBlur;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			glowEffect.glowIntensity = 1.5f;
			glowEffect.blurIterations = 3;
			glowEffect.blurSpread = 0.7f;
			glowEffect.enabled = false;
			motionBlur.blurAmount = 0.35f;
			motionBlur.enabled = false;
			blur.downsample = 0;
			blur.blurSize = 1.36f;
			blur.blurIterations = 3;
			blur.enabled = false;
		}

		protected override void OnUnInit()
		{
			Mem.Del(ref _clsGlowEffect);
			Mem.Del(ref _clsMotionBlur);
			Mem.Del(ref _clsBlur);
			base.OnUnInit();
		}
	}
}
