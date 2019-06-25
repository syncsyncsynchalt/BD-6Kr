using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using local.utils;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesConvertShipManager : MonoBehaviour
	{
		[SerializeField]
		private UISprite fade;

		[SerializeField]
		private Transform mTransform_Convert;

		[SerializeField]
		private Transform mTransform_ConvertComplete;

		[SerializeField]
		private UITexture bg;

		[SerializeField]
		private UITexture bg2;

		[SerializeField]
		private UIPanel bg2mask;

		[SerializeField]
		private UITexture card;

		[SerializeField]
		private Transform mShipOffsetFrame;

		[SerializeField]
		private UITexture ship;

		[SerializeField]
		private PortUpgradesConvertShipReturnButton retBtn;

		[SerializeField]
		private GameObject stripe;

		[SerializeField]
		private GameObject snowflakeInit;

		private GameObject[] snowflakes;

		private bool on;

		public bool finish;

		private float timer;

		public bool isFinished;

		private ShipModelMst mTargetShipModelMst;

		private bool enabledKey;

		private KeyControl mKeyController;

		public void Awake()
		{
			snowflakes = new GameObject[40];
			fade.alpha = 0f;
			bg.alpha = 0f;
			bg2.alpha = 0f;
			card.alpha = 0f;
			ship.alpha = 0f;
			UISprite[] componentsInChildren = stripe.GetComponentsInChildren<UISprite>();
			UISprite[] array = componentsInChildren;
			foreach (UISprite uISprite in array)
			{
				uISprite.alpha = 0f;
			}
			stripe.transform.localScale = new Vector3(1f, 0.01f, 1f);
			on = false;
			finish = false;
			isFinished = false;
			timer = 0f;
		}

		public void Initialize(ShipModelMst targetShipMst, int bgID, int bg2ID, int startDepth)
		{
			mTargetShipModelMst = targetShipMst;
			Vector3 localPosition = Util.Poi2Vec(mTargetShipModelMst.Offsets.GetShipDisplayCenter(damaged: false));
			ship.transform.localPosition = localPosition;
			on = true;
			timer = Time.time;
			ship.GetComponent<UITexture>().mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(mTargetShipModelMst.GetGraphicsMstId(), 9);
			ship.GetComponent<UITexture>().MakePixelPerfect();
			card.GetComponent<UITexture>().mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(mTargetShipModelMst.MstId, 3);
			switch (bgID)
			{
			case 0:
				bg.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_common_1") as Texture);
				break;
			case 1:
				bg.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_common_2") as Texture);
				break;
			case 2:
				bg.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_common_3") as Texture);
				break;
			case 3:
				bg.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_rare_1") as Texture);
				break;
			case 4:
				bg.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_rare_2") as Texture);
				break;
			case 5:
				bg.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_horo_1") as Texture);
				break;
			}
			if (bg.mainTexture == null)
			{
				Debug.Log("Failed to load texture for ./BG/BG");
			}
			switch (bg2ID)
			{
			case 0:
				bg2.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_common_1") as Texture);
				break;
			case 1:
				bg2.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_common_2") as Texture);
				break;
			case 2:
				bg2.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_common_3") as Texture);
				break;
			case 3:
				bg2.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_rare_1") as Texture);
				break;
			case 4:
				bg2.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_rare_2") as Texture);
				break;
			case 5:
				bg2.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_horo_1") as Texture);
				break;
			}
			StartCoroutine(EnableAlphas());
			StartCoroutine(StripeDisable());
			StartCoroutine(SnowflakeExplosion());
			StartCoroutine(CardToFront());
			StartCoroutine(EnableButton());
			((Component)base.transform.Find("BG")).GetComponent<UIPanel>().depth = startDepth;
			((Component)base.transform.Find("BG2")).GetComponent<UIPanel>().depth = startDepth + 1;
			((Component)base.transform.Find("Main")).GetComponent<UIPanel>().depth = startDepth + 2;
		}

		public void Update()
		{
			if (mKeyController != null && enabledKey && mKeyController.keyState[1].down)
			{
				finish = true;
			}
			if (on)
			{
				if (Time.time - timer <= 1f)
				{
					fade.alpha += Mathf.Min(Time.deltaTime, 1f - fade.alpha);
				}
				if (Time.time - timer >= 1f && Time.time - timer <= 2f)
				{
					fade.alpha -= Mathf.Min(Time.deltaTime, fade.alpha);
				}
				if (Time.time - timer >= 1f && Time.time - timer <= 2.25f)
				{
					StripeExpand();
				}
				if (Time.time - timer >= 1.5f && Time.time - timer <= 2f)
				{
					ConvertCompleteSlide();
				}
				if (Time.time - timer >= 3.25f && Time.time - timer <= 4.5f)
				{
					StripeContract();
				}
				if (Time.time - timer >= 4.5f && Time.time - timer <= 5.5f)
				{
					BGExpand();
				}
				if (Time.time - timer >= 4.75f && Time.time - timer <= 6.25f)
				{
					ShipSlide();
				}
				if (Time.time - timer >= 8f && Time.time - timer <= 10f)
				{
					CardSwap();
				}
				if (finish)
				{
					finish = false;
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					TrophyUtil.Unlock_GetShip(mTargetShipModelMst.MstId);
					SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
					{
						isFinished = true;
					});
				}
			}
		}

		private void StripeExpand()
		{
			stripe.transform.localScale += new Vector3(0f, 0.8f * Time.deltaTime, 0f);
			mTransform_Convert.localPosition += new Vector3(700f * Time.deltaTime, 0f, 0f);
		}

		private void ConvertCompleteSlide()
		{
			mTransform_ConvertComplete.localPosition += new Vector3(1334f * Time.deltaTime, 0f, 0f);
		}

		private void StripeContract()
		{
			stripe.transform.localScale -= new Vector3(0f, 0.8f * Time.deltaTime, 0f);
			mTransform_Convert.localPosition += new Vector3(600f * Time.deltaTime, 0f, 0f);
			mTransform_ConvertComplete.localPosition += new Vector3(960f * Time.deltaTime, 0f, 0f);
		}

		private void BGExpand()
		{
			bg2mask.baseClipRegion += new Vector4(0f, 0f, 0f, 544f * Time.deltaTime);
		}

		private void ShipSlide()
		{
			mShipOffsetFrame.localPosition += new Vector3(0f, 300f * Time.deltaTime, 0f);
			ship.alpha += Mathf.Min(Time.deltaTime, 1f - ship.alpha);
		}

		private void CardSwap()
		{
			ship.alpha -= Mathf.Min(0.5f * Time.deltaTime, ship.alpha);
			mShipOffsetFrame.localPosition -= new Vector3(300f * Time.deltaTime * Mathf.Sin((float)Math.PI * (Time.time - timer)), 0f, 0f);
			mShipOffsetFrame.localScale -= new Vector3(0.125f * Time.deltaTime, 0.125f * Time.deltaTime, 0f);
			card.alpha += Mathf.Min(0.5f * Time.deltaTime, 1f - card.alpha);
			card.transform.localPosition += new Vector3(300f * Time.deltaTime * Mathf.Sin((float)Math.PI * (Time.time - timer)), 0f, 0f);
			card.transform.localScale += new Vector3(0.125f * Time.deltaTime, 0.125f * Time.deltaTime, 0f);
		}

		public IEnumerator EnableAlphas()
		{
			yield return new WaitForSeconds(1f);
			bg.alpha = 1f;
			UISprite[] ss = stripe.GetComponentsInChildren<UISprite>();
			UISprite[] array = ss;
			foreach (UISprite s in array)
			{
				s.alpha = 1f;
			}
		}

		public IEnumerator StripeDisable()
		{
			yield return new WaitForSeconds(4.5f);
			UISprite[] ss = stripe.GetComponentsInChildren<UISprite>();
			UISprite[] array = ss;
			foreach (UISprite s in array)
			{
				s.alpha = 0f;
			}
			bg2.alpha = 1f;
		}

		public IEnumerator SnowflakeExplosion()
		{
			yield return new WaitForSeconds(6.25f);
			PlayVoice();
			for (int i = 0; i < 40; i++)
			{
				snowflakes[i] = UnityEngine.Object.Instantiate(snowflakeInit);
				snowflakes[i].transform.parent = base.transform.Find("Main");
				snowflakes[i].transform.localScale = new Vector3(1f, 1f, 1f);
				try
				{
					snowflakes[i].GetComponent<PortUpgradesConvertShipSnowflake>().SetManagerReference();
					snowflakes[i].GetComponent<PortUpgradesConvertShipSnowflake>().Initialize();
				}
				catch (NullReferenceException)
				{
				}
			}
		}

		public IEnumerator CardToFront()
		{
			yield return new WaitForSeconds(9f);
			ship.depth = 3;
			card.depth = 4;
		}

		public IEnumerator EnableButton()
		{
			yield return new WaitForSeconds(10f);
			enabledKey = true;
			retBtn.Activate();
		}

		private void PlayVoice()
		{
			ShipUtils.PlayShipVoice(mTargetShipModelMst, 10);
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public void Finish()
		{
			finish = true;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref fade);
			UserInterfacePortManager.ReleaseUtils.Release(ref bg);
			UserInterfacePortManager.ReleaseUtils.Release(ref bg2);
			UserInterfacePortManager.ReleaseUtils.Release(ref bg2mask);
			UserInterfacePortManager.ReleaseUtils.Release(ref card);
			UserInterfacePortManager.ReleaseUtils.Release(ref ship);
			mTransform_Convert = null;
			mTransform_ConvertComplete = null;
			mShipOffsetFrame = null;
			retBtn = null;
			stripe = null;
			snowflakeInit = null;
			if (snowflakes != null)
			{
				for (int i = 0; i < snowflakes.Length; i++)
				{
					snowflakes[i] = null;
				}
			}
			snowflakes = null;
			mTargetShipModelMst = null;
		}
	}
}
