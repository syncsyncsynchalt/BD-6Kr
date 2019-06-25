using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public class ParticleEmitter : Component
	{
		public bool emit
		{
			get;
			set;
		}

		public float minSize
		{
			get;
			set;
		}

		public float maxSize
		{
			get;
			set;
		}

		public float minEnergy
		{
			get;
			set;
		}

		public float maxEnergy
		{
			get;
			set;
		}

		public float minEmission
		{
			get;
			set;
		}

		public float maxEmission
		{
			get;
			set;
		}

		public float emitterVelocityScale
		{
			get;
			set;
		}

		public Vector3 worldVelocity
		{
			get
			{
				INTERNAL_get_worldVelocity(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_worldVelocity(ref value);
			}
		}

		public Vector3 localVelocity
		{
			get
			{
				INTERNAL_get_localVelocity(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_localVelocity(ref value);
			}
		}

		public Vector3 rndVelocity
		{
			get
			{
				INTERNAL_get_rndVelocity(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_rndVelocity(ref value);
			}
		}

		public bool useWorldSpace
		{
			get;
			set;
		}

		public bool rndRotation
		{
			get;
			set;
		}

		public float angularVelocity
		{
			get;
			set;
		}

		public float rndAngularVelocity
		{
			get;
			set;
		}

		public Particle[] particles
		{
			get;
			set;
		}

		public int particleCount
		{
			get;
		}

		public bool enabled
		{
			get;
			set;
		}

		internal ParticleEmitter()
		{
		}

		private void INTERNAL_get_worldVelocity(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_worldVelocity(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_localVelocity(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_localVelocity(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_rndVelocity(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_rndVelocity(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void ClearParticles()
		{
			INTERNAL_CALL_ClearParticles(this);
		}

		private static void INTERNAL_CALL_ClearParticles(ParticleEmitter self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Emit()
		{
			Emit2((int)Random.Range(minEmission, maxEmission));
		}

		public void Emit(int count)
		{
			Emit2(count);
		}

		public void Emit(Vector3 pos, Vector3 velocity, float size, float energy, Color color)
		{
			InternalEmitParticleArguments args = default(InternalEmitParticleArguments);
			args.pos = pos;
			args.velocity = velocity;
			args.size = size;
			args.energy = energy;
			args.color = color;
			args.rotation = 0f;
			args.angularVelocity = 0f;
			Emit3(ref args);
		}

		public void Emit(Vector3 pos, Vector3 velocity, float size, float energy, Color color, float rotation, float angularVelocity)
		{
			InternalEmitParticleArguments args = default(InternalEmitParticleArguments);
			args.pos = pos;
			args.velocity = velocity;
			args.size = size;
			args.energy = energy;
			args.color = color;
			args.rotation = rotation;
			args.angularVelocity = angularVelocity;
			Emit3(ref args);
		}

		private void Emit2(int count) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Emit3(ref InternalEmitParticleArguments args) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Simulate(float deltaTime) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
