using local.models;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyShipBanners : MonoBehaviour
	{
		private Transform[] SideShips;

		private CommonShipBanner[] Banners;

		private UIGrid Grid;

		private UIPlayTween PlayTween;

		private Vector3[] DefaultPositions;

		[SerializeField]
		private UISprite DeckNoIcon;

		[Button("EnterSideShips", "EnterSideShips", new object[]
		{

		})]
		public int button1;

		[Button("ExitSideShips", "ExitSideShips", new object[]
		{

		})]
		public int button2;

		private readonly Vector3 movePosition = new Vector3(320f, 0f, 0f);

		private void Awake()
		{
			SideShips = new Transform[6];
			Banners = new CommonShipBanner[6];
			Grid = GetComponent<UIGrid>();
			PlayTween = GetComponent<UIPlayTween>();
			DefaultPositions = new Vector3[6];
			for (int i = 0; i < SideShips.Length; i++)
			{
				SideShips[i] = base.transform.FindChild("SideShipBanner" + (i + 1));
				Banners[i] = ((Component)SideShips[i].FindChild("CommonShipBanner2")).GetComponent<CommonShipBanner>();
				DefaultPositions[i] = SideShips[i].localPosition;
			}
		}

		private void Start()
		{
			setShipTweenPosition();
		}

		public void UpdateBanners()
		{
			DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
			for (int i = 0; i < Banners.Length; i++)
			{
				if (currentDeck.GetShip(i) == null)
				{
					SideShips[i].SetActive(isActive: false);
					continue;
				}
				SideShips[i].SetActive(isActive: true);
				Banners[i].isUseSmoke = false;
				Banners[i].SetShipData(currentDeck.GetShip(i));
			}
			DeckNoIcon.spriteName = "icon_deck" + currentDeck.Id;
		}

		public void EnterSideShips()
		{
			UpdateBanners();
			GetComponent<UIWidget>().alpha = 1f;
			setPosition(toScreenIn: true);
			PlayTween.resetOnPlay = true;
			PlayTween.Play(forward: true);
		}

		public void ExitSideShips()
		{
			setPosition(toScreenIn: false);
			PlayTween.resetOnPlay = true;
			PlayTween.Play(forward: false);
		}

		public void setPosition(bool toScreenIn)
		{
		}

		private void setShipTweenPosition()
		{
			for (int i = 0; i < SideShips.Length; i++)
			{
				TweenPosition tweenPosition = TweenPosition.Begin(SideShips[i].gameObject, 0.3f, DefaultPositions[i] + movePosition);
				tweenPosition.worldSpace = false;
				tweenPosition.delay = (float)i * 0.04f;
				tweenPosition.animationCurve = AppInformation.curveDic[AppInformation.CurveType.Live2DEnter];
			}
		}
	}
}
