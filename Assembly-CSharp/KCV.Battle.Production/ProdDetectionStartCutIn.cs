using KCV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	[RequireComponent(typeof(Animation))]
	public class ProdDetectionStartCutIn : MonoBehaviour
	{
		[SerializeField]
		private List<UISprite> _listCircles;

		[SerializeField]
		private List<UISprite> _listLabels;

		private UIPanel _uiPanel;

		private Animation _anim;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		private new Animation animation => this.GetComponentThis(ref _anim);

		public static ProdDetectionStartCutIn Instantiate(ProdDetectionStartCutIn prefab, Transform parent)
		{
			ProdDetectionStartCutIn prodDetectionStartCutIn = UnityEngine.Object.Instantiate(prefab);
			prodDetectionStartCutIn.transform.parent = parent;
			prodDetectionStartCutIn.transform.localScaleZero();
			prodDetectionStartCutIn.transform.localPositionZero();
			prodDetectionStartCutIn.Init();
			return prodDetectionStartCutIn;
		}

		private void OnDestroy()
		{
			_listCircles.ForEach(delegate(UISprite x)
			{
				x.Clear();
			});
			_listLabels.ForEach(delegate(UISprite x)
			{
				x.Clear();
			});
			Mem.DelListSafe(ref _listCircles);
			Mem.DelListSafe(ref _listLabels);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _anim);
		}

		private bool Init()
		{
			panel.widgetsAreStatic = true;
			return true;
		}

		public UniRx.IObservable<bool> Play()
		{
			return Observable.FromCoroutine((UniRx.IObserver<bool> observer) => AnimationObserver(observer));
		}

		private IEnumerator AnimationObserver(UniRx.IObserver<bool> observer)
		{
			if (animation.isPlaying)
			{
				observer.OnNext(value: true);
				observer.OnCompleted();
			}
			panel.widgetsAreStatic = false;
			base.transform.localScaleOne();
			animation.Play(((UnityEngine.Object)animation.clip).name);


            throw new NotImplementedException("‚È‚É‚±‚ê");
            // yield return Observable.Timer(TimeSpan.FromSeconds(animation.get_Item(((UnityEngine.Object)animation.clip).name).length)).StartAsCoroutine();
            yield return Observable.Timer(TimeSpan.FromSeconds(0)).StartAsCoroutine();


            observer.OnNext(value: true);
			observer.OnCompleted();
			panel.widgetsAreStatic = true;
		}

		private void PlayMessageSE()
		{
			SoundUtils.PlaySE(SEFIleInfos.BattleNightMessage);
		}
	}
}
