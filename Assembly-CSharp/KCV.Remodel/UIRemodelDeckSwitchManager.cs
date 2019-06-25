using local.models;
using System.Linq;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelDeckSwitchManager : CommonDeckSwitchManager, UIRemodelView
	{
		private const float ANIMATION_DURATION = 0.2f;

		private Vector3 showPos = new Vector3(-240f, -257f);

		private Vector3 hidePos = new Vector3(-240f, -300f);

		private void Awake()
		{
			base.transform.localPosition = hidePos;
			Show(animation: false);
		}

		public void Init(DeckModel[] decks, CommonDeckSwitchHandler handler, KeyControl keyController, bool otherEnabled)
		{
			decks = (from x in decks
				where !x.HasBling()
				select x).ToArray();
			base.Init(UserInterfaceRemodelManager.instance.mRemodelManager, decks, handler, keyController, otherEnabled, SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck, 25);
		}

		public void Show()
		{
			Show(animation: true);
		}

		public void Show(bool animation)
		{
			base.keyControlEnable = true;
			base.gameObject.SetActive(true);
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.gameObject, showPos, 0.2f, delegate
				{
				});
			}
			else
			{
				base.transform.localPosition = showPos;
			}
		}

		public void Hide()
		{
			Hide(animation: true);
		}

		public void Hide(bool animation)
		{
			base.keyControlEnable = false;
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.gameObject, hidePos, 0.2f, delegate
				{
					base.gameObject.SetActive(false);
				});
				return;
			}
			base.transform.localPosition = hidePos;
			base.gameObject.SetActive(false);
		}
	}
}
