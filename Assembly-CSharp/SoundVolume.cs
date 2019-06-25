public class SoundVolume
{
	private float _fBGM = 1f;

	private float _fVoice = 1f;

	private float _fSE = 1f;

	private bool _isMute;

	public float BGM
	{
		get
		{
			return _fBGM;
		}
		set
		{
			_fBGM = Mathe.MinMax2F01(value);
		}
	}

	public float Voice
	{
		get
		{
			return _fVoice;
		}
		set
		{
			_fVoice = Mathe.MinMax2F01(value);
		}
	}

	public float SE
	{
		get
		{
			return _fSE;
		}
		set
		{
			_fSE = Mathe.MinMax2F01(value);
		}
	}

	public bool Mute
	{
		get
		{
			return _isMute;
		}
		set
		{
			_isMute = value;
		}
	}

	public void Init(float bgm = 0.6f, float voice = 1f, float se = 1f, bool mute = false)
	{
		_fBGM = Mathe.MinMax2F01(bgm);
		_fVoice = Mathe.MinMax2F01(voice);
		_fSE = Mathe.MinMax2F01(se);
		_isMute = mute;
	}
}
