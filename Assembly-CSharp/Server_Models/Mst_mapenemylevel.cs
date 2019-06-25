using Common.Enum;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_mapenemylevel : Model_Base
	{
		private int _enemy_list_id;

		private DifficultKind _difficulty;

		private int _turns;

		private int _choose_rate;

		private int _deck_id;

		private static string _tableName = "mst_mapenemylevel";

		public int Enemy_list_id
		{
			get
			{
				return _enemy_list_id;
			}
			private set
			{
				_enemy_list_id = value;
			}
		}

		public DifficultKind Difficulty
		{
			get
			{
				return _difficulty;
			}
			private set
			{
				_difficulty = value;
			}
		}

		public int Turns
		{
			get
			{
				return _turns;
			}
			private set
			{
				_turns = value;
			}
		}

		public int Choose_rate
		{
			get
			{
				return _choose_rate;
			}
			private set
			{
				_choose_rate = value;
			}
		}

		public int Deck_id
		{
			get
			{
				return _deck_id;
			}
			private set
			{
				_deck_id = value;
			}
		}

		public static string tableName => _tableName;

		protected override void setProperty(XElement element)
		{
			Enemy_list_id = int.Parse(element.Element("Enemy_list_id").Value);
			Difficulty = (DifficultKind)int.Parse(element.Element("Difficulty").Value);
			Turns = int.Parse(element.Element("Turns").Value);
			Enemy_list_id = int.Parse(element.Element("Enemy_list_id").Value);
			Choose_rate = int.Parse(element.Element("Choose_rate").Value);
			Deck_id = int.Parse(element.Element("Deck_id").Value);
		}
	}
}
