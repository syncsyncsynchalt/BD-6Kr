using System;
using UniRx;
using UnityEngine;

public class TouchEventManager : MonoBehaviour
{
	private IDisposable _disStream;

	private Vector2 _vTouchPos;

	private ParticleSystem[] _touchParticles;

	private bool isCancel;

	private void Awake()
	{
		Init();
		StartTouchStream();
	}

	public bool Init()
	{
		_disStream = null;
		_touchParticles = (ParticleSystem[])new ParticleSystem[1];
		for (int i = 0; i < 1; i++)
		{
			Util.FindParentToChild<ParticleSystem>(ref _touchParticles[i], base.transform, "TouchParticle" + (i + 1));
		}
		return true;
	}

	private void OnDestroy()
	{
		Mem.DelIDisposableSafe(ref _disStream);
		Mem.Del(ref _vTouchPos);
		Mem.Del(ref _touchParticles);
	}

	public void StartTouchStream()
	{
		if (_disStream != null)
		{
			_disStream.Dispose();
		}
		_disStream = (from x in Observable.EveryUpdate()
			where Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began
			select (Input.touchCount <= 0) ? new Vector2(1000f, 0f) : Input.GetTouch(0).position).Subscribe(delegate(Vector2 pos)
		{
			_startParticle(pos);
		});
	}

	public void StopStream()
	{
		_disStream.Dispose();
	}

	public void _startParticle(Vector2 pos)
	{
		if (_touchParticles != null)
		{
			if (isCancel)
			{
				isCancel = false;
				return;
			}
			_touchParticles[0].Stop();
			_touchParticles[0].Clear();
			_touchParticles[0].time = 0f;
			Vector2 v = new Vector2(pos.x - 480f, pos.y - 272f);
			((Component)_touchParticles[0]).transform.localPosition = v;
			_touchParticles[0].Play();
		}
	}

	public void stopParticle()
	{
		for (int i = 0; i < 5; i++)
		{
			if (!((UnityEngine.Object)_touchParticles[i] == null) && _touchParticles[i].isPlaying)
			{
				_touchParticles[i].Stop();
				_touchParticles[i].Clear();
				_touchParticles[i].time = 0f;
			}
		}
		isCancel = true;
	}
}
