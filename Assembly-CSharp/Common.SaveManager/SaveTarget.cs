using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.SaveManager
{
	public class SaveTarget
	{
		public Type ClassType;

		public object Data;

		public string TableName;

		public bool IsCollection;

		public SaveTarget(Type type, object data, string table_name)
		{
			ClassType = type;
			Data = data;
			TableName = table_name;
			IsCollection = false;
		}

		public SaveTarget(Type type, IList data, string table_name)
		{
			ClassType = type;
			Data = data;
			TableName = table_name;
			IsCollection = true;
		}

		public SaveTarget(IEnumerable<Mem_ship> ship_datas)
		{
			ClassType = typeof(List<Mem_shipBase>);
			List<Mem_shipBase> list = new List<Mem_shipBase>();
			foreach (Mem_ship ship_data in ship_datas)
			{
				Mem_shipBase item = new Mem_shipBase(ship_data);
				list.Add(item);
			}
			Data = list;
			TableName = Mem_ship.tableName;
			IsCollection = true;
		}
	}
}
