using System;
using UniRx;
using UnityEngine;

namespace KCV.Title
{
	[RequireComponent(typeof(UISprite))]
	[ExecuteInEditMode]
	public class UIWritingBrush : MonoBehaviour
	{
		private int _nIndex;

		private UISprite _uiSprite;

		public UISprite sprite => this.GetComponentThis(ref _uiSprite);

		private int index
		{
			get
			{
				return _nIndex;
			}
			set
			{
				_nIndex = Mathe.MinMax2(value, 0, 12);
				sprite.spriteName = $"btn_on_{_nIndex + 2:D5}";
			}
		}

		public static UIWritingBrush Instantiate(UIWritingBrush prefab, Transform parent)
		{
			UIWritingBrush uIWritingBrush = UnityEngine.Object.Instantiate(prefab);
			uIWritingBrush.transform.parent = parent;
			uIWritingBrush.transform.localScaleZero();
			uIWritingBrush.transform.localPositionZero();
			uIWritingBrush.Init();
			return uIWritingBrush;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _nIndex);
			Mem.Del(ref _uiSprite);
		}

		private bool Init()
		{
			index = 0;
			return true;
		}

		public void Play(Action onFinished)
		{
			base.transform.localScaleOne();
			Observable.IntervalFrame(1).Select(delegate(long x)
			{
				long num = x;
				x = num + 1;
				return num;
			}).TakeWhile((long x) => x <= 12)
				.Subscribe((Action<long>)delegate(long x)
				{
					index = (int)x;
				}, (Action)delegate
				{
					Dlg.Call(ref onFinished);
				})
				.AddTo(base.gameObject);
		}
	}
}
