using KCV.Strategy;
using System;
using UnityEngine;

namespace KCV.Scene.Port
{
	public class UIPortCameraControlMode : MonoBehaviour
	{
		[SerializeField]
		private Camera MenuCamera;

		[SerializeField]
		private Camera InteriorCamera;

		[SerializeField]
		private StrategyShipCharacter character;

		[SerializeField]
		private BoxCollider2D characterSlideCollider;

		private KeyControl key;

		private Action mOnFinishedModeListener;

		[Button("OnExitModeDebug", "OnExitMode", new object[]
		{

		})]
		public int Button1;

		private int WIDTH = 960;

		private int HEIGHT = 544;

		private float mapX1;

		private float mapX2;

		private float mapY1;

		private float mapY2;

		private float cameraX1;

		private float cameraX2;

		private float cameraY1;

		private float cameraY2;

		private void Start()
		{
			characterSlideCollider.enabled = false;
		}

		public void SetKeyController(KeyControl keyControl)
		{
			key = keyControl;
		}

		public void Init()
		{
			characterSlideCollider.enabled = true;
			Vector3 zero = Vector3.zero;
			mapX1 = zero.x - (float)(WIDTH / 2);
			mapX2 = zero.x + (float)(WIDTH / 2);
			mapY1 = zero.y - (float)(HEIGHT / 2);
			mapY2 = zero.y + (float)(HEIGHT / 2);
			Vector3 position = MenuCamera.transform.position;
			cameraX1 = position.x - (float)(WIDTH / 2) * MenuCamera.orthographicSize;
			Vector3 position2 = MenuCamera.transform.position;
			cameraX2 = position2.x + (float)(WIDTH / 2) * MenuCamera.orthographicSize;
			Vector3 position3 = MenuCamera.transform.position;
			cameraY1 = position3.y - (float)(HEIGHT / 2) * MenuCamera.orthographicSize;
			Vector3 position4 = MenuCamera.transform.position;
			cameraY2 = position4.y + (float)(HEIGHT / 2) * MenuCamera.orthographicSize;
		}

		private void Update()
		{
			if (key == null)
			{
				return;
			}
			float axisRaw = Input.GetAxisRaw("Left Stick Horizontal");
			float axisRaw2 = Input.GetAxisRaw("Left Stick Vertical");
			if (key.keyState[16].press || key.keyState[23].press || key.keyState[17].press)
			{
				MenuCamera.orthographicSize -= 0.3f * Time.deltaTime;
				InteriorCamera.orthographicSize -= 0.3f * Time.deltaTime;
			}
			else if (key.keyState[20].press || key.keyState[21].press || key.keyState[19].press)
			{
				MenuCamera.orthographicSize += 0.3f * Time.deltaTime;
				InteriorCamera.orthographicSize += 0.3f * Time.deltaTime;
			}
			else if (key.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			else
			{
				if (key.IsSankakuDown())
				{
					key.ClearKeyAll();
					key.firstUpdate = true;
					ExitMode();
					return;
				}
				if (axisRaw == 0f && axisRaw2 == 0f)
				{
					if (key.keyState[10].press)
					{
						character.transform.AddLocalPositionX(250f * Time.deltaTime);
						Vector3 localPosition = character.transform.localPosition;
						if (localPosition.x > 400f)
						{
							character.transform.localPositionX(400f);
						}
					}
					else if (key.keyState[14].press)
					{
						character.transform.AddLocalPositionX(-250f * Time.deltaTime);
						Vector3 localPosition2 = character.transform.localPosition;
						if (localPosition2.x < -400f)
						{
							character.transform.localPositionX(-400f);
						}
					}
				}
			}
			if (key.keyState[8].press)
			{
				float num = -0.1f * Time.deltaTime;
				Vector3 localScale = character.transform.localScale;
				if (localScale.x + num > 1.1f)
				{
					character.transform.AddLocalScale(num, num, num);
					character.transform.AddLocalPositionY((float)character.render.height * (0f - num) / 4f);
				}
			}
			else if (key.keyState[12].press)
			{
				float num2 = 0.1f * Time.deltaTime;
				float num3 = 0.5f * (float)character.shipModel.Lov / 1000f;
				Vector3 localScale2 = character.transform.localScale;
				if (localScale2.x + num2 < 1.1f + num3)
				{
					character.transform.AddLocalScale(num2, num2, num2);
					character.transform.AddLocalPositionY((float)character.render.height * (0f - num2) / 4f);
				}
			}
			MenuCamera.transform.AddPosX(axisRaw * Time.deltaTime);
			MenuCamera.transform.AddPosY((0f - axisRaw2) * Time.deltaTime);
			InteriorCamera.transform.AddPosX(axisRaw * Time.deltaTime);
			InteriorCamera.transform.AddPosY((0f - axisRaw2) * Time.deltaTime);
			FixSize(MenuCamera);
			FixSize(InteriorCamera);
			FixPosition(MenuCamera);
			FixPosition(InteriorCamera);
		}

		private Vector3 FixPosition(Camera myCamera)
		{
			float orthographicSize = myCamera.orthographicSize;
			Vector3 localPosition = myCamera.transform.localPosition;
			float num = localPosition.x;
			float num2 = localPosition.y;
			Vector3 localPosition2 = myCamera.transform.localPosition;
			float z = localPosition2.z;
			cameraX1 = num - (float)(WIDTH / 2) * orthographicSize;
			cameraX2 = num + (float)(WIDTH / 2) * orthographicSize;
			cameraY1 = num2 - (float)(HEIGHT / 2) * orthographicSize;
			cameraY2 = num2 + (float)(HEIGHT / 2) * orthographicSize;
			if (mapX1 > cameraX1)
			{
				num = mapX1 + (float)(WIDTH / 2) * orthographicSize;
			}
			if (mapX2 < cameraX2)
			{
				num = mapX2 - (float)(WIDTH / 2) * orthographicSize;
			}
			if (mapY1 > cameraY1)
			{
				num2 = mapY1 + (float)(HEIGHT / 2) * orthographicSize;
			}
			if (mapY2 < cameraY2)
			{
				num2 = mapY2 - (float)(HEIGHT / 2) * orthographicSize;
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
			if (num > 1f)
			{
				num = 1f;
			}
			Vector3 localPosition = myCamera.transform.localPosition;
			float num2 = localPosition.x;
			Vector3 localPosition2 = myCamera.transform.localPosition;
			float y = localPosition2.y;
			Vector3 localPosition3 = myCamera.transform.localPosition;
			float z = localPosition3.z;
			float num3 = (myCamera.orthographicSize - num) * (float)WIDTH;
			float num4 = (myCamera.orthographicSize - num) * (float)HEIGHT;
			cameraX1 = num2 - (float)(WIDTH / 2) * num;
			cameraX2 = num2 + (float)(WIDTH / 2) * num;
			cameraY1 = y - (float)(HEIGHT / 2) * num;
			cameraY2 = y + (float)(HEIGHT / 2) * num;
			if (mapX1 > cameraX1)
			{
				num2 += (float)WIDTH * (num - myCamera.orthographicSize);
			}
			if (mapX2 < cameraX2)
			{
				float num5 = num2 + (float)WIDTH * (num - myCamera.orthographicSize);
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

		public void ExitMode()
		{
			key = null;
			characterSlideCollider.enabled = false;
			Vector3 enterPosition = character.getEnterPosition();
			float x = enterPosition.x;
			Vector3 localPosition = character.transform.localPosition;
			if (x == localPosition.x)
			{
				bool flag = MenuCamera.gameObject.transform.localPosition == Vector3.zero && MenuCamera.orthographicSize == 1f;
				bool flag2 = InteriorCamera.gameObject.transform.localPosition == Vector3.zero && InteriorCamera.orthographicSize == 1f;
				if (flag && flag2)
				{
					OnFinishedExitMode();
					return;
				}
			}
			StrategyShipCharacter strategyShipCharacter = character;
			Vector3 enterPosition2 = character.getEnterPosition();
			strategyShipCharacter.moveCharacterX(enterPosition2.x, 0.5f, OnFinishedExitMode);
			Util.MoveTo(MenuCamera.gameObject, 0.5f, Vector3.zero, iTween.EaseType.easeOutQuad);
			Util.MoveTo(InteriorCamera.gameObject, 0.5f, Vector3.zero, iTween.EaseType.easeOutQuad);
			TweenScale.Begin(character.gameObject, 0.5f, new Vector3(1.1f, 1.1f, 1.1f));
			iTween.ValueTo(base.gameObject, iTween.Hash("onupdate", "CameraResetUpdate", "time", 0.5f, "from", MenuCamera.orthographicSize, "to", 1));
		}

		private void CameraResetUpdate(float value)
		{
			MenuCamera.orthographicSize = value;
			InteriorCamera.orthographicSize = value;
		}

		private void OnDestroy()
		{
			MenuCamera = null;
			InteriorCamera = null;
			character = null;
			key = null;
			mOnFinishedModeListener = null;
		}

		private void OnExitModeDebug()
		{
			ExitMode();
		}
	}
}
