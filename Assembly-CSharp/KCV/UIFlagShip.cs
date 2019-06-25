using KCV.Strategy;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PSVita;

namespace KCV
{
	public class UIFlagShip : BaseShipTexture
	{
		private const float SMOOTHING_VAL = 80f;

		private ManagerBase _clsBase;

		private int _clicked_count;

		private long _last_clicked_time;

		private bool _isPortTop;

		private bool _isPlayTimeSignalVoice;

		private Animation TouchAnimation;

		private List<string> AnimationList;

		private int BackRubCount;

		private TweenScale ApproachScale;

		private TweenPosition ApproachPosition;

		private bool isNear;

		private bool isPlayed;

		public bool isEnableBackTouch;

		private Action<bool, bool, bool> OnBackTouch;

		private float[] StartPosY;

		private float[] StartTime;

		[SerializeField]
		private UITexture HeadArea;

		public bool debugBackSlash;

		[Button("PlayBackTouch", "PlayBackTouch", new object[]
		{

		})]
		public int button11;

		[Button("PlayApproach", "PlayApproach", new object[]
		{

		})]
		public int button22;

		private ShipModel shipModel => (ShipModel)_clsIShipModel;

		private new void OnDestroy()
		{
			HeadArea = null;
			_clsBase = null;
			TouchAnimation = null;
			AnimationList.Clear();
			AnimationList = null;
			ApproachScale = null;
			ApproachPosition = null;
			StartPosY = null;
			StartTime = null;
			PSVitaInput.secondaryTouchIsScreenSpace = false;
		}

		private void Awake()
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			_clicked_count = 0;
			_last_clicked_time = DateTime.Now.Ticks;
			_isPortTop = false;
			_isPlayTimeSignalVoice = false;
			TouchAnimation = GetComponent<Animation>();
			AnimationList = new List<string>();
			foreach (AnimationState item in TouchAnimation)
			{
				AnimationState val = item;
				AnimationList.Add(val.name);
			}
			ApproachScale = ((Component)base.transform.GetChild(0)).GetComponent<TweenScale>();
			ApproachPosition = ((Component)base.transform.GetChild(0)).GetComponent<TweenPosition>();
			StartPosY = new float[4]
			{
				-1f,
				-1f,
				-1f,
				-1f
			};
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			StartTime = new float[4]
			{
				realtimeSinceStartup,
				realtimeSinceStartup,
				realtimeSinceStartup,
				realtimeSinceStartup
			};
		}

		public bool SetData(ShipModel ship, ManagerBase manager)
		{
			if (ship != null)
			{
				_clsIShipModel = ship;
				if (!shipModel.IsDamaged())
				{
				}
			}
			if (manager is PortManager)
			{
				_isPortTop = true;
			}
			else
			{
				_isPortTop = false;
			}
			return true;
		}

		private void Update()
		{
			if (SingletonMonoBehaviour<Live2DModel>.exist() && !SingletonMonoBehaviour<Live2DModel>.Instance.isLive2DModel)
			{
				_uiShipTex.transform.localScale = Smoothing();
			}
			if (isEnableBackTouch && !SingletonMonoBehaviour<Live2DModel>.Instance.IsStop)
			{
				InputBackTouch();
			}
		}

		private Vector3 Smoothing()
		{
			Vector3 one = Vector3.one;
			float x = one.x + Mathf.Sin(Time.time) / 80f;
			Vector3 one2 = Vector3.one;
			float y = one2.y + Mathf.Sin(Time.time) / 80f;
			Vector3 one3 = Vector3.one;
			return new Vector3(x, y, one3.z + Mathf.Sin(Time.time) / 80f);
		}

		private bool _chkTimeSignalVoice(ShipModel model, int hour)
		{
			if (App.SystemDateTime.Minute == 0 && App.SystemDateTime.Second == 0)
			{
				StartCoroutine(_playTimeSignalVoice(model, hour));
				return true;
			}
			return false;
		}

		private IEnumerator _playTimeSignalVoice(ShipModel model, int hour)
		{
			_isPlayTimeSignalVoice = true;
			PlayShipVoice(model.MstId, 30 + hour);
			yield return new WaitForSeconds(2f);
			_isPlayTimeSignalVoice = false;
		}

		public void PlayShipVoice(int shipID, int voiceNum)
		{
			SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(shipID, voiceNum), 0);
		}

		private void UIFlagShipEL()
		{
			ShipModel flagShipModel = SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel;
			if (flagShipModel != null)
			{
				ShipUtils.PlayShipVoice(flagShipModel, TouchedPartnerShip(shipModel));
			}
		}

		public int TouchedPartnerShip(ShipModel _partnerShip)
		{
			_clicked_count++;
			_last_clicked_time = DateTime.Now.Ticks;
			if (_clicked_count <= 4)
			{
				int num = new System.Random().Next(1, 11);
				if (_clicked_count == 1 && _partnerShip != null && _partnerShip.IsMarriage())
				{
					return 28;
				}
				if (num <= 6)
				{
					return 2;
				}
				if (num == 7 || num == 8 || num == 9)
				{
					return 3;
				}
				return 4;
			}
			return 4;
		}

		public uint GetNoActionMSec()
		{
			long num = DateTime.Now.Ticks - _last_clicked_time;
			return (uint)num / 10000u;
		}

		public void ResetClickedCount()
		{
			_clicked_count = 0;
			BackRubCount = 0;
		}

		public int getClickedCount()
		{
			return _clicked_count;
		}

		private void InputBackTouch()
		{
			if (StrategyShipCharacter.nowShipModel == null)
			{
				return;
			}
			PSVitaInput.secondaryTouchIsScreenSpace = true;
			bool flag = false;
			Touch[] touchesSecondary = PSVitaInput.touchesSecondary;
			for (int i = 0; i < touchesSecondary.Length; i++)
			{
				Touch touch = touchesSecondary[i];
				if (touch.fingerId == -1)
				{
					continue;
				}
				if (touch.phase == TouchPhase.Began)
				{
					float[] startPosY = StartPosY;
					int fingerId = touch.fingerId;
					Vector2 position = touch.position;
					startPosY[fingerId] = position.y;
					StartTime[touch.fingerId] = Time.realtimeSinceStartup;
				}
				else if (touch.phase == TouchPhase.Ended && StartPosY[touch.fingerId] != -1f)
				{
					float num = StartPosY[touch.fingerId];
					Vector2 position2 = touch.position;
					if (-420f > num - position2.y)
					{
						flag = true;
					}
				}
				if (0.2f < Time.realtimeSinceStartup - StartTime[touch.fingerId] && StartPosY[touch.fingerId] != -1f)
				{
					StartPosY[touch.fingerId] = -1f;
				}
			}
			if (!isPlayed && (flag || debugBackSlash))
			{
				PlayBackTouch();
				isPlayed = true;
				debugBackSlash = false;
			}
			if (PSVitaInput.touchesSecondary.Length == 0)
			{
				isPlayed = false;
			}
		}

		private void PlayBackTouch()
		{
			if (!TouchAnimation.isPlaying)
			{
				int num = (BackRubCount != 0) ? 4 : 3;
				BackRubCount++;
				ShipUtils.PlayShipVoice(StrategyShipCharacter.nowShipModel, num);
				PlayJumpAnimation(1);
				int lov = StrategyShipCharacter.nowShipModel.Lov;
				bool arg = false;
				bool arg2 = false;
				StrategyShipCharacter.nowShipModel.LovAction(1, num);
				if (StrategyShipCharacter.nowShipModel.Lov > lov)
				{
					arg = true;
				}
				if (StrategyShipCharacter.nowShipModel.Lov < lov)
				{
					arg2 = true;
				}
				if (OnBackTouch != null)
				{
					OnBackTouch(arg, arg2, arg3: true);
				}
			}
		}

		private void PlayJumpAnimation(int type)
		{
			TouchAnimation.Play(AnimationList[type]);
		}

		private void PlayApproach()
		{
			ApproachScale.Play(!isNear);
			ApproachPosition.Play(!isNear);
			isNear = !isNear;
		}

		public void SetOnBackTouchCallBack(Action<bool, bool, bool> act)
		{
			OnBackTouch = act;
		}
	}
}
