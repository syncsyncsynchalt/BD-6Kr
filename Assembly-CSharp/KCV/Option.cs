using DG.Tweening;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	public class Option : MonoBehaviour
	{
		public enum OptionMode
		{
			Show,
			Hide
		}

		public enum OptionMenu
		{
			BGM,
			SE,
			VOICE,
			GUIDE
		}

		private bool _Switch_Guide;

		[SerializeField]
		private UISlider _Slider_Volume_BGM;

		[SerializeField]
		private UISlider _Slider_Volume_SE;

		[SerializeField]
		private UISlider _Slider_Volume_Voice;

		[SerializeField]
		private UIButton _Button_Volume_BGM;

		[SerializeField]
		private UIButton _Button_Volume_SE;

		[SerializeField]
		private UIButton _Button_Volume_Voice;

		[SerializeField]
		private UIButton _Button_Guide;

		[SerializeField]
		private UISprite[] _sw_ball = new UISprite[2];

		[SerializeField]
		private UISprite[] _sw_base = new UISprite[2];

		[SerializeField]
		private GameObject[] _Cursor_bar = new GameObject[4];

		[SerializeField]
		private UISprite chara_arm;

		[SerializeField]
		private Transform _Guide;

		private float _Slider_Volume_BGM_temp;

		private float _Slider_Volume_SE_temp;

		private float _Slider_Volume_Voice_temp;

		private bool _AlreadyOpened;

		private Animation _bighand;

		private KeyControl mKeyController;

		private Dictionary<OptionMenu, float> ARM_ANGLEMAP = new Dictionary<OptionMenu, float>
		{
			{
				OptionMenu.BGM,
				5f
			},
			{
				OptionMenu.SE,
				23f
			},
			{
				OptionMenu.VOICE,
				43f
			},
			{
				OptionMenu.GUIDE,
				65f
			}
		};

		private SettingModel _Volumes;

		private OptionMenu mCurrentFocusSetting;

		private Action mOnBackListener;

		private void OnEnable()
		{
			if (_AlreadyOpened)
			{
				TweenAlpha.Begin(base.gameObject, 0.2f, 1f);
			}
		}

		private void ChangeFocus(OptionMenu menu, bool needSe)
		{
			mCurrentFocusSetting = menu;
			switch (mCurrentFocusSetting)
			{
			case OptionMenu.BGM:
				UISelectedObject.SelectedObjectBlink(_Cursor_bar, 0);
				break;
			case OptionMenu.SE:
				UISelectedObject.SelectedObjectBlink(_Cursor_bar, 1);
				break;
			case OptionMenu.VOICE:
				UISelectedObject.SelectedObjectBlink(_Cursor_bar, 2);
				break;
			case OptionMenu.GUIDE:
				UISelectedObject.SelectedObjectBlink(_Cursor_bar, 3);
				break;
			}
			if (needSe)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			float num = ARM_ANGLEMAP[mCurrentFocusSetting];
			iTween.RotateTo(chara_arm.gameObject, iTween.Hash("z", num, "time", 0.25f));
		}

		private void Start()
		{
			_Volumes = new SettingModel();
			UpdateUIState();
			TweenAlpha tweenAlpha = TweenAlpha.Begin(base.gameObject, 0.2f, 1f);
			tweenAlpha.delay = 0.2f;
			_AlreadyOpened = true;
			EventDelegate.Add(_Slider_Volume_BGM.onChange, delegate
			{
				UpdateVolumeParams();
			});
			UISlider slider_Volume_BGM = _Slider_Volume_BGM;
			slider_Volume_BGM.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(slider_Volume_BGM.onDragFinished, (UIProgressBar.OnDragFinished)delegate
			{
				ChangeFocus(OptionMenu.BGM, needSe: false);
			});
			EventDelegate.Add(_Slider_Volume_SE.onChange, delegate
			{
				UpdateVolumeParams();
			});
			UISlider slider_Volume_SE = _Slider_Volume_SE;
			slider_Volume_SE.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(slider_Volume_SE.onDragFinished, (UIProgressBar.OnDragFinished)delegate
			{
				ChangeFocus(OptionMenu.SE, needSe: false);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			});
			EventDelegate.Add(_Slider_Volume_Voice.onChange, delegate
			{
				UpdateVolumeParams();
			});
			UISlider slider_Volume_Voice = _Slider_Volume_Voice;
			slider_Volume_Voice.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(slider_Volume_Voice.onDragFinished, (UIProgressBar.OnDragFinished)delegate
			{
				ChangeFocus(OptionMenu.VOICE, needSe: false);
				PlayVoiceCheck();
			});
			ChangeFocus(OptionMenu.BGM, needSe: false);
			_bighand = GetComponent<Animation>();
		}

		private void UpdateUIState()
		{
			_Slider_Volume_BGM.value = (float)_Volumes.VolumeBGM / 100f;
			_Slider_Volume_SE.value = (float)_Volumes.VolumeSE / 100f;
			_Slider_Volume_Voice.value = (float)_Volumes.VolumeVoice / 100f;
			_Switch_Guide = _Volumes.GuideDisplay;
			_Slider_Volume_BGM_temp = _Slider_Volume_BGM.value;
			_Slider_Volume_SE_temp = _Slider_Volume_SE.value;
			_Slider_Volume_Voice_temp = _Slider_Volume_Voice.value;
			togggleSW(change_sw: false, 1);
		}

		public void togggleSW(bool change_sw, int sw_ch)
		{
			if (change_sw)
			{
				if (sw_ch != 0 && sw_ch == 1)
				{
					if (!_Switch_Guide)
					{
						_sw_ball[sw_ch].gameObject.MoveTo(new Vector3(52f, -5f, 0f), 0.3f, local: true);
						_sw_ball[sw_ch].spriteName = "switch_on_pin";
						_sw_base[sw_ch].spriteName = "switch_on";
						_Button_Guide.normalSprite = "switch_on";
						_Switch_Guide = true;
						_Guide.localScaleOne();
					}
					else
					{
						_sw_ball[sw_ch].gameObject.MoveTo(new Vector3(-52f, -5f, 0f), 0.3f, local: true);
						_sw_ball[sw_ch].spriteName = "switch_off_pin";
						_sw_base[sw_ch].spriteName = "switch_off";
						_Button_Guide.normalSprite = "switch_off";
						_Switch_Guide = false;
						_Guide.localScaleZero();
					}
				}
			}
			else
			{
				base.gameObject.GetComponent<UIPanel>().enabled = false;
				if (sw_ch != 0 && sw_ch == 1)
				{
					if (_Switch_Guide)
					{
						_sw_ball[sw_ch].gameObject.transform.localPosition = new Vector3(52f, -5f, 0f);
						_sw_ball[sw_ch].spriteName = "switch_on_pin";
						_sw_base[sw_ch].spriteName = "switch_on";
						_Button_Guide.normalSprite = "switch_on";
						_Guide.transform.localScaleOne();
					}
					else
					{
						_sw_ball[sw_ch].gameObject.transform.localPosition = new Vector3(-52f, -5f, 0f);
						_sw_ball[sw_ch].spriteName = "switch_off_pin";
						_sw_base[sw_ch].spriteName = "switch_off";
						_Button_Guide.normalSprite = "switch_off";
						_Guide.localScaleZero();
					}
				}
				base.gameObject.GetComponent<UIPanel>().enabled = true;
			}
			if (change_sw)
			{
				SoundUtils.PlaySE(SEFIleInfos.MainMenuOnClick);
			}
		}

		private void UpdateVolumeParams()
		{
			if (_Slider_Volume_BGM.value < 0.002f)
			{
				_Slider_Volume_BGM.value = 0f;
				_Button_Volume_BGM.normalSprite = "speaker_off";
			}
			else
			{
				_Button_Volume_BGM.normalSprite = "speaker_on";
			}
			if (_Slider_Volume_SE.value < 0.002f)
			{
				_Slider_Volume_SE.value = 0f;
				_Button_Volume_SE.normalSprite = "speaker_off";
			}
			else
			{
				_Button_Volume_SE.normalSprite = "speaker_on";
			}
			if (_Slider_Volume_Voice.value < 0.002f)
			{
				_Slider_Volume_Voice.value = 0f;
				_Button_Volume_Voice.normalSprite = "speaker_off";
			}
			else
			{
				_Button_Volume_Voice.normalSprite = "speaker_on";
			}
			SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM = _Slider_Volume_BGM.value;
			SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.SE = _Slider_Volume_SE.value;
			SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.Voice = _Slider_Volume_Voice.value;
			SingletonMonoBehaviour<SoundManager>.Instance.rawBGMVolume = _Slider_Volume_BGM.value;
			_Volumes.VolumeBGM = (int)(_Slider_Volume_BGM.value * 100f);
			_Volumes.VolumeSE = (int)(_Slider_Volume_SE.value * 100f);
			if (_Volumes.VolumeVoice != (int)(_Slider_Volume_Voice.value * 100f))
			{
				_Volumes.VolumeVoice = (int)(_Slider_Volume_Voice.value * 100f);
			}
			_Volumes.GuideDisplay = _Switch_Guide;
		}

		public void SetKeyController(KeyControl keyController)
		{
			if (keyController == null)
			{
				if (mKeyController != null)
				{
					mKeyController.reset();
				}
				mKeyController = null;
			}
			else
			{
				mKeyController = keyController;
				mKeyController.reset(0, 3);
				mKeyController.setChangeValue(-1f, 0f, 1f, 0f);
			}
		}

		public void SetOnBackListener(Action onBackListener)
		{
			mOnBackListener = onBackListener;
		}

		private void OnBack()
		{
			if (mOnBackListener != null)
			{
				mOnBackListener();
			}
		}

		private void Update()
		{
			if (mKeyController != null)
			{
				if (mKeyController.keyState[1].down)
				{
					OnPressDownButtonCircle();
					UpdateVolumeParams();
				}
				else if (mKeyController.keyState[0].down)
				{
					OnPressDownButtonCross();
					UpdateVolumeParams();
				}
				else if (mKeyController.keyState[8].down)
				{
					OnPressDownButtonUp();
				}
				else if (mKeyController.keyState[12].down)
				{
					OnPressDownButtonDown();
				}
				else if (mKeyController.keyState[10].down)
				{
					OnPressDownButtonRight();
					UpdateVolumeParams();
				}
				else if (mKeyController.keyState[10].up)
				{
					OnPressUpButtonRight();
					UpdateVolumeParams();
				}
				else if (mKeyController.keyState[14].down)
				{
					OnPressDownButtonLeft();
					UpdateVolumeParams();
				}
				else if (mKeyController.keyState[14].up)
				{
					OnPressUpButtonLeft();
					UpdateVolumeParams();
				}
			}
		}

		private void OnPressUpButtonLeft()
		{
			if (mCurrentFocusSetting == OptionMenu.VOICE)
			{
				PlayVoiceCheck();
			}
		}

		private void ChangeValueEffect()
		{
			DOTween.Kill(this);
			chara_arm.transform.DOScale(new Vector3(1.1f, 1.1f), 0.1f).OnComplete(delegate
			{
				chara_arm.transform.localScale = Vector3.one;
			}).SetId(this)
				.SetEase(Ease.Linear);
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		}

		private void OnPressDownButtonLeft()
		{
			switch (mCurrentFocusSetting)
			{
			case OptionMenu.BGM:
				if (_Slider_Volume_BGM.value - 0.05f < 0f)
				{
					_Slider_Volume_BGM.value = 0f;
					break;
				}
				_Slider_Volume_BGM.value -= 0.05f;
				ChangeValueEffect();
				break;
			case OptionMenu.SE:
				if (_Slider_Volume_SE.value - 0.05f < 0f)
				{
					_Slider_Volume_SE.value = 0f;
					break;
				}
				_Slider_Volume_SE.value -= 0.05f;
				ChangeValueEffect();
				break;
			case OptionMenu.VOICE:
				if (_Slider_Volume_Voice.value - 0.05f < 0f)
				{
					_Slider_Volume_Voice.value = 0f;
					break;
				}
				_Slider_Volume_Voice.value -= 0.05f;
				ChangeValueEffect();
				break;
			}
		}

		private void OnPressUpButtonRight()
		{
			if (mCurrentFocusSetting == OptionMenu.VOICE)
			{
				PlayVoiceCheck();
			}
		}

		private void OnPressDownButtonRight()
		{
			switch (mCurrentFocusSetting)
			{
			case OptionMenu.BGM:
				if (_Slider_Volume_BGM.value + 0.05f > 1f)
				{
					_Slider_Volume_BGM.value = 1f;
					break;
				}
				_Slider_Volume_BGM.value += 0.05f;
				ChangeValueEffect();
				break;
			case OptionMenu.SE:
				if (_Slider_Volume_SE.value + 0.05f > 1f)
				{
					_Slider_Volume_SE.value = 1f;
					break;
				}
				_Slider_Volume_SE.value += 0.05f;
				ChangeValueEffect();
				break;
			case OptionMenu.VOICE:
				if (_Slider_Volume_Voice.value + 0.05f > 1f)
				{
					_Slider_Volume_Voice.value = 1f;
					break;
				}
				_Slider_Volume_Voice.value += 0.05f;
				ChangeValueEffect();
				break;
			}
		}

		private void OnPressDownButtonUp()
		{
			switch (mCurrentFocusSetting)
			{
			case OptionMenu.SE:
				ChangeFocus(OptionMenu.BGM, needSe: true);
				break;
			case OptionMenu.VOICE:
				ChangeFocus(OptionMenu.SE, needSe: true);
				break;
			case OptionMenu.GUIDE:
				ChangeFocus(OptionMenu.VOICE, needSe: true);
				break;
			}
		}

		private void OnPressDownButtonDown()
		{
			switch (mCurrentFocusSetting)
			{
			case OptionMenu.BGM:
				ChangeFocus(OptionMenu.SE, needSe: true);
				break;
			case OptionMenu.SE:
				ChangeFocus(OptionMenu.VOICE, needSe: true);
				break;
			case OptionMenu.VOICE:
				ChangeFocus(OptionMenu.GUIDE, needSe: true);
				break;
			}
		}

		private void OnPressDownButtonCross()
		{
			bighand(0.1f);
			Pressed_Button_close();
		}

		private void OnPressDownButtonCircle()
		{
			bighand(0.1f);
			switch (mCurrentFocusSetting)
			{
			case OptionMenu.BGM:
				Pressed_Button_Volume_BGM();
				break;
			case OptionMenu.SE:
				Pressed_Button_Volume_SE();
				break;
			case OptionMenu.VOICE:
				Pressed_Button_Volume_Voice();
				break;
			case OptionMenu.GUIDE:
				togggleSW(change_sw: true, 1);
				break;
			}
		}

		private void PlayVoiceCheck()
		{
			ShipUtils.PlayShipVoice(SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel, 8);
		}

		private void bighand(float time)
		{
			_bighand.Stop();
			_bighand.Play();
		}

		public void Pressed_Button_Volume_BGM()
		{
			ChangeFocus(OptionMenu.BGM, needSe: true);
			if (_Slider_Volume_BGM.value != 0f)
			{
				_Slider_Volume_BGM_temp = _Slider_Volume_BGM.value;
				_Slider_Volume_BGM.value = 0f;
			}
			else
			{
				_Slider_Volume_BGM.value = _Slider_Volume_BGM_temp;
			}
		}

		public void Pressed_Button_Volume_SE()
		{
			ChangeFocus(OptionMenu.SE, needSe: true);
			if (_Slider_Volume_SE.value != 0f)
			{
				_Slider_Volume_SE_temp = _Slider_Volume_SE.value;
				_Slider_Volume_SE.value = 0f;
			}
			else
			{
				_Slider_Volume_SE.value = _Slider_Volume_SE_temp;
			}
		}

		public void Pressed_Button_Volume_Voice()
		{
			ChangeFocus(OptionMenu.VOICE, needSe: true);
			if (_Slider_Volume_Voice.value != 0f)
			{
				_Slider_Volume_Voice_temp = _Slider_Volume_Voice.value;
				_Slider_Volume_Voice.value = 0f;
			}
			else
			{
				_Slider_Volume_Voice.value = _Slider_Volume_Voice_temp;
			}
		}

		public void Pressed_Button_Guide()
		{
			ChangeFocus(OptionMenu.GUIDE, needSe: true);
			bighand(0.1f);
			togggleSW(change_sw: true, 1);
		}

		public void Pressed_Button_voice_time()
		{
		}

		public void Pressed_Button_close()
		{
			_onClickOverlayButton();
		}

		public void _onClickOverlayButton()
		{
			_Volumes.Save();
			OnBack();
		}

		private void OnDestroy()
		{
			_Slider_Volume_BGM = null;
			_Slider_Volume_SE = null;
			_Slider_Volume_Voice = null;
			_Button_Volume_BGM = null;
			_Button_Volume_SE = null;
			_Button_Volume_Voice = null;
			_Button_Guide = null;
			_sw_ball = null;
			_sw_base = null;
			_Cursor_bar = null;
			chara_arm = null;
			_Guide = null;
			_bighand = null;
			mKeyController = null;
			ARM_ANGLEMAP = null;
			_Volumes = null;
		}
	}
}
