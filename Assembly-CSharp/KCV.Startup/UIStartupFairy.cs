using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(BoxCollider))]
	[RequireComponent(typeof(UISprite))]
	public class UIStartupFairy : MonoBehaviour
	{
		public enum FairyType
		{
			None,
			Tails,
			Ahoge,
			Braid
		}

		public enum FairyState
		{
			Idle,
			Move,
			Jump
		}

		[Serializable]
		private struct Params
		{
			public float animationTime;
		}

		[Serializable]
		private struct StateParam
		{
			public int spriteNum;

			public int drawIntervalFrame;

			public int[] convertSpriteNumber;
		}

		[Serializable]
		private struct FairyAnimParameter
		{
			public FairyType fairyType;

			public Vector2 baseSpriteSize;

			public StateParam idleStateParam;

			public StateParam moveStateParam;

			public StateParam jumpStateParam;

			public Vector3 startPos;

			public Vector3 endPos;

			public float tripTime;

			public int jumpFrame;

			public float rotTime;

			public StateParam GetStateParam(FairyState iState)
			{
				switch (iState)
				{
				case FairyState.Idle:
					return idleStateParam;
				case FairyState.Move:
					return moveStateParam;
				case FairyState.Jump:
					return jumpStateParam;
				default:
					return default(StateParam);
				}
			}
		}

		private const string FAIRY_SPRITE_NAME = "mini{0}_{1}_{2:D2}";

		[SerializeField]
		private UITexture _uiBalloon;

		[Header("[Animation Parameter]")]
		[SerializeField]
		private Params _strParams;

		[Header("[Fairy Animation Parameters]")]
		[SerializeField]
		private List<FairyAnimParameter> _listFairyAnimationParameter;

		private FairyType _iType;

		private FairyState _iState;

		private FairyState _iStatePrev;

		private UISprite _uiFairySprite;

		private BoxCollider _colBox;

		private List<IDisposable> _listObserverStream;

		private int _nSpriteIndex;

		private AudioSource _asSource;

		private UISprite fairySprite => this.GetComponentThis(ref _uiFairySprite);

		private new BoxCollider collider => this.GetComponentThis(ref _colBox);

		public void Startup()
		{
			_nSpriteIndex = 0;
			_listObserverStream = new List<IDisposable>(Enum.GetValues(typeof(FairyState)).Length);
			for (int i = 0; i < _listObserverStream.Capacity; i++)
			{
				_listObserverStream.Add(null);
			}
			fairySprite.alpha = 0f;
			_uiBalloon.alpha = 0f;
			((Collider)collider).enabled = false;
		}

		private void Awake()
		{
			Startup();
		}

		private void ReqFairyType(FairyType iType)
		{
			if (_iType != iType)
			{
				_iType = iType;
				_iState = (_iStatePrev = FairyState.Idle);
				_nSpriteIndex = 0;
				SetSprite(_iType, _iState, _nSpriteIndex);
				_listObserverStream.ForEach(delegate(IDisposable x)
				{
					x?.Dispose();
				});
			}
		}

		private void SetSprite(FairyType iType, FairyState iState, int nIndex)
		{
			this.fairySprite.spriteName = ((iType != 0) ? $"mini{(int)iType}_{iState.ToString()}_{nIndex:D2}" : string.Empty);
			UISprite fairySprite = this.fairySprite;
			FairyAnimParameter fairyAnimParameter = _listFairyAnimationParameter[(int)iType];
			fairySprite.localSize = fairyAnimParameter.baseSpriteSize;
		}

		private void ReqFairyState(FairyState iState)
		{
			if (_iType == FairyType.None)
			{
				return;
			}
			StateParam stateParam = _listFairyAnimationParameter[(int)_iType].GetStateParam(iState);
			if (stateParam.spriteNum <= 0)
			{
				return;
			}
			_nSpriteIndex = 0;
			_listObserverStream.ForEach(delegate(IDisposable x)
			{
				x?.Dispose();
			});
			List<IDisposable> listObserverStream = _listObserverStream;
			FairyState index2 = iState;
			StateParam stateParam2 = _listFairyAnimationParameter[(int)_iType].GetStateParam(iState);
			listObserverStream[(int)index2] = (from x in Observable.IntervalFrame(stateParam2.drawIntervalFrame)
				select _nSpriteIndex++).Subscribe(delegate(int index)
			{
				StateParam stateParam3 = _listFairyAnimationParameter[(int)_iType].GetStateParam(iState);
				int num = index % stateParam3.spriteNum;
				StateParam stateParam4 = _listFairyAnimationParameter[(int)_iType].GetStateParam(iState);
				int nIndex = stateParam4.convertSpriteNumber[num];
				SetSprite(_iType, iState, nIndex);
			}).AddTo(base.gameObject);
			_iStatePrev = _iState;
			_iState = iState;
			switch (iState)
			{
			case FairyState.Idle:
				break;
			case FairyState.Jump:
			{
				FairyAnimParameter fairyAnimParameter3 = _listFairyAnimationParameter[(int)_iType];
				Observable.TimerFrame(fairyAnimParameter3.jumpFrame).Subscribe(delegate
				{
					ReqFairyState(_iStatePrev);
				});
				break;
			}
			case FairyState.Move:
				if (_iStatePrev != FairyState.Jump)
				{
					FairyAnimParameter fairyAnimParameter = _listFairyAnimationParameter[(int)_iType];
					float tripTime = fairyAnimParameter.tripTime;
					FairyAnimParameter fairyAnimParameter2 = _listFairyAnimationParameter[(int)_iType];
					float rotTime = fairyAnimParameter2.rotTime;
					float delayTime = tripTime * 2f;
					base.transform.LTCancel();
					base.transform.LTDelayedCall(delayTime, (Action)delegate
					{
						base.transform.LTRotateLocal(Vector3.zero, rotTime).setEase(LeanTweenType.easeOutSine);
						Transform transform = base.transform;
						FairyAnimParameter fairyAnimParameter4 = _listFairyAnimationParameter[(int)_iType];
						transform.LTMoveLocalX(fairyAnimParameter4.endPos.x, tripTime).setEase(LeanTweenType.linear);
						base.transform.LTRotateLocal(Vector3.up * 180f, rotTime).setDelay(tripTime).setEase(LeanTweenType.easeOutSine);
						Transform transform2 = base.transform;
						FairyAnimParameter fairyAnimParameter5 = _listFairyAnimationParameter[(int)_iType];
						transform2.LTMoveLocalX(fairyAnimParameter5.startPos.x, tripTime).setDelay(tripTime).setEase(LeanTweenType.linear);
					}).setOnCompleteOnStart(isOn: true).setLoopClamp();
				}
				break;
			}
		}

		public void Show(FairyType iType, FairyState iState, Action onFinished)
		{
			ReqFairyType(iType);
			Transform transform = base.transform;
			FairyAnimParameter fairyAnimParameter = _listFairyAnimationParameter[(int)iType];
			transform.localPosition = fairyAnimParameter.startPos;
			PreparaAnimation(isFoward: true, delegate
			{
				((Collider)collider).enabled = true;
				ReqFairyState(iState);
				Dlg.Call(ref onFinished);
			});
		}

		public void Hide(Action onFinished)
		{
			PreparaAnimation(isFoward: false, delegate
			{
				_uiBalloon.alpha = 0f;
				Dlg.Call(ref onFinished);
			});
		}

		private void PreparaAnimation(bool isFoward, Action onFinished)
		{
			float to = (!isFoward) ? 0f : 1f;
			base.transform.LTValue(fairySprite.alpha, to, _strParams.animationTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				fairySprite.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					Dlg.Call(ref onFinished);
				});
		}

		public void ShowBaloon(int nPage, System.Tuple<Vector3, Vector3> vBalloon, Action onFinished)
		{
			_uiBalloon.mainTexture = Resources.Load<Texture2D>($"Textures/Startup/PictureStoryShow/info{nPage + 1}_fuki");
			_uiBalloon.localSize = vBalloon.Item1;
			_uiBalloon.transform.localPosition = vBalloon.Item2;
			_uiBalloon.alpha = 0f;
			_uiBalloon.transform.LTValue(_uiBalloon.alpha, 1f, 0.15f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiBalloon.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					Dlg.Call(ref onFinished);
				});
		}

		public AudioSource PlayVoice(int nPage, Action onFinished)
		{
			switch (nPage)
			{
			case 0:
				return _asSource = Utils.PlayDescriptionVoice(25, onFinished);
			case 1:
				return _asSource = Utils.PlayDescriptionVoice(28, onFinished);
			case 2:
				return _asSource = Utils.PlayDescriptionVoice(31, onFinished);
			default:
				return null;
			}
		}

		public void StopVoice()
		{
			ShipUtils.StopShipVoice(_asSource, isCallOnFinished: false, 0.25f);
		}

		public void ImmediateIdle()
		{
			((Collider)collider).enabled = false;
			base.transform.LTCancel();
			Transform transform = base.transform;
			FairyAnimParameter fairyAnimParameter = _listFairyAnimationParameter[(int)_iType];
			transform.LTMoveLocal(fairyAnimParameter.startPos, 0.15f).setEase(LeanTweenType.easeOutSine);
			base.transform.LTRotateLocal(Vector3.zero, 0.15f).setEase(LeanTweenType.easeOutSine);
			ReqFairyState(FairyState.Idle);
		}

		private void OnPress(bool isPress)
		{
			if (isPress && _iType != FairyType.Ahoge && _iType != FairyType.Braid && _iState != FairyState.Jump)
			{
				ReqFairyState(FairyState.Jump);
			}
		}
	}
}
