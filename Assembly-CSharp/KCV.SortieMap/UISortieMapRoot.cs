using LT.Tweening;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UISprite))]
	public class UISortieMapRoot : MonoBehaviour
	{
		[SerializeField]
		private bool _isRebellionRoute;

		[SerializeField]
		private UISprite _uiRoute;

		[SerializeField]
		private UISprite _uiRebellionRoute;

		[SerializeField]
		private UISprite _uiRebellionBrightPoint;

		private bool _isPassed;

		[Button("SameRoute", "Sprite設定", new object[]
		{

		})]
		[SerializeField]
		private int _nSameRoute = 1;

		public UISprite route => this.GetComponentThis(ref _uiRoute);

		public UISprite rebellionRoute
		{
			get
			{
				if (_uiRebellionRoute == null)
				{
					if ((bool)base.transform.FindChild("RebellionRoute"))
					{
						_uiRebellionRoute = ((Component)base.transform.FindChild("RebellionRoute")).GetComponent<UISprite>();
					}
					else
					{
						GameObject gameObject = new GameObject("RebellionRoute");
						gameObject.transform.parent = base.transform;
						gameObject.transform.localScaleOne();
						gameObject.transform.localPositionZero();
						UISprite uISprite = _uiRebellionRoute = gameObject.AddComponent<UISprite>();
					}
				}
				return _uiRebellionRoute;
			}
		}

		public UISprite rebellionBrightPoint
		{
			get
			{
				if (_uiRebellionBrightPoint == null)
				{
					if ((bool)base.transform.FindChild("RebellionBrightPoint"))
					{
						_uiRebellionBrightPoint = ((Component)base.transform.FindChild("RebellionBrightPoint")).GetComponent<UISprite>();
					}
					else
					{
						GameObject gameObject = new GameObject("RebellionBrightPoint");
						gameObject.transform.parent = base.transform;
						gameObject.transform.localScaleOne();
						gameObject.transform.localPositionZero();
						UISprite uISprite = _uiRebellionBrightPoint = gameObject.AddComponent<UISprite>();
					}
				}
				return _uiRebellionBrightPoint;
			}
		}

		public bool isRebellionRoute => _isRebellionRoute;

		public bool isPassed
		{
			get
			{
				return _isPassed;
			}
			set
			{
				if (value)
				{
					route.enabled = true;
					if (isRebellionRoute)
					{
						rebellionBrightPoint.transform.LTCancel();
						this.rebellionRoute.SetActive(isActive: false);
						rebellionBrightPoint.SetActive(isActive: false);
						UISprite rebellionRoute = this.rebellionRoute;
						bool enabled = false;
						rebellionBrightPoint.enabled = enabled;
						rebellionRoute.enabled = enabled;
					}
				}
				else
				{
					route.enabled = false;
					if (isRebellionRoute)
					{
						this.rebellionRoute.SetActive(isActive: true);
						rebellionBrightPoint.SetActive(isActive: true);
						UISprite rebellionRoute2 = this.rebellionRoute;
						bool enabled = true;
						rebellionBrightPoint.enabled = enabled;
						rebellionRoute2.enabled = enabled;
						rebellionBrightPoint.transform.LTValue(0f, 1f, 1f).setEase(LeanTweenType.linear).setLoopPingPong()
							.setOnUpdate(delegate(float x)
							{
								rebellionBrightPoint.alpha = x;
							});
					}
				}
			}
		}

		private void Awake()
		{
			_isPassed = false;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiRoute);
			Mem.Del(ref _uiRebellionRoute);
			Mem.Del(ref _uiRebellionBrightPoint);
		}

		public void Passed(bool isPassed)
		{
			route.enabled = isPassed;
			if (isRebellionRoute)
			{
				UISprite rebellionRoute = this.rebellionRoute;
				bool enabled = false;
				rebellionBrightPoint.enabled = enabled;
				rebellionRoute.enabled = enabled;
			}
		}
	}
}
