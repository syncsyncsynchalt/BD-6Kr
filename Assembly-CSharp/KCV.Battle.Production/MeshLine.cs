using System;
using System.Collections;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class MeshLine : MonoBehaviour
	{
		private Transform _traTorpedoTarget;

		private Vector3 _vecTorpedoTarget;

		private float _moveTime;

		private float _delayTime;

		private Action _actCallback;

		private void Awake()
		{
			_actCallback = null;
		}

		private void Start()
		{
		}

		private void OnDestroy()
		{
			_traTorpedoTarget = null;
			_actCallback = null;
		}

		private void Update()
		{
		}

		public void SetDestroy()
		{
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			Vector3 euler = new Vector3(eulerAngles.x, eulerAngles.y + 180f, eulerAngles.z);
			base.transform.rotation = Quaternion.Euler(euler);
			base.transform.position = _vecTorpedoTarget;
			Hashtable hashtable = new Hashtable();
			hashtable.Add("z", 0f);
			hashtable.Add("isLocal", false);
			hashtable.Add("time", 0.5f);
			hashtable.Add("delay", 0f);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("oncomplete", "_onFinishedInjection");
			hashtable.Add("oncompletetarget", base.gameObject);
			base.gameObject.ScaleTo(hashtable);
		}

		public void Extend(bool isVec, Action callback)
		{
			_actCallback = callback;
			base.transform.LookAt(_vecTorpedoTarget);
			Vector3 vector = Mathe.Direction(base.transform.position, _vecTorpedoTarget);
			Hashtable hashtable = new Hashtable();
			if (isVec)
			{
				hashtable.Add("z", vector.z);
			}
			else
			{
				hashtable.Add("z", vector.z * -1f);
			}
			hashtable.Add("isLocal", false);
			hashtable.Add("time", _moveTime);
			hashtable.Add("delay", _delayTime);
			hashtable.Add("easeType", iTween.EaseType.linear);
			base.gameObject.ScaleTo(hashtable);
		}

		private void _onFinishedInjection()
		{
			StartCoroutine(_delayDiscard(0.1f));
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

		public static MeshLine Instantiate(MeshLine prefab, Transform parent, Vector3 injectionVec, Vector3 target, float _time, float _delay)
		{
			MeshLine meshLine = UnityEngine.Object.Instantiate(prefab);
			meshLine.transform.parent = BattleTaskManager.GetBattleField().transform;
			meshLine.transform.position = injectionVec;
			Vector3 vector = meshLine._vecTorpedoTarget = new Vector3(target.x, 1f, target.z);
			meshLine._moveTime = _time;
			meshLine._delayTime = _delay;
			return meshLine;
		}
	}
}
