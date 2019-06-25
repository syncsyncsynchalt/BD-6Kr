using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class PSSplashSmoke : MonoBehaviour
	{
		public Vector3 randMinPos;

		public Vector3 randMaxPos;

		public float moveTime;

		public int count;

		public bool isPlayAwake;

		private Bezier[] _clsBezier;

		private float _timer;

		private List<ParticleSystem> _listPS;

		private List<Vector3> _listPos;

		private ParticleSystem _par;

		private bool isPlay;

		private Action _actCallback;

		private void Awake()
		{
			_actCallback = null;
			if (isPlayAwake)
			{
				setSmoke();
				play();
			}
		}

		private void Start()
		{
		}

		private void OnDestroy()
		{
			_actCallback = null;
			if (_listPS != null)
			{
				foreach (ParticleSystem listP in _listPS)
				{
					if (((Component)listP).gameObject != null)
					{
						listP.Stop();
						UnityEngine.Object.Destroy(((Component)listP).gameObject);
					}
				}
				_listPS.Clear();
			}
			_listPS = null;
		}

		private void Update()
		{
			if (isPlay && _timer < moveTime)
			{
				_timer += Time.deltaTime;
				float num = _timer * 2f;
				for (int i = 0; i < count; i++)
				{
					((Component)_listPS[i]).transform.position = _clsBezier[i].Interpolate(_timer);
				}
				if (_timer > moveTime)
				{
					SetDestroy();
				}
			}
		}

		public void SetDestroy()
		{
			for (int i = 0; i < _listPS.Count; i++)
			{
				_listPS[i].Stop();
			}
		}

		public void setSmoke()
		{
			if (count <= 5)
			{
				count = 5;
			}
			_listPS = new List<ParticleSystem>();
			_listPos = new List<Vector3>();
			for (int i = 0; i < count; i++)
			{
				_listPS.Add(Instantiate());
			}
			Vector3 position = base.transform.position;
			float fMin = randMinPos.x + (randMaxPos.x - randMinPos.x) * 0.8f;
			float fMin2 = randMinPos.x + (randMaxPos.x - randMinPos.x) * 0.5f;
			float fMin3 = randMinPos.y + (randMaxPos.y - randMinPos.y) * 0.8f;
			float fMin4 = randMinPos.y + (randMaxPos.y - randMinPos.y) * 0.5f;
			_listPos.Add(new Vector3(position.x, position.y + XorRandom.GetFLim(randMinPos.y, randMaxPos.y), position.z + randMaxPos.z));
			_listPos.Add(new Vector3(position.x + XorRandom.GetFLim(fMin, randMaxPos.x), position.y + XorRandom.GetFLim(randMinPos.y, randMaxPos.y), position.z + XorRandom.GetFLim(fMin4, randMaxPos.y)));
			_listPos.Add(new Vector3(position.x - XorRandom.GetFLim(fMin, randMaxPos.x), position.y + XorRandom.GetFLim(randMinPos.y, randMaxPos.y), position.z + XorRandom.GetFLim(fMin4, randMaxPos.y)));
			_listPos.Add(new Vector3(position.x + XorRandom.GetFLim(fMin2, randMaxPos.x), position.y + XorRandom.GetFLim(randMinPos.y, randMaxPos.y), position.z - XorRandom.GetFLim(fMin3, randMaxPos.y)));
			_listPos.Add(new Vector3(position.x - XorRandom.GetFLim(fMin2, randMaxPos.x), position.y + XorRandom.GetFLim(randMinPos.y, randMaxPos.y), position.z - XorRandom.GetFLim(fMin3, randMaxPos.y)));
			float[] array = new float[2];
			float[] array2 = new float[2];
			for (int j = 0; j < count - 5; j++)
			{
				array[0] = XorRandom.GetFLim(randMinPos.x, randMaxPos.x);
				array[1] = XorRandom.GetFLim(randMaxPos.x * -1f, randMinPos.x * -1f);
				array2[0] = XorRandom.GetFLim(randMinPos.z, randMaxPos.z);
				array2[1] = XorRandom.GetFLim(randMaxPos.z * -1f, randMinPos.z * -1f);
				_listPos.Add(new Vector3(position.x + array[XorRandom.GetILim(0, 1)], position.y + XorRandom.GetFLim(randMinPos.y, randMaxPos.y), position.z + array2[XorRandom.GetILim(0, 1)]));
			}
		}

		public void play()
		{
			_timer = 0f;
			_clsBezier = new Bezier[count];
			Vector3 vector5 = default(Vector3);
			for (int i = 0; i < count; i++)
			{
				Vector3 vector = Vector3.Lerp(base.transform.position, _listPos[i], 0.3f);
				Vector3 vector2 = Vector3.Lerp(base.transform.position, _listPos[i], 0.5f);
				Vector3 vector3 = _listPos[i];
				float x = vector3.x;
				Vector3 position = base.transform.position;
				float y = position.y - 5f;
				Vector3 vector4 = _listPos[i];
				vector5 = new Vector3(x, y, vector4.z);
				Bezier[] clsBezier = _clsBezier;
				int num = i;
				Vector3 position2 = base.transform.position;
				Vector3 end = vector5;
				float x2 = vector.x;
				Vector3 vector6 = _listPos[i];
				Vector3 mid = new Vector3(x2, vector6.y, vector.z);
				float x3 = vector2.x;
				Vector3 vector7 = _listPos[i];
				clsBezier[num] = new Bezier(Bezier.BezierType.Cubic, position2, end, mid, new Vector3(x3, vector7.y - 2f, vector2.z));
			}
			isPlay = true;
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

		private ParticleSystem Instantiate()
		{
			if ((UnityEngine.Object)_par == null)
			{
				_par = ParticleFile.Load<ParticleSystem>(ParticleFileInfos.BattlePSSplashSmoke);
			}
			ParticleSystem val = UnityEngine.Object.Instantiate<ParticleSystem>(_par);
			((Component)val).transform.parent = base.transform;
			((Component)val).transform.position = base.transform.position;
			return val;
		}
	}
}
