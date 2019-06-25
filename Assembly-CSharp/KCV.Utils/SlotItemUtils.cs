using local.models;
using UnityEngine;

namespace KCV.Utils
{
	public class SlotItemUtils
	{
		public static Texture2D LoadTexture(int mstID, int texNum)
		{
			if (SingletonMonoBehaviour<ResourceManager>.Instance != null)
			{
				return SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(mstID, texNum);
			}
			return null;
		}

		public static Texture2D LoadTexture(SlotitemModel model, int texNum)
		{
			return LoadTexture(model.MstId, texNum);
		}

		public static void SetTexture(UITexture tex, int mstID, int texNum)
		{
			tex.mainTexture = LoadTexture(mstID, texNum);
			if (!(tex.mainTexture == null))
			{
				if (ResourceManager.SLOTITEM_TEXTURE_SIZE.ContainsKey(texNum))
				{
					tex.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE[texNum];
				}
				else
				{
					tex.MakePixelPerfect();
				}
			}
		}

		public static void SetTexture(UITexture tex, SlotitemModel model, int texNum)
		{
			SetTexture(tex, model.MstId, texNum);
		}
	}
}
