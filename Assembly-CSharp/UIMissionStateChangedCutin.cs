using KCV;
using local.models;
using System;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(UIPanel))]
public class UIMissionStateChangedCutin : MonoBehaviour
{
	[SerializeField]
	private CommonShipBanner[] mShipBannerSlots;

	[SerializeField]
	private UISprite[] mSpriteFlashes;

	[SerializeField]
	private UISprite mSprite_CutinMessage;

	[SerializeField]
	private UITexture[] mTextures_Flag;

	private UIPanel mPanelThis;

	private DeckModel mDeckModel;

	private Action mAnimationFinishedCallBack;

	private int mCallFlashCount;

	private void Awake()
	{
		mPanelThis = GetComponent<UIPanel>();
		mPanelThis.alpha = 0.01f;
	}

	public void Initialize(DeckModel deck)
	{
		mDeckModel = deck;
		ShipModel[] ships = mDeckModel.GetShips();
		UITexture[] array = mTextures_Flag;
		foreach (UITexture component in array)
		{
			component.SetActive(isActive: false);
		}
		CommonShipBanner[] array2 = mShipBannerSlots;
		foreach (CommonShipBanner commonShipBanner in array2)
		{
			commonShipBanner.StopParticle();
		}
		switch (ships.Length)
		{
		case 1:
			mShipBannerSlots[0].gameObject.SetActive(true);
			mShipBannerSlots[0].SetShipData(ships[0]);
			mTextures_Flag[0].mainTexture = Resources.Load<Texture>("Textures/Common/DeckFlag/icon_deck" + deck.Id + "_fs");
			mTextures_Flag[0].SetActive(isActive: true);
			break;
		case 2:
			mShipBannerSlots[0].gameObject.SetActive(true);
			mShipBannerSlots[5].gameObject.SetActive(true);
			mShipBannerSlots[0].SetShipData(ships[0]);
			mShipBannerSlots[5].SetShipData(ships[1]);
			mTextures_Flag[0].mainTexture = Resources.Load<Texture>("Textures/Common/DeckFlag/icon_deck" + deck.Id + "_fs");
			mTextures_Flag[0].SetActive(isActive: true);
			break;
		case 3:
			mShipBannerSlots[0].gameObject.SetActive(true);
			mShipBannerSlots[1].gameObject.SetActive(true);
			mShipBannerSlots[5].gameObject.SetActive(true);
			mShipBannerSlots[0].SetShipData(ships[2]);
			mShipBannerSlots[1].SetShipData(ships[0]);
			mShipBannerSlots[5].SetShipData(ships[1]);
			mTextures_Flag[1].mainTexture = Resources.Load<Texture>("Textures/Common/DeckFlag/icon_deck" + deck.Id + "_fs");
			mTextures_Flag[1].SetActive(isActive: true);
			break;
		case 4:
			mShipBannerSlots[0].gameObject.SetActive(true);
			mShipBannerSlots[1].gameObject.SetActive(true);
			mShipBannerSlots[4].gameObject.SetActive(true);
			mShipBannerSlots[5].gameObject.SetActive(true);
			mShipBannerSlots[0].SetShipData(ships[2]);
			mShipBannerSlots[1].SetShipData(ships[0]);
			mShipBannerSlots[4].SetShipData(ships[1]);
			mShipBannerSlots[5].SetShipData(ships[3]);
			mTextures_Flag[1].mainTexture = Resources.Load<Texture>("Textures/Common/DeckFlag/icon_deck" + deck.Id + "_fs");
			mTextures_Flag[1].SetActive(isActive: true);
			break;
		case 5:
			mShipBannerSlots[0].gameObject.SetActive(true);
			mShipBannerSlots[1].gameObject.SetActive(true);
			mShipBannerSlots[2].gameObject.SetActive(true);
			mShipBannerSlots[4].gameObject.SetActive(true);
			mShipBannerSlots[5].gameObject.SetActive(true);
			mShipBannerSlots[0].SetShipData(ships[2]);
			mShipBannerSlots[1].SetShipData(ships[0]);
			mShipBannerSlots[2].SetShipData(ships[4]);
			mShipBannerSlots[4].SetShipData(ships[1]);
			mShipBannerSlots[5].SetShipData(ships[3]);
			mTextures_Flag[1].mainTexture = Resources.Load<Texture>("Textures/Common/DeckFlag/icon_deck" + deck.Id + "_fs");
			mTextures_Flag[1].SetActive(isActive: true);
			break;
		case 6:
			mShipBannerSlots[0].gameObject.SetActive(true);
			mShipBannerSlots[1].gameObject.SetActive(true);
			mShipBannerSlots[2].gameObject.SetActive(true);
			mShipBannerSlots[3].gameObject.SetActive(true);
			mShipBannerSlots[4].gameObject.SetActive(true);
			mShipBannerSlots[5].gameObject.SetActive(true);
			mShipBannerSlots[0].SetShipData(ships[2]);
			mShipBannerSlots[1].SetShipData(ships[0]);
			mShipBannerSlots[2].SetShipData(ships[4]);
			mShipBannerSlots[3].SetShipData(ships[5]);
			mShipBannerSlots[4].SetShipData(ships[1]);
			mShipBannerSlots[5].SetShipData(ships[3]);
			mTextures_Flag[1].mainTexture = Resources.Load<Texture>("Textures/Common/DeckFlag/icon_deck" + deck.Id + "_fs");
			mTextures_Flag[1].SetActive(isActive: true);
			break;
		}
	}

	public void PlayStartCutin(Action onFinishedCallBack)
	{
		mSprite_CutinMessage.spriteName = "expedition_txt_go";
		mAnimationFinishedCallBack = onFinishedCallBack;
		mPanelThis.alpha = 1f;
		GetComponent<Animation>().Play("Anim_MissionStartCutinShutter");
		GetComponent<Animation>().Blend("Anim_MissionStartCutinLevel" + mDeckModel.GetShips().Length);
	}

	public void PlayFinishedCutin(Action onFinishedCallBack)
	{
		mSprite_CutinMessage.spriteName = "expedition_txt_finish";
		mAnimationFinishedCallBack = onFinishedCallBack;
		mPanelThis.alpha = 1f;
		GetComponent<Animation>().Play("Anim_MissionStartCutinShutter");
		GetComponent<Animation>().Blend("Anim_MissionStartCutinLevel" + mDeckModel.GetShips().Length);
	}

	public void OnFinishedAnimation()
	{
		Debug.Log("Call:OnFinishedAnimation");
		if (mAnimationFinishedCallBack != null)
		{
			mAnimationFinishedCallBack();
		}
	}

	public void AnimFlash()
	{
		mSpriteFlashes[mCallFlashCount++].GetComponent<Animation>().Play("Anim_Flash");
	}

	public void OnFinishParticle()
	{
		CommonShipBanner[] array = mShipBannerSlots;
		foreach (CommonShipBanner commonShipBanner in array)
		{
			if (commonShipBanner.gameObject.activeSelf)
			{
				Debug.Log("KiraParLoop::False");
				commonShipBanner.StopParticle();
			}
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < mShipBannerSlots.Length; i++)
		{
			mShipBannerSlots[i] = null;
		}
		mShipBannerSlots = null;
		for (int j = 0; j < mSpriteFlashes.Length; j++)
		{
			mSpriteFlashes[j] = null;
		}
		mSpriteFlashes = null;
		for (int k = 0; k < mTextures_Flag.Length; k++)
		{
			mTextures_Flag[k] = null;
		}
		mTextures_Flag = null;
		mSprite_CutinMessage = null;
		mPanelThis = null;
		mDeckModel = null;
		mAnimationFinishedCallBack = null;
	}
}
