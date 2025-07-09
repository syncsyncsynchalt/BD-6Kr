using local.models;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIHowToRemodel : MonoBehaviour
	{
		[SerializeField]
		private UserInterfaceRemodelManager _rm;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private GameObject SpriteButtonL;

		[SerializeField]
		private GameObject SpriteButtonR;

		[SerializeField]
		private GameObject SpriteButtonShikaku;

		[SerializeField]
		private GameObject SpriteButtonO;

		[SerializeField]
		private GameObject SpriteStickR;

		private KeyControl key;

		private KeyControl key2;

		private ScreenStatus _now_mode;

		private SettingModel model;

		private UILabel _uil;

		private float time;

		private bool isShow;

		private static readonly Vector3 ShowPos = Vector3.right * -450f + Vector3.up * -259f;

		private static readonly Vector3 HidePos = Vector3.right * -450f + Vector3.up * -289f;

		private void Awake()
		{
			Transform transform = base.transform;
			Vector3 hidePos = HidePos;
			transform.localPositionY(hidePos.y);
			model = new SettingModel();
			key2 = new KeyControl(0, 1);
			key2.setChangeValue(0f, 0f, 0f, 0f);
			SpriteButtonShikaku.transform.localScale = Vector3.zero;
			_now_mode = ScreenStatus.MODE_SOUBI_HENKOU;
			if (SpriteButtonR == null)
			{
				Util.FindParentToChild(ref SpriteButtonR, SpriteButtonL.transform.parent, "SpriteButtonR");
			}
			if (SpriteButtonO == null)
			{
				Util.FindParentToChild(ref SpriteButtonO, SpriteButtonL.transform.parent, "SpriteButtonO");
			}
		}

		private void R_slide(bool value)
		{
			if (!value)
			{
				SpriteButtonL.transform.localPositionX(186f);
				SpriteButtonR.transform.localPositionX(347f);
				SpriteButtonO.transform.localPositionX(444f);
				SpriteButtonX.transform.localPositionX(529f);
				SpriteButtonShikaku.transform.localPositionX(609f);
			}
			else
			{
				SpriteButtonL.transform.localPositionX(66f);
				SpriteButtonR.transform.localPositionX(227f);
				SpriteButtonO.transform.localPositionX(324f);
				SpriteButtonX.transform.localPositionX(409f);
				SpriteButtonShikaku.transform.localPositionX(489f);
			}
		}

		private void change_guide()
		{
			if (_now_mode != _rm.status)
			{
				switch (_now_mode)
				{
					case ScreenStatus.SELECT_SETTING_MODE:
						SpriteStickR.SetActive(true);
						R_slide(value: false);
						break;
					case ScreenStatus.MODE_SOUBI_HENKOU:
						SpriteStickR.SetActive(true);
						SpriteButtonShikaku.SetActive(false);
						R_slide(value: false);
						break;
					case ScreenStatus.MODE_KINDAIKA_KAISHU:
						SpriteStickR.SetActive(true);
						R_slide(value: false);
						break;
					case ScreenStatus.SELECT_OTHER_SHIP:
					case ScreenStatus.MODE_KINDAIKA_KAISHU_SOZAI_SENTAKU:
						SpriteStickR.SetActive(true);
						SpriteButtonShikaku.GetComponent<UISprite>().spriteName = "btn_shikaku";
						((Component)SpriteButtonShikaku.transform.Find("Label")).GetComponent<UILabel>().text = "全てはずす";
						SpriteButtonShikaku.transform.localScale = Vector3.zero;
						R_slide(value: false);
						break;
					case ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT:
						SpriteButtonShikaku.SetActive(false);
						R_slide(value: true);
						break;
				}
				_now_mode = _rm.status;
				switch (_rm.status)
				{
					case ScreenStatus.MODE_SOUBI_HENKOU:
						SpriteButtonShikaku.transform.localScale = Vector3.one;
						_uil = ((Component)SpriteButtonShikaku.transform.FindChild("Label")).GetComponent<UILabel>();
						SpriteStickR.SetActive(false);
						R_slide(value: true);
						SpriteButtonShikaku.SetActive(true);
						_uil.text = "全てはずす";
						break;
					case ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT:
						SpriteButtonShikaku.SetActive(true);
						R_slide(value: true);
						SpriteButtonShikaku.transform.localScale = Vector3.one;
						_uil = ((Component)SpriteButtonShikaku.transform.FindChild("Label")).GetComponent<UILabel>();
						_uil.text = "装備ロック";
						break;
					case ScreenStatus.SELECT_SETTING_MODE:
						SpriteStickR.SetActive(false);
						R_slide(value: true);
						break;
					case ScreenStatus.MODE_SOUBI_HENKOU_TYPE_SELECT:
						SpriteStickR.SetActive(false);
						R_slide(value: true);
						break;
					case ScreenStatus.MODE_KINDAIKA_KAISHU:
						SpriteStickR.SetActive(false);
						R_slide(value: true);
						break;
					case ScreenStatus.MODE_KINDAIKA_KAISHU_SOZAI_SENTAKU:
						SpriteStickR.SetActive(false);
						R_slide(value: true);
						SpriteButtonShikaku.SetActive(true);
						SpriteButtonShikaku.GetComponent<UISprite>().spriteName = "btn_sankaku";
						((Component)SpriteButtonShikaku.transform.Find("Label")).GetComponent<UILabel>().text = "ソート";
						SpriteButtonShikaku.transform.localScale = Vector3.one;
						break;
					case ScreenStatus.MODE_KAIZO:
						SpriteStickR.SetActive(false);
						R_slide(value: true);
						break;
					case ScreenStatus.SELECT_OTHER_SHIP:
						SpriteButtonShikaku.SetActive(true);
						SpriteButtonShikaku.GetComponent<UISprite>().spriteName = "btn_sankaku";
						((Component)SpriteButtonShikaku.transform.Find("Label")).GetComponent<UILabel>().text = "ソート";
						SpriteButtonShikaku.transform.localScale = Vector3.one;
						break;
					case ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN:
						SpriteStickR.SetActive(false);
						R_slide(value: true);
						break;
					default:
						SpriteButtonShikaku.transform.localScale = Vector3.zero;
						break;
				}
			}
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
				else if (2f < time && !isShow && !_rm.guideoff)
				{
					Show();
				}
			}
			change_guide();
			if (_rm.guideoff)
			{
				Hide();
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

		private void OnDestroy()
		{
			_rm = null;
			SpriteButtonX = null;
			SpriteButtonL = null;
			SpriteButtonR = null;
			SpriteButtonShikaku = null;
			SpriteButtonO = null;
			SpriteStickR = null;
			key = null;
			key2 = null;
			model = null;
			_uil = null;
		}
	}
}
