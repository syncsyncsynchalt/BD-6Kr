using local.models;
using UnityEngine;

namespace KCV.Interior
{
	public class UIHowToInterior : MonoBehaviour
	{
		private KeyControl key;

		private KeyControl key2;

		private float time;

		private bool isShow;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private GameObject SpriteButtonO;

		[SerializeField]
		private GameObject SpriteStickR;

		private bool now_mode;

		private static readonly Vector3 ShowPos = new Vector3(-450f, -253f, 0f);

		private static readonly Vector3 HidePos = new Vector3(-450f, -289f, 0f);

		private SettingModel model;

		private UILabel _uil;

		private UISprite _uis;

		private GameObject _tes;

		private void Awake()
		{
			Transform transform = base.transform;
			Vector3 hidePos = HidePos;
			transform.localPositionY(hidePos.y);
			model = new SettingModel();
			key2 = new KeyControl(0, 1);
			key2.setChangeValue(0f, 0f, 0f, 0f);
			now_mode = true;
			if (SpriteButtonO == null)
			{
				Util.FindParentToChild(ref SpriteButtonO, SpriteButtonX.transform.parent, "SpriteButtonO");
			}
			if (SpriteStickR == null)
			{
				Util.FindParentToChild(ref SpriteStickR, SpriteButtonX.transform.parent, "SpriteStickR");
			}
		}

		private bool isRuse()
		{
			if (GameObject.Find("CategoryTabs") != null)
			{
				if (GameObject.Find("CategoryTabs/Tab1").GetComponent<UISprite>().color != Color.white || GameObject.Find("CategoryTabs/Tab2").GetComponent<UISprite>().color != Color.white)
				{
					return false;
				}
				return true;
			}
			if (GameObject.Find("UIInteriorFurnitureDetail") != null)
			{
				Vector3 localPosition = GameObject.Find("UIInteriorFurnitureDetail").GetComponent<Transform>().localPosition;
				if (localPosition.x < 10f)
				{
					return false;
				}
				return true;
			}
			return true;
		}

		private bool isPreview()
		{
			if (GameObject.Find("UIInteriorFurnitureDetail") != null)
			{
				Vector3 localScale = GameObject.Find("UIInteriorFurnitureDetail").GetComponent<Transform>().localScale;
				if (localScale.x != 1f)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		private void change_guide()
		{
			if (isRuse())
			{
				SpriteStickR.transform.localPositionX(334f);
				SpriteStickR.transform.localScale = Vector3.one;
				SpriteButtonO.transform.localPositionX(457f);
				SpriteButtonX.transform.localPositionX(542f);
			}
			else
			{
				SpriteStickR.transform.localPositionX(334f);
				SpriteStickR.transform.localScale = Vector3.zero;
				SpriteButtonO.transform.localPositionX(334f);
				SpriteButtonX.transform.localPositionX(418f);
			}
			if (isPreview())
			{
				Hide();
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
					if (isShow || isPreview())
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
			if (model.GuideDisplay && !isPreview())
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
			SpriteButtonX = null;
			SpriteButtonO = null;
			SpriteStickR = null;
			model = null;
			_uil = null;
			_uis = null;
		}
	}
}
