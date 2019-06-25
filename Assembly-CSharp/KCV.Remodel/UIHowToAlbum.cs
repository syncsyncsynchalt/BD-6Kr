using local.models;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIHowToAlbum : MonoBehaviour
	{
		public enum GuideState
		{
			Gate,
			List,
			Detail,
			Quiet
		}

		private enum KeyType
		{
			MARU,
			BATU,
			L,
			Arrow,
			RS
		}

		[SerializeField]
		private GameObject SpriteStickR;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private GameObject SpriteButtonL;

		[SerializeField]
		private GameObject SpriteButtonO;

		private Vector3 ShowPos = Vector3.right * -450f + Vector3.up * -259f;

		private Vector3 HidePos = Vector3.right * -450f + Vector3.up * -289f;

		private ScreenStatus _now_mode;

		private KeyControl key;

		private float time;

		private bool isShow;

		private SettingModel model;

		private UILabel _uil;

		private GuideState mCurrentGuideState;

		private void Awake()
		{
			key = new KeyControl();
			base.transform.localPositionY(HidePos.y);
			model = new SettingModel();
			_now_mode = ScreenStatus.MODE_SOUBI_HENKOU;
		}

		private void HideKey(KeyType keyType)
		{
			switch (keyType)
			{
			case KeyType.MARU:
				break;
			case KeyType.BATU:
				break;
			case KeyType.L:
				break;
			case KeyType.Arrow:
				break;
			case KeyType.RS:
				SpriteStickR.transform.localScale = Vector3.zero;
				SpriteButtonL.transform.localPositionX(71f);
				SpriteButtonO.transform.localPositionX(330f);
				SpriteButtonX.transform.localPositionX(415f);
				break;
			}
		}

		private void ShowKey(KeyType keyType)
		{
			switch (keyType)
			{
			case KeyType.MARU:
				break;
			case KeyType.BATU:
				break;
			case KeyType.L:
				break;
			case KeyType.Arrow:
				break;
			case KeyType.RS:
				SpriteStickR.transform.localScale = Vector3.one;
				SpriteButtonL.transform.localPositionX(164f);
				SpriteButtonO.transform.localPositionX(425f);
				SpriteButtonX.transform.localPositionX(512f);
				break;
			}
		}

		public void ChangeGuideStatus(GuideState state)
		{
			switch (mCurrentGuideState)
			{
			case GuideState.Gate:
				HideKey(KeyType.Arrow);
				HideKey(KeyType.MARU);
				HideKey(KeyType.BATU);
				HideKey(KeyType.L);
				break;
			case GuideState.List:
				HideKey(KeyType.Arrow);
				HideKey(KeyType.MARU);
				HideKey(KeyType.BATU);
				HideKey(KeyType.L);
				HideKey(KeyType.RS);
				break;
			case GuideState.Detail:
				HideKey(KeyType.Arrow);
				HideKey(KeyType.MARU);
				HideKey(KeyType.BATU);
				HideKey(KeyType.L);
				break;
			}
			mCurrentGuideState = state;
			switch (mCurrentGuideState)
			{
			case GuideState.Gate:
				ShowKey(KeyType.Arrow);
				ShowKey(KeyType.MARU);
				ShowKey(KeyType.BATU);
				ShowKey(KeyType.L);
				HideKey(KeyType.RS);
				break;
			case GuideState.List:
				ShowKey(KeyType.Arrow);
				ShowKey(KeyType.MARU);
				ShowKey(KeyType.BATU);
				ShowKey(KeyType.L);
				ShowKey(KeyType.RS);
				break;
			case GuideState.Detail:
				ShowKey(KeyType.Arrow);
				ShowKey(KeyType.MARU);
				ShowKey(KeyType.BATU);
				ShowKey(KeyType.L);
				break;
			case GuideState.Quiet:
				Hide();
				break;
			}
		}

		private void Update()
		{
			key.Update();
			if (key == null)
			{
				if (isShow)
				{
					Hide();
				}
			}
			else
			{
				if (!key.IsRun)
				{
					return;
				}
				time += Time.deltaTime;
				if (key.IsAnyKey)
				{
					time = 0f;
					if (isShow)
					{
						isShow = false;
						Hide();
					}
				}
				else if (2f < time && !isShow && mCurrentGuideState != GuideState.Quiet)
				{
					isShow = true;
					Show();
				}
			}
		}

		public void Show()
		{
			if (model.GuideDisplay)
			{
				Util.MoveTo(base.gameObject, 0.4f, ShowPos, iTween.EaseType.easeInSine);
			}
		}

		public void Hide()
		{
			Util.MoveTo(base.gameObject, 0.4f, HidePos, iTween.EaseType.easeInSine);
		}

		private void OnDestroy()
		{
			SpriteStickR = null;
			SpriteButtonX = null;
			SpriteButtonL = null;
			SpriteButtonO = null;
			key = null;
			model = null;
			_uil = null;
		}
	}
}
