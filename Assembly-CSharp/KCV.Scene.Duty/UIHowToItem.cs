using local.models;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIHowToItem : MonoBehaviour
	{
		private KeyControl key;

		private KeyControl key2;

		private float time;

		private bool isShow;

		[SerializeField]
		private UserInterfaceItemManager _itemm;

		[SerializeField]
		private GameObject SpriteStickR;

		[SerializeField]
		private GameObject SpriteButtonO;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private DialogAnimation mDialogAnimation_Exchange;

		[SerializeField]
		private DialogAnimation mDialogAnimation_UseLimit;

		[SerializeField]
		private DialogAnimation mDialogAnimation_StoreBuy;

		[SerializeField]
		private DialogAnimation mDialogAnimation_UseConfirm;

		private UILabel _uiLabelX;

		private bool now_mode;

		private static readonly Vector3 ShowPos = new Vector3(-450f, -259f, 0f);

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
			key2 = new KeyControl(0, 1);
			key2.setChangeValue(0f, 0f, 0f, 0f);
			now_mode = true;
			SpriteButtonX.transform.localScale = Vector3.zero;
			Util.FindParentToChild(ref _uiLabelX, SpriteButtonX.transform, "Label");
			_setButtonX("戻る", 550f);
			R_hide(value: false);
		}

		private void R_hide(bool value)
		{
			if (!value)
			{
				SpriteStickR.transform.localPositionX(336f);
				SpriteStickR.transform.localScale = Vector3.one;
				SpriteButtonO.transform.localPositionX(466f);
				SpriteButtonX.transform.localPositionX(551f);
			}
			else
			{
				SpriteStickR.transform.localPositionX(336f);
				SpriteStickR.transform.localScale = Vector3.zero;
				SpriteButtonO.transform.localPositionX(336f);
				SpriteButtonX.transform.localPositionX(421f);
			}
		}

		private void change_guide()
		{
			if (now_mode != (mDialogAnimation_Exchange.IsOpen || mDialogAnimation_UseLimit.IsOpen || mDialogAnimation_StoreBuy.IsOpen || mDialogAnimation_UseConfirm.IsOpen))
			{
				now_mode = (mDialogAnimation_Exchange.IsOpen || mDialogAnimation_UseLimit.IsOpen || mDialogAnimation_StoreBuy.IsOpen || mDialogAnimation_UseConfirm.IsOpen);
				if (now_mode)
				{
					R_hide(value: true);
				}
				else
				{
					R_hide(value: false);
				}
			}
		}

		private void _setButtonX(string text, float posX)
		{
			SpriteButtonX.transform.localScale = ((!(text == string.Empty)) ? Vector3.one : Vector3.zero);
			SpriteButtonX.transform.localPositionX(posX);
			_uiLabelX.text = text;
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

		private void OnDestroy()
		{
			key = null;
			key2 = null;
			_itemm = null;
			SpriteButtonO = null;
			SpriteButtonX = null;
			SpriteStickR = null;
			mDialogAnimation_Exchange = null;
			mDialogAnimation_UseLimit = null;
			mDialogAnimation_StoreBuy = null;
			mDialogAnimation_UseConfirm = null;
			_uiLabelX = null;
			model = null;
			_uil = null;
			_uis = null;
		}
	}
}
