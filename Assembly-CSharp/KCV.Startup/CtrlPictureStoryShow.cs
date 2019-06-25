using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(UIPanel))]
	public class CtrlPictureStoryShow : MonoBehaviour
	{
		[Serializable]
		private struct Params : IDisposable
		{
			[Header("[Fairy Properties]")]
			public List<Vector3> fairyPos;

			public List<Vector3> fairySize;

			public List<Vector3> fairyBalloonPos;

			public List<Vector3> fairyBalloonSize;

			public float showBalloonTime;

			public float frameAnimationInterval;

			[Header("[Description Properties]")]
			public List<Vector3> descriptionPos;

			public List<Vector3> descriptionSize;

			[Header("[MessageWindow Properties]")]
			public List<Vector3> messageWindowPos;

			public float showHideMessageWindowTime;

			public void Dispose()
			{
				Mem.DelListSafe(ref fairyPos);
				Mem.DelListSafe(ref fairySize);
				Mem.DelListSafe(ref fairyBalloonPos);
				Mem.DelListSafe(ref fairyBalloonSize);
				Mem.Del(ref showBalloonTime);
				Mem.Del(ref frameAnimationInterval);
				Mem.DelListSafe(ref descriptionPos);
				Mem.DelListSafe(ref descriptionSize);
				Mem.DelListSafe(ref messageWindowPos);
				Mem.Del(ref showHideMessageWindowTime);
			}
		}

		[Serializable]
		private class UIFairy : IDisposable
		{
			[Serializable]
			private struct FairyTexture : IDisposable
			{
				public Texture2D item1;

				public Texture2D item2;

				public void Dispose()
				{
					Mem.Del(ref item1);
					Mem.Del(ref item2);
				}
			}

			[SerializeField]
			private UITexture _uiFairy;

			[SerializeField]
			private UITexture _uiBalloon;

			[SerializeField]
			private List<FairyTexture> _listFairyTexture;

			private IDisposable _disAnimation;

			public UITexture fairy => _uiFairy;

			public bool Init()
			{
				UITexture uiFairy = _uiFairy;
				float alpha = 0f;
				_uiBalloon.alpha = alpha;
				uiFairy.alpha = alpha;
				return true;
			}

			public void Dispose()
			{
				Mem.Del(ref _uiFairy);
				Mem.Del(ref _uiBalloon);
				if (_listFairyTexture != null)
				{
					_listFairyTexture.ForEach(delegate(FairyTexture x)
					{
						x.Dispose();
					});
				}
				Mem.DelListSafe(ref _listFairyTexture);
				Mem.DelIDisposableSafe(ref _disAnimation);
			}

			public void SetFairy(int nPage, System.Tuple<Vector3, Vector3> vFairy, System.Tuple<Vector3, Vector3> vBalloon)
			{
				UITexture uiFairy = _uiFairy;
				FairyTexture fairyTexture = _listFairyTexture[nPage];
				uiFairy.mainTexture = fairyTexture.item1;
				_uiFairy.localSize = vFairy.Item1;
				_uiFairy.transform.localPosition = vFairy.Item2;
				_uiFairy.alpha = 1f;
				_uiBalloon.mainTexture = Resources.Load<Texture2D>($"Textures/Startup/PictureStoryShow/info{nPage + 1}_fuki");
				_uiBalloon.localSize = vBalloon.Item1;
				_uiBalloon.transform.localPosition = vBalloon.Item2;
				_uiBalloon.alpha = 0f;
			}

			public void PlayFairyAnimation(int nPage, float fFrameTime)
			{
				if (_disAnimation != null)
				{
					_disAnimation.Dispose();
				}
				bool isFoward = false;
				_disAnimation = Observable.Interval(TimeSpan.FromSeconds(fFrameTime)).Subscribe(delegate
				{
					UITexture uiFairy = _uiFairy;
					object mainTexture;
					if (isFoward)
					{
						FairyTexture fairyTexture = _listFairyTexture[nPage];
						mainTexture = fairyTexture.item1;
					}
					else
					{
						FairyTexture fairyTexture2 = _listFairyTexture[nPage];
						mainTexture = fairyTexture2.item2;
					}
					uiFairy.mainTexture = (Texture)mainTexture;
					isFoward = !isFoward;
				});
			}

			public LTDescr ShowBalloon(float fShowTime)
			{
				return _uiBalloon.transform.LTValue(_uiBalloon.alpha, 1f, fShowTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					_uiBalloon.alpha = x;
				});
			}

			public AudioSource PlayVoice(int nPage, Action onFinished)
			{
				switch (nPage)
				{
				case 0:
					return Utils.PlayDescriptionVoice(25, onFinished);
				case 1:
					return Utils.PlayDescriptionVoice(28, onFinished);
				case 2:
					return Utils.PlayDescriptionVoice(31, onFinished);
				default:
					return null;
				}
			}

			public void StopVoice()
			{
				ShipUtils.StopShipVoice();
			}
		}

		[Serializable]
		private class UISheet : IDisposable
		{
			[SerializeField]
			private UITexture _uiBackground;

			[SerializeField]
			private UITexture _uiDescription;

			[SerializeField]
			[Header("[Animation Properties]")]
			private Vector3 _vStartPos = Vector3.zero;

			[SerializeField]
			private float _fShowTime = 1f;

			[SerializeField]
			private LeanTweenType _iShowTweenType = LeanTweenType.linear;

			public Transform transform => _uiBackground.transform;

			public bool Init()
			{
				transform.localPosition = _vStartPos;
				return true;
			}

			public void Dispose()
			{
				Mem.Del(ref _uiBackground);
				Mem.Del(ref _uiDescription);
			}

			public void Show(Action onFinished)
			{
				transform.LTMoveLocal(Vector3.zero, _fShowTime).setEase(_iShowTweenType).setOnComplete(onFinished);
			}
		}

		private const int PAGE_MAX_NUM = 3;

		[SerializeField]
		private List<UISheet> _listSheets;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiDescription;

		[SerializeField]
		private UIButton _uiGearButton;

		[SerializeField]
		private UIStartupFairy _uiStartupFairy;

		[Header("[Animation Parameters]")]
		[SerializeField]
		private Params _strParams;

		private int _nNowPage;

		private UIPanel _uiPanel;

		private Transform _traPartnerShip;

		private Action _actOnFinished;

		private List<Texture2D> _listDescriptionTex;

		private StatementMachine _clsState;

		public int nowPage
		{
			get
			{
				return _nNowPage;
			}
			private set
			{
				_nNowPage = value;
			}
		}

		private UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static CtrlPictureStoryShow Instantiate(CtrlPictureStoryShow prefab, Transform parent, Action onFinished)
		{
			CtrlPictureStoryShow ctrlPictureStoryShow = UnityEngine.Object.Instantiate(prefab);
			ctrlPictureStoryShow.transform.parent = parent;
			ctrlPictureStoryShow.transform.localScaleOne();
			ctrlPictureStoryShow.transform.localPositionZero();
			return ctrlPictureStoryShow.VitualCtor(onFinished);
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiDescription);
			Mem.Del(ref _uiGearButton);
			Mem.Del(ref _uiStartupFairy);
			Mem.DelIDisposable(ref _strParams);
			Mem.Del(ref _nNowPage);
			Mem.Del(ref _actOnFinished);
			Mem.DelListSafe(ref _listDescriptionTex);
			if (_clsState != null)
			{
				_clsState.Clear();
			}
			Mem.Del(ref _clsState);
		}

		private CtrlPictureStoryShow VitualCtor(Action onFinished)
		{
			nowPage = 0;
			_actOnFinished = onFinished;
			_uiBackground.alpha = 0f;
			_uiDescription.alpha = 0f;
			_uiGearButton.GetComponent<UISprite>().alpha = 0f;
			_uiGearButton.enabled = false;
			_traPartnerShip = StartupTaskManager.GetPartnerSelect().transform;
			_uiStartupFairy.Startup();
			_listSheets.ForEach(delegate(UISheet x)
			{
				x.Init();
			});
			Observable.FromCoroutine((UniRx.IObserver<bool> observer) => PlayPictureStoryShow(observer)).Subscribe(delegate
			{
				HidePictureStoryShow(OnFinished);
			});
			return this;
		}

		private IEnumerator PlayPictureStoryShow(UniRx.IObserver<bool> observer)
		{
			KeyControl input = StartupTaskManager.GetKeyControl();
			bool isAnimationWait9 = true;
			yield return Observable.TimerFrame(2, FrameCountType.EndOfFrame).StartAsCoroutine();
			_listSheets[0].Show(delegate
			{
				isAnimationWait9 = false;
			});

            UISprite gear = this._uiGearButton.GetComponent<UISprite>();
            LeanTweenExtesntions.LTValue(from: _uiGearButton.GetComponent<UISprite>().alpha, self: _uiGearButton.transform, to: 1f, time: _strParams.showHideMessageWindowTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
                gear.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					this._uiGearButton.enabled = true;
				});
			while (isAnimationWait9)
			{
				yield return null;
			}
			isAnimationWait9 = true;
			_uiStartupFairy.Show(UIStartupFairy.FairyType.Tails, UIStartupFairy.FairyState.Move, delegate
			{
				this._uiGearButton.onClick.Clear();
				this._uiGearButton.onClick.Add(new EventDelegate(delegate
				{
					isAnimationWait9 = false;
				}));
			});
			PlayDescriotionVoice(0);
			while (isAnimationWait9)
			{
				if (input.GetDown(KeyControl.KeyName.MARU))
				{
					isAnimationWait9 = false;
				}
				yield return null;
			}
			_uiGearButton.onClick.Clear();
			_uiStartupFairy.ImmediateIdle();
			Utils.StopDescriptionVoice();
			yield return Observable.TimerFrame(2, FrameCountType.EndOfFrame).StartAsCoroutine();
			isAnimationWait9 = true;
			_uiStartupFairy.ShowBaloon(0, new System.Tuple<Vector3, Vector3>(_strParams.fairyBalloonSize[0], _strParams.fairyBalloonPos[0]), null);
			_uiStartupFairy.PlayVoice(0, null);
			_uiGearButton.onClick.Clear();
			_uiGearButton.onClick.Add(new EventDelegate(delegate
			{
				isAnimationWait9 = false;
			}));
			while (isAnimationWait9)
			{
				if (input.GetDown(KeyControl.KeyName.MARU))
				{
					isAnimationWait9 = false;
				}
				yield return null;
			}
			_uiGearButton.onClick.Clear();
			_uiStartupFairy.StopVoice();
			_uiStartupFairy.Hide(null);
			yield return Observable.Timer(TimeSpan.FromSeconds(0.75)).StartAsCoroutine();
			yield return Observable.TimerFrame(2, FrameCountType.EndOfFrame).StartAsCoroutine();
			isAnimationWait9 = true;
			_listSheets[1].Show(delegate
			{
                isAnimationWait9 = false;
			});
			while (isAnimationWait9)
			{
				yield return null;
			}
			isAnimationWait9 = true;
			_uiStartupFairy.Show(UIStartupFairy.FairyType.Ahoge, UIStartupFairy.FairyState.Move, delegate
			{
				this._uiGearButton.onClick.Add(new EventDelegate(delegate
				{
					isAnimationWait9 = false;
				}));
			});
			PlayDescriotionVoice(1);
			while (isAnimationWait9)
			{
				if (input.GetDown(KeyControl.KeyName.MARU))
				{
					isAnimationWait9 = false;
				}
				yield return null;
			}
			_uiGearButton.onClick.Clear();
			_uiStartupFairy.ImmediateIdle();
			Utils.StopDescriptionVoice();
			yield return Observable.TimerFrame(2, FrameCountType.EndOfFrame).StartAsCoroutine();
			isAnimationWait9 = true;
			_uiStartupFairy.ShowBaloon(1, new System.Tuple<Vector3, Vector3>(_strParams.fairyBalloonSize[1], _strParams.fairyBalloonPos[1]), null);
			_uiStartupFairy.PlayVoice(1, null);
			_uiGearButton.onClick.Clear();
			_uiGearButton.onClick.Add(new EventDelegate(delegate
			{
				isAnimationWait9 = false;
			}));
			while (isAnimationWait9)
			{
				if (input.GetDown(KeyControl.KeyName.MARU))
				{
					isAnimationWait9 = false;
				}
				yield return null;
			}
			_uiGearButton.onClick.Clear();
			_uiStartupFairy.StopVoice();
			_uiStartupFairy.Hide(null);
			yield return Observable.Timer(TimeSpan.FromSeconds(0.75)).StartAsCoroutine();
			yield return Observable.TimerFrame(2, FrameCountType.EndOfFrame).StartAsCoroutine();
			isAnimationWait9 = true;
			_listSheets[2].Show(delegate
			{
				isAnimationWait9 = false;
			});
			while (isAnimationWait9)
			{
				yield return null;
			}
			isAnimationWait9 = true;
			_uiStartupFairy.Show(UIStartupFairy.FairyType.Braid, UIStartupFairy.FairyState.Idle, delegate
			{
				this._uiGearButton.onClick.Add(new EventDelegate(delegate
				{
					isAnimationWait9 = false;
				}));
			});
			PlayDescriotionVoice(2);
			while (isAnimationWait9)
			{
				if (input.GetDown(KeyControl.KeyName.MARU))
				{
					isAnimationWait9 = false;
				}
				yield return null;
			}
			_uiGearButton.onClick.Clear();
			_uiStartupFairy.ImmediateIdle();
			Utils.StopDescriptionVoice();
			yield return Observable.TimerFrame(2, FrameCountType.EndOfFrame).StartAsCoroutine();
			isAnimationWait9 = true;
			_uiStartupFairy.ShowBaloon(2, new System.Tuple<Vector3, Vector3>(_strParams.fairyBalloonSize[2], _strParams.fairyBalloonPos[2]), null);
			_uiStartupFairy.PlayVoice(2, null);
			_uiGearButton.onClick.Clear();
			_uiGearButton.onClick.Add(new EventDelegate(delegate
			{
				isAnimationWait9 = false;
			}));
			while (isAnimationWait9)
			{
				if (input.GetDown(KeyControl.KeyName.MARU))
				{
					isAnimationWait9 = false;
				}
				yield return null;
			}
			_uiGearButton.onClick.Clear();
			_uiStartupFairy.StopVoice();
			_uiStartupFairy.Hide(null);
			yield return Observable.Timer(TimeSpan.FromSeconds(0.75)).StartAsCoroutine();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private IEnumerator PlayDesctiption()
		{
			KeyControl input = StartupTaskManager.GetKeyControl();
			bool isWait2 = true;
			yield return Observable.TimerFrame(2, FrameCountType.EndOfFrame).StartAsCoroutine();
			PlayDescriotionVoice(nowPage);
			_uiGearButton.onClick.Clear();
			_uiGearButton.onClick.Add(new EventDelegate(delegate
			{
				isWait2 = false;
			}));
			while (isWait2)
			{
				if (input.GetDown(KeyControl.KeyName.MARU))
				{
					isWait2 = false;
				}
				yield return null;
			}
			Utils.StopDescriptionVoice();
			yield return Observable.TimerFrame(2, FrameCountType.EndOfFrame).StartAsCoroutine();
			isWait2 = true;
			_uiGearButton.onClick.Clear();
			_uiGearButton.onClick.Add(new EventDelegate(delegate
			{
				isWait2 = false;
			}));
			while (isWait2)
			{
				if (input.GetDown(KeyControl.KeyName.MARU))
				{
					isWait2 = false;
				}
				yield return null;
			}
			yield return Observable.TimerFrame(2, FrameCountType.EndOfFrame).StartAsCoroutine();
			nowPage++;
		}

		private void PlayDescriotionVoice(int nPage)
		{
			List<int> voiceNum = new List<int>();
			switch (nPage)
			{
			case 0:
				voiceNum.Add(23);
				voiceNum.Add(24);
				break;
			case 1:
				voiceNum.Add(26);
				voiceNum.Add(27);
				break;
			case 2:
				voiceNum.Add(29);
				voiceNum.Add(30);
				break;
			}
			Utils.PlayDescriptionVoice(voiceNum[0], delegate
			{
				Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate
				{
					Utils.PlayDescriptionVoice(voiceNum[1], null);
				});
			});
		}

		private void ShowPictureStoryShow(Action onFinished)
		{
			UISprite gear = _uiGearButton.GetComponent<UISprite>();
			_uiBackground.transform.LTValue(0f, 1f, _strParams.showHideMessageWindowTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiBackground.alpha = x;
				_uiDescription.alpha = x;
				gear.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					_uiGearButton.enabled = true;
					Dlg.Call(ref onFinished);
				});
		}

		private void HidePictureStoryShow(Action onFinished)
		{
			UISprite gear = _uiGearButton.GetComponent<UISprite>();
			_uiGearButton.enabled = false;
			_uiBackground.transform.LTValue(1f, 0f, _strParams.showHideMessageWindowTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				gear.alpha = x;
			})
				.setOnStart(delegate
				{
					Dlg.Call(ref onFinished);
				});
		}

		private void OnFinished()
		{
			Dlg.Call(ref _actOnFinished);
		}
	}
}
