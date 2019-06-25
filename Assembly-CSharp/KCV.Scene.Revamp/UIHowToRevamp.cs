using local.models;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIHowToRevamp : MonoBehaviour
	{
		private KeyControl keyController;

		private float time;

		private bool isShow;

		[SerializeField]
		private UserInterfaceRevampManager _revm;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private GameObject SpriteButtonShikaku;

		private bool now_mode;

		private static readonly Vector3 ShowPos = new Vector3(-450f, -253f, 0f);

		private static readonly Vector3 HidePos = new Vector3(-450f, -289f, 0f);

		private SettingModel model;

		private UILabel _uil;

		private UISprite _uis;

		private void Awake()
		{
			Transform transform = base.transform;
			Vector3 hidePos = HidePos;
			transform.localPositionY(hidePos.y);
			model = new SettingModel();
			keyController = new KeyControl();
			now_mode = true;
			SpriteButtonX.transform.localScale = Vector3.one;
			SpriteButtonShikaku.transform.localPositionX(469f);
		}

		private void Update()
		{
			if (keyController == null)
			{
				if (isShow)
				{
					Hide();
				}
			}
			else
			{
				keyController.Update();
				if (keyController.IsRun)
				{
					time += Time.deltaTime;
					if (keyController.IsAnyKey)
					{
						time = 0f;
						if (isShow)
						{
							Hide();
						}
					}
					else if (2f < time && !isShow && !_revm._isAnimation)
					{
						Show();
					}
				}
			}
			if (_revm._isSettingMode)
			{
				SpriteButtonShikaku.SetActive(true);
			}
			else
			{
				SpriteButtonShikaku.SetActive(false);
			}
			if (_revm._isAnimation)
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

		private void OnDestroy()
		{
			keyController = null;
			_revm = null;
			SpriteButtonX = null;
			SpriteButtonShikaku = null;
			model = null;
			_uil = null;
			_uis = null;
		}
	}
}
