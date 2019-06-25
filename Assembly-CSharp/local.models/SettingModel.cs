using Server_Controllers;
using Server_Models;

namespace local.models
{
	public class SettingModel
	{
		private Mem_option _mem_option;

		public int VolumeBGM;

		public int VolumeSE;

		public int VolumeVoice;

		public bool GuideDisplay;

		public SettingModel()
		{
			_mem_option = new Api_get_Member().Option();
			VolumeBGM = _mem_option.VolumeBGM;
			VolumeSE = _mem_option.VolumeSE;
			VolumeVoice = _mem_option.VolumeVoice;
			GuideDisplay = _mem_option.GuideDisplay;
		}

		public bool IsChanged()
		{
			if (VolumeBGM != _mem_option.VolumeBGM)
			{
				return true;
			}
			if (VolumeSE != _mem_option.VolumeSE)
			{
				return true;
			}
			if (VolumeVoice != _mem_option.VolumeVoice)
			{
				return true;
			}
			if (GuideDisplay != _mem_option.GuideDisplay)
			{
				return true;
			}
			return false;
		}

		public bool Save()
		{
			if (IsChanged())
			{
				_mem_option.VolumeBGM = VolumeBGM;
				_mem_option.VolumeSE = VolumeSE;
				_mem_option.VolumeVoice = VolumeVoice;
				_mem_option.GuideDisplay = GuideDisplay;
				return _mem_option.UpdateSetting();
			}
			return false;
		}

		public override string ToString()
		{
			string str = "[設定]BGM/SE/ボイス:" + VolumeBGM + "/" + VolumeSE + "/" + VolumeVoice;
			return str + "\tガイド表示:" + ((!GuideDisplay) ? "OFF" : "ON");
		}
	}
}
