using System;
using UnityEngine;

namespace KCV.Battle
{
	[Serializable]
	public class BattleParticleFile : IDisposable
	{
		[SerializeField]
		private Transform _prefabPSExplosionB2;

		[SerializeField]
		private Transform _prefabPSExplosionB3WhiteSmoke;

		[SerializeField]
		private Transform _prefabPSExplosionAntiGround;

		[SerializeField]
		private Transform _prefabPSDustDepthCharge;

		[SerializeField]
		private Transform _prefabPSSplashMiss;

		[SerializeField]
		private Transform _prefabPSSplash;

		[SerializeField]
		private Transform _prefabPSExplosionAerial;

		private ParticleSystem _psExplosionB2;

		private ParticleSystem _psExplosionB3WhiteSmoke;

		private ParticleSystem _psExplosionAntiGround;

		private ParticleSystem _psDustDepthCharge;

		private ParticleSystem _psSplashMiss;

		private ParticleSystem _psSplash;

		private ParticleSystem _psExplosionAerial;

		public ParticleSystem explosionB2
		{
			get
			{
				if ((UnityEngine.Object)_psExplosionB2 == null)
				{
					InstantiateParticle(ref _psExplosionB2, ref _prefabPSExplosionB2);
				}
				return _psExplosionB2;
			}
		}

		public ParticleSystem explosionB3WhiteSmoke
		{
			get
			{
				if ((UnityEngine.Object)_psExplosionB3WhiteSmoke == null)
				{
					InstantiateParticle(ref _psExplosionB3WhiteSmoke, ref _prefabPSExplosionB3WhiteSmoke);
				}
				return _psExplosionB3WhiteSmoke;
			}
		}

		public ParticleSystem explosionAntiGround
		{
			get
			{
				if ((UnityEngine.Object)_psExplosionAntiGround == null)
				{
					InstantiateParticle(ref _psExplosionAntiGround, ref _prefabPSExplosionAntiGround);
				}
				return _psExplosionAntiGround;
			}
		}

		public ParticleSystem dustDepthCharge
		{
			get
			{
				if ((UnityEngine.Object)_psDustDepthCharge == null)
				{
					InstantiateParticle(ref _psDustDepthCharge, ref _prefabPSDustDepthCharge);
				}
				return _psDustDepthCharge;
			}
		}

		public ParticleSystem splashMiss
		{
			get
			{
				if ((UnityEngine.Object)_psSplashMiss == null)
				{
					InstantiateParticle(ref _psSplashMiss, ref _prefabPSSplashMiss);
				}
				return _psSplashMiss;
			}
		}

		public ParticleSystem splash
		{
			get
			{
				if ((UnityEngine.Object)_psSplash == null)
				{
					InstantiateParticle(ref _psSplash, ref _prefabPSSplash);
				}
				return _psSplash;
			}
		}

		public ParticleSystem explosionAerial
		{
			get
			{
				if ((UnityEngine.Object)_psExplosionAerial == null)
				{
					InstantiateParticle(ref _psExplosionAerial, ref _prefabPSExplosionAerial);
				}
				return _psExplosionAerial;
			}
		}

		public void Dispose()
		{
			Mem.Del(ref _prefabPSExplosionB2);
			Mem.Del(ref _prefabPSExplosionB3WhiteSmoke);
			Mem.Del(ref _prefabPSExplosionAntiGround);
			Mem.Del(ref _prefabPSDustDepthCharge);
			Mem.Del(ref _prefabPSSplashMiss);
			Mem.Del(ref _prefabPSSplash);
			Mem.Del(ref _prefabPSExplosionAerial);
			Mem.Del(ref _psExplosionB2);
			Mem.Del(ref _psExplosionB3WhiteSmoke);
			Mem.Del(ref _psExplosionAntiGround);
			Mem.Del(ref _psDustDepthCharge);
			Mem.Del(ref _psSplashMiss);
			Mem.Del(ref _psSplash);
			Mem.Del(ref _psExplosionAerial);
		}

		public static ParticleSystem InstantiateParticle(ref ParticleSystem system, ref Transform prefab)
		{
			return InstantiateParticle(ref system, ref prefab, BattleTaskManager.GetBattleField().transform);
		}

		public static ParticleSystem InstantiateParticle(ref ParticleSystem system, ref Transform prefab, Transform parent)
		{
			system = Util.Instantiate(prefab.gameObject, parent.gameObject).GetComponent<ParticleSystem>();
			((Component)system).SetActive(isActive: false);
			prefab = null;
			return system;
		}

		public static void ReleaseParticle(ref ParticleSystem system)
		{
			if (!((UnityEngine.Object)system == null))
			{
				if (((Component)system).gameObject != null)
				{
					UnityEngine.Object.Destroy(((Component)system).gameObject);
				}
				system = null;
			}
		}
	}
}
