using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyCamera : MonoBehaviour
	{
		private const int WIDTH = 960;

		private const int HEIGHT = 544;

		private Vector2 mapLeftTop;

		private Vector2 mapRightBottom;

		private Vector2 cameraLeftTop;

		private Vector2 cameraRightBottom;

		public Camera myCamera;

		private float mapX1;

		private float mapX2;

		private float mapY1;

		private float mapY2;

		private float cameraX1;

		private float cameraX2;

		private float cameraY1;

		private float cameraY2;

		private Blur blur;

		public bool isMoving
		{
			get;
			private set;
		}

		private void Awake()
		{
			myCamera = base.gameObject.GetComponent<Camera>();
			blur = GetComponent<Blur>();
		}

		private void Start()
		{
			GameObject gameObject = GameObject.Find("Map_BG_Riku");
			Vector3 localPosition = gameObject.transform.localPosition;
			UITexture component = gameObject.GetComponent<UITexture>();
			float num = component.width;
			float num2 = component.height;
			base.transform.localPosition = StrategyTopTaskManager.Instance.TileManager.Tiles[1].getDefaultPosition();
			base.transform.localPositionZ(-500f);
			mapX1 = localPosition.x - num / 2f;
			mapX2 = localPosition.x + num / 2f;
			mapY1 = localPosition.y - num2 / 2f;
			mapY2 = localPosition.y + num2 / 2f;
			Vector3 position = myCamera.transform.position;
			cameraX1 = position.x - 480f * myCamera.orthographicSize;
			Vector3 position2 = myCamera.transform.position;
			cameraX2 = position2.x + 480f * myCamera.orthographicSize;
			Vector3 position3 = myCamera.transform.position;
			cameraY1 = position3.y - 272f * myCamera.orthographicSize;
			Vector3 position4 = myCamera.transform.position;
			cameraY2 = position4.y + 272f * myCamera.orthographicSize;
		}

		public Vector3 FixPosition(Vector3 vec, float nextSize)
		{
			if (nextSize == 0f)
			{
				nextSize = myCamera.orthographicSize;
			}
			if ((double)nextSize < 0.6)
			{
				nextSize = 0.6f;
			}
			float num = vec.x;
			float num2 = vec.y;
			Vector3 localPosition = myCamera.transform.localPosition;
			float z = localPosition.z;
			cameraX1 = num - 480f * nextSize;
			cameraX2 = num + 480f * nextSize;
			cameraY1 = num2 - 272f * nextSize;
			cameraY2 = num2 + 272f * nextSize;
			if (mapX1 > cameraX1)
			{
				num = mapX1 + 480f * nextSize;
			}
			if (mapX2 < cameraX2)
			{
				num = mapX2 - 480f * nextSize;
			}
			if (mapY1 > cameraY1)
			{
				num2 = mapY1 + 272f * nextSize;
			}
			if (mapY2 < cameraY2)
			{
				num2 = mapY2 - 272f * nextSize;
			}
			if (!(mapX1 > cameraX1) && !(mapX2 < cameraX2) && !(mapY1 > cameraY1) && !(mapY2 < cameraY2))
			{
				myCamera.orthographicSize = nextSize;
			}
			return new Vector3(num, num2, z);
		}

		public void FixSize(Vector3 targetTilePos, float nextSize)
		{
			if (!((double)nextSize < 0.6) && !(1.4 < (double)nextSize))
			{
				myCamera.transform.position = targetTilePos;
				Vector3 localPosition = myCamera.transform.localPosition;
				float num = localPosition.x;
				Vector3 localPosition2 = myCamera.transform.localPosition;
				float y = localPosition2.y;
				Vector3 localPosition3 = myCamera.transform.localPosition;
				float z = localPosition3.z;
				float num2 = (myCamera.orthographicSize - nextSize) * 960f;
				float num3 = (myCamera.orthographicSize - nextSize) * 544f;
				cameraX1 = num - 480f * nextSize;
				cameraX2 = num + 480f * nextSize;
				cameraY1 = y - 272f * nextSize;
				cameraY2 = y + 272f * nextSize;
				if (mapX1 > cameraX1)
				{
					num += 960f * (nextSize - myCamera.orthographicSize);
				}
				if (mapX2 < cameraX2)
				{
					float num4 = num + 960f * (nextSize - myCamera.orthographicSize);
				}
				Debug.Log(mapX1);
				Debug.Log(cameraX1);
				if (mapX1 > cameraX1)
				{
					Debug.Log("fixsize");
					myCamera.transform.AddLocalPositionX(mapX1 - cameraX1);
				}
				if (mapX2 < cameraX2)
				{
					myCamera.transform.AddLocalPositionX(mapX2 - cameraX2);
				}
				if (mapY1 > cameraY1)
				{
					myCamera.transform.AddLocalPositionY(mapY1 - cameraY1);
				}
				if (mapY2 < cameraY2)
				{
					myCamera.transform.AddLocalPositionY(mapY2 - cameraY2);
				}
				myCamera.orthographicSize = nextSize;
			}
		}

		public Coroutine MoveToTargetTile(int TargetAreaNo, bool immediate = false)
		{
			float time = (!immediate) ? 0.5f : 0f;
			return StartCoroutine(MoveToTargetTile(TargetAreaNo, time));
		}

		public IEnumerator MoveToTargetTile(int TargetAreaNo, float time)
		{
			isMoving = true;
			Vector3 TilePos = StrategyTopTaskManager.Instance.TileManager.Tiles[TargetAreaNo].getDefaultPosition();
			Vector3 nextPos = FixPosition(TilePos, 0f);
			StrategyTopTaskManager.Instance.GetInfoMng().changeAreaName(TargetAreaNo);
			yield return StartCoroutine(Util.WaitEndOfFrames(3));
			Util.MoveTo(base.gameObject, time, nextPos, iTween.EaseType.easeOutQuint);
			yield return new WaitForSeconds(time);
			yield return StartCoroutine(Util.WaitEndOfFrames(3));
			isMoving = false;
		}

		public void InitPositionTargetTile(int TargetAreaNo)
		{
			Vector3 defaultPosition = StrategyTopTaskManager.Instance.TileManager.Tiles[TargetAreaNo].getDefaultPosition();
			Vector3 localPosition = FixPosition(defaultPosition, 0f);
			base.transform.localPosition = localPosition;
		}

		public void setBlurEnable(bool enable)
		{
			blur.enabled = enable;
			if (enable)
			{
				blur.blurSize = 0f;
				iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 2.5f, "time", 0.5f, "onupdate", "OnBlurUpdate"));
			}
		}

		private void OnBlurUpdate(float value)
		{
			blur.blurSize = value;
		}
	}
}
