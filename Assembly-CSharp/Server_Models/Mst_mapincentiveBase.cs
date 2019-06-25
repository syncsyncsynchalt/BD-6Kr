using Server_Common.Formats;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_mapincentiveBase : Model_Base
	{
		protected int _id;

		protected int _incentive_no;

		protected double _choose_rate;

		protected MapItemGetFmt.enumCategory _getCategory;

		protected int _get_id;

		protected int _get_count;

		public int Id
		{
			get
			{
				return _id;
			}
			protected set
			{
				_id = value;
			}
		}

		public int Incentive_no
		{
			get
			{
				return _incentive_no;
			}
			protected set
			{
				_incentive_no = value;
			}
		}

		public double Choose_rate
		{
			get
			{
				return _choose_rate;
			}
			protected set
			{
				_choose_rate = value;
			}
		}

		public MapItemGetFmt.enumCategory GetCategory
		{
			get
			{
				return _getCategory;
			}
			protected set
			{
				_getCategory = value;
			}
		}

		public int Get_id
		{
			get
			{
				return _get_id;
			}
			protected set
			{
				_get_id = value;
			}
		}

		public int Get_count
		{
			get
			{
				return _get_count;
			}
			protected set
			{
				_get_count = value;
			}
		}

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			Incentive_no = int.Parse(element.Element("Incentive_no").Value);
			Choose_rate = double.Parse(element.Element("Choose_rate").Value);
			setIncentiveItem(element);
		}

		protected virtual void setIncentiveItem(XElement element)
		{
			if (element.Element("Ship_id") != null)
			{
				GetCategory = MapItemGetFmt.enumCategory.Ship;
				Get_id = int.Parse(element.Element("Ship_id").Value);
				Get_count = int.Parse(element.Element("Ship_count").Value);
			}
			else if (element.Element("Slotitem_id") != null)
			{
				GetCategory = MapItemGetFmt.enumCategory.Slotitem;
				Get_id = int.Parse(element.Element("Slotitem_id").Value);
				Get_count = int.Parse(element.Element("Slotitem_count").Value);
			}
			else if (element.Element("Useitem_id") != null)
			{
				GetCategory = MapItemGetFmt.enumCategory.UseItem;
				Get_id = int.Parse(element.Element("Useitem_id").Value);
				Get_count = int.Parse(element.Element("Useitem_count").Value);
			}
			else if (element.Element("Material_id") != null)
			{
				GetCategory = MapItemGetFmt.enumCategory.Material;
				Get_id = int.Parse(element.Element("Material_id").Value);
				Get_count = int.Parse(element.Element("Material_count").Value);
			}
			else if (element.Element("Furniture_id") != null)
			{
				GetCategory = MapItemGetFmt.enumCategory.Furniture;
				Get_id = int.Parse(element.Element("Furniture_id").Value);
				Get_count = int.Parse(element.Element("Furniture_count").Value);
			}
		}
	}
}
