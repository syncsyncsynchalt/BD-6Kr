namespace KCV.Battle.Utils
{
	public struct SoundKeep
	{
		private float _bgmVol;

		private float _seVol;

		private float _voiceVol;

		public float BGMVolume
		{
			get
			{
				return _bgmVol;
			}
			set
			{
				_bgmVol = value;
			}
		}

		public float SeVolume
		{
			get
			{
				return _seVol;
			}
			set
			{
				_seVol = value;
			}
		}

		public float VoiceVolume
		{
			get
			{
				return _voiceVol;
			}
			set
			{
				_voiceVol = value;
			}
		}
	}
}
