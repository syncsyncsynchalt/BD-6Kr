using local.models;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UIHowToArsenal : MonoBehaviour
	{
		[SerializeField]
		private GameObject _DesideBtn;

		[SerializeField]
		private GameObject _CancelBtn;

		[SerializeField]
		private GameObject _CrossBtn;

		[SerializeField]
		private TaskMainArsenalManager taskMainArsenalManager;

		[SerializeField]
		private GameObject _Btn_L;

		[SerializeField]
		private GameObject _Btn_R;

		[SerializeField]
		private GameObject _Btn_Shikaku;

		[SerializeField]
		private TaskArsenalListManager taskArsenalListManager;

		private UILabel _DesideBtnLabel;

		private UILabel _CancelBtnLabel;

		private UISprite _DesideBtnSprite;

		private UISprite _CrossBtnSprite;

		private UISprite _ShikakuBtnSprite;

		private UILabel _Btn_L_Label;

		private UILabel _Btn_R_Label;

		private UILabel _Btn_Shikaku_Label;

		private KeyControl key;

		private KeyControl key2;

		private float time;

		private bool isShow;

		private bool _now_mode;

		private static readonly Vector3 ShowPos = new Vector3(-480f, -259f, 0f);

		private static readonly Vector3 HidePos = new Vector3(-480f, -289f, 0f);

		private SettingModel model;

		private bool _IsLightFocusLeft;

		private string Now_status = string.Empty;

		private void Awake()
		{
			Transform transform = base.transform;
			Vector3 hidePos = HidePos;
			transform.localPositionY(hidePos.y);
			model = new SettingModel();
			key2 = new KeyControl(0, 1);
			key2.setChangeValue(0f, 0f, 0f, 0f);
			_DesideBtnLabel = ((Component)_DesideBtn.transform.FindChild("Label")).GetComponent<UILabel>();
			_DesideBtnSprite = ((Component)_DesideBtn.transform.FindChild("Icon")).GetComponent<UISprite>();
			_IsLightFocusLeft = true;
			Now_status = string.Empty;
			_DesideBtnLabel.text = "決定";
			_DesideBtnSprite.spriteName = "btn_maru";
			_CancelBtnLabel = ((Component)_CancelBtn.transform.FindChild("Label")).GetComponent<UILabel>();
			_CancelBtnLabel.text = "戻る";
			_CrossBtnSprite = ((Component)_CrossBtn.transform.FindChild("Icon")).GetComponent<UISprite>();
			_CrossBtnSprite.spriteName = "arrow_UDLR";
			_now_mode = true;
			_Btn_L_Label = ((Component)_Btn_L.transform.FindChild("Label")).GetComponent<UILabel>();
			_Btn_L_Label.text = "提督コマンド";
			_Btn_R_Label = ((Component)_Btn_R.transform.FindChild("Label")).GetComponent<UILabel>();
			_Btn_R_Label.text = "戦略へ";
			_Btn_Shikaku.transform.localScale = Vector3.zero;
			_Btn_Shikaku.transform.localPositionX(525f);
			_ShikakuBtnSprite = ((Component)_Btn_Shikaku.transform.FindChild("Icon")).GetComponent<UISprite>();
			_Btn_Shikaku_Label = ((Component)_Btn_Shikaku.transform.FindChild("Label")).GetComponent<UILabel>();
			_Btn_Shikaku_Label.text = "高速建造材";
		}

		private void OnDestroy()
		{
			_DesideBtn = null;
			_CancelBtn = null;
			_CrossBtn = null;
			taskMainArsenalManager = null;
			_Btn_L = null;
			_Btn_R = null;
			_Btn_Shikaku = null;
			taskArsenalListManager = null;
			key = null;
			key2 = null;
			_DesideBtnLabel = null;
			_CancelBtnLabel = null;
			_DesideBtnSprite = null;
			_CrossBtnSprite = null;
			_ShikakuBtnSprite = null;
			_Btn_L_Label = null;
			_Btn_R_Label = null;
			_Btn_Shikaku_Label = null;
			model = null;
		}

		public void change_guide()
		{
			Vector3 localPosition = GameObject.Find("DismantlePanel").transform.localPosition;
			if (localPosition.x < 10f && GameObject.Find("DismantlePanel/UIShipSortButton") != null)
			{
				Vector3 localPosition2 = GameObject.Find("DismantlePanel/OverlayBtn4").transform.localPosition;
				if (localPosition2.x != -344f)
				{
					_Btn_Shikaku.transform.localScale = Vector3.one;
					_Btn_Shikaku.transform.localPositionX(525f);
					_ShikakuBtnSprite.spriteName = "btn_sankaku";
					_Btn_Shikaku_Label.text = "ソート";
					goto IL_0152;
				}
			}
			_Btn_Shikaku.transform.localPositionX(525f);
			_ShikakuBtnSprite.spriteName = "btn_shikaku";
			_Btn_Shikaku_Label.text = "高速建造材";
			if (taskMainArsenalManager.isInConstructDialog())
			{
				_Btn_Shikaku.transform.localScale = Vector3.zero;
				_Btn_Shikaku.transform.localPositionX(525f);
			}
			else
			{
				_Btn_Shikaku.transform.localScale = Vector3.one;
				_Btn_Shikaku.transform.localPositionX(525f);
			}
			goto IL_0152;
		IL_0152:
			_now_mode = taskMainArsenalManager.isInConstructDialog();
			if (taskArsenalListManager._ShikakuON)
			{
				_DesideBtnSprite.spriteName = "btn_shikaku";
			}
			else
			{
				_DesideBtnSprite.spriteName = "btn_maru";
			}
		}

		private void _setButtonX(string text, float posX)
		{
			_CancelBtn.transform.localScale = ((!(text == string.Empty)) ? Vector3.one : Vector3.zero);
			_CancelBtn.transform.localPositionX(posX);
			_CancelBtnLabel.text = text;
		}

		public void DesideBtnChange()
		{
		}

		private void Update()
		{
			change_guide();
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
