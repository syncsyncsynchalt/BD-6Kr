using KCV.Battle.Utils;
using KCV.Generic;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class UIBufferCircle : InstantiateObject<UIBufferCircle>
	{
		[Serializable]
		private struct Param : IDisposable
		{
			public float gearRotateSpeed;

			public float circleRotateSpeed;

			public float lookAtLineSize;

			public float lookAtLineAnimTime;

			public float focusCircleScale;

			public float focusCircleScalingTime;

			public float focusCircleColorlingTime;

			public Color focusCircleColor;

			public void Dispose()
			{
				Mem.Del(ref gearRotateSpeed);
				Mem.Del(ref circleRotateSpeed);
				Mem.Del(ref lookAtLineSize);
				Mem.Del(ref lookAtLineAnimTime);
				Mem.Del(ref focusCircleScale);
				Mem.Del(ref focusCircleScalingTime);
				Mem.Del(ref focusCircleColorlingTime);
				Mem.Del(ref focusCircleColor);
			}
		}

		[SerializeField]
		private float _fCircleScale = 50f;

		[SerializeField]
		private Material _material;

		[SerializeField]
		private List<MeshRenderer> _listMeshRenderer;

		[SerializeField]
		private List<Texture2D> _listCircleTexture;

		[SerializeField]
		private MeshRenderer _mrGear;

		[SerializeField]
		private Texture2D _texGear;

		[SerializeField]
		private MeshRenderer _mrLine;

		[SerializeField]
		private Texture2D _texLine;

		[SerializeField]
		private Param _strParam;

		[Button("ReflectionMaterial", "マテリアル反映", new object[]
		{

		})]
		[SerializeField]
		private int _nReflectionMaterial;

		private Transform _traTarget;

		private bool _isLootAtLine;

		private Quaternion _quaStartLine;

		private Color _cDefaultBaseColor;

		private Color _cDefaultLineColor;

		public static UIBufferCircle Instantiate(UIBufferCircle prefab, Transform parent, FleetType iType, Transform target)
		{
			UIBufferCircle uIBufferCircle = InstantiateObject<UIBufferCircle>.Instantiate(prefab);
			uIBufferCircle.transform.localScale = new Vector3(uIBufferCircle._fCircleScale, 0f, uIBufferCircle._fCircleScale);
			uIBufferCircle.transform.parent = parent;
			uIBufferCircle.transform.localPositionZero();
			uIBufferCircle.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
			uIBufferCircle.Init(iType, target);
			return uIBufferCircle;
		}

		private bool Init(FleetType iType, Transform target)
		{
			ReflectionMaterial();
			SetCircleColor(iType);
			PlayGearAnimation();
			return true;
		}

		private void OnDestroy()
		{
			Transform p = ((Component)_listMeshRenderer[0]).transform;
			Mem.DelMeshSafe(ref p);
			p = ((Component)_listMeshRenderer[1]).transform;
			Mem.DelMeshSafe(ref p);
			p = ((Component)_mrGear).transform;
			Mem.DelMeshSafe(ref p);
			p = ((Component)_mrLine).transform;
			Mem.DelMeshSafe(ref p);
			Mem.Del(ref _fCircleScale);
			Mem.Del(ref _material);
			Mem.DelListSafe(ref _listMeshRenderer);
			Mem.DelListSafe(ref _listCircleTexture);
			Mem.Del(ref _mrGear);
			Mem.Del(ref _texGear);
			Mem.Del(ref _mrLine);
			Mem.Del(ref _texLine);
			Mem.DelIDisposableSafe(ref _strParam);
			Mem.Del(ref _nReflectionMaterial);
			Mem.Del(ref _traTarget);
			Mem.Del(ref _isLootAtLine);
			Mem.Del(ref _quaStartLine);
			Mem.Del(ref _cDefaultBaseColor);
			Mem.Del(ref _cDefaultLineColor);
			Mem.Del(ref p);
		}

		public bool Run()
		{
			if (_isLootAtLine && (UnityEngine.Object)_mrLine != null)
			{
				((Component)_mrLine).transform.LookAt(_traTarget);
			}
			return true;
		}

		public void SetDefault()
		{
			_listMeshRenderer.ForEach(delegate(MeshRenderer x)
			{
				((Component)x).transform.localScale = Vector3.one;
				x.sharedMaterial.color = _cDefaultBaseColor;
			});
			((Component)_mrGear).transform.localScale = Vector3.one;
			_mrGear.sharedMaterial.color = _cDefaultBaseColor;
			((Component)_mrLine).transform.localScale = new Vector3(0.6f, 1f, 0.8f);
			_mrLine.sharedMaterial.color = _cDefaultLineColor;
		}

		public void CalcInitLineRotation(Transform target)
		{
			_traTarget = target;
			((Component)_mrLine).transform.LookAt(target.position);
			_quaStartLine = ((Component)_mrLine).transform.rotation;
		}

		public void PlayLineAnimation()
		{
			((Component)_mrLine).transform.LTCancel();
			((Component)_mrLine).transform.LTRotateAroundLocal(Vector3.up, XorRandom.GetFLim(0.1f, 1f) * 50f, XorRandom.GetFLim(2.5f, 4.6f)).setEase(LeanTweenType.linear).setLoopPingPong();
		}

		public void PlayLookAtLine()
		{
			((Component)_mrLine).transform.LTCancel();
			((Component)_mrLine).transform.LTValue(_cDefaultLineColor, new Color(KCVColor.ColorRate(0f), KCVColor.ColorRate(255f), KCVColor.ColorRate(60f), KCVColor.ColorRate(175f)), _strParam.lookAtLineAnimTime).setEase(LeanTweenType.easeInQuint).setOnUpdate(delegate(Color color)
			{
				_mrLine.sharedMaterial.color = color;
			});
			((Component)_mrLine).transform.LookTo(_traTarget.position, _strParam.lookAtLineAnimTime, iTween.EaseType.easeInQuint, delegate
			{
				_isLootAtLine = true;
			});
			((Component)_mrLine).transform.LTScale(Vector3.one * _strParam.lookAtLineSize, _strParam.lookAtLineAnimTime).setEase(LeanTweenType.easeInQuint);
		}

		public void PlayLookAtLine2Assult()
		{
			((Component)_mrLine).transform.LTCancel();
			((Component)_mrLine).transform.LookTo(_traTarget.position, _strParam.lookAtLineAnimTime, iTween.EaseType.easeInQuint, delegate
			{
				_isLootAtLine = true;
			});
			((Component)_mrLine).transform.LTValue(_cDefaultLineColor, new Color(KCVColor.ColorRate(0f), KCVColor.ColorRate(255f), KCVColor.ColorRate(255f), KCVColor.ColorRate(175f)), _strParam.lookAtLineAnimTime).setEase(LeanTweenType.easeInQuint).setOnUpdate(delegate(Color color)
			{
				_mrLine.sharedMaterial.color = color;
			});
		}

		public void StopLineAnimation()
		{
			((Component)_mrLine).transform.LTCancel();
		}

		public void PlayGearAnimation()
		{
			((Component)_listMeshRenderer[1]).transform.LTRotateAround(Vector3.up, 360f, _strParam.circleRotateSpeed).setEase(LeanTweenType.linear).setLoopClamp();
			((Component)_mrGear).transform.LTRotateAround(Vector3.up, -360f, _strParam.gearRotateSpeed).setEase(LeanTweenType.linear).setLoopClamp();
		}

		public void StopGearAnimation()
		{
			((Component)_mrLine).transform.LTCancel();
			((Component)_mrGear).transform.LTCancel();
		}

		public void PlayFocusCircleAnimation(bool isFocus)
		{
			if (isFocus)
			{
				base.transform.LTValue(1f, _strParam.focusCircleScale, _strParam.focusCircleScalingTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					UIBufferCircle uIBufferCircle2 = this;
					_listMeshRenderer.ForEach(delegate(MeshRenderer obj)
					{
						((Component)obj).transform.localScale = Vector3.one * x;
					});
					((Component)_mrGear).transform.localScale = Vector3.one * x;
				});
				base.transform.LTValue(_cDefaultBaseColor, _strParam.focusCircleColor, _strParam.focusCircleColorlingTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(Color x)
				{
					UIBufferCircle uIBufferCircle = this;
					_listMeshRenderer.ForEach(delegate(MeshRenderer y)
					{
						y.sharedMaterial.color = x;
					});
					_mrGear.sharedMaterial.color = x;
				});
			}
			else
			{
				Color baseColor = _cDefaultBaseColor;
				base.transform.LTValue(_cDefaultBaseColor.a, Mathe.Rate(0f, 255f, 70f), _strParam.focusCircleColorlingTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					baseColor.a = x;
					_listMeshRenderer.ForEach(delegate(MeshRenderer y)
					{
						y.sharedMaterial.color = baseColor;
					});
					_mrGear.sharedMaterial.color = baseColor;
				});
			}
		}

		private void ReflectionMaterial()
		{
			int cnt = 0;
			_listMeshRenderer.ForEach(delegate(MeshRenderer x)
			{
				x.material = new Material(_material);
				x.sharedMaterial.mainTexture = _listCircleTexture[cnt];
				cnt++;
			});
			if (Application.isPlaying)
			{
				Mem.DelListSafe(ref _listCircleTexture);
			}
			_mrGear.material = new Material(_material);
			_mrGear.sharedMaterial.mainTexture = _texGear;
			if (Application.isPlaying)
			{
				Mem.Del(ref _texGear);
			}
			_mrLine.material = new Material(_material);
			_mrLine.sharedMaterial.mainTexture = _texLine;
			if (Application.isPlaying)
			{
				Mem.Del(ref _texLine);
			}
			if (Application.isPlaying)
			{
				Mem.Del(ref _material);
			}
		}

		private void SetCircleColor(FleetType iType)
		{
			Color baseColor = (iType != 0) ? new Color(KCVColor.ColorRate(238f), KCVColor.ColorRate(35f), KCVColor.ColorRate(36f), 1f) : new Color(KCVColor.ColorRate(0f), KCVColor.ColorRate(255f), KCVColor.ColorRate(255f), 1f);
			_cDefaultBaseColor = baseColor;
			_listMeshRenderer.ForEach(delegate(MeshRenderer x)
			{
				x.sharedMaterial.color = baseColor;
			});
			_mrGear.sharedMaterial.color = baseColor;
			Color color = (iType != 0) ? new Color(KCVColor.ColorRate(238f), KCVColor.ColorRate(35f), KCVColor.ColorRate(36f), KCVColor.ColorRate(103f)) : new Color(KCVColor.ColorRate(0f), KCVColor.ColorRate(255f), KCVColor.ColorRate(255f), KCVColor.ColorRate(103f));
			_mrLine.sharedMaterial.color = color;
			_cDefaultLineColor = color;
		}
	}
}
