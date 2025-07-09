using Common.Enum;
using KCV.SortieBattle;
using LT.Tweening;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIWidget))]
	public class UICompassGirl : MonoBehaviour
	{
		[SerializeField]
		private UITexture _uiCompassGirl;

		private UIWidget _uiWidget;

		private CompassType _iCompassType;

		private Action<string> _actOnCompassGirlMessage;

		private Action<UICompass.Power> _actOnStopRollCompass;

		private UIWidget widget => this.GetComponentThis(ref _uiWidget);

		private void OnDestroy()
		{
			Mem.Del(ref _uiCompassGirl);
			Mem.Del(ref _uiWidget);
			Mem.Del(ref _iCompassType);
			Mem.Del(ref _actOnCompassGirlMessage);
			Mem.Del(ref _actOnStopRollCompass);
		}

		public bool Init(Action<string> onCompassGirlMessage, Action<UICompass.Power> onStopRollCompass, CompassType iRashinType)
		{
			_actOnCompassGirlMessage = onCompassGirlMessage;
			_actOnStopRollCompass = onStopRollCompass;
			widget.alpha = 0f;
			base.transform.LTCancel();
			_uiCompassGirl.transform.LTCancel();
			_iCompassType = iRashinType;
			switch (_iCompassType)
			{
			case CompassType.Stupid:
				InitStupid();
				break;
			case CompassType.Normal:
				InitNormal();
				break;
			case CompassType.Super:
				InitSuper();
				break;
			case CompassType.Wizard:
				InitWizard();
				break;
			}
			return true;
		}

		private bool InitStupid()
		{
			_uiCompassGirl.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/CompassGirls/CompassGirl_01-1");
			_uiCompassGirl.SetDimensions(112, 146);
			_uiCompassGirl.alpha = 0f;
			return true;
		}

		private bool InitNormal()
		{
			_uiCompassGirl.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/CompassGirls/CompassGirl_02-1");
			_uiCompassGirl.SetDimensions(115, 164);
			_uiCompassGirl.alpha = 0f;
			return true;
		}

		private bool InitSuper()
		{
			_uiCompassGirl.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/CompassGirls/CompassGirl_03-2");
			_uiCompassGirl.SetDimensions(115, 164);
			_uiCompassGirl.alpha = 0f;
			return true;
		}

		private bool InitWizard()
		{
			_uiCompassGirl.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/CompassGirls/CompassGirl_04-1");
			_uiCompassGirl.SetDimensions(300, 190);
			_uiCompassGirl.alpha = 0f;
			return true;
		}

		public void Play()
		{
            widget.alpha = 1f;
            UniRx.IObservable<Unit> source = Observable.FromCoroutine(InDisplay);
            UniRx.IObservable<Unit> source2 = Observable.FromCoroutine(WaitRoll);
			Observable.FromCoroutine(StartRoll);
			source.SelectMany(source2.SelectMany(StartRoll)).Subscribe();
		}

		public void Hide()
		{
			OutDisplay();
		}

		private IEnumerator InDisplay()
		{
			switch (_iCompassType)
			{
			case CompassType.Normal:
			case CompassType.Super:
				yield return Observable.FromCoroutine(InDisplayFadeInFromTop).StartAsCoroutine();
				break;
			case CompassType.Stupid:
				yield return Observable.FromCoroutine(InDisplayFadeInFromBottom).StartAsCoroutine();
				break;
			case CompassType.Wizard:
				yield return Observable.FromCoroutine(InDisplayFadeInFromTopRight).StartAsCoroutine();
				break;
			}
			yield return null;
		}

		private IEnumerator InDisplayFadeInFromTop()
		{
			bool isPlayAnimation = true;
			_uiCompassGirl.transform.localPosition = new Vector3(0f, 30f);
			float animationTime = 0.5f;
			LeanTween.delayedCall(animationTime, (Action)delegate
			{
				isPlayAnimation = false;
			}).setOnStart(delegate
			{
                throw new NotImplementedException("なにこれ");
                // this._uiCompassGirl.transform.LTValue(this._uiCompassGirl.alpha, 1f, animationTime).setEase(base._003CtweenType_003E__2).setOnUpdate(delegate(float x)
				// {
				// 	this._uiCompassGirl.alpha = x;
				// });
				// this._uiCompassGirl.transform.LTMoveLocal(Vector3.zero, animationTime).setEase(base._003CtweenType_003E__2);
			});
			while (isPlayAnimation)
			{
				yield return null;
			}
		}

		private IEnumerator InDisplayFadeInFromBottom()
		{
			bool isPlayAnimation = true;
			_uiCompassGirl.transform.localPosition = new Vector3(0f, -30f);
			float animationTime = 0.5f;
			base.transform.LTDelayedCall(animationTime, (Action)delegate
			{
				isPlayAnimation = false;
			}).setOnStart(delegate
			{
                throw new NotImplementedException("なにこれ");
                // this._uiCompassGirl.transform.LTValue(this._uiCompassGirl.alpha, 1f, animationTime).setEase(base._003CtweenType_003E__2).setOnUpdate(delegate(float x)
				// {
				// 	this._uiCompassGirl.alpha = x;
				// });

				this._uiCompassGirl.transform.LTMoveLocal(Vector3.zero, 1f);
			});
			while (isPlayAnimation)
			{
				yield return null;
			}
		}

		private IEnumerator InDisplayFadeInFromTopRight()
		{
			bool isPlayAnimation = true;
			_uiCompassGirl.transform.localPosition = new Vector2(30f, 30f);
			float animationTime = 0.5f;
			LeanTween.delayedCall(animationTime, (Action)delegate
			{
				isPlayAnimation = false;
			}).setOnStart(delegate
			{
                throw new NotImplementedException("なにこれ");
                // this._uiCompassGirl.transform.LTValue(this._uiCompassGirl.alpha, 1f, animationTime).setEase(base._003CtweenType_003E__2).setOnUpdate(delegate(float x)
				// {
				// 	this._uiCompassGirl.alpha = x;
				// });

				this._uiCompassGirl.transform.LTMoveLocal(Vector3.zero, animationTime);
			});
			while (isPlayAnimation)
			{
				yield return null;
			}
		}

		private IEnumerator WaitRoll()
		{
			switch (_iCompassType)
			{
			case CompassType.Super:
				OnCompassGirlMessage("はやくはやくー！");
				break;
			case CompassType.Normal:
				OnCompassGirlMessage("よーし、らしんばんまわすよー！");
				break;
			case CompassType.Stupid:
				OnCompassGirlMessage("えー？らしんばん、まわすのー？");
				break;
			case CompassType.Wizard:
				_uiCompassGirl.transform.LTMoveLocalY(30f, 1f).setEase(LeanTweenType.easeInQuad).setLoopPingPong();
				OnCompassGirlMessage("らしんばんをまわしてね！");
				break;
			}
			KeyControl input = SortieBattleTaskManager.GetKeyControl();
			while (!input.GetDown(KeyControl.KeyName.MARU) && !Input.GetMouseButton(0) && Input.touchCount == 0)
			{
				yield return null;
			}
			yield return null;
		}

		private IEnumerator StartRoll()
		{
			switch (_iCompassType)
			{
			case CompassType.Super:
				yield return StartCoroutine(StartRollPony());
				yield return new WaitForSeconds(1.3f);
				break;
			case CompassType.Normal:
				yield return StartCoroutine(StartRollBob());
				break;
			case CompassType.Stupid:
				yield return StartCoroutine(StartRollDal());
				break;
			case CompassType.Wizard:
				yield return StartCoroutine(StartRollMajo());
				break;
			}
			yield return null;
		}

		private IEnumerator StartRollMajo()
		{
			bool isPlayAnimation2 = true;
			_uiCompassGirl.transform.LTCancel();
			Transform transform = _uiCompassGirl.transform;
			Vector3 localPosition = _uiCompassGirl.transform.localPosition;
			transform.LTMoveLocalY(localPosition.y + 15f, 1f).setEase(LeanTweenType.easeOutElastic).setOnComplete((Action)delegate
			{
				isPlayAnimation2 = false;
			});
			OnCompassGirlMessage("えいっ");
			OnStopRollCompass(UICompass.Power.Low);
			while (isPlayAnimation2)
			{
				yield return null;
			}
			isPlayAnimation2 = true;
			OnCompassGirlMessage("それっ");
			Transform transform2 = _uiCompassGirl.transform;
			Vector3 localPosition2 = _uiCompassGirl.transform.localPosition;
			transform2.LTMoveLocalY(localPosition2.y + 15f, 1f).setEase(LeanTweenType.easeOutElastic).setOnComplete((Action)delegate
			{
				isPlayAnimation2 = false;
			});
			while (isPlayAnimation2)
			{
				yield return null;
			}
			yield return null;
		}

		private IEnumerator StartRollBob()
		{
			OnCompassGirlMessage("えいっ");
			OnStopRollCompass(UICompass.Power.High);
			_uiCompassGirl.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/CompassGirls/CompassGirl_02-2");
			yield return new WaitForSeconds(0.2f);
			_uiCompassGirl.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/CompassGirls/CompassGirl_02-1");
			yield return new WaitForSeconds(1f);
			OnCompassGirlMessage("ここっ");
			_uiCompassGirl.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/CompassGirls/CompassGirl_02-2");
			Shake();
			yield return new WaitForSeconds(0.2f);
		}

		private IEnumerator StartRollDal()
		{
			OnStopRollCompass(UICompass.Power.Low);
			_uiCompassGirl.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/CompassGirls/CompassGirl_01-2");
			yield return new WaitForSeconds(0.2f);
			_uiCompassGirl.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/CompassGirls/CompassGirl_01-1");
			OnCompassGirlMessage("……ん");
			yield return new WaitForSeconds(1f);
			OnCompassGirlMessage("……あい");
		}

		private IEnumerator StartRollPony()
		{
			OnCompassGirlMessage("えいえいえーいっ");
			OnStopRollCompass(UICompass.Power.Low);
			for (int i = 0; i < 5; i++)
			{
				_uiCompassGirl.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/CompassGirls/CompassGirl_03-2");
				yield return new WaitForSeconds(0.03f);
				_uiCompassGirl.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/CompassGirls/CompassGirl_03-1");
				yield return new WaitForSeconds(0.075f);
				if (i == 2)
				{
					OnCompassGirlMessage("とまれーっ");
					yield return new WaitForSeconds(0.1f);
				}
			}
			Shake();
		}

		private void Shake()
		{
			int count = 15;
			Vector3 originPos = _uiCompassGirl.transform.localPosition;
			Observable.IntervalFrame(1).Take(count).Subscribe((Action<long>)delegate(long x)
			{
				float num = Mathe.Rate(0f, 15f, x);
				_uiCompassGirl.transform.localPosition = new Vector3(originPos.x + XorRandom.GetF11() * 5f * num, originPos.y + XorRandom.GetF11() * 5f * num, 0f);
			}, (Action)delegate
			{
				_uiCompassGirl.transform.localPosition = originPos;
			})
				.AddTo(base.gameObject);
		}

		private void OutDisplay()
		{
			switch (_iCompassType)
			{
			case CompassType.Super:
				OutDisplayFadeOutToTop();
				break;
			case CompassType.Normal:
				OutDisplayFadeOut();
				break;
			case CompassType.Stupid:
				OutDisplayFadeOutToBottom();
				break;
			case CompassType.Wizard:
				OutDisplayFadeOutToTopRight();
				break;
			}
		}

		private void OutDisplayFadeOutToTop()
		{
			float time = 0.5f;
			LeanTweenType ease = LeanTweenType.easeOutQuad;
			_uiCompassGirl.transform.LTValue(_uiCompassGirl.alpha, 0f, time).setEase(ease).setOnUpdate(delegate(float x)
			{
				_uiCompassGirl.alpha = x;
			});
			_uiCompassGirl.transform.LTMoveLocalY(30f, time).setEase(ease);
		}

		private void OutDisplayFadeOut()
		{
			_uiCompassGirl.transform.LTValue(_uiCompassGirl.alpha, 0f, 0.5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				_uiCompassGirl.alpha = x;
			});
		}

		private void OutDisplayFadeOutToBottom()
		{
			float time = 0.5f;
			LeanTweenType ease = LeanTweenType.easeOutQuad;
			_uiCompassGirl.transform.LTValue(_uiCompassGirl.alpha, 0f, time).setEase(ease).setOnUpdate(delegate(float x)
			{
				_uiCompassGirl.alpha = x;
			});
			_uiCompassGirl.transform.LTMoveLocalY(-30f, time).setEase(ease);
		}

		private void OutDisplayFadeOutToTopRight()
		{
			float time = 0.5f;
			LeanTweenType ease = LeanTweenType.easeOutQuad;
			_uiCompassGirl.transform.LTValue(_uiCompassGirl.alpha, 0f, time).setEase(ease).setOnUpdate(delegate(float x)
			{
				_uiCompassGirl.alpha = x;
			});
			Transform transform = _uiCompassGirl.transform;
			Vector3 localPosition = _uiCompassGirl.transform.localPosition;
			float x2 = localPosition.x + 30f;
			Vector3 localPosition2 = _uiCompassGirl.transform.localPosition;
			transform.LTMoveLocal(new Vector3(x2, localPosition2.y + 30f, 0f), time).setEase(ease);
		}

		private void OnCompassGirlMessage(string message)
		{
			Dlg.Call(ref _actOnCompassGirlMessage, message);
		}

		private void OnStopRollCompass(UICompass.Power power)
		{
			Dlg.Call(ref _actOnStopRollCompass, power);
		}
	}
}
