using local.models;
using System;
using UnityEngine;

namespace KCV.Utils
{
	public class ShipUtils
	{
		public static Texture2D LoadTexture(int mstID, int texNum)
		{
			if (SingletonMonoBehaviour<ResourceManager>.Instance == null)
			{
				DebugUtils.Error("リソ\u30fcスマネ\u30fcジャ\u30fcが存在しません。");
				return null;
			}
			return SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(mstID, texNum);
		}

		[Obsolete("")]
		public static Texture2D LoadTexture(ShipModel model, int texNum)
		{
			return LoadTexture(model.GetGraphicsMstId(), texNum);
		}

		public static Texture2D LoadTexture(ShipModel model, bool isDamaged)
		{
			return LoadTexture(model.GetGraphicsMstId(), (!isDamaged) ? 9 : 10);
		}

		public static Texture2D LoadTexture(ShipModel model)
		{
			return LoadTexture(model, model.IsDamaged());
		}

		public static Texture2D LoadBannerTexture(IShipModel model, bool isDamaged)
		{
			return LoadTexture(model.MstId, (!isDamaged) ? 1 : 2);
		}

		public static Texture2D LoadBannerTexture(ShipModel model)
		{
			return LoadBannerTexture(model, model.IsDamaged());
		}

		public static Texture2D LoadBannerTexture(ShipModel_BattleAll model)
		{
			return LoadBannerTexture(model, model.DamagedFlgEnd);
		}

		public static Texture2D LoadBannerTexture(ShipModel_BattleResult model)
		{
			bool isDamaged = model.IsFriend() && model.IsDamaged();
			return LoadBannerTexture(model, isDamaged);
		}

		public static Texture2D LoadCardTexture(IShipModel model, bool isDamaged)
		{
			return LoadTexture(model.MstId, (!isDamaged) ? 3 : 4);
		}

		public static Texture2D LoadCardTexture(ShipModel model)
		{
			return LoadCardTexture(model, model.IsDamaged());
		}

		public static void SetTexture(UITexture tex, ShipModel model, int texNum)
		{
			tex.mainTexture = LoadTexture(model.MstId, texNum);
			if (!(tex.mainTexture == null))
			{
				if (ResourceManager.SHIP_TEXTURE_SIZE.ContainsKey(texNum))
				{
					tex.localSize = ResourceManager.SHIP_TEXTURE_SIZE[texNum];
				}
				else
				{
					tex.MakePixelPerfect();
				}
			}
		}

		public static int GetShipStandingTextureID(bool isFriend, bool isDamaged)
		{
			if (!isFriend)
			{
				return 9;
			}
			if (isDamaged)
			{
				return 10;
			}
			return 9;
		}

		private static AudioSource PlayShipVoice(int mstId, int voiceNum, int channel, Action onFinished)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null && SingletonMonoBehaviour<ResourceManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(mstId, voiceNum), channel, isObserver: true, onFinished);
			}
			return null;
		}

		public static AudioSource PlayEndingVoice(ShipModel model, int voiceNum)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null && SingletonMonoBehaviour<ResourceManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(model.GetVoiceMstId(voiceNum), voiceNum), 0, isObserver: false, null);
			}
			return null;
		}

		public static AudioSource PlayShipVoice(ShipModelMst model, int voiceNum, int channel, Action onFinished)
		{
			return PlayShipVoice(model.GetVoiceMstId(voiceNum), voiceNum, channel, onFinished);
		}

		public static AudioSource PlayShipVoice(ShipModelMst model, int voiceNum)
		{
			return PlayShipVoice(model, voiceNum, 0, null);
		}

		public static AudioSource PlayShipVoice(ShipModelMst model, int voiceNum, Action onFinished)
		{
			return PlayShipVoice(model, voiceNum, 0, onFinished);
		}

		public static AudioSource PlayPortVoice(int voiceNum)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null && SingletonMonoBehaviour<ResourceManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.PlayOneShotVoice(SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(0, voiceNum));
			}
			return null;
		}

		public static AudioSource PlayPortVoice(int voiceNum, Action Onfinished)
		{
			return PlayShipVoice(0, voiceNum, 0, Onfinished);
		}

		public static AudioSource PlayTitleVoice(int voiceNum)
		{
			return PlayShipVoice(9999, voiceNum, 0, null);
		}

		public static AudioSource StopShipVoice(int channel)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null && SingletonMonoBehaviour<ResourceManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.StopVoice(channel);
			}
			return null;
		}

		public static AudioSource StopShipVoice()
		{
			return StopShipVoice(0);
		}

		public static AudioSource StopShipVoice(AudioSource source, bool isCallOnFinished, float fDuration)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.StopVoice(source, isCallOnFinished, fDuration);
			}
			return null;
		}
	}
}
