using local.models;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class UIHowToRepair : MonoBehaviour
	{
		private KeyControl key;

		private KeyControl key2;

		private float time;

		private bool isShow;

		private repair _rep;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private GameObject SpriteButtonL;

		[SerializeField]
		private GameObject SpriteButtonShikaku;

		[SerializeField]
		private GameObject SpriteButtonSankaku;

		private UILabel _uiLabelX;

		private int now_mode;

		private static readonly Vector3 ShowPos = Vector3.right * -450f + Vector3.up * -259f;

		private static readonly Vector3 HidePos = Vector3.right * -450f + Vector3.up * -289f;

		private SettingModel model;

		private UILabel _uil;

		private void Awake()
		{
			Transform transform = base.transform;
			Vector3 hidePos = HidePos;
			transform.localPositionY(hidePos.y);
			model = new SettingModel();
			key2 = new KeyControl(0, 1);
			key2.setChangeValue(0f, 0f, 0f, 0f);
			_rep = ((Component)base.gameObject.transform.parent).GetComponent<repair>();
			now_mode = 1;
			SpriteButtonX.transform.localScale = Vector3.zero;
			_uil = ((Component)SpriteButtonL.transform.FindChild("Label")).GetComponent<UILabel>();
			_uil.text = "提督コマンド";
			Util.FindParentToChild(ref _uiLabelX, SpriteButtonX.transform, "Label");
			_setButtonX("戻る", 429f);
			SpriteButtonShikaku.transform.localScale = Vector3.zero;
			SpriteButtonSankaku.transform.localScale = Vector3.zero;
		}

		private void OnDestroy()
		{
			key = null;
			key2 = null;
			_rep = null;
			SpriteButtonX = null;
			SpriteButtonL = null;
			SpriteButtonShikaku = null;
			SpriteButtonSankaku = null;
			_uiLabelX = null;
			model = null;
			_uil = null;
		}

		private void change_guide()
		{
			if (_rep.now_mode() != now_mode)
			{
				now_mode = _rep.now_mode();
				if (now_mode == 1)
				{
					_setButtonX("戻る", 429f);
				}
				else
				{
					_setButtonX("戻る", 429f);
				}
				if (now_mode == 2)
				{
					SpriteButtonSankaku.transform.localScale = Vector3.one;
				}
				else
				{
					SpriteButtonSankaku.transform.localScale = Vector3.zero;
				}
				if (now_mode == 3)
				{
					SpriteButtonShikaku.transform.localScale = Vector3.one;
				}
				else
				{
					SpriteButtonShikaku.transform.localScale = Vector3.zero;
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
	}
}
