using KCV;
using System;
using UnityEngine;

public class UIAlbumShipViewerCameraController : MonoBehaviour
{
	[SerializeField]
	private Camera MenuCamera;

	private KeyControl mKeyController;

	private int mWidth = 960;

	private int mHeight = 544;

	private float mMaximumOrthographicSize;

	private float mapX1;

	private float mapX2;

	private float mapY1;

	private float mapY2;

	private float cameraX1;

	private float cameraX2;

	private float cameraY1;

	private float cameraY2;

	private Action mOnFinishedModeListener;

	public void SetKeyController(KeyControl keyControl)
	{
		mKeyController = keyControl;
	}

	public void Initialize(int width, int height, float maximumOrthographicSize)
	{
		width = 2048;
		height = 2048;
		mWidth = width;
		mHeight = height;
		mMaximumOrthographicSize = maximumOrthographicSize;
		Vector3 zero = Vector3.zero;
		mapX1 = zero.x - (float)(mWidth / 2);
		mapX2 = zero.x + (float)(mWidth / 2);
		mapY1 = zero.y - (float)(mHeight / 2);
		mapY2 = zero.y + (float)(mHeight / 2);
		Vector3 position = MenuCamera.transform.position;
		cameraX1 = position.x - (float)(mWidth / 2) * MenuCamera.orthographicSize;
		Vector3 position2 = MenuCamera.transform.position;
		cameraX2 = position2.x + (float)(mWidth / 2) * MenuCamera.orthographicSize;
		Vector3 position3 = MenuCamera.transform.position;
		cameraY1 = position3.y - (float)(mHeight / 2) * MenuCamera.orthographicSize;
		Vector3 position4 = MenuCamera.transform.position;
		cameraY2 = position4.y + (float)(mHeight / 2) * MenuCamera.orthographicSize;
	}

	private void Update()
	{
		if (mKeyController != null)
		{
			if (mKeyController.keyState[16].press || mKeyController.keyState[23].press || mKeyController.keyState[17].press)
			{
				MenuCamera.orthographicSize -= 0.3f * Time.deltaTime;
			}
			else if (mKeyController.keyState[20].press || mKeyController.keyState[21].press || mKeyController.keyState[19].press)
			{
				MenuCamera.orthographicSize += 0.3f * Time.deltaTime;
			}
			float axisRaw = Input.GetAxisRaw("Left Stick Horizontal");
			float axisRaw2 = Input.GetAxisRaw("Left Stick Vertical");
			MenuCamera.transform.AddPosX(axisRaw * Time.deltaTime);
			MenuCamera.transform.AddPosY((0f - axisRaw2) * Time.deltaTime);
			FixSize(MenuCamera);
			FixPosition(MenuCamera);
		}
	}

	private Vector3 FixPosition(Camera myCamera)
	{
		float orthographicSize = myCamera.orthographicSize;
		Vector3 localPosition = myCamera.transform.localPosition;
		float num = localPosition.x;
		float num2 = localPosition.y;
		Vector3 localPosition2 = myCamera.transform.localPosition;
		float z = localPosition2.z;
		cameraX1 = num - (float)(mWidth / 2) * orthographicSize;
		cameraX2 = num + (float)(mWidth / 2) * orthographicSize;
		cameraY1 = num2 - (float)(mHeight / 2) * orthographicSize;
		cameraY2 = num2 + (float)(mHeight / 2) * orthographicSize;
		if (mapX1 > cameraX1)
		{
			num = mapX1 + (float)(mWidth / 2) * orthographicSize;
		}
		if (mapX2 < cameraX2)
		{
			num = mapX2 - (float)(mWidth / 2) * orthographicSize;
		}
		if (mapY1 > cameraY1)
		{
			num2 = mapY1 + (float)(mHeight / 2) * orthographicSize;
		}
		if (mapY2 < cameraY2)
		{
			num2 = mapY2 - (float)(mHeight / 2) * orthographicSize;
		}
		return new Vector3(num, num2, z);
	}

	private void FixSize(Camera myCamera)
	{
		float num = myCamera.orthographicSize;
		if (num == 0f)
		{
			num = myCamera.orthographicSize;
		}
		if ((double)num < 0.5)
		{
			num = 0.5f;
		}
		if (num > mMaximumOrthographicSize)
		{
			num = mMaximumOrthographicSize;
		}
		Vector3 localPosition = myCamera.transform.localPosition;
		float num2 = localPosition.x;
		Vector3 localPosition2 = myCamera.transform.localPosition;
		float y = localPosition2.y;
		Vector3 localPosition3 = myCamera.transform.localPosition;
		float z = localPosition3.z;
		float num3 = (myCamera.orthographicSize - num) * (float)mWidth;
		float num4 = (myCamera.orthographicSize - num) * (float)mHeight;
		cameraX1 = num2 - (float)(mWidth / 2) * num;
		cameraX2 = num2 + (float)(mWidth / 2) * num;
		cameraY1 = y - (float)(mHeight / 2) * num;
		cameraY2 = y + (float)(mHeight / 2) * num;
		if (mapX1 > cameraX1)
		{
			num2 += (float)mWidth * (num - myCamera.orthographicSize);
		}
		if (mapX2 < cameraX2)
		{
			float num5 = num2 + (float)mWidth * (num - myCamera.orthographicSize);
		}
		if (mapX1 > cameraX1)
		{
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
		myCamera.orthographicSize = num;
	}

	public void SetOnFinishedModeListener(Action onFinishedModeListener)
	{
		mOnFinishedModeListener = onFinishedModeListener;
	}

	private void OnFinishedExitMode()
	{
		if (mOnFinishedModeListener != null)
		{
			mOnFinishedModeListener();
		}
	}

	public void QuitState()
	{
	}

	private void OnDestroy()
	{
		MenuCamera = null;
		mKeyController = null;
	}
}
