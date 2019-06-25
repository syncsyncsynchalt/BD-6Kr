using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.PSVita;

[RequireComponent(typeof(Camera))]
public class PSVitaMovie : MonoBehaviour
{
	[SerializeField]
	private RenderTexture _renderTexture;

	private string _strMoviePath = string.Empty;

	private string _strSubTitleText = string.Empty;

	private long _lSubTitleTimeStamp;

	private bool _isPlaying;

	private bool _isBufferingSuccess;

	private bool _isPause;

	private int _iLooping;

	private int _iMode;

	private int _strPlayParams = 0;

	private Action _actOnStop;

	private Action _actOnReady;

	private Action _actOnPlay;

	private Action _actOnPause;

	private Action _actOnBuffering;

	private Action _actOnTimedTextDelivery;

	private Action _actOnWarningID;

	private Action _actOnEncryption;

	private Action _actOnFinished;

	public bool isPlaying => _isPlaying;

	public bool isPause
	{
		get
		{
			return _isPause;
		}
		private set
		{
			_isPause = value;
		}
	}

	public string moviePath
	{
		get
		{
			return _strMoviePath;
		}
		private set
		{
			_strMoviePath = value;
		}
	}

	public RenderTexture renderTexture
	{
		get
		{
			return _renderTexture;
		}
		set
		{
			if (_renderTexture != value)
			{
				_renderTexture = value;
			}
		}
	}

	public long currentTime => PSVitaVideoPlayer.videoTime;

	public long movieDuration => PSVitaVideoPlayer.videoDuration;

	public float volume
	{
		set
		{
			PSVitaVideoPlayer.SetVolume(value);
		}
	}

	public int trickSpeed
	{
		set
		{
            throw new NotImplementedException("‚È‚É‚±‚ê");
            //IL_0000: Unknown result type (might be due to invalid IL or missing references)
            // PSVitaVideoPlayer.SetTrickSpeed(value);
		}
	}

	private void OnPostRender()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Invalid comparison between Unknown and I4
		PSVitaVideoPlayer.Update();
		if (isPlaying)
		{
			if (!_isBufferingSuccess && currentTime != 0L)
			{
				OnEvent(5);
			}
			else if (_isBufferingSuccess && currentTime == 0L && movieDuration == 0L && (int)_iLooping != 1)
			{
				AutoOnFinished();
			}
		}
	}

	private void OnDestroy()
	{
		_renderTexture = null;
		_strMoviePath = null;
		_actOnStop = null;
		_actOnReady = null;
		_actOnPlay = null;
		_actOnPause = null;
		_actOnBuffering = null;
		_actOnTimedTextDelivery = null;
		_actOnWarningID = null;
		_actOnEncryption = null;
	}

	public PSVitaMovie Play(string moviePath)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		_isPlaying = false;
		_isBufferingSuccess = false;
		_isPause = false;
		this.moviePath = moviePath;
		if (renderTexture == null && (int)_iMode == 1)
		{
			renderTexture = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGB32);
		}
		PSVitaVideoPlayer.Init(renderTexture);
		Observable.FromCoroutine((UniRx.IObserver<bool> observer) => PlayMovie(observer)).Subscribe(delegate(bool observer)
		{
			if (observer)
			{
				OnEvent(3);
			}
			else
			{
				OnEvent(32);
			}
		});
		return this;
	}

	private IEnumerator PlayMovie(UniRx.IObserver<bool> observer)
	{
		int retryCnt = 0;
		while (true)
		{
            throw new NotImplementedException("‚È‚É‚±‚ê");
            //if (PSVitaVideoPlayer.Play(moviePath, _iLooping, _iMode))
            if (true)
            {
				observer.OnNext(value: true);
				observer.OnCompleted();
				yield break;
			}
			if (retryCnt >= 3)
			{
				break;
			}
			retryCnt++;
			yield return null;
		}
		observer.OnNext(value: false);
		observer.OnCompleted();
	}

	public PSVitaMovie PlayEx(string moviePath)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		this.moviePath = moviePath;
		PSVitaVideoPlayer.Init(renderTexture);

        throw new NotImplementedException("‚È‚É‚±‚ê");
        // if (PSVitaVideoPlayer.PlayEx(this.moviePath, _strPlayParams))
        if (true)
        {
            OnEvent(3);
		}
		else
		{
			OnEvent(32);
		}
		return this;
	}

	public void Stop()
	{
		OnEvent(1);
	}

	public bool Pause()
	{
		OnEvent(4);
		isPause = true;
		return PSVitaVideoPlayer.Pause();
	}

	public bool Resume()
	{
		isPause = false;
		return PSVitaVideoPlayer.Resume();
	}

	public void ImmediateOnFinished()
	{
		PSVitaVideoPlayer.Stop();
		AutoOnFinished();
	}

	private void AutoOnFinished()
	{
		_isPlaying = false;
		_isBufferingSuccess = false;
		if (_actOnFinished != null)
		{
			_actOnFinished();
		}
	}

	public bool JumpToTime(ulong jumpTimeMsec)
	{
		return PSVitaVideoPlayer.JumpToTime(jumpTimeMsec);
	}

	public PSVitaMovie SetPlayParams(int param)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_strPlayParams = param;
		return this;
	}

	public PSVitaMovie SetLooping(int iLooping)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_iLooping = iLooping;
		return this;
	}

    // public PSVitaMovie SetMode(Mode iMode)
    public PSVitaMovie SetMode(int iMode)
    {
        //IL_0001: Unknown result type (might be due to invalid IL or missing references)
        //IL_0002: Unknown result type (might be due to invalid IL or missing references)
        _iMode = iMode;
		return this;
	}

	public PSVitaMovie SetOnStop(Action onStop)
	{
		_actOnStop = onStop;
		return this;
	}

	public PSVitaMovie SetOnReady(Action onReady)
	{
		_actOnReady = onReady;
		return this;
	}

	public PSVitaMovie SetOnPlay(Action onPlay)
	{
		_actOnPlay = onPlay;
		return this;
	}

	public PSVitaMovie SetOnPause(Action onPause)
	{
		_actOnPause = onPause;
		return this;
	}

	public PSVitaMovie SetOnBuffering(Action onBuffering)
	{
		_actOnBuffering = onBuffering;
		return this;
	}

	public PSVitaMovie SetOnTimedTextDelivery(Action onTimedTextDelivery)
	{
		_actOnTimedTextDelivery = onTimedTextDelivery;
		return this;
	}

	public PSVitaMovie SetOnWarningID(Action onWarningID)
	{
		_actOnWarningID = onWarningID;
		return this;
	}

	public PSVitaMovie SetOnEncryption(Action onEncryption)
	{
		_actOnEncryption = onEncryption;
		return this;
	}

	public PSVitaMovie SetOnFinished(Action onFinished)
	{
		_actOnFinished = onFinished;
		return this;
	}

    //private void OnEvent(MovieEvent iEventID)
    private void OnEvent(int iEventID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected I4, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		switch (iEventID - 1)
		{
		case 0:
			PSVitaVideoPlayer.Stop();
			_isPlaying = false;
			if (_actOnStop != null)
			{
				_actOnStop();
			}
			_strSubTitleText = string.Empty;
			return;
		case 1:
			if (_actOnReady != null)
			{
				_actOnReady();
			}
			return;
		case 2:
			_isPlaying = true;
			if (_actOnPlay != null)
			{
				_actOnPlay();
			}
			return;
		case 3:
			if (_actOnPause != null)
			{
				_actOnPause();
			}
			return;
		case 4:
			_isBufferingSuccess = true;
			if (_actOnBuffering != null)
			{
				_actOnBuffering();
			}
			return;
		}
		if ((int)iEventID != 16)
		{
			if ((int)iEventID != 32)
			{
				if ((int)iEventID == 48 && _actOnEncryption != null)
				{
					_actOnEncryption();
				}
				return;
			}
			if (_actOnWarningID != null)
			{
				_actOnWarningID();
			}
			ImmediateOnFinished();
		}
		else
		{
			_strSubTitleText = PSVitaVideoPlayer.subtitleText;
			if (_actOnTimedTextDelivery != null)
			{
				_actOnTimedTextDelivery();
			}
		}
	}
}
