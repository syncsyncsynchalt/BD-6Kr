using Common.Enum;
using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class TileAnimationAttackShell : MonoBehaviour
	{
		private UITexture tex;

		public void Awake()
		{
			tex = GetComponent<UITexture>();
			if (tex == null)
			{
				Debug.Log("Warning: UITexture not attached");
			}
			tex.alpha = 0f;
		}

		public void Initialize(Vector3 origin, Vector3 target, RadingKind type)
		{
			base.transform.localPosition = origin;
			base.transform.localScale = Vector3.one;
			base.transform.eulerAngles = Vector3.zero;
			tex.alpha = 0f;
			switch (type)
			{
			case RadingKind.AIR_ATTACK:
				if (StrategyTopTaskManager.GetLogicManager().Turn >= 500)
				{
					tex.mainTexture = (Resources.Load("Textures/TileAnimations/item_up_e54") as Texture);
					tex.width = 60;
					tex.height = 60;
				}
				else
				{
					tex.mainTexture = (Resources.Load("Textures/TileAnimations/item_up_506_2") as Texture);
					tex.width = 50;
					tex.height = 100;
				}
				tex.alpha = 1f;
				base.transform.localScale = 0.001f * Vector3.one;
				iTween.ScaleTo(base.gameObject, iTween.Hash("scale", Vector3.one, "islocal", true, "time", 0.2f, "delay", 0.5f, "easeType", iTween.EaseType.easeInOutQuad));
				iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.05f, "delay", 0.5f, "onupdate", "Alpha", "onupdatetarget", base.gameObject));
				iTween.MoveTo(base.gameObject, iTween.Hash("path", new Vector3[3]
				{
					origin,
					origin + 0.5f * (target - origin) + 50f * Vector3.down,
					1.2f * target - 0.2f * origin
				}, "islocal", true, "time", 1.2f, "easeType", iTween.EaseType.linear));
				iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.05f, "delay", 1.15f, "onupdate", "Alpha", "onupdatetarget", base.gameObject));
				this.DelayAction(0.2f, delegate
				{
					SoundUtils.PlaySE(SEFIleInfos.BattleTookOffAircraft);
				});
				break;
			case RadingKind.SUBMARINE_ATTACK:
			{
				tex.mainTexture = (Resources.Load("Textures/TileAnimations/kouseki") as Texture);
				tex.MakePixelPerfect();
				Vector3 vector2 = target - origin;
				base.transform.Rotate(Vector3.forward, 180f / (float)Math.PI * Mathf.Atan2(vector2.y, vector2.x));
				base.transform.localScale = new Vector3(0.001f, 0.75f, 0.75f);
				iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.05f, "onupdate", "Alpha", "onupdatetarget", base.gameObject));
				iTween.ScaleTo(base.gameObject, iTween.Hash("scale", 0.75f * Vector3.one, "islocal", true, "time", 0.4f, "easeType", iTween.EaseType.linear));
				iTween.MoveTo(base.gameObject, iTween.Hash("position", 0.8f * target + 0.2f * origin, "islocal", true, "time", 1, "easeType", iTween.EaseType.linear));
				iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.05f, "delay", 0.95f, "onupdate", "Alpha", "onupdatetarget", base.gameObject));
				SoundUtils.PlaySE(SEFIleInfos.BattleTorpedo);
				break;
			}
			default:
			{
				tex.mainTexture = (Resources.Load("Textures/TileAnimations/fire_5") as Texture);
				tex.MakePixelPerfect();
				Vector3 vector = target - origin;
				base.transform.Rotate(Vector3.forward, 180f / (float)Math.PI * Mathf.Atan2(vector.y, vector.x));
				iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.05f, "onupdate", "Alpha", "onupdatetarget", base.gameObject));
				iTween.MoveTo(base.gameObject, iTween.Hash("position", target, "islocal", true, "time", 0.25f, "easeType", iTween.EaseType.linear));
				iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.05f, "delay", 0.2f, "onupdate", "Alpha", "onupdatetarget", base.gameObject));
				SoundUtils.PlaySE(SEFIleInfos.SE_901);
				break;
			}
			}
		}

		public void Alpha(float f)
		{
			tex.alpha = f;
		}

		private void OnDestroy()
		{
			tex = null;
		}
	}
}
