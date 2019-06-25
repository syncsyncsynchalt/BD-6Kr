using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesModernizeShipManager : MonoBehaviour
	{
		[SerializeField]
		private UITexture mTexture_Fade;

		[SerializeField]
		private GameObject mGameObject_Upgrade;

		[SerializeField]
		private UITexture mTexture_Background;

		[SerializeField]
		private UITexture mTexture_Ship;

		[SerializeField]
		private GameObject mGameObject_SparkleInit;

		[SerializeField]
		private GameObject mGameObjects_UpSpiritInit;

		[SerializeField]
		private GameObject mGameObject_UpStripe;

		[SerializeField]
		private UISprite mSprite_UpText;

		[SerializeField]
		private UITexture mSprite_UpText_S;

		[SerializeField]
		private GameObject mGameObject_Failed;

		[SerializeField]
		private UITexture mTexture_FailBackground;

		[SerializeField]
		private UISprite mSprite_FailBox;

		[SerializeField]
		private UISprite mSprite_FailDashInit;

		[SerializeField]
		private PortUpgradesModernizeShipLeaf mPortUpgradesModernizeShipLeaf_FailLeaf;

		[SerializeField]
		private UISprite mSprite_FailSpotTop;

		[SerializeField]
		private UISprite mSprite_FailSpotBottom;

		[SerializeField]
		private UISprite mSprite_FailTextbox;

		[SerializeField]
		private PortUpgradesModernizeShipText mPortUpgradesModernizeShipText_FailText;

		[SerializeField]
		private PortUpgradesModernizeShipReturnButton mPortUpgradesModernizeShipReturnButton_ReturnButton;

		[SerializeField]
		private GameObject kamihubuki;

		private GameObject[] mGameObjects_FailDashe = new GameObject[20];

		private GameObject[] mGameObjects_UpSpirit = new GameObject[5];

		private GameObject[] mGameObjects_UpSparkle = new GameObject[16];

		private bool[,] elmts;

		private int cnt;

		private bool fail;

		private bool on;

		private bool finish;

		private float timer;

		public bool isFinished;

		private ShipModelMst mTargetShipModelMst;

		private bool _isDamaged;

		private bool _isSuperSucessed;

		private KeyControl mKeyController;

		private bool enabledKey;

		private Coroutine waitAndVoiceCoroutine;

		public void Awake()
		{
			mTexture_Fade.alpha = 0f;
			mTexture_Background.alpha = 0f;
			mTexture_Ship.alpha = 0f;
			mGameObject_UpStripe.transform.localScale = new Vector3(1f, 0.01f, 1f);
			mGameObject_UpStripe.SetActive(false);
			mSprite_FailSpotTop.alpha = 0f;
			mSprite_FailSpotBottom.alpha = 0f;
			cnt = -1;
			fail = false;
			on = false;
			finish = false;
			isFinished = false;
			timer = 0f;
		}

		public void Initialize(ShipModelMst targetShipModelMst, int bgID, bool fail, bool SuperSuccessed, int sozai_count)
		{
			Initialize(targetShipModelMst, bgID, fail, SuperSuccessed, sozai_count, isDamaged: false);
		}

		public void Initialize(ShipModelMst targetShipModelMst, int bgID, bool fail, bool SuperSuccessed, int sozai_count, bool isDamaged)
		{
			this.fail = fail;
			mTargetShipModelMst = targetShipModelMst;
			_isDamaged = isDamaged;
			_isSuperSucessed = SuperSuccessed;
			on = true;
			timer = Time.time;
			if (isDamaged)
			{
				mTexture_Ship.GetComponent<UITexture>().mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(mTargetShipModelMst.GetGraphicsMstId(), 10);
			}
			else
			{
				mTexture_Ship.GetComponent<UITexture>().mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(mTargetShipModelMst.GetGraphicsMstId(), 9);
			}
			mTexture_Ship.GetComponent<UITexture>().MakePixelPerfect();
			elmts = new bool[5, 4]
			{
				{
					true,
					true,
					true,
					true
				},
				{
					true,
					true,
					true,
					true
				},
				{
					true,
					true,
					true,
					true
				},
				{
					true,
					true,
					true,
					true
				},
				{
					true,
					true,
					true,
					true
				}
			};
			cnt = sozai_count;
			StartCoroutine(EnableAlphas());
			StartCoroutine(SpawnSpirits());
			if (fail)
			{
				StartCoroutine(EnableButton());
				StartCoroutine(SwapActive());
				StartCoroutine(LeafBlow());
				StartCoroutine(Text());
			}
			else
			{
				StartCoroutine(SpawnSparkle());
				StartCoroutine(EnableButton());
			}
		}

		public void Update()
		{
			if (mKeyController != null && mKeyController.keyState[1].down && enabledKey)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
			if (!on)
			{
				return;
			}
			if (Time.time - timer <= 1f)
			{
				mTexture_Fade.alpha += Mathf.Min(Time.deltaTime, 1f - mTexture_Fade.alpha);
			}
			if (Time.time - timer >= 1f && Time.time - timer <= 2f)
			{
				mTexture_Fade.alpha -= Mathf.Min(Time.deltaTime, mTexture_Fade.alpha);
			}
			if (Time.time - timer >= 1.5f && Time.time - timer <= 2f)
			{
				ShipSlide();
			}
			if (!fail)
			{
				if (Time.time - timer >= 4f && Time.time - timer <= 4.5f)
				{
					StripeExpand();
				}
				if (Time.time - timer >= 4f && Time.time - timer <= 5.5f)
				{
					if (_isSuperSucessed)
					{
						if (!kamihubuki.activeSelf)
						{
							kamihubuki.SetActive(true);
						}
						TextSlide_S();
					}
					else
					{
						TextSlide();
					}
				}
			}
			if (fail)
			{
				if (Time.time - timer >= 4f && Time.time - timer <= 5f)
				{
					mTexture_Fade.alpha += Mathf.Min(Time.deltaTime, 1f - mTexture_Fade.alpha);
				}
				if (Time.time - timer >= 5f && Time.time - timer <= 6f)
				{
					mTexture_Fade.alpha -= Mathf.Min(Time.deltaTime, mTexture_Fade.alpha);
				}
				if (Time.time - timer >= 6f && Time.time - timer <= 6.5f)
				{
					SpotlightFadeIn();
				}
				if (Time.time - timer >= 6.5f && Time.time - timer <= 7.5f)
				{
					TextboxSlide();
				}
			}
			if (finish)
			{
				if (waitAndVoiceCoroutine != null)
				{
					StopCoroutine(waitAndVoiceCoroutine);
				}
				mTexture_Fade.alpha += Mathf.Min(Time.deltaTime, 1f - mTexture_Fade.alpha);
				if (mTexture_Fade.alpha == 1f)
				{
					isFinished = true;
				}
			}
		}

		private void ShipSlide()
		{
			Vector3 vector = Util.Poi2Vec(new ShipOffset(mTargetShipModelMst.GetGraphicsMstId()).GetSlotItemCategory(_isDamaged));
			mTexture_Ship.transform.localPosition = new Vector3(vector.x - 270f + 44f, vector.y + 29f, 0f);
			mTexture_Ship.alpha += Mathf.Min(3f * Time.deltaTime, 1f - mTexture_Ship.alpha);
		}

		private void StripeExpand()
		{
			mGameObject_UpStripe.SetActive(true);
			mGameObject_UpStripe.transform.localScale += new Vector3(0f, 2f * Time.deltaTime, 0f);
		}

		private void TextSlide()
		{
			Vector3 localPosition = mSprite_UpText.transform.localPosition;
			if (localPosition.x > 100f)
			{
				mSprite_UpText.transform.localPosition -= new Vector3(745f * Time.deltaTime, 0f, 0f);
			}
			else
			{
				mSprite_UpText.transform.localPosition -= new Vector3(200f * Time.deltaTime, 0f, 0f);
			}
		}

		private void TextSlide_S()
		{
			Vector3 localPosition = mSprite_UpText_S.transform.localPosition;
			if (localPosition.x > 100f)
			{
				mSprite_UpText_S.transform.localPosition -= new Vector3(745f * Time.deltaTime, 0f, 0f);
			}
			else
			{
				mSprite_UpText_S.transform.localPosition -= new Vector3(200f * Time.deltaTime, 0f, 0f);
			}
		}

		private void SpotlightFadeIn()
		{
			mSprite_FailSpotTop.alpha += Mathf.Min(2f * Time.deltaTime, 1f - mSprite_FailSpotTop.alpha);
			mSprite_FailSpotBottom.alpha += Mathf.Min(2f * Time.deltaTime, 1f - mSprite_FailSpotBottom.alpha);
		}

		private void TextboxSlide()
		{
			mSprite_FailTextbox.transform.localPosition += new Vector3(0f, 177f * Time.deltaTime, 0f);
		}

		public IEnumerator EnableAlphas()
		{
			yield return new WaitForSeconds(1f);
			mTexture_Background.alpha = 1f;
		}

		public IEnumerator SpawnSpirits()
		{
			yield return new WaitForSeconds(2f);
			for (int i = 0; i < cnt; i++)
			{
				mGameObjects_UpSpirit[i] = UnityEngine.Object.Instantiate(mGameObjects_UpSpiritInit);
				mGameObjects_UpSpirit[i].transform.parent = mGameObject_Upgrade.transform;
				mGameObjects_UpSpirit[i].transform.localScale = new Vector3(1f, 1f, 1f);
				mGameObjects_UpSpirit[i].transform.localPosition = new Vector3(400f * Mathf.Cos((float)Math.PI * 2f * (float)i / (float)cnt), 400f * Mathf.Sin((float)Math.PI * 2f * (float)i / (float)cnt), 0f);
				mGameObjects_UpSpirit[i].GetComponent<PortUpgradesModernizeShipSpirit>().Initialize(elmts[i, 0], elmts[i, 1], elmts[i, 2], elmts[i, 3]);
			}
		}

		public IEnumerator SpawnSparkle()
		{
			yield return new WaitForSeconds(3.3f);
			SoundUtils.PlaySE(SEFIleInfos.SE_010);
			yield return new WaitForSeconds(1.6f);
			PlayVoice();
			for (int i = 0; i < 16; i++)
			{
				mGameObjects_UpSparkle[i] = UnityEngine.Object.Instantiate(mGameObject_SparkleInit);
				mGameObjects_UpSparkle[i].transform.parent = mGameObject_Upgrade.transform;
				mGameObjects_UpSparkle[i].transform.localScale = new Vector3(1f, 1f, 1f);
				mGameObjects_UpSparkle[i].transform.localPosition = new Vector3(-400 + 50 * i, -75 + 84 * i % 150, 0f);
				mGameObjects_UpSparkle[i].GetComponent<PortUpgradesModernizeShipSparkle>().Initialize();
				yield return new WaitForSeconds(0.075f);
			}
		}

		public IEnumerator SwapActive()
		{
			yield return new WaitForSeconds(5f);
			mGameObject_Upgrade.SetActive(false);
			mGameObject_Failed.SetActive(true);
			mGameObject_Failed.SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, mGameObject_Failed, string.Empty);
		}

		public IEnumerator LeafBlow()
		{
			yield return new WaitForSeconds(6f);
			mPortUpgradesModernizeShipLeaf_FailLeaf.Initialize();
		}

		public IEnumerator Text()
		{
			yield return new WaitForSeconds(4.5f);
			SoundUtils.PlaySE(SEFIleInfos.SE_011);
			yield return new WaitForSeconds(3f);
			mPortUpgradesModernizeShipText_FailText.Initialize("近代化改修（合成）に失敗しました。", 0.08f, 400);
			mPortUpgradesModernizeShipText_FailText.Text();
		}

		public IEnumerator EnableButton()
		{
			float wait = 5.5f;
			if (fail)
			{
				wait = 9.5f;
			}
			yield return new WaitForSeconds(wait);
			enabledKey = true;
			mPortUpgradesModernizeShipReturnButton_ReturnButton.Activate();
		}

		private void PlayVoice()
		{
			ShipUtils.PlayShipVoice(mTargetShipModelMst, (UnityEngine.Random.Range(0, 2) != 0) ? 10 : 9);
			if (_isSuperSucessed)
			{
				waitAndVoiceCoroutine = StartCoroutine(WaitAndVoice());
			}
		}

		public IEnumerator WaitAndVoice()
		{
			while (SingletonMonoBehaviour<SoundManager>.Instance.isAnyVoicePlaying)
			{
				yield return null;
			}
			ShipUtils.PlayShipVoice(mTargetShipModelMst, 26);
			waitAndVoiceCoroutine = null;
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
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Fade);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Background);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Ship);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_UpText);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_UpText_S);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_FailBackground);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_FailBox);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_FailDashInit);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_FailSpotTop);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_FailSpotBottom);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_FailTextbox);
			mGameObject_Upgrade = null;
			mGameObject_SparkleInit = null;
			mGameObjects_UpSpiritInit = null;
			mGameObject_UpStripe = null;
			mGameObject_Failed = null;
			mPortUpgradesModernizeShipLeaf_FailLeaf = null;
			mPortUpgradesModernizeShipText_FailText = null;
			mPortUpgradesModernizeShipReturnButton_ReturnButton = null;
			kamihubuki = null;
			if (mGameObjects_FailDashe != null)
			{
				for (int i = 0; i < mGameObjects_FailDashe.Length; i++)
				{
					mGameObjects_FailDashe[i] = null;
				}
			}
			mGameObjects_FailDashe = null;
			if (mGameObjects_UpSpirit != null)
			{
				for (int j = 0; j < mGameObjects_UpSpirit.Length; j++)
				{
					mGameObjects_UpSpirit[j] = null;
				}
			}
			mGameObjects_UpSpirit = null;
			if (mGameObjects_UpSparkle != null)
			{
				for (int k = 0; k < mGameObjects_UpSparkle.Length; k++)
				{
					mGameObjects_UpSparkle[k] = null;
				}
			}
			mGameObjects_UpSparkle = null;
			mTargetShipModelMst = null;
			mKeyController = null;
			waitAndVoiceCoroutine = null;
		}
	}
}
