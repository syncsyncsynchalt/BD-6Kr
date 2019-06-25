using local.models;
using UnityEngine;

namespace KCV
{
	public class BaseDialogPopup
	{
		private UITexture BG;

		public void setBG(UITexture bg)
		{
			BG = bg;
		}

		public virtual bool Init(ShipModel ship, UITexture _texture)
		{
			return true;
		}

		public virtual void Open(GameObject obj, float fromX, float fromY, float toX, float toY)
		{
			Vector3 localScale = new Vector3(fromX, fromY);
			obj.transform.localScale = localScale;
			iTween.ScaleTo(obj, iTween.Hash("islocal", true, "x", toX, "y", toY, "z", 1f, "time", 0.4f, "easetype", iTween.EaseType.easeOutBack));
			obj.SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, obj, string.Empty);
			if (BG != null)
			{
				TweenAlpha.Begin(BG.gameObject, 0.2f, 0.8f);
			}
		}

		public static void Open2(GameObject obj, float fromX, float fromY, float toX, float toY)
		{
			Vector3 localScale = new Vector3(fromX, fromY);
			obj.transform.localScale = localScale;
			iTween.ScaleTo(obj, iTween.Hash("islocal", true, "x", toX, "y", toY, "z", 1f, "time", 0.4f, "easetype", iTween.EaseType.easeOutBack));
			obj.SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, obj, string.Empty);
		}

		public virtual void Close(GameObject obj, float fromX, float fromY, float toX, float toY)
		{
			Vector3 localScale = new Vector3(fromX, fromY);
			obj.transform.localScale = localScale;
			iTween.ScaleTo(obj, iTween.Hash("islocal", true, "x", toX, "y", toY, "z", 1f, "time", 0.4f, "easetype", iTween.EaseType.linear));
			if (BG != null)
			{
				TweenAlpha.Begin(BG.gameObject, 0.4f, 0f);
			}
		}

		public static void Close(GameObject obj, float duration, UITweener.Method _tween)
		{
			obj.SafeGetTweenAlpha(1f, 0f, duration, 0f, UITweener.Method.Linear, UITweener.Style.Once, obj, string.Empty);
		}

		public virtual void Open(GameObject obj, Vector3 from, Vector3 to)
		{
			Open(obj, from.x, from.y, to.x, to.y);
		}

		public virtual void Close(GameObject obj, Vector3 from, Vector3 to)
		{
			Close(obj, from.x, from.y, to.x, to.y);
		}
	}
}
