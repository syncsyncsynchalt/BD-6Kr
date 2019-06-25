using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV
{
	public class KeyControl
	{
		public struct IndexMap
		{
			public int up;

			public int rightUp;

			public int right;

			public int rightDown;

			public int down;

			public int leftDown;

			public int left;

			public int leftUp;
		}

		public enum Mode
		{
			NOMAL,
			DOUBL_INDEX,
			INDEX_MAP
		}

		public enum KeyName
		{
			BATU,
			MARU,
			SHIKAKU,
			SANKAKU,
			L,
			R,
			SELECT,
			START,
			UP,
			UP_RIGHT,
			RIGHT,
			DOWN_RIGHT,
			DOWN,
			DOWN_LEFT,
			LEFT,
			UP_LEFT,
			RS_UP,
			RS_UP_RIGHT,
			RS_RIGHT,
			RS_DOWN_RIGHT,
			RS_DOWN,
			RS_DOWN_LEFT,
			RS_LEFT,
			RS_UP_LEFT,
			KEY_NUM
		}

		public class KeyState
		{
			public bool down;

			public bool press;

			public bool hold;

			public float holdTime;

			public bool up;

			public bool wClick;
		}

		public const int NONE_INDEX = -99999;

		public int maxIndex;

		public int maxIndex2;

		public int minIndex;

		public int minIndex2;

		private int index;

		private int index2;

		public int prevIndex;

		public int prevIndexChangeValue;

		public float upKeyChangeValue;

		public float rightKeyChangeValue;

		public float downKeyChangeValue;

		public float leftKeyChangeValue;

		private bool isChangeIndex;

		private bool isUpdateIndex;

		private bool isAnyKey;

		private bool isDirectKeyHold;

		private KeyName HoldLockKey;

		private bool isStickNeutral;

		private bool _isRun;

		public bool firstUpdate;

		private bool isCleared;

		private List<IndexMap> indexMapList;

		private int[] indexMapBuf;

		private int[,] orignalIndexMap;

		private Mode controllerMode;

		public bool isLoopIndex;

		public bool isStopIndex;

		private bool isLeftStickStandAlone;

		private bool isRightStickStandAlone;

		private GameObject origine;

		private GameObject[] searchObjs;

		private List<GameObject> targetList;

		private float range;

		private float holdJudgeTime;

		private float intervalTime;

		private float keyInputInterval;

		private float keyInputIntervalButton;

		private List<KeyName> AutoDownKeys;

		public Dictionary<int, KeyState> keyState;

		private KeyCode[] keyCodeArray;

		public int Index
		{
			get
			{
				return index;
			}
			set
			{
				prevIndex = index;
				index = value;
				prevIndexChangeValue = index - prevIndex;
				index = ((!isLoopIndex) ? ((int)Util.RangeValue(index, minIndex, maxIndex)) : ((int)Util.LoopValue(index, minIndex, maxIndex)));
				if (index != prevIndex)
				{
					isChangeIndex = true;
				}
				isUpdateIndex = true;
				isAnyKey = true;
			}
		}

		public int Index2
		{
			get
			{
				return index2;
			}
			set
			{
				int num = index2;
				index2 = value;
				if (index2 > maxIndex2)
				{
					index2 = maxIndex2;
					if (controllerMode == Mode.INDEX_MAP)
					{
						index2 = num;
					}
				}
				if (index2 < minIndex2 && index2 != -99999)
				{
					index2 = minIndex2;
					if (controllerMode == Mode.INDEX_MAP)
					{
						index2 = num;
					}
				}
				if (index2 != num)
				{
					isChangeIndex = true;
				}
				isUpdateIndex = true;
			}
		}

		public bool IsChangeIndex => isChangeIndex;

		public bool IsUpdateIndex => isUpdateIndex;

		public bool IsAnyKey => isAnyKey;

		public bool IsDirectKeyHold => isDirectKeyHold;

		public bool IsRun
		{
			get
			{
				return _isRun;
			}
			set
			{
				if (!value)
				{
					ClearKeyAll();
				}
				_isRun = value;
			}
		}

		public float HoldJudgeTime
		{
			set
			{
				holdJudgeTime = value;
			}
		}

		public float KeyInputInterval
		{
			get
			{
				return keyInputInterval;
			}
			set
			{
				keyInputInterval = value;
			}
		}

		public float KeyInputIntervalButton
		{
			set
			{
				keyInputIntervalButton = value;
			}
		}

		public KeyControl(int min = 0, int max = 0, float holdJudgeTime = 0.4f, float keyInputInterval = 0.1f)
		{
			keyState = new Dictionary<int, KeyState>(24);
			for (int i = 0; i < 24; i++)
			{
				keyState[i] = new KeyState();
				initKeyState(keyState[i]);
			}
			controllerMode = Mode.NOMAL;
			index = 0;
			minIndex = min;
			maxIndex = max;
			isChangeIndex = false;
			this.holdJudgeTime = holdJudgeTime;
			this.keyInputInterval = keyInputInterval;
			keyInputIntervalButton = 0.2f;
			intervalTime = 0f;
			upKeyChangeValue = -1f;
			rightKeyChangeValue = 1f;
			downKeyChangeValue = 1f;
			leftKeyChangeValue = -1f;
			firstUpdate = true;
			isLoopIndex = true;
			_isRun = true;
			keyCodeArray = new KeyCode[24];
			keyCodeArray[0] = KeyCode.Joystick1Button0;
			keyCodeArray[1] = KeyCode.Joystick1Button1;
			keyCodeArray[2] = KeyCode.Joystick1Button2;
			keyCodeArray[3] = KeyCode.Joystick1Button3;
			keyCodeArray[4] = KeyCode.Joystick1Button4;
			keyCodeArray[5] = KeyCode.Joystick1Button5;
			keyCodeArray[6] = KeyCode.Joystick1Button6;
			keyCodeArray[7] = KeyCode.Joystick1Button7;
			keyCodeArray[8] = KeyCode.Joystick1Button8;
			keyCodeArray[10] = KeyCode.Joystick1Button9;
			keyCodeArray[12] = KeyCode.Joystick1Button10;
			keyCodeArray[14] = KeyCode.Joystick1Button11;
			isLeftStickStandAlone = false;
			isRightStickStandAlone = false;
			AutoDownKeys = new List<KeyName>();
			AutoDownKeys.Add(KeyName.UP);
			AutoDownKeys.Add(KeyName.RIGHT);
			AutoDownKeys.Add(KeyName.DOWN);
			AutoDownKeys.Add(KeyName.LEFT);
			if (KeyControlManager.exist())
			{
				KeyControlManager.Instance.KeyController = this;
			}
		}

		public void SilentChangeIndex(int index)
		{
			this.index = index;
		}

		public void initKeyState(KeyState keyState)
		{
			keyState.down = false;
			keyState.press = false;
			keyState.hold = false;
			keyState.holdTime = 0f;
			keyState.up = false;
			keyState.wClick = false;
		}

		public void Update()
		{
			if (!_isRun || isOnlyControllerExist())
			{
				return;
			}
			isAnyKey = false;
			checkKeyState();
			isChangeIndex = false;
			isUpdateIndex = false;
			if (!isStopIndex)
			{
				updateIndex();
			}
			checkAllFirstUpdate();
			if (firstUpdate)
			{
				for (int i = 0; i < 24; i++)
				{
					keyState[i].down = false;
				}
				firstUpdate = false;
			}
		}

		private bool isOnlyControllerExist()
		{
			if (App.OnlyController != null && App.OnlyController != this)
			{
				if (!isCleared)
				{
					ClearKeyAll();
				}
				return true;
			}
			return false;
		}

		private void checkAllFirstUpdate()
		{
			if (App.isFirstUpdate)
			{
				firstUpdate = true;
				if (SingletonMonoBehaviour<AppInformation>.exist())
				{
					SingletonMonoBehaviour<AppInformation>.Instance.FirstUpdateEnd();
				}
				else
				{
					App.isFirstUpdate = false;
				}
			}
		}

		public void LeftStickUpdate()
		{
			if (!_isRun)
			{
				return;
			}
			if (App.OnlyController != null && App.OnlyController != this)
			{
				if (!isCleared)
				{
					ClearKeyAll();
				}
				return;
			}
			checkStickState();
			updateIndex();
			for (int i = 0; i < 24; i++)
			{
				if (checkStickState() == i)
				{
					setKeyState(i, press: true);
				}
				else
				{
					setKeyState(i, press: false);
				}
			}
		}

		public void RightStickUpdate()
		{
			if (!_isRun)
			{
				return;
			}
			if (App.OnlyController != null && App.OnlyController != this)
			{
				if (!isCleared)
				{
					ClearKeyAll();
				}
				return;
			}
			RightStickToDigital();
			updateIndex();
			for (int i = 0; i < 24; i++)
			{
				if (RightStickToDigital() == i)
				{
					setKeyState(i, press: true);
				}
				else
				{
					setKeyState(i, press: false);
				}
			}
		}

		private void checkKeyState()
		{
			int num = checkStickState();
			int num2 = checkRightStickState();
			for (int i = 0; i < 24; i++)
			{
				if (Input.GetKey(keyCodeArray[i]) || (!isLeftStickStandAlone && num == i) || (!isRightStickStandAlone && num2 == i))
				{
					setKeyState(i, press: true);
				}
				else
				{
					setKeyState(i, press: false);
				}
			}
			OnlyOneDirectKey();
			isCleared = false;
		}

		private void OnlyOneDirectKey()
		{
			bool flag = false;
			for (int i = 8; i <= 15; i++)
			{
				if (flag)
				{
					keyState[i].down = false;
				}
				if (keyState[i].down)
				{
					flag = true;
				}
			}
		}

		private void setKeyState(int keyName, bool press)
		{
			KeyState keyState = this.keyState[keyName];
			if (press)
			{
				isAnyKey = true;
				if (!keyState.press)
				{
					keyState.down = true;
					if (LogDrawer.exist())
					{
						SingletonMonoBehaviour<LogDrawer>.Instance.addDebugText(keyName.ToString());
					}
				}
				else
				{
					keyState.down = false;
				}
				keyState.holdTime += Time.deltaTime;
				if (keyState.holdTime > holdJudgeTime)
				{
					keyState.hold = true;
				}
				keyState.up = false;
				keyState.press = true;
				if (AutoDownKeys.IndexOf((KeyName)keyName) != -1 && keyState.hold && (HoldLockKey == KeyName.KEY_NUM || keyName == (int)HoldLockKey))
				{
					HoldLockKey = (KeyName)keyName;
					float num;
					if (keyName != 8 && keyName != 10 && keyName != 12 && keyName != 14)
					{
						num = keyInputIntervalButton;
						isDirectKeyHold = true;
					}
					else
					{
						num = keyInputInterval;
					}
					if (intervalTime > num)
					{
						keyState.down = true;
						intervalTime = 0f;
					}
					else
					{
						intervalTime += Time.deltaTime;
					}
				}
			}
			else
			{
				keyState.down = false;
				keyState.holdTime = 0f;
				keyState.hold = false;
				if (keyState.press)
				{
					keyState.up = true;
				}
				else
				{
					keyState.up = false;
				}
				keyState.press = false;
				if (HoldLockKey == (KeyName)keyName)
				{
					HoldLockKey = KeyName.KEY_NUM;
				}
			}
		}

		private int checkStickState()
		{
			float axisRaw = Input.GetAxisRaw("Left Stick Horizontal");
			float axisRaw2 = Input.GetAxisRaw("Left Stick Vertical");
			if (0f < axisRaw && axisRaw2 < 0f)
			{
				return 9;
			}
			if (0f < axisRaw && axisRaw2 > 0f)
			{
				return 11;
			}
			if (0f > axisRaw && axisRaw2 < 0f)
			{
				return 15;
			}
			if (0f > axisRaw && axisRaw2 > 0f)
			{
				return 13;
			}
			if (axisRaw > 0f)
			{
				return 10;
			}
			if (axisRaw < 0f)
			{
				return 14;
			}
			if (axisRaw2 > 0f)
			{
				return 12;
			}
			if (axisRaw2 < 0f)
			{
				return 8;
			}
			return 24;
		}

		private int checkStickStateHEX(bool isUseLeftStick)
		{
			float num = (!isUseLeftStick) ? Input.GetAxisRaw("Right Stick Horizontal") : Input.GetAxisRaw("Left Stick Horizontal");
			float num2 = (!isUseLeftStick) ? Input.GetAxisRaw("Right Stick Vertical") : Input.GetAxisRaw("Left Stick Vertical");
			if (0.8f < num && num2 < 0f)
			{
				return 9;
			}
			if (0.8f < num && num2 > 0f)
			{
				return 11;
			}
			if (-0.8f > num && num2 < 0f)
			{
				return 15;
			}
			if (-0.8f > num && num2 > 0f)
			{
				return 13;
			}
			if (num2 > 0f)
			{
				return 12;
			}
			if (num2 < 0f)
			{
				return 8;
			}
			return 24;
		}

		private int checkRightStickState()
		{
			float axisRaw = Input.GetAxisRaw("Right Stick Horizontal");
			float axisRaw2 = Input.GetAxisRaw("Right Stick Vertical");
			if (0f < axisRaw && axisRaw2 < 0f)
			{
				return 17;
			}
			if (0f < axisRaw && axisRaw2 > 0f)
			{
				return 19;
			}
			if (0f > axisRaw && axisRaw2 < 0f)
			{
				return 23;
			}
			if (0f > axisRaw && axisRaw2 > 0f)
			{
				return 21;
			}
			if (axisRaw > 0f)
			{
				return 18;
			}
			if (axisRaw < 0f)
			{
				return 22;
			}
			if (axisRaw2 > 0f)
			{
				return 20;
			}
			if (axisRaw2 < 0f)
			{
				return 16;
			}
			return 24;
		}

		private int RightStickToDigital()
		{
			float axisRaw = Input.GetAxisRaw("Right Stick Horizontal");
			float axisRaw2 = Input.GetAxisRaw("Right Stick Vertical");
			if (0f < axisRaw && axisRaw2 < 0f)
			{
				return 9;
			}
			if (0f < axisRaw && axisRaw2 > 0f)
			{
				return 11;
			}
			if (0f > axisRaw && axisRaw2 < 0f)
			{
				return 15;
			}
			if (0f > axisRaw && axisRaw2 > 0f)
			{
				return 13;
			}
			if (axisRaw > 0f)
			{
				return 10;
			}
			if (axisRaw < 0f)
			{
				return 14;
			}
			if (axisRaw2 > 0f)
			{
				return 12;
			}
			if (axisRaw2 < 0f)
			{
				return 8;
			}
			return 24;
		}

		private void updateIndex()
		{
			isChangeIndex = false;
			isUpdateIndex = false;
			int index3 = Index;
			int index4 = Index2;
			if (controllerMode == Mode.NOMAL)
			{
				if (keyState[8].down)
				{
					Index += (int)upKeyChangeValue;
				}
				if (keyState[10].down)
				{
					Index += (int)rightKeyChangeValue;
				}
				if (keyState[12].down)
				{
					Index += (int)downKeyChangeValue;
				}
				if (keyState[14].down)
				{
					Index += (int)leftKeyChangeValue;
				}
			}
			else if (controllerMode == Mode.INDEX_MAP)
			{
				if (keyState[8].down)
				{
					Index = getIndexMapValue(index, KeyName.UP);
				}
				if (keyState[10].down)
				{
					Index = getIndexMapValue(index, KeyName.RIGHT);
				}
				if (keyState[12].down)
				{
					Index = getIndexMapValue(index, KeyName.DOWN);
				}
				if (keyState[14].down)
				{
					Index = getIndexMapValue(index, KeyName.LEFT);
				}
				if (keyState[9].down)
				{
					Index = getIndexMapValue(index, KeyName.UP_RIGHT);
				}
				if (keyState[15].down)
				{
					Index = getIndexMapValue(index, KeyName.UP_LEFT);
				}
				if (keyState[11].down)
				{
					Index = getIndexMapValue(index, KeyName.DOWN_RIGHT);
				}
				if (keyState[13].down)
				{
					Index = getIndexMapValue(index, KeyName.DOWN_LEFT);
				}
			}
			else if (controllerMode == Mode.DOUBL_INDEX)
			{
				if (keyState[8].down)
				{
					Index += (int)upKeyChangeValue;
				}
				if (keyState[10].down)
				{
					Index2 += (int)rightKeyChangeValue;
				}
				if (keyState[12].down)
				{
					Index += (int)downKeyChangeValue;
				}
				if (keyState[14].down)
				{
					Index2 += (int)leftKeyChangeValue;
				}
			}
			if (!isChangeIndex)
			{
			}
		}

		public void setChangeValue(float up, float right, float down, float left)
		{
			upKeyChangeValue = up;
			rightKeyChangeValue = right;
			downKeyChangeValue = down;
			leftKeyChangeValue = left;
		}

		public void setMinMaxIndex(int min = 0, int max = 0)
		{
			minIndex = min;
			maxIndex = max;
		}

		public void setMinIndex(int min = 0)
		{
			setMinMaxIndex(min, maxIndex);
		}

		public void setMaxIndex(int max = 0)
		{
			setMinMaxIndex(minIndex, max);
		}

		public void useDoubleIndex(int min, int max)
		{
			controllerMode = Mode.DOUBL_INDEX;
			index2 = min;
			minIndex2 = min;
			maxIndex2 = max;
		}

		private void addIndexMap(int[,] IndexMapArray)
		{
			for (int i = 0; i < IndexMapArray.GetLength(0); i++)
			{
				IndexMap item = default(IndexMap);
				item.up = IndexMapArray[i, 0];
				item.rightUp = IndexMapArray[i, 1];
				item.right = IndexMapArray[i, 2];
				item.rightDown = IndexMapArray[i, 3];
				item.down = IndexMapArray[i, 4];
				item.leftDown = IndexMapArray[i, 5];
				item.left = IndexMapArray[i, 6];
				item.leftUp = IndexMapArray[i, 7];
				indexMapList.Add(item);
			}
		}

		public void setUseIndexMap(int[,] indexMapArray)
		{
			controllerMode = Mode.INDEX_MAP;
			indexMapList = new List<IndexMap>();
			addIndexMap(indexMapArray);
			orignalIndexMap = new int[indexMapArray.GetLength(0), indexMapArray.GetLength(1)];
			Array.Copy(indexMapArray, 0, orignalIndexMap, 0, indexMapArray.Length);
			indexMapBuf = new int[8];
		}

		public void setEnableIndex(int[] indexArray)
		{
			int[,] array = new int[orignalIndexMap.GetLength(0), orignalIndexMap.GetLength(1)];
			Array.Copy(orignalIndexMap, 0, array, 0, orignalIndexMap.Length);
			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					if (array[i, j] == 0)
					{
						continue;
					}
					bool flag = false;
					for (int k = 0; k < indexArray.Length; k++)
					{
						if (array[i, j] == indexArray[k])
						{
							flag = true;
						}
					}
					if (!flag)
					{
						array[i, j] = 0;
					}
				}
			}
			controllerMode = Mode.INDEX_MAP;
			indexMapList = new List<IndexMap>();
			addIndexMap(array);
		}

		private int getIndexMapValue(int nowIndex, KeyName keyName)
		{
			int num = (int)(keyName - 8);
			int num2 = num + 1;
			int num3 = num - 1;
			if (num2 > 7)
			{
				num2 = 0;
			}
			if (num3 < 0)
			{
				num3 = 7;
			}
			int[] array = indexMapBuf;
			IndexMap indexMap = indexMapList[nowIndex];
			array[0] = indexMap.up;
			int[] array2 = indexMapBuf;
			IndexMap indexMap2 = indexMapList[nowIndex];
			array2[1] = indexMap2.rightUp;
			int[] array3 = indexMapBuf;
			IndexMap indexMap3 = indexMapList[nowIndex];
			array3[2] = indexMap3.right;
			int[] array4 = indexMapBuf;
			IndexMap indexMap4 = indexMapList[nowIndex];
			array4[3] = indexMap4.rightDown;
			int[] array5 = indexMapBuf;
			IndexMap indexMap5 = indexMapList[nowIndex];
			array5[4] = indexMap5.down;
			int[] array6 = indexMapBuf;
			IndexMap indexMap6 = indexMapList[nowIndex];
			array6[5] = indexMap6.leftDown;
			int[] array7 = indexMapBuf;
			IndexMap indexMap7 = indexMapList[nowIndex];
			array7[6] = indexMap7.left;
			int[] array8 = indexMapBuf;
			IndexMap indexMap8 = indexMapList[nowIndex];
			array8[7] = indexMap8.leftUp;
			if (indexMapBuf[num] != 0)
			{
				return indexMapBuf[num];
			}
			if (indexMapBuf[num2] != 0)
			{
				return indexMapBuf[num2];
			}
			if (indexMapBuf[num3] != 0)
			{
				return indexMapBuf[num3];
			}
			return nowIndex;
		}

		public void ClearKeyAll()
		{
			for (int i = 0; i < 24; i++)
			{
				initKeyState(keyState[i]);
			}
			isChangeIndex = false;
			isUpdateIndex = false;
			isCleared = true;
		}

		public void reset(int min = 0, int max = 0, float holdJudgeTime = 0.4f, float keyInputInterval = 0.1f)
		{
			controllerMode = Mode.NOMAL;
			index = 0;
			minIndex = min;
			maxIndex = max;
			isChangeIndex = false;
			this.holdJudgeTime = holdJudgeTime;
			this.keyInputInterval = keyInputInterval;
			intervalTime = 0f;
			upKeyChangeValue = -1f;
			rightKeyChangeValue = 1f;
			downKeyChangeValue = 1f;
			leftKeyChangeValue = -1f;
			firstUpdate = true;
			isLoopIndex = true;
			_isRun = true;
			isLeftStickStandAlone = false;
			isRightStickStandAlone = false;
		}

		public void selectButton(GameObject[] Buttons)
		{
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (Index == i)
				{
					TweenColor.Begin(Buttons[i], 0.2f, Util.CursolColor);
				}
				else
				{
					TweenColor.Begin(Buttons[i], 0.2f, Color.white);
				}
			}
		}

		public void setStickStandAlone(bool LeftStick, bool RightStick)
		{
			isLeftStickStandAlone = LeftStick;
			isRightStickStandAlone = RightStick;
		}

		public void unselectButton(UIButton[] Buttons)
		{
			for (int i = 0; i < Buttons.Length; i++)
			{
				TweenColor.Begin(Buttons[i].gameObject, 0.2f, Color.white);
			}
		}

		public void addHoldAutoDownKey(KeyName keyname)
		{
			AutoDownKeys.Add(keyname);
		}

		public void InitSeachFocus(GameObject[] objs)
		{
			searchObjs = objs;
			targetList = new List<GameObject>();
			range = 100f;
		}

		public GameObject SeachFocusObject(GameObject origine)
		{
			Vector3 originalPos = origine.transform.position;
			if (Input.GetAxisRaw("Left Stick Vertical") == 1f)
			{
				GameObject[] array = searchObjs;
				foreach (GameObject gameObject in array)
				{
					if (!(gameObject != origine))
					{
						continue;
					}
					Vector3 position = gameObject.transform.position;
					if (!(position.y > originalPos.y + 0.01f))
					{
						continue;
					}
					Vector3 position2 = gameObject.transform.position;
					if (position2.x >= originalPos.x - range)
					{
						Vector3 position3 = gameObject.transform.position;
						if (position3.x <= originalPos.x + range)
						{
							targetList.Add(gameObject);
						}
					}
				}
			}
			else if (Input.GetAxisRaw("Left Stick Vertical") == -1f)
			{
				GameObject[] array2 = searchObjs;
				foreach (GameObject gameObject2 in array2)
				{
					if (!(gameObject2 != origine))
					{
						continue;
					}
					Vector3 position4 = gameObject2.transform.position;
					if (!(position4.y + 0.01f < originalPos.y))
					{
						continue;
					}
					Vector3 position5 = gameObject2.transform.position;
					if (position5.x >= originalPos.x - range)
					{
						Vector3 position6 = gameObject2.transform.position;
						if (position6.x <= originalPos.x + range)
						{
							targetList.Add(gameObject2);
						}
					}
				}
			}
			else if (Input.GetAxisRaw("Left Stick Horizontal") == -1f)
			{
				GameObject[] array3 = searchObjs;
				foreach (GameObject gameObject3 in array3)
				{
					if (!(gameObject3 != origine))
					{
						continue;
					}
					Vector3 position7 = gameObject3.transform.position;
					if (!(position7.x + 0.01f < originalPos.x))
					{
						continue;
					}
					Vector3 position8 = gameObject3.transform.position;
					if (position8.y >= originalPos.y - range)
					{
						Vector3 position9 = gameObject3.transform.position;
						if (position9.y <= originalPos.y + range)
						{
							targetList.Add(gameObject3);
						}
					}
				}
			}
			else if (Input.GetAxisRaw("Left Stick Horizontal") == 1f)
			{
				GameObject[] array4 = searchObjs;
				foreach (GameObject gameObject4 in array4)
				{
					if (!(gameObject4 != origine))
					{
						continue;
					}
					Vector3 position10 = gameObject4.transform.position;
					if (!(position10.x > originalPos.x + 0.01f))
					{
						continue;
					}
					Vector3 position11 = gameObject4.transform.position;
					if (position11.y >= originalPos.y - range)
					{
						Vector3 position12 = gameObject4.transform.position;
						if (position12.y <= originalPos.y + range)
						{
							targetList.Add(gameObject4);
						}
					}
				}
			}
			if (targetList.Count <= 0)
			{
				return origine;
			}
			GameObject gameObject5 = (from obj in targetList
				orderby (obj.transform.position - originalPos).magnitude
				select obj).First().gameObject;
			targetList.Clear();
			return gameObject5;
		}

		public bool IsMaruDown()
		{
			return keyState[1].down;
		}

		public bool IsBatuDown()
		{
			return keyState[0].down;
		}

		public bool IsSankakuDown()
		{
			return keyState[3].down;
		}

		public bool IsShikakuDown()
		{
			return keyState[2].down;
		}

		public bool IsUpDown()
		{
			return keyState[8].down;
		}

		public bool IsDownDown()
		{
			return keyState[12].down;
		}

		public bool IsLeftDown()
		{
			return keyState[14].down;
		}

		public bool IsRightDown()
		{
			return keyState[10].down;
		}

		public bool IsLDown()
		{
			return keyState[4].down;
		}

		public bool IsRDown()
		{
			return keyState[5].down;
		}

		public bool GetDown(int nKey)
		{
			return keyState[nKey].down;
		}

		public bool GetDown(KeyName iName)
		{
			return GetDown((int)iName);
		}

		public bool GetPress(int nKey)
		{
			return keyState[nKey].press;
		}

		public bool GetPress(KeyName iName)
		{
			return GetPress((int)iName);
		}

		public bool GetHold(int nKey)
		{
			return keyState[nKey].hold;
		}

		public bool GetHold(KeyName iName)
		{
			return GetHold((int)iName);
		}

		public float GetHoldTime(int nKey)
		{
			return keyState[nKey].holdTime;
		}

		public float GetHoldTime(KeyName iName)
		{
			return GetHoldTime((int)iName);
		}

		public bool GetUp(int nKey)
		{
			return keyState[nKey].up;
		}

		public bool GetUp(KeyName iName)
		{
			return GetUp((int)iName);
		}

		public bool GetWClick(int nKey)
		{
			return keyState[nKey].wClick;
		}

		public bool GetWClick(KeyName iName)
		{
			return GetWClick((int)iName);
		}

		internal bool IsRSLeftDown()
		{
			return keyState[22].down;
		}

		internal bool IsRSRightDown()
		{
			return keyState[18].down;
		}
	}
}
