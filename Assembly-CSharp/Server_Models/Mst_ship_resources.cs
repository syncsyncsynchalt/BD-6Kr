using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_ship_resources : Model_Base
	{
		private const int _voiceMaxNo = 29;

		private int _id;

		private int _standing_id;

		private int _voicef;

		private List<int> Motions;

		private Dictionary<int, int> _voiceId;

		private int _voice_practice_no;

		private static string _tableName = "mst_ship_resources";

		public static int VoiceMaxNo => 29;

		public int Id
		{
			get
			{
				return _id;
			}
			private set
			{
				_id = value;
			}
		}

		public int Standing_id
		{
			get
			{
				return (_standing_id != 0) ? _standing_id : _id;
			}
			private set
			{
				_standing_id = value;
			}
		}

		public int Voicef
		{
			get
			{
				return _voicef;
			}
			private set
			{
				_voicef = value;
			}
		}

		public int Motion1
		{
			get
			{
				if (Motions == null)
				{
					return 0;
				}
				return Motions[0];
			}
		}

		public int Motion2
		{
			get
			{
				if (Motions == null)
				{
					return 0;
				}
				return Motions[1];
			}
		}

		public int Motion3
		{
			get
			{
				if (Motions == null)
				{
					return 0;
				}
				return Motions[2];
			}
		}

		public int Motion4
		{
			get
			{
				if (Motions == null)
				{
					return 0;
				}
				return Motions[3];
			}
		}

		public static string tableName => _tableName;

		public Mst_ship_resources()
		{
			_voiceId = new Dictionary<int, int>();
		}

		public int GetVoiceId(int voiceNo)
		{
			int value = 0;
			_voiceId.TryGetValue(voiceNo, out value);
			return value;
		}

		public int GetDeckPracticeVoiceNo()
		{
			return _voice_practice_no;
		}

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			if ((Standing_id = int.Parse(element.Element("Standing_id").Value)) == 0)
			{
				return;
			}
			Voicef = int.Parse(element.Element("Voicef").Value);
			if (element.Element("Voiceitem") == null)
			{
				return;
			}
			string[] array = element.Element("Voiceitem").Value.Split(',');
			for (int i = 0; i < array.Length - 1; i++)
			{
				int num2 = int.Parse(array[i]);
				if (num2 != 0)
				{
					_voiceId.Add(i + 1, num2);
				}
			}
			_voice_practice_no = int.Parse(array[array.Length - 1]);
			string[] array2 = element.Element("Motion").Value.Split(',');
			Motions = Array.ConvertAll(array2, (string x) => int.Parse(x)).ToList();
		}

		public static List<int> GetRequireStandingIds(IEnumerable<Mst_ship_resources> resources)
		{
			return new List<int>();
		}

		public static List<int> GetRequireVoiceNo(int shipId)
		{
			return new List<int>();
		}
	}
}
