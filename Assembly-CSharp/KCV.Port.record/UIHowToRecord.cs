using local.models;
using UnityEngine;

namespace KCV.Port.record
{
	public class UIHowToRecord : MonoBehaviour
	{
		private KeyControl key;

		private KeyControl key2;

		private float time;

		private bool isShow;

		private static readonly Vector3 ShowPos = new Vector3(1521f, -263f, 0f);

		private static readonly Vector3 HidePos = new Vector3(1521f, -289f, 0f);

		private SettingModel model;

		private void Awake()
		{
			Transform transform = base.transform;
			Vector3 hidePos = HidePos;
			transform.localPositionY(hidePos.y);
			model = new SettingModel();
			key2 = new KeyControl(0, 1);
			key2.setChangeValue(0f, 0f, 0f, 0f);
		}

		private void OnDestroy()
		{
			key = null;
			key2 = null;
			model = null;
		}

		private void Update()
		{
			key2.Update();
			SetKeyController(key2);
			if (key == null || !key.IsRun)
			{
				return;
			}
			time += Time.deltaTime;
			if (key.IsAnyKey)
			{
				time = 0f;
				if (isShow)
				{
					Hide();
				}
			}
			else if (2f < time && !isShow)
			{
				Show();
			}
		}

		public void SetKeyController(KeyControl key)
		{
			this.key = key;
			if (key == null && isShow)
			{
				Hide();
			}
		}

		public void Show()
		{
			if (model.GuideDisplay)
			{
				Util.MoveTo(base.gameObject, 0.4f, ShowPos, iTween.EaseType.easeInSine);
				isShow = true;
			}
		}

		public void Hide()
		{
			Util.MoveTo(base.gameObject, 0.4f, HidePos, iTween.EaseType.easeInSine);
			isShow = false;
		}
	}
}
