using System.Collections;
using UnityEngine;

namespace KCV.Scene.Practice.Deck
{
	public class UIDeckPracticeTrainingGround : MonoBehaviour
	{
		public enum SeaType
		{
			A,
			B,
			C
		}

		public enum Direction
		{
			LEFT,
			RIGHT
		}

		[SerializeField]
		private UITexture mTexture_Sea;

		[SerializeField]
		private UITexture mTexture_SeaOverlay;

		[SerializeField]
		private UITexture mTexture_Mountain;

		[SerializeField]
		private UITexture mTexture_Sky;

		private SeaType mSeaType;

		private Direction mSeaDirection;

		private SeaType mSeaOverlayType;

		private Direction mSeaOverlayDirection;

		private float mSeaMoveSpeed;

		private float mSeaOverlayMoveSpeed;

		private IEnumerator mSeaAnimationCoroutine;

		private IEnumerator mSeaOverlayAnimationCoroutine;

		public void InitializeSea(Direction direction, SeaType seaType, float moveSpeed)
		{
			mSeaType = seaType;
			mSeaDirection = direction;
			mSeaMoveSpeed = moveSpeed;
		}

		public void InitializeSeaOverlay(Direction direction, SeaType seaType, float moveSpeed)
		{
			mSeaType = seaType;
			mSeaOverlayDirection = direction;
			mSeaOverlayMoveSpeed = moveSpeed;
		}

		public void PlaySea()
		{
			if (mTexture_Sea != null)
			{
				if (mSeaAnimationCoroutine != null)
				{
					StopCoroutine(mSeaAnimationCoroutine);
					mSeaAnimationCoroutine = null;
				}
				mSeaAnimationCoroutine = SeaAnimationCoroutine(mSeaDirection, mSeaMoveSpeed);
				StartCoroutine(mSeaAnimationCoroutine);
			}
			if (mTexture_SeaOverlay != null)
			{
				if (mSeaOverlayAnimationCoroutine != null)
				{
					StopCoroutine(mSeaOverlayAnimationCoroutine);
					mSeaOverlayAnimationCoroutine = null;
				}
				mSeaOverlayAnimationCoroutine = SeaOverlayAnimationCoroutine(mSeaOverlayDirection, mSeaOverlayMoveSpeed);
				StartCoroutine(mSeaOverlayAnimationCoroutine);
			}
		}

		private IEnumerator SeaAnimationCoroutine(Direction direction, float waveSpeed)
		{
			Rect seaRect = default(Rect);
			Rect mountainRect = default(Rect);
			float moveXPerSecond;
			switch (direction)
			{
			case Direction.LEFT:
				moveXPerSecond = waveSpeed;
				break;
			case Direction.RIGHT:
				moveXPerSecond = 0f - waveSpeed;
				break;
			default:
				moveXPerSecond = 0f;
				break;
			}
			while (true)
			{
				seaRect.Set(mTexture_Sea.uvRect.x + moveXPerSecond * Time.deltaTime, mTexture_Sea.uvRect.y, mTexture_Sea.uvRect.width, mTexture_Sea.uvRect.height);
				mountainRect.Set(mTexture_Mountain.uvRect.x + moveXPerSecond * Time.deltaTime, mTexture_Mountain.uvRect.y, mTexture_Mountain.uvRect.width, mTexture_Mountain.uvRect.height);
				mTexture_Sea.uvRect = seaRect;
				mTexture_Mountain.uvRect = mountainRect;
				yield return null;
			}
		}

		private IEnumerator SeaOverlayAnimationCoroutine(Direction direction, float waveSpeed)
		{
			Rect seaRect = default(Rect);
			float moveXPerSecond;
			switch (direction)
			{
			case Direction.LEFT:
				moveXPerSecond = waveSpeed;
				break;
			case Direction.RIGHT:
				moveXPerSecond = 0f - waveSpeed;
				break;
			default:
				moveXPerSecond = 0f;
				break;
			}
			while (true)
			{
				seaRect.Set(mTexture_SeaOverlay.uvRect.x + moveXPerSecond * Time.deltaTime, mTexture_SeaOverlay.uvRect.y, mTexture_SeaOverlay.uvRect.width, mTexture_SeaOverlay.uvRect.height);
				mTexture_SeaOverlay.uvRect = seaRect;
				yield return null;
			}
		}

		private void OnDisable()
		{
			if (mSeaAnimationCoroutine != null)
			{
				StopCoroutine(mSeaAnimationCoroutine);
			}
		}
	}
}
