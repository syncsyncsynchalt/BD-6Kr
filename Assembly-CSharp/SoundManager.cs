using KCV;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
	[Serializable]
	public class AudioSourceObserver : IDisposable
	{
		private AudioSource _asAudioSource;

		private IDisposable _disFinishedDisposable;

		private IDisposable _disStopDisposable;

		private Action _actOnFinished;

		public AudioSource source
		{
			get
			{
				return _asAudioSource;
			}
			private set
			{
				_asAudioSource = value;
			}
		}

		public float clipLength => (!((UnityEngine.Object)_asAudioSource.clip != null)) ? 0f : _asAudioSource.clip.length;

		public AudioSourceObserver(AudioSource source)
		{
			_asAudioSource = source;
			_disFinishedDisposable = null;
			_disStopDisposable = null;
			_actOnFinished = null;
		}

		public void Dispose()
		{
			Mem.Del(ref _asAudioSource);
			Mem.DelIDisposableSafe(ref _disFinishedDisposable);
			Mem.DelIDisposableSafe(ref _disStopDisposable);
			Mem.Del(ref _actOnFinished);
		}

		public AudioSourceObserver PlayOneShot(AudioClip clip, float fVolume)
		{
			return Play(clip, fVolume, isOneShot: true, isObserver: false, null);
		}

		public AudioSourceObserver Play(AudioClip clip, float fVolume, bool isObserver, Action onFinished)
		{
			return Play(clip, fVolume, isOneShot: false, isObserver, onFinished);
		}

		private AudioSourceObserver Play(AudioClip clip, float fVolume, bool isOneShot, bool isObserver, Action onFinished)
		{
			if ((UnityEngine.Object)clip == null)
			{
				return this;
			}
			Mem.DelIDisposableSafe(ref _disFinishedDisposable);
			Mem.DelIDisposableSafe(ref _disStopDisposable);
			_actOnFinished = onFinished;
			_asAudioSource.volume = fVolume;
			if (isOneShot)
			{
				_asAudioSource.PlayOneShot(clip);
			}
			else
			{
				_asAudioSource.clip = clip;
				_asAudioSource.Play();
				if (!_asAudioSource.loop)
				{
					TimeSpan dueTime = TimeSpan.FromSeconds(_asAudioSource.clip.length);
					object scheduler;
					if (isObserver)
					{
						IScheduler mainThread = Scheduler.MainThread;
						scheduler = mainThread;
					}
					else
					{
						scheduler = Scheduler.MainThreadIgnoreTimeScale;
					}
					_disFinishedDisposable = Observable.Timer(dueTime, (IScheduler)scheduler).Subscribe(delegate
					{
						OnAudioPlayFinished();
					});
				}
			}
			return this;
		}

		private void OnAudioPlayFinished()
		{
			Dlg.Call(ref _actOnFinished);
			Clear();
		}

		public AudioSourceObserver Stop(bool isCallOnFinished, float fDuration)
		{
			if (_disFinishedDisposable != null)
			{
				_disFinishedDisposable.Dispose();
			}
			_disStopDisposable = null;
			if (fDuration == 0f || !_asAudioSource.isPlaying)
			{
				if (isCallOnFinished)
				{
					Dlg.Call(ref _actOnFinished);
				}
				Clear();
			}
			else
			{
				_disStopDisposable = Utils.Fade(_asAudioSource, 0f, fDuration, delegate
				{
					if (isCallOnFinished)
					{
						Dlg.Call(ref _actOnFinished);
					}
					Clear();
				});
			}
			return this;
		}

		public AudioSourceObserver Stop(bool isCallOnFinished)
		{
			return Stop(isCallOnFinished, 0f);
		}

		public AudioSourceObserver StopFade(float fDuration, Action onFinished)
		{
			Mem.DelIDisposableSafe(ref _disFinishedDisposable);
			_disStopDisposable = Utils.Fade(_asAudioSource, 0f, fDuration, delegate
			{
				_asAudioSource.Stop();
				Mem.DelIDisposableSafe(ref _disStopDisposable);
				Dlg.Call(ref onFinished);
			});
			return this;
		}

		public void Clear()
		{
			Mem.DelIDisposableSafe(ref _disFinishedDisposable);
			Mem.DelIDisposableSafe(ref _disStopDisposable);
			UnLoadAudioSourceClip();
			Mem.Del(ref _actOnFinished);
		}

		private void UnLoadAudioSourceClip()
		{
			if ((UnityEngine.Object)_asAudioSource != null)
			{
				if (_asAudioSource.isPlaying)
				{
					_asAudioSource.Stop();
				}
				_asAudioSource.clip = null;
			}
		}
	}

	public class Utils
	{
		public static IDisposable Fade(AudioSource source, float fTo, float fDuration, Action onFinished)
		{
			float time = 0f;
			float tempVolime = source.volume;
			return (from x in Observable.EveryUpdate()
				select time += Time.deltaTime).TakeWhile((float x) => fDuration >= x).Subscribe((Action<float>)delegate(float _)
			{
				source.volume = Mathe.Lerp(tempVolime, fTo, _ / fDuration);
			}, (Action)delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		public static float GetClipLength(AudioClip clip)
		{
			if ((UnityEngine.Object)clip == null)
			{
				return 0f;
			}
			return clip.length;
		}
	}

	public const int SIMULTANEOUS_PLAYBACK_NUM = 18;

	private SoundVolume _clsVolume = new SoundVolume();

	private AudioSourceObserver _clsBGMObserver;

	private List<AudioSourceObserver> _listSEObserver;

	private List<AudioSourceObserver> _listVoiceObserver;

	public SoundVolume soundVolume => _clsVolume;

	public AudioSource bgmSource => _clsBGMObserver.source;

	public List<AudioSourceObserver> voiceSource => _listVoiceObserver;

	public List<AudioSourceObserver> seSourceObserver => _listSEObserver;

	public bool isBGMPlaying => _clsBGMObserver.source.isPlaying;

	public bool isAnySEPlaying
	{
		get
		{
			foreach (AudioSourceObserver item in _listSEObserver)
			{
				if (item.source.isPlaying)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool isAnyVoicePlaying
	{
		get
		{
			foreach (AudioSourceObserver item in _listVoiceObserver)
			{
				if (item.source.isPlaying)
				{
					return true;
				}
			}
			return false;
		}
	}

	public float rawBGMVolume
	{
		get
		{
			return _clsBGMObserver.source.volume;
		}
		set
		{
			SoundVolume soundVolume = this.soundVolume;
			float num = Mathe.MinMax2F01(value);
			_clsBGMObserver.source.volume = num;
			soundVolume.BGM = num;
		}
	}

	public float rawVoiceVolume
	{
		get
		{
			return _listVoiceObserver[0].source.volume;
		}
		set
		{
			float val = Mathe.MinMax2F01(value);
			soundVolume.Voice = val;
			_listVoiceObserver.ForEach(delegate(AudioSourceObserver x)
			{
				x.source.volume = val;
			});
		}
	}

	public float rawSEVolume
	{
		get
		{
			return _listSEObserver[0].source.volume;
		}
		set
		{
			float num = Mathe.MinMax2F01(value);
			soundVolume.SE = num;
			foreach (AudioSourceObserver item in _listSEObserver)
			{
				item.source.volume = num;
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_clsBGMObserver = new AudioSourceObserver(base.transform.FindChild("BGM").AddComponent<AudioSource>());
		_clsBGMObserver.source.priority = 0;
		_clsBGMObserver.source.playOnAwake = false;
		_clsBGMObserver.source.loop = true;
		_listVoiceObserver = new List<AudioSourceObserver>(18);
		for (int i = 0; i < 18; i++)
		{
			_listVoiceObserver.Add(new AudioSourceObserver(base.transform.FindChild("Voice").AddComponent<AudioSource>()));
			_listVoiceObserver[i].source.playOnAwake = false;
			_listVoiceObserver[i].source.loop = false;
		}
		_listSEObserver = new List<AudioSourceObserver>(18);
		for (int j = 0; j < 18; j++)
		{
			_listSEObserver.Add(new AudioSourceObserver(base.transform.FindChild("SE").AddComponent<AudioSource>()));
			_listSEObserver[j].source.playOnAwake = false;
			_listSEObserver[j].source.loop = false;
		}
		_clsVolume.Init();
	}

	public AudioSource PlayBGM(AudioClip clip)
	{
		return PlayBGM(clip, isLoop: true);
	}

	public AudioSource PlayBGM(BGMFileInfos file)
	{
		return PlayBGM(file, isLoop: true);
	}

	public AudioSource PlayBGM(BGMFileInfos file, bool isLoop)
	{
		return PlayBGM(SoundFile.LoadBGM(file), isLoop);
	}

	public AudioSource PlayBGM(AudioClip clip, bool isLoop)
	{
		if ((UnityEngine.Object)clip == null)
		{
			return _clsBGMObserver.source;
		}
		if ((UnityEngine.Object)_clsBGMObserver.source.clip == (UnityEngine.Object)clip && _clsBGMObserver.source.isPlaying)
		{
			return _clsBGMObserver.source;
		}
		_clsBGMObserver.source.loop = isLoop;
		_clsBGMObserver.Play(clip, soundVolume.BGM, isObserver: true, null);
		return _clsBGMObserver.source;
	}

	[Obsolete("KCV.Utils.SoundUtils::PlaySwotchBGM()を使用してください", false)]
	public AudioSource SwitchBGM(BGMFileInfos file)
	{
		return SwitchBGM(SoundFile.LoadBGM(file), isLoop: true);
	}

	private AudioSource SwitchBGM(AudioClip clip, bool isLoop)
	{
		if ((UnityEngine.Object)clip == null)
		{
			return _clsBGMObserver.source;
		}
		if ((UnityEngine.Object)_clsBGMObserver.source.clip != null && (UnityEngine.Object)clip == (UnityEngine.Object)_clsBGMObserver.source.clip && _clsBGMObserver.source.isPlaying)
		{
			return _clsBGMObserver.source;
		}
		_clsBGMObserver.StopFade(0.05f, delegate
		{
			PlayBGM(clip, isLoop);
		});
		return _clsBGMObserver.source;
	}

	public AudioSource StopBGM()
	{
		return _clsBGMObserver.Stop(isCallOnFinished: false).source;
	}

	public AudioSource StopFadeBGM(float duration, Action onFinished)
	{
		return _clsBGMObserver.StopFade(duration, delegate
		{
			Dlg.Call(ref onFinished);
		}).source;
	}

	public AudioSource PlaySE(SEFIleInfos file, Action onFinished)
	{
		return PlaySE(SoundFile.LoadSE(file), isOneShot: false, onFinished);
	}

	public AudioSource PlayOneShotSE(SEFIleInfos info)
	{
		return PlaySE(SoundFile.LoadSE(info), isOneShot: true, null);
	}

	public AudioSource PlaySE(AudioClip clip, bool isOneShot, Action onFinished)
	{
		foreach (AudioSourceObserver item in _listSEObserver)
		{
			if (!item.source.isPlaying)
			{
				return (!isOneShot) ? item.Play(clip, _clsVolume.SE, isOneShot, onFinished).source : item.PlayOneShot(clip, _clsVolume.SE).source;
			}
		}
		return null;
	}

	public AudioSource StopSE()
	{
		return _listSEObserver[0].Stop(isCallOnFinished: false).source;
	}

	public AudioSource StopSE(AudioSource source, bool isCallOnFinished, float fDuration)
	{
		if ((UnityEngine.Object)source == null)
		{
			return null;
		}
		foreach (AudioSourceObserver item in _listSEObserver)
		{
			if ((UnityEngine.Object)item.source == (UnityEngine.Object)source)
			{
				return item.Stop(isCallOnFinished, fDuration).source;
			}
		}
		return null;
	}

	public void StopSE(float duration)
	{
		_listSEObserver[0].Stop(isCallOnFinished: false, duration);
	}

	public void StopAllSE(float fDuration)
	{
		foreach (AudioSourceObserver item in _listSEObserver)
		{
			item.Stop(isCallOnFinished: false, fDuration);
		}
	}

	public AudioSource PlayVoice(AudioClip clip)
	{
		return PlayVoice(clip, 0);
	}

	public AudioSource PlayVoice(AudioClip clip, int index)
	{
		return PlayVoice(clip, index, isObserver: true, null);
	}

	public AudioSource PlayVoice(AudioClip clip, int nIndex, bool isObserver, Action onFinished)
	{
		return _listVoiceObserver[nIndex].Play(clip, _clsVolume.Voice, isObserver, onFinished).source;
	}

	public AudioSource PlayOneShotVoice(AudioClip clip)
	{
		return _listVoiceObserver[0].PlayOneShot(clip, _clsVolume.Voice).source;
	}

	public AudioSource PlayVoice(AudioClip clip, Action onFinished)
	{
		return PlayVoice(clip, 0, isObserver: true, onFinished);
	}

	public float VoiceLength(int nIndex)
	{
		return _listVoiceObserver[nIndex].clipLength;
	}

	public float VoiceLength(AudioClip clip)
	{
		return Utils.GetClipLength(clip);
	}

	public AudioSource StopVoice()
	{
		return StopVoice(0);
	}

	public AudioSource StopVoice(int index)
	{
		if (_listVoiceObserver != null && index < _listVoiceObserver.Count - 1 && _listVoiceObserver[index].source.isPlaying)
		{
			return _listVoiceObserver[index].Stop(isCallOnFinished: false).source;
		}
		return null;
	}

	public AudioSource StopVoice(AudioSource source, bool isCallOnFinished, float fDuration)
	{
		if ((UnityEngine.Object)source == null)
		{
			return null;
		}
		foreach (AudioSourceObserver item in _listVoiceObserver)
		{
			if ((UnityEngine.Object)item.source == (UnityEngine.Object)source)
			{
				return item.Stop(isCallOnFinished, fDuration).source;
			}
		}
		return null;
	}

	public bool isVoicePlaying(int index)
	{
		if (_listVoiceObserver[index] == null)
		{
			return false;
		}
		return _listVoiceObserver[index].source.isPlaying;
	}

	public void Release()
	{
		ReleaseBGM();
		ReleaseSE();
		ReleaseVoice();
	}

	public void ReleaseBGM()
	{
		_clsBGMObserver.Clear();
	}

	public void ReleaseSE()
	{
		_listSEObserver.ForEach(delegate(AudioSourceObserver x)
		{
			x.Clear();
		});
	}

	public void ReleaseVoice()
	{
		_listVoiceObserver.ForEach(delegate(AudioSourceObserver x)
		{
			x.Clear();
		});
	}

	public AudioSource SwitchBGM(AudioClip file)
	{
		return SwitchBGM(file, isLoop: true);
	}

	[Obsolete("KCV.Utils.SoundUtilsに追加して使用してください。", false)]
	public AudioSource GeneratePlayJukeAudioSource(BGMFileInfos file)
	{
		AudioClip clip = SoundFile.LoadBGM(file);
		_clsBGMObserver.source.clip = clip;
		return _clsBGMObserver.source;
	}
}
