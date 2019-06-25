using KCV.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class PSTorpedoWake : MonoBehaviour
	{
		private int _attakerIndex;

		private float _moveTime;

		private bool _isDetonation;

		private bool _isMiss;

		private ParticleSystem _psTorpedoWake;

		private Vector3 _vecTarget;

		private Action _actCallback;

		[SerializeField]
		private ParticleSystem splashT;

		public bool GetMiss()
		{
			return _isMiss;
		}

		public int GetAttakerIndex()
		{
			return _attakerIndex;
		}

		private bool _init()
		{
			_attakerIndex = 0;
			_moveTime = 0f;
			_isDetonation = false;
			_isMiss = false;
			_actCallback = null;
			_psTorpedoWake = base.gameObject.SafeGetComponent<ParticleSystem>();
			return true;
		}

		private void OnDestroy()
		{
			if ((UnityEngine.Object)splashT != null)
			{
				splashT.Stop();
				splashT.Clear();
				UnityEngine.Object.Destroy(((Component)splashT).gameObject);
			}
			_psTorpedoWake.Stop();
			_psTorpedoWake.Clear();
			((Component)_psTorpedoWake).SetActive(isActive: false);
			UnityEngine.Object.Destroy(((Component)_psTorpedoWake).gameObject);
			Mem.Del(ref _vecTarget);
			Mem.Del(ref _psTorpedoWake);
			Mem.Del(ref splashT);
			Mem.Del(ref _actCallback);
		}

		public void Injection(iTween.EaseType eType, bool isPlaySE, bool isTC, Action callback)
		{
			_actCallback = callback;
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", _vecTarget);
			hashtable.Add("isLocal", false);
			hashtable.Add("delay", 0.1f);
			hashtable.Add("time", _moveTime);
			hashtable.Add("easeType", eType);
			hashtable.Add("oncomplete", "_onFinishedInjection");
			hashtable.Add("oncompletetarget", base.gameObject);
			base.gameObject.MoveTo(hashtable);
			base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
			_psTorpedoWake.Play();
			if (isPlaySE)
			{
				if (isTC)
				{
					SoundUtils.PlaySE(SEFIleInfos.SE_905);
				}
				else
				{
					SoundUtils.PlaySE(SEFIleInfos.SE_904);
				}
			}
		}

		public void Reset()
		{
			base.transform.iTweenStop();
			_psTorpedoWake.Stop();
		}

		public void ReStart(float time, iTween.EaseType eType)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", _vecTarget);
			hashtable.Add("isLocal", false);
			hashtable.Add("time", time);
			hashtable.Add("easeType", eType);
			hashtable.Add("oncomplete", "_onFinishedInjection");
			hashtable.Add("oncompletetarget", base.gameObject);
			base.gameObject.MoveTo(hashtable);
			base.transform.rotation = Quaternion.Euler(Vector3.zero);
			_psTorpedoWake.Play();
		}

		public void ReStart(Vector3 fromPos, Vector3 toPos, float time, iTween.EaseType eType, Action callback)
		{
			_actCallback = callback;
			base.transform.position = fromPos;
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", toPos);
			hashtable.Add("isLocal", false);
			hashtable.Add("delay", 0.1f);
			hashtable.Add("time", time);
			hashtable.Add("easeType", eType);
			hashtable.Add("oncomplete", "_onFinishedInjection");
			hashtable.Add("oncompletetarget", base.gameObject);
			base.gameObject.MoveTo(hashtable);
			base.transform.rotation = Quaternion.Euler(Vector3.zero);
			_psTorpedoWake.Play();
		}

		public void PlaySplash()
		{
			splashT = ((!((UnityEngine.Object)BattleTaskManager.GetParticleFile().splash == null)) ? UnityEngine.Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().splash) : BattleTaskManager.GetParticleFile().splash);
			((Component)splashT).SetActive(isActive: true);
			((Component)splashT).transform.parent = BattleTaskManager.GetBattleField().transform;
			((Component)splashT).transform.position = _vecTarget;
			splashT.Play();
		}

		public void PlaySplash(Vector3 fromPos)
		{
			splashT = ((!((UnityEngine.Object)BattleTaskManager.GetParticleFile().splash == null)) ? UnityEngine.Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().splash) : BattleTaskManager.GetParticleFile().splash);
			((Component)splashT).SetActive(isActive: true);
			((Component)splashT).transform.parent = BattleTaskManager.GetBattleField().transform;
			((Component)splashT).transform.position = fromPos;
			splashT.Play();
		}

		private void _onFinishedInjection()
		{
			if (_isDetonation)
			{
				splashT = ((!((UnityEngine.Object)BattleTaskManager.GetParticleFile().splash == null)) ? UnityEngine.Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().splash) : BattleTaskManager.GetParticleFile().splash);
				((Component)splashT).SetActive(isActive: true);
				((Component)splashT).transform.parent = BattleTaskManager.GetBattleField().transform;
				((Component)splashT).transform.position = _vecTarget;
				splashT.Play();
			}
			_psTorpedoWake.Stop();
			_psTorpedoWake.time = 0f;
			if (_actCallback != null)
			{
				_actCallback();
			}
		}

		private IEnumerator _delayDiscard(float delay)
		{
			yield return new WaitForSeconds(delay);
			if (_actCallback != null)
			{
				_actCallback();
			}
			yield return new WaitForSeconds(0.001f);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public static PSTorpedoWake Instantiate(PSTorpedoWake prefab, Transform parent, Vector3 injectionVec, Vector3 target, int attacker, float _time, bool isDet, bool isMiss)
		{
			PSTorpedoWake pSTorpedoWake = UnityEngine.Object.Instantiate(prefab);
			pSTorpedoWake._init();
			((Component)pSTorpedoWake._psTorpedoWake).transform.parent = parent;
			pSTorpedoWake.transform.parent = BattleTaskManager.GetBattleField().transform;
			pSTorpedoWake.transform.position = injectionVec;
			pSTorpedoWake.transform.transform.rotation = Quaternion.Euler(new Vector3(-180f, 0f, 0f));
			pSTorpedoWake._vecTarget = target;
			pSTorpedoWake._attakerIndex = attacker;
			pSTorpedoWake._moveTime = _time;
			pSTorpedoWake._psTorpedoWake.Stop();
			pSTorpedoWake._isDetonation = isDet;
			pSTorpedoWake._isMiss = isMiss;
			return pSTorpedoWake;
		}
	}
}
