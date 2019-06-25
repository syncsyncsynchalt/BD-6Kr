using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Remodel
{
	[RequireComponent(typeof(UIPanel))]
	public class MarriageManager : MonoBehaviour
	{
		public delegate void OnFinish();

		private UIPanel mPanelThis;

		private bool isKeyControllable;

		private bool isButtonPressed;

		private KeyControl mKeyController;

		[SerializeField]
		private UITexture bg;

		[SerializeField]
		private UITexture white;

		[SerializeField]
		private UITexture vignette;

		[SerializeField]
		private UITexture ch;

		private int id;

		[SerializeField]
		private UISprite[] giveRing;

		[SerializeField]
		private UISprite[] ringBox;

		[SerializeField]
		private GameObject featherInit;

		private GameObject[] feathers;

		[SerializeField]
		private GameObject flareInit;

		private GameObject[] flares;

		[SerializeField]
		private GameObject petalInit;

		private GameObject[] petals;

		[SerializeField]
		private GameObject sparkleInit;

		private GameObject[] sparkles;

		[SerializeField]
		private UISprite[] letters;

		[SerializeField]
		private GameObject btn;

		[SerializeField]
		private Blur blur;

		[SerializeField]
		private Texture2D[] bgTexs;

		private bool on;

		private bool finish;

		private bool floating;

		private float startTime;

		private OnFinish Callback;

		private readonly Vector3[] CHARACTER_POSITIONS = new Vector3[4]
		{
			new Vector3(7f, -51f, 0f),
			new Vector3(240f, 576f, 0f),
			new Vector3(-96f, 82f, 0f),
			new Vector3(230f, -293f, 0f)
		};

		private readonly Vector3[] CHARACTER_POSITION_TOS = new Vector3[4]
		{
			new Vector3(-51f, -96f, 0f),
			new Vector3(288f, 640f, 0f),
			new Vector3(-40f, 105f, 0f),
			new Vector3(112f, -216f, 0f)
		};

		private readonly Vector3[] CHARACTER_SCALES = new Vector3[4]
		{
			Vector3.one * 1.5f,
			Vector3.one * 2f,
			Vector3.one * 1.7f,
			Vector3.one * 2.5f
		};

		private readonly Vector3[] CHARACTER_SCALE_TOS = new Vector3[4]
		{
			Vector3.one * 1.7f,
			Vector3.one * 2.5f,
			Vector3.one * 1.6f,
			Vector3.one * 2.2f
		};

		private ShipModelMst mTargetShipModelMst;

		public float alpha
		{
			get
			{
				if (mPanelThis != null)
				{
					return mPanelThis.alpha;
				}
				return -1f;
			}
			set
			{
				if (mPanelThis != null)
				{
					mPanelThis.alpha = alpha;
				}
			}
		}

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			id = -1;
			on = false;
			finish = false;
			floating = false;
			startTime = 0f;
		}

		private void Update()
		{
			if (on && floating)
			{
				for (int i = 0; i < 3; i++)
				{
					giveRing[i].transform.localPosition = (-100f + 10f * (float)Math.Sin(Time.time * 1.2f)) * Vector3.up;
				}
			}
			if (isKeyControllable && mKeyController.keyState[1].down)
			{
				isKeyControllable = false;
				ButtonClick();
			}
		}

		public void Initialize(ShipModelMst targetShipModel, KeyControl kCtrl, OnFinish func = null)
		{
			on = true;
			startTime = Time.time;
			Callback = func;
			mTargetShipModelMst = targetShipModel;
			ch.mainTexture = ShipUtils.LoadTexture(mTargetShipModelMst.GetGraphicsMstId(), 9 + Convert.ToInt32(value: false));
			ch.MakePixelPerfect();
			mKeyController = kCtrl;
		}

		public IEnumerator PlayAnimation()
		{
			flares = new GameObject[8];
			for (int i = 0; i < 8; i++)
			{
				flares[i] = UnityEngine.Object.Instantiate(flareInit);
				if (flares[i] != null)
				{
					flares[i].name = "Flare" + i;
					flares[i].transform.parent = flareInit.transform.parent;
					flares[i].transform.localScale = new Vector3(1f, 1f, 1f);
					flares[i].transform.localPosition = new Vector3(0f, 0f, 0f);
					flares[i].gameObject.SetActive(false);
				}
			}
			feathers = new GameObject[20];
			for (int j = 0; j < 20; j++)
			{
				feathers[j] = UnityEngine.Object.Instantiate(featherInit);
				if (feathers[j] != null)
				{
					feathers[j].name = "Feather" + j;
					feathers[j].transform.parent = featherInit.transform.parent;
					feathers[j].transform.localScale = new Vector3(1f, 1f, 1f);
					feathers[j].transform.localPosition = new Vector3(0f, 0f, 0f);
					feathers[j].gameObject.SetActive(false);
				}
			}
			yield return new WaitForEndOfFrame();
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 1, "onupdate", "BGAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(1f);
			SingletonMonoBehaviour<SoundManager>.Instance.PlayBGM(BGMFileInfos.Kekkon, isLoop: false);
			for (int k = 0; k < 8; k++)
			{
				if (flares[k] != null)
				{
					flares[k].gameObject.SetActive(true);
					MarriageFlare script = flares[k].GetComponent<MarriageFlare>();
					if (script != null)
					{
						script.Initialize();
					}
				}
			}
			for (int l = 0; l < 20; l++)
			{
				if (feathers[l] != null)
				{
					feathers[l].gameObject.SetActive(true);
					MarriageFeather script2 = feathers[l].GetComponent<MarriageFeather>();
					if (script2 != null)
					{
						script2.Initialize();
					}
				}
			}
			yield return new WaitForSeconds(0.5f);
			iTween.MoveTo(bg.gameObject, iTween.Hash("y", -592, "islocal", true, "time", 4.4f, "easetype", iTween.EaseType.easeOutExpo));
			yield return new WaitForSeconds(4f);
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.5f, "onupdate", "WhiteAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(0.5f);
			bg.transform.localPosition = Vector3.zero;
			if (bgTexs.Length >= 2)
			{
				bg.mainTexture = bgTexs[1];
			}
			bg.width = 1600;
			bg.height = 960;
			bg.transform.localScale = 0.6f * Vector3.one;
			blur.enabled = true;
			blur.blurSize = 1.5f;
			vignette.alpha = 1f;
			for (int m = 0; m < feathers.Length; m++)
			{
				UnityEngine.Object.Destroy(feathers[m]);
			}
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.5f, "onupdate", "WhiteAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(1f);
			iTween.ScaleTo(bg.gameObject, iTween.Hash("scale", Vector3.one, "time", 7.5f, "easetype", iTween.EaseType.easeOutQuad));
			yield return new WaitForSeconds(2.5f);
			petals = new GameObject[10];
			for (int n = 0; n < 10; n++)
			{
				petals[n] = UnityEngine.Object.Instantiate(petalInit.gameObject);
				if (petals[n] != null)
				{
					petals[n].name = "Petal" + n;
					petals[n].transform.parent = petalInit.transform.parent;
					petals[n].transform.localScale = new Vector3(1f, 1f, 1f);
					petals[n].transform.localPosition = new Vector3(0f, 0f, 0f);
					MarriagePetal script3 = petals[n].GetComponent<MarriagePetal>();
					if (script3 != null)
					{
						script3.Initialize(loop: false);
					}
				}
			}
			yield return new WaitForSeconds(1f);
			ch.transform.localScale = Vector3.one * 1.5f;
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.5f, "onupdate", "CharAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(2.5f);
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.5f, "onupdate", "CharAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(1f);
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.5f, "onupdate", "WhiteAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(0.5f);
			if (bgTexs.Length >= 3)
			{
				bg.mainTexture = bgTexs[2];
			}
			blur.enabled = false;
			bg.width = 960;
			bg.height = 1202;
			bg.transform.localPosition = 329f * Vector3.down;
			for (int i2 = 0; i2 < petals.Length; i2++)
			{
				UnityEngine.Object.Destroy(petals[i2]);
			}
			ringBox[0].alpha = 1f;
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.5f, "onupdate", "WhiteAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(0.01f);
			iTween.MoveTo(bg.gameObject, iTween.Hash("y", 329, "islocal", true, "time", 3, "easetype", iTween.EaseType.easeInOutQuad));
			iTween.MoveTo(ringBox[0].gameObject, iTween.Hash("y", -21, "islocal", true, "time", 3, "easetype", iTween.EaseType.easeInOutQuad));
			iTween.MoveTo(ringBox[1].gameObject, iTween.Hash("y", -21, "islocal", true, "time", 3, "easetype", iTween.EaseType.easeInOutQuad));
			yield return new WaitForSeconds(3f);
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 1, "onupdate", "RingBoxAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(0.5f);
			sparkles = new GameObject[6];
			for (int i3 = 0; i3 < 6; i3++)
			{
				sparkles[i3] = UnityEngine.Object.Instantiate(sparkleInit.gameObject);
				if (sparkles[i3] != null)
				{
					sparkles[i3].name = "Sparkle" + i3;
					sparkles[i3].transform.parent = sparkleInit.transform.parent;
					sparkles[i3].transform.localScale = new Vector3(1f, 1f, 1f);
					sparkles[i3].transform.localPosition = new Vector3(0f, 0f, 0f);
					MarriageSparkle script4 = sparkles[i3].GetComponent<MarriageSparkle>();
					if (script4 != null)
					{
						script4.Initialize(bob: false);
					}
				}
			}
			yield return new WaitForSeconds(1f);
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.5f, "onupdate", "WhiteAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(0.5f);
			for (int i4 = 0; i4 < sparkles.Length; i4++)
			{
				UnityEngine.Object.Destroy(sparkles[i4]);
			}
			ringBox[0].alpha = 0f;
			ringBox[1].alpha = 0f;
			bg.transform.localPosition = 329f * Vector3.down;
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.5f, "onupdate", "WhiteAlpha", "onupdatetarget", base.gameObject));
			for (int i5 = 0; i5 < 4; i5++)
			{
				ch.transform.localScale = CHARACTER_SCALES[i5];
				ch.transform.localPosition = CHARACTER_POSITIONS[i5];
				iTween.ScaleTo(ch.gameObject, iTween.Hash("scale", CHARACTER_SCALE_TOS[i5], "time", 1.16f, "easetype", iTween.EaseType.linear));
				iTween.MoveTo(ch.gameObject, iTween.Hash("scale", CHARACTER_POSITION_TOS[i5], "time", 1.16f, "easetype", iTween.EaseType.linear));
				iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.58f, "onupdate", "CharAlpha", "onupdatetarget", base.gameObject));
				iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.58f, "delay", 0.58f, "onupdate", "CharAlpha", "onupdatetarget", base.gameObject));
				yield return new WaitForSeconds(1.2f);
			}
			yield return new WaitForSeconds(1.2f);
			ch.alpha = 1f;
			ch.transform.localScale = Vector3.one * 1.5f;
			ch.transform.localPosition = Vector3.up * 850f;
			iTween.MoveTo(ch.gameObject, iTween.Hash("y", -100, "islocal", true, "time", 3, "easetype", iTween.EaseType.easeOutExpo));
			yield return new WaitForSeconds(1.5f);
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 1, "onupdate", "WhiteAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(1.5f);
			if (bgTexs.Length >= 2)
			{
				bg.mainTexture = bgTexs[1];
			}
			bg.width = 1600;
			bg.height = 960;
			bg.transform.localScale = 0.6f * Vector3.one;
			bg.transform.localPosition = Vector3.zero;
			ch.transform.localScale = Vector3.one * 2f;
			ch.transform.localPosition = Vector3.down * 500f;
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.5f, "onupdate", "WhiteAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(2f);
			floating = true;
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 1, "onupdate", "GiveRingBoxClosedAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(2.5f);
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 1, "onupdate", "GiveRingBoxAlpha", "onupdatetarget", base.gameObject));
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 1, "onupdate", "GiveRingAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(0.5f);
			sparkles = new GameObject[8];
			for (int i6 = 0; i6 < 8; i6++)
			{
				sparkles[i6] = UnityEngine.Object.Instantiate(sparkleInit.gameObject);
				if (sparkles[i6] != null)
				{
					sparkles[i6].name = "Sparkle" + i6;
					sparkles[i6].transform.parent = sparkleInit.transform.parent;
					sparkles[i6].transform.localScale = new Vector3(1f, 1f, 1f);
					sparkles[i6].transform.localPosition = new Vector3(0f, 0f, 0f);
					MarriageSparkle script6 = sparkles[i6].GetComponent<MarriageSparkle>();
					if (script6 != null)
					{
						script6.Initialize(bob: true);
					}
				}
			}
			yield return new WaitForSeconds(1.5f);
			ShipUtils.PlayShipVoice(mTargetShipModelMst, 24);
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0, "time", 1, "onupdate", "GiveRingBoxClosedAlpha", "onupdatetarget", base.gameObject));
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0, "time", 1, "onupdate", "GiveRingBoxAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(9f);
			iTween.ScaleTo(bg.gameObject, iTween.Hash("scale", 1.5f * Vector3.one, "time", 4, "easetype", iTween.EaseType.linear));
			blur.enabled = true;
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 2, "time", 4, "onupdate", "Blur", "onupdatetarget", base.gameObject));
			iTween.ScaleTo(ch.gameObject, iTween.Hash("scale", 1.2f * Vector3.one, "time", 4, "easetype", iTween.EaseType.linear));
			iTween.MoveTo(ch.gameObject, iTween.Hash("y", -280, "islocal", true, "time", 4, "easetype", iTween.EaseType.linear));
			yield return new WaitForSeconds(2.5f);
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 1, "onupdate", "WhiteAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(1.8f);
			for (int i7 = 0; i7 < flares.Length; i7++)
			{
				UnityEngine.Object.Destroy(flares[i7]);
			}
			for (int i8 = 0; i8 < sparkles.Length; i8++)
			{
				UnityEngine.Object.Destroy(sparkles[i8].gameObject);
			}
			giveRing[2].alpha = 0f;
			floating = false;
			if (bgTexs.Length >= 4)
			{
				bg.mainTexture = bgTexs[3];
			}
			blur.enabled = false;
			bg.width = 960;
			bg.height = 544;
			bg.transform.localScale = Vector3.one;
			ch.transform.localScale = Vector3.one * 0.8f;
			ch.transform.localPosition = Vector3.down * 110f;
			petals = new GameObject[20];
			for (int i9 = 0; i9 < 20; i9++)
			{
				petals[i9] = UnityEngine.Object.Instantiate(petalInit.gameObject);
				if (petals[i9] != null)
				{
					petals[i9].name = "Petal" + i9;
					petals[i9].transform.parent = petalInit.transform.parent;
					petals[i9].transform.localScale = new Vector3(1f, 1f, 1f);
					petals[i9].transform.localPosition = new Vector3(0f, 0f, 0f);
					MarriagePetal script5 = petals[i9].GetComponent<MarriagePetal>();
					if (script5 != null)
					{
						script5.Initialize(loop: true);
					}
				}
			}
			yield return new WaitForSeconds(0.01f);
			white.alpha = 0f;
			yield return new WaitForSeconds(8.7f);
			iTween.ScaleTo(ch.gameObject, iTween.Hash("scale", 2f * Vector3.one, "time", 2, "easetype", iTween.EaseType.easeOutQuad));
			iTween.MoveTo(ch.gameObject, iTween.Hash("y", -350, "islocal", true, "time", 2, "easetype", iTween.EaseType.easeOutQuad));
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0, "time", 2, "onupdate", "CharAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(2.5f);
			for (int i10 = 0; i10 < 16; i10++)
			{
				letters[i10].alpha = 1f;
				iTween.ScaleFrom(letters[i10].gameObject, iTween.Hash("scale", 0.001f * Vector3.one, "time", 0.32f, "easetype", iTween.EaseType.easeOutBack));
				yield return new WaitForSeconds(0.35f);
			}
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 1, "onupdate", "SubtitleAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(3.9f);
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 3, "onupdate", "WhiteAlpha", "onupdatetarget", base.gameObject));
			yield return new WaitForSeconds(4.35f);
			btn.SetActive(true);
			isKeyControllable = true;
		}

		public void BGAlpha(float f)
		{
			bg.alpha = f;
		}

		public void Blur(float f)
		{
			blur.blurSize = f;
		}

		public void WhiteAlpha(float f)
		{
			white.alpha = f;
		}

		public void CharAlpha(float f)
		{
			ch.alpha = f;
		}

		public void RingBoxAlpha(float f)
		{
			ringBox[1].alpha = f;
		}

		public void GiveRingBoxClosedAlpha(float f)
		{
			giveRing[0].alpha = f;
		}

		public void GiveRingBoxAlpha(float f)
		{
			giveRing[1].alpha = f;
		}

		public void GiveRingAlpha(float f)
		{
			giveRing[2].alpha = f;
		}

		public void SubtitleAlpha(float f)
		{
			letters[16].alpha = f;
		}

		public void ButtonClick()
		{
			if (!isButtonPressed)
			{
				isButtonPressed = true;
				for (int i = 0; i < petals.Length; i++)
				{
					UnityEngine.Object.Destroy(petals[i]);
				}
				if (Callback != null)
				{
					Callback();
				}
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref giveRing);
			UserInterfacePortManager.ReleaseUtils.Releases(ref ringBox);
			UserInterfacePortManager.ReleaseUtils.Releases(ref letters);
			UserInterfacePortManager.ReleaseUtils.Releases(ref feathers);
			UserInterfacePortManager.ReleaseUtils.Releases(ref flares);
			UserInterfacePortManager.ReleaseUtils.Releases(ref petals);
			UserInterfacePortManager.ReleaseUtils.Releases(ref sparkles);
			UserInterfacePortManager.ReleaseUtils.Release(ref mPanelThis);
			UserInterfacePortManager.ReleaseUtils.Release(ref bg);
			UserInterfacePortManager.ReleaseUtils.Release(ref white);
			UserInterfacePortManager.ReleaseUtils.Release(ref vignette);
			UserInterfacePortManager.ReleaseUtils.Release(ref ch);
			mKeyController = null;
			featherInit = null;
			flareInit = null;
			petalInit = null;
			sparkleInit = null;
			btn = null;
			blur = null;
			bgTexs = null;
			Callback = null;
		}
	}
}
