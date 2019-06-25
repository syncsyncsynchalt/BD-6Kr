using KCV.Scene.Port;
using local.models;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIHowToDuty : MonoBehaviour
	{
		private KeyControl key;

		private KeyControl key2;

		private float time;

		private bool isShow;

		[SerializeField]
		private UserInterfaceDutyManager _dm;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private GameObject SpriteButtonShikaku;

		private bool now_mode;

		private readonly Vector3 ShowPos = new Vector3(-450f, -259f, 0f);

		private readonly Vector3 HidePos = new Vector3(-450f, -289f, 0f);

		private SettingModel model;

		private UILabel _uil;

		private UISprite _uis;

		private void Awake()
		{
			Transform transform = base.transform;
			Vector3 hidePos = HidePos;
			transform.localPositionY(hidePos.y);
			model = new SettingModel();
			key2 = new KeyControl(0, 1);
			key2.setChangeValue(0f, 0f, 0f, 0f);
			now_mode = false;
			SpriteButtonShikaku.transform.localScale = Vector3.one;
			_uil = ((Component)SpriteButtonX.transform.FindChild("Label")).GetComponent<UILabel>();
			_setButtonX("戻る", 605f);
			GetComponent<UIPanel>().depth = 0;
		}

		private void change_guide()
		{
			if (now_mode != _dm._DeteilMode)
			{
				now_mode = _dm._DeteilMode;
				if (_dm._DeteilMode)
				{
					_setButtonX("戻る", 432f);
					SpriteButtonShikaku.transform.localScale = Vector3.zero;
				}
				else
				{
					_setButtonX("戻る", 605f);
					SpriteButtonShikaku.transform.localScale = Vector3.one;
				}
			}
		}

		private void _setButtonX(string text, float posX)
		{
			SpriteButtonX.transform.localScale = ((!(text == string.Empty)) ? Vector3.one : Vector3.zero);
			SpriteButtonX.transform.localPositionX(posX);
			_uil.text = text;
		}

		private void Update()
		{
			key2.Update();
			SetKeyController(key2);
			if (key != null && key.IsRun)
			{
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
			change_guide();
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

		private void OnDestriy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref _uil);
			UserInterfacePortManager.ReleaseUtils.Release(ref _uis);
			key = null;
			key2 = null;
			_dm = null;
			SpriteButtonX = null;
			SpriteButtonShikaku = null;
			model = null;
		}
	}
}
