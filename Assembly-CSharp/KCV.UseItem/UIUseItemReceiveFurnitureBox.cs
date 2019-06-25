using DG.Tweening;
using KCV.Base;
using KCV.Scene.Port;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.UseItem
{
	public class UIUseItemReceiveFurnitureBox : PageDialog
	{
		public delegate void UIUseItemReceiveFurnitureBoxCallBack(UIUseItemReceiveFurnitureBox calledObject);

		private const int SPARK_VALUE = 10;

		[SerializeField]
		private UITexture mTexture_Box;

		[SerializeField]
		private UITexture mTexture_Lid;

		[SerializeField]
		private UITexture mTexture_Coin;

		[SerializeField]
		private UILabel mLabel_RewardText;

		[SerializeField]
		private UITexture mTexture_Flare;

		[SerializeField]
		private UITexture mTexture_Template_Sparkle;

		private UIUseItemReceiveFurnitureBoxCallBack mActionCallBack;

		private KeyControl mKeyController;

		private UITexture[] mTexture_Sparkles;

		private float[] mSparkleStart;

		private float[] mSparkleSc;

		private float[] mSparkleSp;

		private bool mIsFinishedAnimation;

		private float mStartTime;

		private float mFlareStart;

		private void Update()
		{
			if (mKeyController != null)
			{
				if (mKeyController.keyState[0].down)
				{
					OnClick();
				}
				else if (mKeyController.keyState[1].down)
				{
					OnClick();
				}
			}
		}

		public void OnClick()
		{
			mIsFinishedAnimation = true;
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public IEnumerator Show(int typ, int coins, Action onFinished)
		{
			mTexture_Sparkles = new UITexture[10];
			mSparkleStart = new float[10];
			mSparkleSc = new float[10];
			mSparkleSp = new float[10];
			mLabel_RewardText.text = "家具コイン ✕ " + Mathf.Max(0, coins);
			yield return new WaitForSeconds(0.05f);
			base.transform.localPosition = new Vector3(0f, -140f, 0f);
			yield return new WaitForSeconds(0.05f);
			mStartTime = Time.time;
			DOVirtual.Float(0f, 1f, 0.5f, delegate(float alpha)
			{
				this.mPanel.alpha = alpha;
				this.transform.localPositionY(-40f * alpha);
			}).SetEase(Ease.OutCirc);
			mTexture_Box.mainTexture = (Resources.Load("Textures/item/furniture_box/box" + (typ - 9) + "_3") as Texture);
			mTexture_Lid.mainTexture = (Resources.Load("Textures/item/furniture_box/box" + (typ - 9) + "_1") as Texture);
			yield return StartCoroutine(StartOpenLid());
			yield return new WaitForSeconds(0.2f);
			ShowCoin();
			DOVirtual.Float(0f, 1f, 1f, delegate(float alpha)
			{
				this.mLabel_RewardText.alpha = alpha;
			});
			mFlareStart = Time.time + 1f;
			for (int i = 0; i < 10; i++)
			{
				mTexture_Sparkles[i] = UnityEngine.Object.Instantiate(mTexture_Template_Sparkle.gameObject).GetComponent<UITexture>();
				if (mTexture_Sparkles[i] != null)
				{
					mTexture_Sparkles[i].name = "Sparkle" + i;
					mTexture_Sparkles[i].transform.parent = mTexture_Template_Sparkle.transform.parent;
					mTexture_Sparkles[i].transform.localScale = 0.0001f * Vector3.one;
					mTexture_Sparkles[i].transform.localPosition = new Vector3(UnityEngine.Random.value * 200f - 100f, UnityEngine.Random.value * 100f + 50f, 0f);
					mTexture_Sparkles[i].transform.Rotate(Vector3.forward, UnityEngine.Random.value * 359.99f);
					mSparkleStart[i] = Time.time + 1f + UnityEngine.Random.value;
					mSparkleSc[i] = UnityEngine.Random.value * 0.25f + 0.75f;
					mSparkleSp[i] = ((float)Convert.ToInt32((double)UnityEngine.Random.value > 0.5) - 0.5f) * (UnityEngine.Random.value * 20f + 60f);
				}
			}
			StartCoroutine(OnAnimation(onFinished));
		}

		private void ShowCoin()
		{
			DOTween.Sequence().Append(mTexture_Coin.transform.DOLocalMoveY(160f, 0.3f).SetDelay(0.625f).OnStart(delegate
			{
				mTexture_Coin.gameObject.SetActive(true);
				mTexture_Coin.alpha = 1f;
			})).Append(mTexture_Coin.transform.DOLocalMoveY(120f, 1.5f).SetLoops(int.MaxValue, LoopType.Yoyo).SetEase(Ease.InOutSine));
		}

		private IEnumerator StartOpenLid()
		{
			DOTween.Sequence().Append(mTexture_Lid.transform.DOLocalMoveY(80f, 0.3f)).Append(mTexture_Lid.transform.DOShakePosition(0.3f, 0.3f))
				.Append(mTexture_Lid.transform.DOLocalMove(new Vector3(140f, 140f), 0.25f).SetEase(Ease.OutCubic).OnStart(delegate
				{
					this.mTexture_Lid.transform.DORotate(new Vector3(0f, 0f, -40f), 0.5f);
				}));
			yield return null;
		}

		private IEnumerator OnAnimation(Action onFinished)
		{
			while (true)
			{
				if (Time.time > mStartTime + 1f)
				{
					if (Time.time > mFlareStart + 2f)
					{
						mFlareStart = Time.time + 2f;
						mTexture_Flare.transform.eulerAngles = new Vector3(0f, 0f, UnityEngine.Random.value * 40f - 20f);
					}
					mTexture_Flare.alpha = Mathf.PingPong(Time.time - mFlareStart, 1f);
					for (int i = 0; i < 10; i++)
					{
						if (Time.time > mSparkleStart[i] + 1f)
						{
							mSparkleStart[i] = Time.time + 1f;
							mTexture_Sparkles[i].transform.localPosition = new Vector3(UnityEngine.Random.value * 200f - 100f, UnityEngine.Random.value * 100f + 50f, 0f);
							mSparkleSc[i] = UnityEngine.Random.value * 0.25f + 0.75f;
						}
						mTexture_Sparkles[i].alpha = Mathf.PingPong((Time.time - mSparkleStart[i]) * 2f, 1f);
						mTexture_Sparkles[i].transform.localScale = Mathf.PingPong((Time.time - mSparkleStart[i]) * 2f, mSparkleSc[i]) * Vector3.one;
						mTexture_Sparkles[i].transform.Rotate(Vector3.forward, Time.deltaTime * mSparkleSp[i]);
					}
				}
				if (mIsFinishedAnimation)
				{
					mPanel.alpha -= Mathf.Min(mPanel.alpha, Time.deltaTime);
					if (mPanel.alpha == 0f)
					{
						onFinished();
					}
				}
				yield return null;
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Box);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Lid);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Coin);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Flare);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Template_Sparkle);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_RewardText);
			if (mTexture_Sparkles != null)
			{
				for (int i = 0; i < mTexture_Sparkles.Length; i++)
				{
					UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Sparkles[i]);
				}
			}
			mTexture_Sparkles = null;
		}
	}
}
