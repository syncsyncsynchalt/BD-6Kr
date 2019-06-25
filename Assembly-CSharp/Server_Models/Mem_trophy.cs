using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_trophy", Namespace = "")]
	public class Mem_trophy : Model_Base
	{
		[DataMember]
		private int _start_map_count;

		[DataMember]
		private int _win_S_count;

		[DataMember]
		private int _use_recovery_item_count;

		[DataMember]
		private int _revamp_count;

		public bool IsFleetLevelUp;

		private static string _tableName = "mem_trophy";

		public int Start_map_count
		{
			get
			{
				return _start_map_count;
			}
			set
			{
				_start_map_count = value;
			}
		}

		public int Win_S_count
		{
			get
			{
				return _win_S_count;
			}
			set
			{
				_win_S_count = value;
			}
		}

		public int Use_recovery_item_count
		{
			get
			{
				return _use_recovery_item_count;
			}
			set
			{
				_use_recovery_item_count = value;
			}
		}

		public int Revamp_count
		{
			get
			{
				return _revamp_count;
			}
			set
			{
				_revamp_count = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_trophy()
		{
			IsFleetLevelUp = false;
		}

		protected override void setProperty(XElement element)
		{
			_start_map_count = int.Parse(element.Element("_start_map_count").Value);
			_win_S_count = int.Parse(element.Element("_win_S_count").Value);
			_use_recovery_item_count = int.Parse(element.Element("_use_recovery_item_count").Value);
			_revamp_count = int.Parse(element.Element("_revamp_count").Value);
		}
	}
}
