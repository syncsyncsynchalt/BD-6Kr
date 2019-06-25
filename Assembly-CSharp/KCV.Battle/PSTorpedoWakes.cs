using KCV.Battle.Production;
using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class PSTorpedoWakes : IDisposable
	{
		private PSTorpedoWake _torpedoWake;

		private PSTorpedoWake _torpedoWakeD;

		private List<PSTorpedoWake> _listTorpedoWake;

		private Vector3[] _vecProtectTorpedo;

		public PSTorpedoWakes()
		{
			Init();
		}

		public bool Init()
		{
			_torpedoWake = null;
			_torpedoWakeD = null;
			_vecProtectTorpedo = new Vector3[6];
			_listTorpedoWake = new List<PSTorpedoWake>();
			return true;
		}

		public void SetDestroy()
		{
			Dispose();
			Mem.Del(ref _torpedoWake);
			Mem.Del(ref _torpedoWakeD);
			Mem.DelListSafe(ref _listTorpedoWake);
		}

		public void Dispose()
		{
			if (_listTorpedoWake != null)
			{
				foreach (PSTorpedoWake item in _listTorpedoWake)
				{
					if (item != null)
					{
						UnityEngine.Object.Destroy(item);
					}
				}
				_listTorpedoWake.Clear();
			}
			_listTorpedoWake = null;
		}

		public void AddInstantiates(Transform parent, Vector3 injectionVec, Vector3 targetVec, bool isFull, int attackerIndex, float time, bool isDet, bool isMiss)
		{
			if (_torpedoWake == null)
			{
				_torpedoWake = ParticleFile.Load<PSTorpedoWake>(ParticleFileInfos.PARTICLE_FILE_INFOS_ID_ST);
			}
			if (_torpedoWakeD == null)
			{
				_torpedoWakeD = ParticleFile.Load<PSTorpedoWake>(ParticleFileInfos.BattlePSTorpedowakeD);
			}
			_listTorpedoWake.Add(PSTorpedoWake.Instantiate((!isFull) ? _torpedoWake : _torpedoWakeD, parent, injectionVec, targetVec, attackerIndex, time, isDet, isMiss));
		}

		public void InitProtectVector()
		{
			if (_vecProtectTorpedo != null)
			{
				for (int i = 0; i < 6; i++)
				{
					_vecProtectTorpedo[i] = Vector3.zero;
				}
			}
		}

		public void SetProtectVector(int index, Vector3 pos)
		{
			if (_vecProtectTorpedo != null && index >= 0 && index < 6)
			{
				_vecProtectTorpedo[index] = pos;
			}
		}

		public void InjectionAll()
		{
			foreach (PSTorpedoWake item in _listTorpedoWake)
			{
				item.Injection(iTween.EaseType.easeInCubic, isPlaySE: false, isTC: false, delegate
				{
				});
			}
			if (_listTorpedoWake.Count > 0)
			{
				SoundUtils.PlaySE(SEFIleInfos.BattleTorpedo);
			}
		}

		public void ReStartAll()
		{
			if (_listTorpedoWake != null)
			{
				for (int i = 0; i < _listTorpedoWake.Count; i++)
				{
					_listTorpedoWake[i].ReStart(0.9f, iTween.EaseType.linear);
				}
			}
		}

		public void PlaySplashAll()
		{
			if (_listTorpedoWake == null)
			{
				return;
			}
			for (int i = 0; i < _listTorpedoWake.Count; i++)
			{
				if (!_listTorpedoWake[i].GetMiss())
				{
					if (_vecProtectTorpedo[i] != Vector3.zero)
					{
						_listTorpedoWake[i].PlaySplash(_vecProtectTorpedo[i]);
					}
					else
					{
						_listTorpedoWake[i].PlaySplash();
					}
				}
			}
		}
	}
}
