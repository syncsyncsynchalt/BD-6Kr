using KCV.Supply;
using local.models;
using UnityEngine;

namespace KCV.Port.Supply
{
	public class UIHowToSupply : MonoBehaviour
	{
		private KeyControl key;

		private KeyControl key2;

		private float time;

		private bool isShow;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private GameObject SpriteButtonL;

		[SerializeField]
		private GameObject SpriteButtonR;

		[SerializeField]
		private GameObject SpriteButtonShikaku;

		[SerializeField]
		private GameObject SpriteStickR;

		private UILabel _uiLabelX;

		private int now_mode;

		private static readonly Vector3 ShowPos = new Vector3(-450f, -259f, 0f);

		private static readonly Vector3 HidePos = new Vector3(-450f, -289f, 0f);

		private SettingModel model;

		private SupplyMainManager _smm;

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
			now_mode = 1;
			SpriteButtonX.transform.localScale = Vector3.zero;
			_uil = ((Component)SpriteButtonL.transform.FindChild("Label")).GetComponent<UILabel>();
			_uil.text = "提督コマンド";
			_uil = ((Component)SpriteButtonR.transform.FindChild("Label")).GetComponent<UILabel>();
			_smm = ((Component)base.transform.parent).GetComponent<SupplyMainManager>();
			Util.FindParentToChild(ref _uiLabelX, SpriteButtonX.transform, "Label");
		}

		private void OnDestroy()
		{
			key = null;
			key2 = null;
			SpriteButtonX = null;
			SpriteButtonL = null;
			SpriteButtonR = null;
			SpriteButtonShikaku = null;
			SpriteStickR = null;
			model = null;
			_smm = null;
			_uil = null;
			_uis = null;
		}

		private void change_guide()
		{
			if (_smm.isNowRightFocus())
			{
				SpriteButtonShikaku.transform.localScale = Vector3.zero;
				_setButtonX("戻る", 572f);
			}
			else if (_smm.isNowDeckIsOther())
			{
				SpriteButtonShikaku.transform.localScale = Vector3.one;
				_uil = ((Component)SpriteButtonShikaku.transform.FindChild("Label")).GetComponent<UILabel>();
				_uil.text = "ソート";
				_uis = SpriteButtonShikaku.GetComponent<UISprite>();
				_uis.spriteName = "btn_sankaku";
				_setButtonX("戻る", 662f);
			}
			else
			{
				SpriteButtonShikaku.transform.localScale = Vector3.one;
				_uil = ((Component)SpriteButtonShikaku.transform.FindChild("Label")).GetComponent<UILabel>();
				_uil.text = "まとめて選択";
				_uis = SpriteButtonShikaku.GetComponent<UISprite>();
				_uis.spriteName = "btn_shikaku";
				_setButtonX("戻る", 727f);
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
			change_guide();
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
