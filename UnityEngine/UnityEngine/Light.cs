using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine
{
	public sealed class Light : Behaviour
	{
		public LightType type
		{
			get;
			set;
		}

		public Color color
		{
			get
			{
				INTERNAL_get_color(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_color(ref value);
			}
		}

		public float intensity
		{
			get;
			set;
		}

		public float bounceIntensity
		{
			get;
			set;
		}

		public LightShadows shadows
		{
			get;
			set;
		}

		public float shadowStrength
		{
			get;
			set;
		}

		public float shadowBias
		{
			get;
			set;
		}

		public float shadowNormalBias
		{
			get;
			set;
		}

		[Obsolete("Shadow softness is removed in Unity 5.0+")]
		public float shadowSoftness
		{
			get;
			set;
		}

		[Obsolete("Shadow softness is removed in Unity 5.0+")]
		public float shadowSoftnessFade
		{
			get;
			set;
		}

		public float range
		{
			get;
			set;
		}

		public float spotAngle
		{
			get;
			set;
		}

		public float cookieSize
		{
			get;
			set;
		}

		public Texture cookie
		{
			get;
			set;
		}

		public Flare flare
		{
			get;
			set;
		}

		public LightRenderMode renderMode
		{
			get;
			set;
		}

		public bool alreadyLightmapped
		{
			get;
			set;
		}

		public int cullingMask
		{
			get;
			set;
		}

		public int commandBufferCount
		{
			get;
		}

		public static int pixelLightCount
		{
			get;
			set;
		}

		[Obsolete("light.shadowConstantBias was removed, use light.shadowBias", true)]
		public float shadowConstantBias
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		[Obsolete("light.shadowObjectSizeBias was removed, use light.shadowBias", true)]
		public float shadowObjectSizeBias
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		[Obsolete("light.attenuate was removed; all lights always attenuate now", true)]
		public bool attenuate
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		private void INTERNAL_get_color(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_color(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void AddCommandBuffer(LightEvent evt, CommandBuffer buffer) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void RemoveCommandBuffer(LightEvent evt, CommandBuffer buffer) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void RemoveCommandBuffers(LightEvent evt) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void RemoveAllCommandBuffers() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public CommandBuffer[] GetCommandBuffers(LightEvent evt) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static Light[] GetLights(LightType type, int layer) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
