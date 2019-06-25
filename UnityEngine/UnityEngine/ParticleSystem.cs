using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class ParticleSystem : Component
	{
		public struct Particle
		{
			private Vector3 m_Position;

			private Vector3 m_Velocity;

			private Vector3 m_AnimatedVelocity;

			private Vector3 m_AxisOfRotation;

			private float m_Rotation;

			private float m_AngularVelocity;

			private float m_Size;

			private Color32 m_Color;

			private uint m_RandomSeed;

			private float m_Lifetime;

			private float m_StartLifetime;

			private float m_EmitAccumulator0;

			private float m_EmitAccumulator1;

			public Vector3 position
			{
				get
				{
					return m_Position;
				}
				set
				{
					m_Position = value;
				}
			}

			public Vector3 velocity
			{
				get
				{
					return m_Velocity;
				}
				set
				{
					m_Velocity = value;
				}
			}

			public float lifetime
			{
				get
				{
					return m_Lifetime;
				}
				set
				{
					m_Lifetime = value;
				}
			}

			public float startLifetime
			{
				get
				{
					return m_StartLifetime;
				}
				set
				{
					m_StartLifetime = value;
				}
			}

			public float size
			{
				get
				{
					return m_Size;
				}
				set
				{
					m_Size = value;
				}
			}

			public Vector3 axisOfRotation
			{
				get
				{
					return m_AxisOfRotation;
				}
				set
				{
					m_AxisOfRotation = value;
				}
			}

			public float rotation
			{
				get
				{
					return m_Rotation * 57.29578f;
				}
				set
				{
					m_Rotation = value * ((float)Math.PI / 180f);
				}
			}

			public float angularVelocity
			{
				get
				{
					return m_AngularVelocity * 57.29578f;
				}
				set
				{
					m_AngularVelocity = value * ((float)Math.PI / 180f);
				}
			}

			public Color32 color
			{
				get
				{
					return m_Color;
				}
				set
				{
					m_Color = value;
				}
			}

			[Obsolete("randomValue property is deprecated. Use randomSeed instead to control random behavior of particles.")]
			public float randomValue
			{
				get
				{
					return BitConverter.ToSingle(BitConverter.GetBytes(m_RandomSeed), 0);
				}
				set
				{
					m_RandomSeed = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
				}
			}

			public uint randomSeed
			{
				get
				{
					return m_RandomSeed;
				}
				set
				{
					m_RandomSeed = value;
				}
			}
		}

		public float startDelay
		{
			get;
			set;
		}

		public bool isPlaying
		{
			get;
		}

		public bool isStopped
		{
			get;
		}

		public bool isPaused
		{
			get;
		}

		public bool loop
		{
			get;
			set;
		}

		public bool playOnAwake
		{
			get;
			set;
		}

		public float time
		{
			get;
			set;
		}

		public float duration
		{
			get;
		}

		public float playbackSpeed
		{
			get;
			set;
		}

		public int particleCount
		{
			get;
		}

		public bool enableEmission
		{
			get;
			set;
		}

		public float emissionRate
		{
			get;
			set;
		}

		public float startSpeed
		{
			get;
			set;
		}

		public float startSize
		{
			get;
			set;
		}

		public Color startColor
		{
			get
			{
				INTERNAL_get_startColor(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_startColor(ref value);
			}
		}

		public float startRotation
		{
			get;
			set;
		}

		public float startLifetime
		{
			get;
			set;
		}

		public float gravityModifier
		{
			get;
			set;
		}

		public int maxParticles
		{
			get;
			set;
		}

		public ParticleSystemSimulationSpace simulationSpace
		{
			get;
			set;
		}

		public uint randomSeed
		{
			get;
			set;
		}

		private void INTERNAL_get_startColor(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_startColor(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetParticles(Particle[] particles, int size) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public int GetParticles(Particle[] particles) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Internal_Simulate(float t, bool restart) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Internal_Play() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Internal_Stop() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Internal_Pause() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Internal_Clear() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private bool Internal_IsAlive() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void Simulate(float t, bool withChildren)
		{
			bool restart = true;
			Simulate(t, withChildren, restart);
		}

		[ExcludeFromDocs]
		public void Simulate(float t)
		{
			bool restart = true;
			bool withChildren = true;
			Simulate(t, withChildren, restart);
		}

		public void Simulate(float t, [DefaultValue("true")] bool withChildren, [DefaultValue("true")] bool restart)
		{
			if (withChildren)
			{
				ParticleSystem[] particleSystems = GetParticleSystems(this);
				ParticleSystem[] array = particleSystems;
				foreach (ParticleSystem particleSystem in array)
				{
					particleSystem.Internal_Simulate(t, restart);
				}
			}
			else
			{
				Internal_Simulate(t, restart);
			}
		}

		[ExcludeFromDocs]
		public void Play()
		{
			bool withChildren = true;
			Play(withChildren);
		}

		public void Play([DefaultValue("true")] bool withChildren)
		{
			if (withChildren)
			{
				ParticleSystem[] particleSystems = GetParticleSystems(this);
				ParticleSystem[] array = particleSystems;
				foreach (ParticleSystem particleSystem in array)
				{
					particleSystem.Internal_Play();
				}
			}
			else
			{
				Internal_Play();
			}
		}

		[ExcludeFromDocs]
		public void Stop()
		{
			bool withChildren = true;
			Stop(withChildren);
		}

		public void Stop([DefaultValue("true")] bool withChildren)
		{
			if (withChildren)
			{
				ParticleSystem[] particleSystems = GetParticleSystems(this);
				ParticleSystem[] array = particleSystems;
				foreach (ParticleSystem particleSystem in array)
				{
					particleSystem.Internal_Stop();
				}
			}
			else
			{
				Internal_Stop();
			}
		}

		[ExcludeFromDocs]
		public void Pause()
		{
			bool withChildren = true;
			Pause(withChildren);
		}

		public void Pause([DefaultValue("true")] bool withChildren)
		{
			if (withChildren)
			{
				ParticleSystem[] particleSystems = GetParticleSystems(this);
				ParticleSystem[] array = particleSystems;
				foreach (ParticleSystem particleSystem in array)
				{
					particleSystem.Internal_Pause();
				}
			}
			else
			{
				Internal_Pause();
			}
		}

		[ExcludeFromDocs]
		public void Clear()
		{
			bool withChildren = true;
			Clear(withChildren);
		}

		public void Clear([DefaultValue("true")] bool withChildren)
		{
			if (withChildren)
			{
				ParticleSystem[] particleSystems = GetParticleSystems(this);
				ParticleSystem[] array = particleSystems;
				foreach (ParticleSystem particleSystem in array)
				{
					particleSystem.Internal_Clear();
				}
			}
			else
			{
				Internal_Clear();
			}
		}

		[ExcludeFromDocs]
		public bool IsAlive()
		{
			bool withChildren = true;
			return IsAlive(withChildren);
		}

		public bool IsAlive([DefaultValue("true")] bool withChildren)
		{
			if (withChildren)
			{
				ParticleSystem[] particleSystems = GetParticleSystems(this);
				ParticleSystem[] array = particleSystems;
				foreach (ParticleSystem particleSystem in array)
				{
					if (particleSystem.Internal_IsAlive())
					{
						return true;
					}
				}
				return false;
			}
			return Internal_IsAlive();
		}

		public void Emit(int count)
		{
			INTERNAL_CALL_Emit(this, count);
		}

		private static void INTERNAL_CALL_Emit(ParticleSystem self, int count) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Emit(Vector3 position, Vector3 velocity, float size, float lifetime, Color32 color)
		{
			Particle particle = default(Particle);
			particle.position = position;
			particle.velocity = velocity;
			particle.lifetime = lifetime;
			particle.startLifetime = lifetime;
			particle.size = size;
			particle.rotation = 0f;
			particle.angularVelocity = 0f;
			particle.color = color;
			particle.randomSeed = 5u;
			Internal_Emit(ref particle);
		}

		public void Emit(Particle particle)
		{
			Internal_Emit(ref particle);
		}

		private void Internal_Emit(ref Particle particle) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static ParticleSystem[] GetParticleSystems(ParticleSystem root)
		{
			if (!root)
			{
				return null;
			}
			List<ParticleSystem> list = new List<ParticleSystem>();
			list.Add(root);
			GetDirectParticleSystemChildrenRecursive(root.transform, list);
			return list.ToArray();
		}

		private static void GetDirectParticleSystemChildrenRecursive(Transform transform, List<ParticleSystem> particleSystems)
		{
			foreach (Transform item in transform)
			{
				ParticleSystem component = item.gameObject.GetComponent<ParticleSystem>();
				if (component != null)
				{
					particleSystems.Add(component);
					GetDirectParticleSystemChildrenRecursive(item, particleSystems);
				}
			}
		}
	}
}
