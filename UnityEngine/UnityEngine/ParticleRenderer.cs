using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class ParticleRenderer : Renderer
	{
		public ParticleRenderMode particleRenderMode
		{
			get;
			set;
		}

		public float lengthScale
		{
			get;
			set;
		}

		public float velocityScale
		{
			get;
			set;
		}

		public float cameraVelocityScale
		{
			get;
			set;
		}

		public float maxParticleSize
		{
			get;
			set;
		}

		public int uvAnimationXTile
		{
			get;
			set;
		}

		public int uvAnimationYTile
		{
			get;
			set;
		}

		public float uvAnimationCycles
		{
			get;
			set;
		}

		[Obsolete("animatedTextureCount has been replaced by uvAnimationXTile and uvAnimationYTile.")]
		public int animatedTextureCount
		{
			get
			{
				return uvAnimationXTile;
			}
			set
			{
				uvAnimationXTile = value;
			}
		}

		public float maxPartileSize
		{
			get
			{
				return maxParticleSize;
			}
			set
			{
				maxParticleSize = value;
			}
		}

		public Rect[] uvTiles
		{
			get;
			set;
		}

		[Obsolete("This function has been removed.", true)]
		public AnimationCurve widthCurve
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		[Obsolete("This function has been removed.", true)]
		public AnimationCurve heightCurve
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		[Obsolete("This function has been removed.", true)]
		public AnimationCurve rotationCurve
		{
			get
			{
				return null;
			}
			set
			{
			}
		}
	}
}
