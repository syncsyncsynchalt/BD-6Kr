using KCV.Remodel;
using local.models;
using UnityEngine;

namespace KCV.Organize
{
	public class UIHowToOrganize : MonoBehaviour
	{
		[SerializeField]
		private TaskOrganizeTop _tot;

		[SerializeField]
		private OrganizeTender _tod;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private GameObject _buttonSR;

		[SerializeField]
		private GameObject _buttonL;

		[SerializeField]
		private GameObject _buttonR;

		[SerializeField]
		private GameObject _buttonMaru;

		[SerializeField]
		private GameObject _buttonBatu;

		[SerializeField]
		private GameObject _buttonShikaku;

		[SerializeField]
		private GameObject _buttonSankaku;

		private UILabel _uiLabelShikaku;

		private UILabel _uiLabelBatu;

		private ScreenStatus _now_mode;

		private KeyControl key;

		private KeyControl key2;

		private float time;

		private bool isShow;

		private static readonly Vector3 ShowPos = Vector3.right * 600f + Vector3.up * -959f;

		private static readonly Vector3 HidePos = Vector3.right * 600f + Vector3.up * -989f;

		private SettingModel model;

		private void Awake()
		{
			Transform transform = base.transform;
			Vector3 hidePos = HidePos;
			transform.localPositionY(hidePos.y);
			model = new SettingModel();
			key2 = new KeyControl(0, 1);
			key2.setChangeValue(0f, 0f, 0f, 0f);
			_buttonShikaku.transform.localScale = Vector3.zero;
			Util.FindParentToChild(ref _uiLabelShikaku, _buttonShikaku.transform, "Label");
			Util.FindParentToChild(ref _uiLabelBatu, _buttonBatu.transform, "Label");
			_uiLabelShikaku.text = "はずす";
		}

		private void change_guide()
		{
			switch (_tot._state2)
			{
			case TaskOrganizeTop.OrganizeState.Top:
				_setButtonX("戻る", 656f);
				_setButtonShikaku("一括解除", 530f);
				_buttonL.transform.localPositionX(186f);
				_buttonR.transform.localPositionX(347f);
				_buttonMaru.transform.localPositionX(447f);
				_buttonSankaku.transform.localScale = Vector3.zero;
				_buttonSR.transform.localScale = Vector3.one;
				break;
			case TaskOrganizeTop.OrganizeState.Detail:
				_setButtonX("戻る", 413f);
				_setButtonShikaku(string.Empty, 0f);
				_buttonL.transform.localPositionX(67f);
				_buttonR.transform.localPositionX(229f);
				_buttonMaru.transform.localPositionX(328f);
				_buttonSR.transform.localScale = Vector3.zero;
				_buttonSankaku.transform.localScale = Vector3.zero;
				break;
			case TaskOrganizeTop.OrganizeState.DetailList:
				_setButtonX("戻る", 413f);
				_setButtonShikaku("ロック", 490f);
				_buttonL.transform.localPositionX(67f);
				_buttonR.transform.localPositionX(229f);
				_buttonMaru.transform.localPositionX(328f);
				_buttonSR.transform.localScale = Vector3.zero;
				_buttonSankaku.transform.localScale = Vector3.zero;
				break;
			case TaskOrganizeTop.OrganizeState.List:
				_setButtonX("戻る", 413f);
				_setButtonShikaku("ロック", 490f);
				_buttonL.transform.localPositionX(67f);
				_buttonR.transform.localPositionX(229f);
				_buttonMaru.transform.localPositionX(328f);
				_buttonSR.transform.localScale = Vector3.zero;
				_buttonSankaku.transform.localScale = Vector3.one;
				_buttonSankaku.transform.localPositionX(583f);
				break;
			case TaskOrganizeTop.OrganizeState.System:
				_setButtonX("戻る", 530f);
				_setButtonShikaku(string.Empty, 0f);
				_buttonL.transform.localPositionX(186f);
				_buttonR.transform.localPositionX(347f);
				_buttonMaru.transform.localPositionX(447f);
				_buttonSankaku.transform.localScale = Vector3.zero;
				_buttonSR.transform.localScale = Vector3.one;
				break;
			case TaskOrganizeTop.OrganizeState.Tender:
				_setButtonX("戻る", 413f);
				_setButtonShikaku(string.Empty, 0f);
				_buttonL.transform.localPositionX(67f);
				_buttonR.transform.localPositionX(229f);
				_buttonMaru.transform.localPositionX(328f);
				_buttonSR.transform.localScale = Vector3.zero;
				_buttonSankaku.transform.localScale = Vector3.zero;
				break;
			}
		}

		private void _setButtonShikaku(string text, float posX)
		{
			_buttonShikaku.transform.localScale = ((!(text == string.Empty)) ? Vector3.one : Vector3.zero);
			_buttonShikaku.transform.localPositionX(posX);
			_uiLabelShikaku.text = text;
		}

		private void _setButtonX(string text, float posX)
		{
			_buttonBatu.transform.localScale = ((!(text == string.Empty)) ? Vector3.one : Vector3.zero);
			_buttonBatu.transform.localPositionX(posX);
			_uiLabelBatu.text = text;
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
					if (isShow || _tod._GuideOff)
					{
						Hide();
					}
				}
				else if (2f < time && !isShow && !_tod._GuideOff)
				{
					Show();
				}
			}
			change_guide();
			if (_tod._GuideOff)
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
			_tot = null;
			_tod = null;
			SpriteButtonX = null;
			_buttonSR = null;
			_buttonL = null;
			_buttonR = null;
			_buttonMaru = null;
			_buttonBatu = null;
			_buttonShikaku = null;
			_buttonSankaku = null;
			_uiLabelShikaku = null;
			_uiLabelBatu = null;
			key = null;
			key2 = null;
			model = null;
		}
	}
}
