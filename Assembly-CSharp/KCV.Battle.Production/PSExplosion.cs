using KCV.Utils;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class PSExplosion : MonoBehaviour
	{
		public enum ExplosionType
		{
			Large,
			Middle,
			Small
		}

		private ParticleSystem _psExplosion;

		private void Awake()
		{
			_psExplosion = ((Component)this).SafeGetComponent<ParticleSystem>();
			_psExplosion.Stop();
		}

		private void OnDestroy()
		{
			Mem.Del(ref _psExplosion);
		}

		public void Explode(ExplosionType iType)
		{
			_psExplosion.Play();
			SoundUtils.PlaySE(SEFIleInfos.SE_044);
		}
	}
}
