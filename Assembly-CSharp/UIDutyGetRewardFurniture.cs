using KCV.Scene.Port;
using local.models;
using UnityEngine;

public class UIDutyGetRewardFurniture : MonoBehaviour
{
	[SerializeField]
	private UILabel mLabel_Message;

	[SerializeField]
	private UITexture mTexture_Image;

	public void Initialize(Reward_Furniture rewardFurniture)
	{
		mLabel_Message.text = $"{rewardFurniture.Name}を\n手に入れました";
		mTexture_Image.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.Furniture.LoadInteriorStoreFurniture(rewardFurniture.Type, rewardFurniture.MstId);
		mTexture_Image.MakePixelPerfect();
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Message);
		UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Image);
	}
}
