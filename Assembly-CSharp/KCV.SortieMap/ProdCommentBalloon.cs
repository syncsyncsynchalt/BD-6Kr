using Common.Enum;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UISprite))]
	public class ProdCommentBalloon : AbsBalloon
	{
		public static ProdCommentBalloon Instantiate(ProdCommentBalloon prefab, Transform parent, UISortieShip.Direction iDirection, MapCommentKind iKind)
		{
			ProdCommentBalloon prodCommentBalloon = Object.Instantiate(prefab);
			prodCommentBalloon.transform.parent = parent;
			prodCommentBalloon.transform.localScaleZero();
			prodCommentBalloon.transform.localPositionZero();
			prodCommentBalloon.Init(iDirection, iKind);
			return prodCommentBalloon;
		}

		public static ProdCommentBalloon Instantiate(ProdCommentBalloon prefab, Transform parent, UISortieShip.Direction iDirection)
		{
			ProdCommentBalloon prodCommentBalloon = Object.Instantiate(prefab);
			prodCommentBalloon.transform.parent = parent;
			prodCommentBalloon.transform.localScaleZero();
			prodCommentBalloon.transform.localPositionZero();
			prodCommentBalloon.Init(iDirection);
			return prodCommentBalloon;
		}

		private bool Init(UISortieShip.Direction iDirection, MapCommentKind iKind)
		{
			SetBalloonComment(iKind);
			SetBalloonPos(iDirection);
			return true;
		}

		private bool Init(UISortieShip.Direction iDirection)
		{
			base.sprite.spriteName = "fuki_yojou";
			base.sprite.MakePixelPerfect();
			SetBalloonPos(iDirection);
			base.transform.localScaleZero();
			return true;
		}

		private void SetBalloonComment(MapCommentKind iKind)
		{
			string spriteName = string.Empty;
			switch (iKind)
			{
			case MapCommentKind.Enemy:
				spriteName = "fuki_ship1";
				break;
			case MapCommentKind.Atack:
				spriteName = "fuki_ship2";
				break;
			}
			base.sprite.spriteName = spriteName;
			base.sprite.MakePixelPerfect();
			base.transform.localScaleZero();
		}

		protected override void SetBalloonPos(UISortieShip.Direction iDirection)
		{
			switch (iDirection)
			{
			case UISortieShip.Direction.Left:
				base.transform.localPosition = new Vector3(-71f, 17f, 0f);
				break;
			case UISortieShip.Direction.Right:
				base.transform.localPosition = new Vector3(71f, 17f, 0f);
				break;
			}
		}
	}
}
