using DG.Tweening;
using local.models;
using UnityEngine;

[RequireComponent(typeof(UIWidget))]
public class UIMapHP : MonoBehaviour
{
	private const int MAX_GAUGE_SIZE = 180;

	private const int MIN_GAUGE_SIZE = 0;

	[SerializeField]
	private UITexture mTexture_Base;

	[SerializeField]
	private UITexture mTexture_Light;

	[SerializeField]
	private UITexture mTexture_Gauge;

	private UIWidget mWidgetThis;

	private MapHPModel mMapHPModel;

	private Tweener mTweener;

	private void Awake()
	{
		mWidgetThis = GetComponent<UIWidget>();
		mWidgetThis.alpha = 0.001f;
	}

	public void Initialize(MapHPModel model)
	{
		mWidgetThis.alpha = 1f;
		mMapHPModel = model;
		float percentage = mMapHPModel.NowValue % mMapHPModel.MaxValue;
		InitializeHPGauge(percentage);
	}

	public void Play()
	{
		if (mTweener != null)
		{
			mTweener.Kill();
			mTweener = null;
		}
		mTweener = DOVirtual.Float(mTexture_Light.alpha, 0.3f, 0.8f, delegate(float alpha)
		{
			mTexture_Light.alpha = alpha;
		}).SetLoops(int.MaxValue, LoopType.Yoyo);
	}

	public void Stop()
	{
		if (mTweener != null)
		{
			mTweener.Kill();
			mTweener = null;
		}
	}

	private void InitializeHPGauge(float percentage)
	{
		mTexture_Gauge.width = (int)(180f * percentage);
	}
}
