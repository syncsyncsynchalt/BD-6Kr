using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UISprite))]
	public class ProdMailstrom : MonoBehaviour
	{
		private UISprite _uiMailstrom;

		private Action _actOnFinished;

		private MapEventHappeningModel _clsEventHappeningModel;

		private UISprite mailstrom => this.GetComponentThis(ref _uiMailstrom);

		public static ProdMailstrom Instantiate(ProdMailstrom prefab, Transform parent, MapEventHappeningModel eventHappeningModel)
		{
			ProdMailstrom prodMailstrom = UnityEngine.Object.Instantiate(prefab);
			prodMailstrom.transform.parent = parent;
			prodMailstrom.transform.localScaleOne();
			prodMailstrom.transform.localPositionZero();
			prodMailstrom.Init(eventHappeningModel);
			return prodMailstrom;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiMailstrom);
			Mem.Del(ref _actOnFinished);
			Mem.Del(ref _clsEventHappeningModel);
		}

		private bool Init(MapEventHappeningModel eventHappeningModel)
		{
			_clsEventHappeningModel = eventHappeningModel;
			return true;
		}

		public void PlayMailstrom(UISortieShip sortieShip, ProdShipRipple ripple, Action onFinished)
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_033);
			PlayRotation();
			PlayShipMoveAnim(sortieShip).setOnComplete((Action)delegate
			{
				sortieShip.PlayLostMaterial(_clsEventHappeningModel, null);
			});
			ripple.Play(Color.white);
			_actOnFinished = onFinished;
			Observable.Timer(TimeSpan.FromSeconds(4.5)).Subscribe(delegate
			{
				OnFinished(ripple);
			});
		}

		private void PlayRotation()
		{
			base.transform.LTRotateAroundLocal(Vector3.back, 1080f, 6f).setEase(LeanTweenType.easeOutQuad).setLoopClamp();
		}

		private LTDescr PlayShipMoveAnim(UISortieShip sortieShip)
		{
			Vector3 originPos = sortieShip.transform.localPosition;
			float rotCnt = 3f;
			return base.transform.LTValue(0f, rotCnt, 3f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				float num = (!(x < rotCnt / 2f)) ? (30f * (rotCnt - x)) : (30f * x);
				Vector3 a = new Vector2(num * Mathf.Sin(x % 1f * (float)Math.PI * 2f), num * Mathf.Cos(x % 1f * (float)Math.PI * 2f));
				sortieShip.transform.localPosition = a + originPos;
			});
		}

		private void OnFinished(ProdShipRipple ripple)
		{
			ripple.Stop();
			base.transform.LTValue(1f, 0f, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				mailstrom.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					Dlg.Call(ref _actOnFinished);
					UnityEngine.Object.Destroy(base.gameObject);
				});
		}
	}
}
