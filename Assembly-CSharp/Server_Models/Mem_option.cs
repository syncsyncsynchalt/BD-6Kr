using System.Runtime.Serialization;
using System.Xml.Linq;
using UnityEngine;

namespace Server_Models
{
	[DataContract(Name = "mem_option", Namespace = "")]
	public class Mem_option : Model_Base
	{
		private int _volumeBGM;

		private int _volumeSE;

		private int _volumeVoice;

		private bool _guideDisplay;

		private static string _tableName = "mem_option";

		[DataMember]
		public int VolumeBGM
		{
			get
			{
				return _volumeBGM;
			}
			set
			{
				_volumeBGM = value;
			}
		}

		[DataMember]
		public int VolumeSE
		{
			get
			{
				return _volumeSE;
			}
			set
			{
				_volumeSE = value;
			}
		}

		[DataMember]
		public int VolumeVoice
		{
			get
			{
				return _volumeVoice;
			}
			set
			{
				_volumeVoice = value;
			}
		}

		[DataMember]
		public bool GuideDisplay
		{
			get
			{
				return _guideDisplay;
			}
			set
			{
				_guideDisplay = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_option()
		{
			VolumeBGM = PlayerPrefs.GetInt("VolumeBGM", 50);
			VolumeSE = PlayerPrefs.GetInt("VolumeSE", 50);
			VolumeVoice = PlayerPrefs.GetInt("VolumeVoice", 50);
			int @int = PlayerPrefs.GetInt("GuideDisplay", 1);
			GuideDisplay = ((@int == 1) ? true : false);
		}

		public bool UpdateSetting()
		{
			PlayerPrefs.SetInt("VolumeBGM", VolumeBGM);
			PlayerPrefs.SetInt("VolumeSE", VolumeSE);
			PlayerPrefs.SetInt("VolumeVoice", VolumeVoice);
			PlayerPrefs.SetInt("GuideDisplay", GuideDisplay ? 1 : 0);
			PlayerPrefs.Save();
			return true;
		}

		protected override void setProperty(XElement element)
		{
			VolumeBGM = int.Parse(element.Element("VolumeBGM").Value);
			VolumeSE = int.Parse(element.Element("VolumeSE").Value);
			VolumeVoice = int.Parse(element.Element("VolumeVoice").Value);
			GuideDisplay = bool.Parse(element.Element("GuideDisplay").Value);
		}
	}
}
