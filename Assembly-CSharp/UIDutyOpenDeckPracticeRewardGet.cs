using Common.Enum;
using KCV.Scene.Port;
using local.models;
using UnityEngine;

public class UIDutyOpenDeckPracticeRewardGet : MonoBehaviour
{
	[SerializeField]
	private UILabel mLabel_Message;

	[SerializeField]
	private UITexture mTexture_PracticeImage;

	private string DeckPracticeTypeToString(DeckPracticeType deckPracticeType)
	{
		switch (deckPracticeType)
		{
		case DeckPracticeType.Hou:
			return "砲戦";
		case DeckPracticeType.Kouku:
			return "航空戦";
		case DeckPracticeType.Normal:
			return "通常";
		case DeckPracticeType.Rai:
			return "雷撃";
		case DeckPracticeType.Sougou:
			return "総合";
		case DeckPracticeType.Taisen:
			return "対潜戦";
		default:
			return string.Empty;
		}
	}

	public void Initialize(Reward_DeckPracitce reward)
	{
		mLabel_Message.text = $"{DeckPracticeTypeToString(reward.type)}演習\nが開放されました！";
		mTexture_PracticeImage.mainTexture = RequestDeckPracticeImage(reward.type);
	}

	private Texture RequestDeckPracticeImage(DeckPracticeType deckPracticeType)
	{
		switch (deckPracticeType)
		{
		case DeckPracticeType.Hou:
			return Resources.Load<Texture>("Textures/Duty/open_img3");
		case DeckPracticeType.Kouku:
			return Resources.Load<Texture>("Textures/Duty/open_img6");
		case DeckPracticeType.Rai:
			return Resources.Load<Texture>("Textures/Duty/open_img4");
		case DeckPracticeType.Sougou:
			return Resources.Load<Texture>("Textures/Duty/open_img7");
		case DeckPracticeType.Taisen:
			return Resources.Load<Texture>("Textures/Duty/open_img5");
		default:
			return null;
		}
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Message);
		UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_PracticeImage);
	}
}
