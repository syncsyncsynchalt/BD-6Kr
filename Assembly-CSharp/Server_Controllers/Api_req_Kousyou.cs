using Common.Enum;
using Server_Common;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers
{
	public class Api_req_Kousyou : Mem_slotitem.IMemSlotIdOperator
	{
		public readonly int Stratege_Min;

		public readonly int Stratege_Max;

		private Dictionary<int, Dictionary<enumMaterialCategory, int>> mat_min;

		private Dictionary<int, Dictionary<enumMaterialCategory, int>> mat_max;

		private IEnumerable<XElement> createTable;

		private IEnumerable<XElement> createLargeTable;

		private IEnumerable<XElement> createTable_change;

		private IEnumerable<XElement> createLargeTable_change;

		private IEnumerable<XElement> createItemTable;

		private IEnumerable<XElement> createItemGroup;

		private Dictionary<int, List<Mst_slotitem_remodel>> mst_remodel;

		private Dictionary<int, Mst_slotitem_remodel> mst_remodel_list;

		private Dictionary<int, List<Mst_slotitem_remodel_detail>> mst_remodel_detail;

		private readonly int[] requireHighSpeedNum;

		private bool questDestroyItemDisable;

		public Api_req_Kousyou()
		{
			requireHighSpeedNum = new int[3]
			{
				1,
				10,
				1
			};
			makeRequireMaterialNum();
			Stratege_Min = 50;
			Stratege_Max = 400;
			mst_remodel = null;
			mst_remodel_list = null;
			mst_remodel_detail = null;
		}

		public Dictionary<enumMaterialCategory, int> GetRequireMaterials_Min(int type)
		{
			return mat_min[type];
		}

		public Dictionary<enumMaterialCategory, int> GetRequireMaterials_Max(int type)
		{
			return mat_max[type];
		}

		public int GetRequireHighSpeedNum(int type)
		{
			return requireHighSpeedNum[type];
		}

		public bool ValidStart(int rid, bool highSpeed, bool largeDock, ref Dictionary<enumMaterialCategory, int> materials, int deck_rid)
		{
			Mem_kdock value = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, out value))
			{
				return false;
			}
			if (value.State != KdockStates.EMPTY)
			{
				return false;
			}
			if (Comm_UserDatas.Instance.User_basic.IsMaxChara() || Comm_UserDatas.Instance.User_basic.IsMaxSlotitem())
			{
				return false;
			}
			if (largeDock && Comm_UserDatas.Instance.User_basic.Large_dock <= 0)
			{
				return false;
			}
			if (!materials.Keys.Contains(enumMaterialCategory.Fuel) || !materials.Keys.Contains(enumMaterialCategory.Bull) || !materials.Keys.Contains(enumMaterialCategory.Steel) || !materials.Keys.Contains(enumMaterialCategory.Bauxite) || !materials.Keys.Contains(enumMaterialCategory.Dev_Kit))
			{
				return false;
			}
			int value2 = 0;
			int key = largeDock ? 1 : 0;
			if (highSpeed)
			{
				value2 = mat_max[key][enumMaterialCategory.Build_Kit];
			}
			materials.Add(enumMaterialCategory.Build_Kit, value2);
			foreach (KeyValuePair<enumMaterialCategory, int> material in materials)
			{
				if (material.Value > Comm_UserDatas.Instance.User_material[material.Key].Value)
				{
					return false;
				}
				if (material.Value < mat_min[key][material.Key])
				{
					return false;
				}
				if (material.Value > mat_max[key][material.Key])
				{
					return false;
				}
			}
			Mem_deck value3 = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, out value3))
			{
				return false;
			}
			if (!Comm_UserDatas.Instance.User_ship.ContainsKey(value3.Ship[0]))
			{
				return false;
			}
			return true;
		}

		public bool ValidStartTanker(int rid, bool highSpeed, ref Dictionary<enumMaterialCategory, int> materials, int stratege_point)
		{
			Mem_kdock value = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, out value))
			{
				return false;
			}
			if (value.State != KdockStates.EMPTY)
			{
				return false;
			}
			if (!materials.Keys.Contains(enumMaterialCategory.Fuel) || !materials.Keys.Contains(enumMaterialCategory.Bull) || !materials.Keys.Contains(enumMaterialCategory.Steel) || !materials.Keys.Contains(enumMaterialCategory.Bauxite))
			{
				return false;
			}
			Dictionary<enumMaterialCategory, int> dictionary = mat_min[2];
			Dictionary<enumMaterialCategory, int> dictionary2 = mat_max[2];
			int value2 = 0;
			if (highSpeed)
			{
				value2 = dictionary2[enumMaterialCategory.Build_Kit];
			}
			materials.Add(enumMaterialCategory.Build_Kit, value2);
			foreach (KeyValuePair<enumMaterialCategory, int> material in materials)
			{
				if (material.Value > Comm_UserDatas.Instance.User_material[material.Key].Value)
				{
					return false;
				}
				if (material.Value < dictionary[material.Key])
				{
					return false;
				}
				if (material.Value > dictionary2[material.Key])
				{
					return false;
				}
			}
			if (stratege_point > Comm_UserDatas.Instance.User_basic.Strategy_point)
			{
				return false;
			}
			if (stratege_point < Stratege_Min || stratege_point > Stratege_Max)
			{
				return false;
			}
			return true;
		}

		public bool ValidSpeedChange(int rid)
		{
			Mem_kdock value = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, out value))
			{
				return false;
			}
			if (value.State != KdockStates.CREATE)
			{
				return false;
			}
			int num = value.IsLargeDock() ? requireHighSpeedNum[1] : ((!value.IsTunkerDock()) ? requireHighSpeedNum[0] : requireHighSpeedNum[2]);
			if (Comm_UserDatas.Instance.User_material[enumMaterialCategory.Build_Kit].Value < num)
			{
				return false;
			}
			return true;
		}

		public bool ValidSpeedChangeTanker(int rid)
		{
			return ValidSpeedChange(rid);
		}

		public bool ValidGetShip(int rid)
		{
			Mem_kdock value = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, out value))
			{
				return false;
			}
			if (value.State != KdockStates.COMPLETE)
			{
				return false;
			}
			if (Comm_UserDatas.Instance.User_basic.IsMaxChara() || Comm_UserDatas.Instance.User_basic.IsMaxSlotitem())
			{
				return false;
			}
			return true;
		}

		public bool ValidGetTanker(int rid)
		{
			Mem_kdock value = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, out value))
			{
				return false;
			}
			if (value.State != KdockStates.COMPLETE)
			{
				return false;
			}
			return true;
		}

		public Api_Result<Mem_kdock> Start(int rid, bool highSpeed, bool largeDock, Dictionary<enumMaterialCategory, int> materials, int deck_rid)
		{
			Api_Result<Mem_kdock> api_Result = new Api_Result<Mem_kdock>();
			Mem_kdock value = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			int create_ship = 0;
			Func<Dictionary<enumMaterialCategory, int>, int, int> func = (!largeDock) ? new Func<Dictionary<enumMaterialCategory, int>, int, int>(getShipId) : new Func<Dictionary<enumMaterialCategory, int>, int, int>(getShipIdLarge);
			create_ship = func(materials, deck_rid);
			if (Comm_UserDatas.Instance.User_ship.Values.Any((Mem_ship x) => x.Ship_id == create_ship))
			{
				create_ship = func(materials, deck_rid);
			}
			Mst_ship value2 = null;
			if (!Mst_DataManager.Instance.Mst_ship.TryGetValue(create_ship, out value2))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			TimeSpan span = new TimeSpan(0, value2.Buildtime, 0);
			value.CreateStart(create_ship, materials, span);
			if (materials[enumMaterialCategory.Build_Kit] != 0)
			{
				value.CreateEnd(timeChk: false);
			}
			api_Result.data = value;
			QuestKousyou questKousyou = new QuestKousyou(materials, value2.Id);
			questKousyou.ExecuteCheck();
			return api_Result;
		}

		public Api_Result<Mem_kdock> StartTanker(int rid, bool highSpeed, Dictionary<enumMaterialCategory, int> materials, int strategy_point)
		{
			Api_Result<Mem_kdock> api_Result = new Api_Result<Mem_kdock>();
			Mem_kdock value = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			int req_turn = 0;
			int createTankerNum = getCreateTankerNum(materials, strategy_point, ref req_turn);
			value.CreateTunker(createTankerNum, materials, strategy_point, req_turn);
			if (materials[enumMaterialCategory.Build_Kit] != 0)
			{
				value.CreateEnd(timeChk: false);
			}
			api_Result.data = value;
			return api_Result;
		}

		public Api_Result<Mem_kdock> SpeedChange(int rid)
		{
			Api_Result<Mem_kdock> api_Result = new Api_Result<Mem_kdock>();
			if (!ValidSpeedChange(rid))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_kdock value = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			int num = value.IsLargeDock() ? requireHighSpeedNum[1] : ((!value.IsTunkerDock()) ? requireHighSpeedNum[0] : requireHighSpeedNum[2]);
			if (value.CreateEnd(timeChk: false))
			{
				Comm_UserDatas.Instance.User_material[enumMaterialCategory.Build_Kit].Sub_Material(num);
				api_Result.data = value;
				return api_Result;
			}
			api_Result.state = Api_Result_State.Parameter_Error;
			return api_Result;
		}

		public Api_Result<Mem_kdock> SpeedChangeTanker(int rid)
		{
			return SpeedChange(rid);
		}

		public Api_Result<Mem_kdock> GetShip(int rid)
		{
			Api_Result<Mem_kdock> api_Result = new Api_Result<Mem_kdock>();
			Mem_kdock value = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (!ValidGetShip(rid))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (!value.GetShip())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			api_Result.data = value;
			return api_Result;
		}

		public Api_Result<Mem_kdock> GetTanker(int rid)
		{
			Api_Result<Mem_kdock> api_Result = new Api_Result<Mem_kdock>();
			Mem_kdock value = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (!ValidGetTanker(rid))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (!value.GetTunker())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			api_Result.data = value;
			return api_Result;
		}

		private void makeRequireMaterialNum()
		{
			mat_min = new Dictionary<int, Dictionary<enumMaterialCategory, int>>();
			mat_max = new Dictionary<int, Dictionary<enumMaterialCategory, int>>();
			mat_min.Add(0, new Dictionary<enumMaterialCategory, int>
			{
				{
					enumMaterialCategory.Fuel,
					30
				},
				{
					enumMaterialCategory.Bull,
					30
				},
				{
					enumMaterialCategory.Steel,
					30
				},
				{
					enumMaterialCategory.Bauxite,
					30
				},
				{
					enumMaterialCategory.Dev_Kit,
					1
				},
				{
					enumMaterialCategory.Build_Kit,
					0
				}
			});
			mat_max.Add(0, new Dictionary<enumMaterialCategory, int>
			{
				{
					enumMaterialCategory.Fuel,
					999
				},
				{
					enumMaterialCategory.Bull,
					999
				},
				{
					enumMaterialCategory.Steel,
					999
				},
				{
					enumMaterialCategory.Bauxite,
					999
				},
				{
					enumMaterialCategory.Dev_Kit,
					1
				},
				{
					enumMaterialCategory.Build_Kit,
					requireHighSpeedNum[0]
				}
			});
			mat_min.Add(1, new Dictionary<enumMaterialCategory, int>
			{
				{
					enumMaterialCategory.Fuel,
					1500
				},
				{
					enumMaterialCategory.Bull,
					1500
				},
				{
					enumMaterialCategory.Steel,
					2000
				},
				{
					enumMaterialCategory.Bauxite,
					1000
				},
				{
					enumMaterialCategory.Dev_Kit,
					1
				},
				{
					enumMaterialCategory.Build_Kit,
					0
				}
			});
			mat_max.Add(1, new Dictionary<enumMaterialCategory, int>
			{
				{
					enumMaterialCategory.Fuel,
					7000
				},
				{
					enumMaterialCategory.Bull,
					7000
				},
				{
					enumMaterialCategory.Steel,
					7000
				},
				{
					enumMaterialCategory.Bauxite,
					7000
				},
				{
					enumMaterialCategory.Dev_Kit,
					100
				},
				{
					enumMaterialCategory.Build_Kit,
					requireHighSpeedNum[1]
				}
			});
			mat_min.Add(2, new Dictionary<enumMaterialCategory, int>
			{
				{
					enumMaterialCategory.Fuel,
					40
				},
				{
					enumMaterialCategory.Bull,
					10
				},
				{
					enumMaterialCategory.Steel,
					40
				},
				{
					enumMaterialCategory.Bauxite,
					10
				},
				{
					enumMaterialCategory.Build_Kit,
					0
				}
			});
			mat_max.Add(2, new Dictionary<enumMaterialCategory, int>
			{
				{
					enumMaterialCategory.Fuel,
					999
				},
				{
					enumMaterialCategory.Bull,
					999
				},
				{
					enumMaterialCategory.Steel,
					999
				},
				{
					enumMaterialCategory.Bauxite,
					999
				},
				{
					enumMaterialCategory.Build_Kit,
					requireHighSpeedNum[2]
				}
			});
		}

		private int getShipId(Dictionary<enumMaterialCategory, int> material, int deck_rid)
		{
			int group_id = 0;
			if (material[enumMaterialCategory.Steel] >= 400 && material[enumMaterialCategory.Fuel] >= 300 && material[enumMaterialCategory.Bauxite] >= 300)
			{
				group_id = 1;
			}
			else if (material[enumMaterialCategory.Steel] >= 600 && material[enumMaterialCategory.Fuel] >= 400 && material[enumMaterialCategory.Bauxite] >= 0)
			{
				group_id = 2;
			}
			else if (material[enumMaterialCategory.Steel] >= 200 && material[enumMaterialCategory.Fuel] >= 250 && material[enumMaterialCategory.Bauxite] >= 0)
			{
				group_id = 3;
			}
			else
			{
				if (material[enumMaterialCategory.Steel] < 30 || material[enumMaterialCategory.Fuel] < 30 || material[enumMaterialCategory.Bauxite] < 0)
				{
					return 0;
				}
				group_id = 4;
			}
			Random random = new Random();
			int num = random.Next(100) + 1;
			int value = 0;
			double num2 = material[enumMaterialCategory.Bauxite];
			double num3 = material[enumMaterialCategory.Steel];
			double num4 = material[enumMaterialCategory.Fuel];
			double num5 = material[enumMaterialCategory.Bull];
			if (group_id == 1)
			{
				value = (int)(Math.Ceiling((num2 - 300.0) / 20.0) + Math.Ceiling((num3 - 400.0) / 25.0));
			}
			else if (group_id == 2)
			{
				value = (int)(Math.Ceiling((num5 - 400.0) / 25.0) + Math.Ceiling((num3 - 600.0) / 30.0));
			}
			else if (group_id == 3)
			{
				value = (int)(Math.Ceiling((num5 - 200.0) / 13.0) + Math.Ceiling((num3 - 200.0) / 20.0));
			}
			else if (group_id == 4)
			{
				value = (int)(Math.Ceiling((num4 - 100.0) / 10.0) + Math.Ceiling((num3 - 30.0) / 15.0));
			}
			value = Math.Abs(value);
			if (value > 51)
			{
				value = 51;
			}
			value = random.Next(value);
			int num6 = num - value;
			if (num6 < 1)
			{
				num6 = 2 - num6;
			}
			if (createTable == null)
			{
				createTable = Utils.Xml_Result("mst_createship", "mst_createship", "Id");
			}
			var anon = (from data in createTable
				where data.Element("Group_id").Value == group_id.ToString()
				select new
				{
					ship_id = int.Parse(data.Element("Ship_id").Value),
					change_flag = int.Parse(data.Element("Change_flag").Value)
				}).Skip(num6).First();
			int result = anon.ship_id;
			if (anon.change_flag == 1)
			{
				int changeCreateShipId = getChangeCreateShipId(largeDock: false, anon.ship_id, deck_rid);
				if (changeCreateShipId != -1)
				{
					result = changeCreateShipId;
				}
			}
			return result;
		}

		private int getShipIdLarge(Dictionary<enumMaterialCategory, int> material, int deck_rid)
		{
			Random random = new Random();
			enumMaterialCategory key = enumMaterialCategory.Steel;
			enumMaterialCategory key2 = enumMaterialCategory.Fuel;
			enumMaterialCategory key3 = enumMaterialCategory.Bull;
			enumMaterialCategory key4 = enumMaterialCategory.Bauxite;
			enumMaterialCategory key5 = enumMaterialCategory.Dev_Kit;
			int group_id = 0;
			if (material[key] >= 2800 + random.Next(1400) && material[key2] >= 2400 + random.Next(1200) && material[key3] >= 1050 + random.Next(900) && material[key4] >= 2800 + random.Next(2400) && material[key5] >= 1 + random.Next(0))
			{
				group_id = 1;
			}
			else if (material[key] >= 4400 + random.Next(2200) && material[key2] >= 2240 + random.Next(1120) && material[key3] >= 2940 + random.Next(2520) && material[key4] >= 1050 + random.Next(900) && material[key5] >= 20 + random.Next(0))
			{
				group_id = 2;
			}
			else if (material[key] >= 3040 + random.Next(1520) && material[key2] >= 1920 + random.Next(960) && material[key3] >= 2240 + random.Next(1920) && material[key4] >= 910 + random.Next(780) && material[key5] >= 1 + random.Next(0))
			{
				group_id = 3;
			}
			else
			{
				group_id = 4;
			}
			HashSet<KdockStates> enableState = new HashSet<KdockStates>
			{
				KdockStates.EMPTY,
				KdockStates.COMPLETE
			};
			int num = Comm_UserDatas.Instance.User_kdock.Values.Count((Mem_kdock dockItem) => enableState.Contains(dockItem.State));
			int key6 = num - 1;
			Dictionary<int, int[]> dictionary = new Dictionary<int, int[]>();
			dictionary.Add(0, new int[2]
			{
				3,
				100
			});
			dictionary.Add(1, new int[2]
			{
				1,
				100
			});
			dictionary.Add(2, new int[2]
			{
				1,
				96
			});
			dictionary.Add(3, new int[2]
			{
				1,
				92
			});
			Dictionary<int, int[]> dictionary2 = dictionary;
			int num2 = random.Next(dictionary2[key6][0], dictionary2[key6][1]);
			Dictionary<enumMaterialCategory, double[]> dictionary3 = new Dictionary<enumMaterialCategory, double[]>();
			if (group_id == 1)
			{
				Dictionary<enumMaterialCategory, double[]> dictionary4 = new Dictionary<enumMaterialCategory, double[]>();
				dictionary4.Add(key, new double[2]
				{
					4000.0,
					0.004
				});
				dictionary4.Add(key2, new double[2]
				{
					3000.0,
					0.003
				});
				dictionary4.Add(key3, new double[2]
				{
					2000.0,
					0.003
				});
				dictionary4.Add(key4, new double[2]
				{
					5000.0,
					0.005
				});
				dictionary4.Add(key5, new double[2]
				{
					50.0,
					0.1
				});
				dictionary3 = dictionary4;
			}
			else if (group_id == 2)
			{
				Dictionary<enumMaterialCategory, double[]> dictionary4 = new Dictionary<enumMaterialCategory, double[]>();
				dictionary4.Add(key, new double[2]
				{
					5500.0,
					0.004
				});
				dictionary4.Add(key2, new double[2]
				{
					3500.0,
					0.003
				});
				dictionary4.Add(key3, new double[2]
				{
					4500.0,
					0.005
				});
				dictionary4.Add(key4, new double[2]
				{
					2200.0,
					0.002
				});
				dictionary4.Add(key5, new double[2]
				{
					60.0,
					0.2
				});
				dictionary3 = dictionary4;
			}
			else if (group_id == 3)
			{
				Dictionary<enumMaterialCategory, double[]> dictionary4 = new Dictionary<enumMaterialCategory, double[]>();
				dictionary4.Add(key, new double[2]
				{
					4000.0,
					0.003
				});
				dictionary4.Add(key2, new double[2]
				{
					2500.0,
					0.002
				});
				dictionary4.Add(key3, new double[2]
				{
					3000.0,
					0.003
				});
				dictionary4.Add(key4, new double[2]
				{
					1800.0,
					0.002
				});
				dictionary4.Add(key5, new double[2]
				{
					40.0,
					0.2
				});
				dictionary3 = dictionary4;
			}
			else if (group_id == 4)
			{
				Dictionary<enumMaterialCategory, double[]> dictionary4 = new Dictionary<enumMaterialCategory, double[]>();
				dictionary4.Add(key, new double[2]
				{
					3000.0,
					0.002
				});
				dictionary4.Add(key2, new double[2]
				{
					2000.0,
					0.002
				});
				dictionary4.Add(key3, new double[2]
				{
					2500.0,
					0.002
				});
				dictionary4.Add(key4, new double[2]
				{
					1500.0,
					0.002
				});
				dictionary4.Add(key5, new double[2]
				{
					40.0,
					0.2
				});
				dictionary3 = dictionary4;
			}
			int num3 = (int)(((double)material[key] - dictionary3[key][0]) * dictionary3[key][1]);
			int num4 = (int)(((double)material[key2] - dictionary3[key2][0]) * dictionary3[key2][1]);
			int num5 = (int)(((double)material[key3] - dictionary3[key3][0]) * dictionary3[key3][1]);
			int num6 = (int)(((double)material[key4] - dictionary3[key4][0]) * dictionary3[key4][1]);
			int num7 = (int)(((double)material[key5] - dictionary3[key5][0]) * dictionary3[key5][1]);
			int num8 = num3 + num4 + num5 + num6 + num7;
			int num9 = (num8 >= 0) ? random.Next(num8) : 0;
			if (num9 > 50)
			{
				num9 = 50;
			}
			int num10 = num2 - num9;
			if (num10 < 1)
			{
				num10 = 2 - num10;
			}
			if (createLargeTable == null)
			{
				createLargeTable = Utils.Xml_Result("mst_createship_large", "mst_createship_large", "Id");
			}
			var anon = (from data in createLargeTable
				where data.Element("Group_id").Value == group_id.ToString()
				select new
				{
					id = int.Parse(data.Element("Id").Value),
					ship_id = int.Parse(data.Element("Ship_id").Value),
					change_flag = int.Parse(data.Element("Change_flag").Value)
				}).Skip(num10).First();
			int result = anon.ship_id;
			if (anon.change_flag == 1)
			{
				int changeCreateShipId = getChangeCreateShipId(largeDock: true, anon.id, deck_rid);
				if (changeCreateShipId != -1)
				{
					result = changeCreateShipId;
				}
			}
			return result;
		}

		private int getChangeCreateShipId(bool largeDock, int create_id, int deck_rid)
		{
			Mem_deck value = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, out value))
			{
				return -1;
			}
			List<Mem_ship> memShip = value.Ship.getMemShip();
			if (memShip.Count == 0)
			{
				return -1;
			}
			int flag_shipid = memShip[0].Ship_id;
			IEnumerable<XElement> source;
			if (largeDock)
			{
				if (createLargeTable_change == null)
				{
					createLargeTable_change = Utils.Xml_Result("mst_createship_large_change", "mst_createship_large_change", "Id");
				}
				source = createLargeTable_change;
			}
			else
			{
				if (createTable_change == null)
				{
					createTable_change = Utils.Xml_Result("mst_createship_change", "mst_createship_change", "Id");
				}
				source = createTable_change;
			}
			var source2 = from data in source
				where data.Element("Flag_ship_id").Value == flag_shipid.ToString()
				where data.Element("Ship_create_id").Value == create_id.ToString()
				select new
				{
					changed_ship_id = int.Parse(data.Element("Changed_ship_id").Value)
				};
			if (!source2.Any())
			{
				return -1;
			}
			return source2.First().changed_ship_id;
		}

		private int getCreateTankerNum(Dictionary<enumMaterialCategory, int> use_mat, int use_point, ref int req_turn)
		{
			int num = 10;
			Random random = new Random();
			Dictionary<enumMaterialCategory, double> dictionary = ((IEnumerable<KeyValuePair<enumMaterialCategory, int>>)use_mat).ToDictionary((Func<KeyValuePair<enumMaterialCategory, int>, enumMaterialCategory>)((KeyValuePair<enumMaterialCategory, int> key) => key.Key), (Func<KeyValuePair<enumMaterialCategory, int>, double>)((KeyValuePair<enumMaterialCategory, int> val) => val.Value));
			double num2 = Math.Sqrt(dictionary[enumMaterialCategory.Steel]);
			double num3 = Math.Sqrt(dictionary[enumMaterialCategory.Fuel] / 2.0);
			double num4 = Math.Sqrt(dictionary[enumMaterialCategory.Bull] / 40.0 + dictionary[enumMaterialCategory.Bauxite] / 10.0);
			double num5 = num2 + num3 + num4;
			double num6 = 0.75;
			switch (use_point)
			{
			case 100:
				num6 = 1.0;
				break;
			case 200:
				num6 = 1.3;
				break;
			}
			double num7 = random.Next(5);
			double num8 = num5 / 10.0 + num5 / 50.0 * num7 * num6 + 0.3;
			if (num3 < num2 / 2.0)
			{
				num8 *= 0.65;
			}
			int num9 = (int)num8;
			if (num9 >= 5 && use_point < 200)
			{
				num9 = 5;
			}
			if (num9 >= 7 && use_point < 400)
			{
				num9 = 7;
			}
			if (num9 > num)
			{
				num9 = num;
			}
			else if (num9 < 1)
			{
				num9 = 1;
			}
			if (num9 > 7)
			{
				req_turn = 3;
			}
			else if (num9 > 2)
			{
				req_turn = 2;
			}
			else
			{
				req_turn = 1;
			}
			return num9;
		}

		public Api_Result<object> DestroyShip(int ship_rid)
		{
			Api_Result<object> api_Result = new Api_Result<object>();
			Mem_ship value = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (value.Locked == 1 || value.IsBlingShip() || value.GetLockSlotItems().Count > 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (value.Rid == Comm_UserDatas.Instance.User_deck[1].Ship[0])
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			List<int> slot_rids = new List<int>();
			value.Slot.ForEach(delegate(int x)
			{
				if (x > 0)
				{
					slot_rids.Add(x);
				}
			});
			if (value.Exslot > 0)
			{
				slot_rids.Add(value.Exslot);
			}
			questDestroyItemDisable = true;
			DestroyItem(slot_rids);
			questDestroyItemDisable = false;
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[value.Ship_id];
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Fuel].Add_Material(mst_ship.Broken1);
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Bull].Add_Material(mst_ship.Broken2);
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Steel].Add_Material(mst_ship.Broken3);
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Bauxite].Add_Material(mst_ship.Broken4);
			int[] array = Comm_UserDatas.Instance.User_deck[1].Search_ShipIdx(ship_rid);
			if (array[0] != -1)
			{
				Mem_deck mem_deck = Comm_UserDatas.Instance.User_deck[array[0]];
				mem_deck.Ship.RemoveShip(ship_rid);
			}
			Comm_UserDatas.Instance.User_ship.Remove(ship_rid);
			QuestKousyou questKousyou = new QuestKousyou(mst_ship);
			questKousyou.ExecuteCheck();
			api_Result.data = null;
			return api_Result;
		}

		public Api_Result<object> DestroyItem(List<int> slot_rids)
		{
			Api_Result<object> api_Result = new Api_Result<object>();
			if (slot_rids == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.Add(enumMaterialCategory.Fuel, 0);
			dictionary.Add(enumMaterialCategory.Bull, 0);
			dictionary.Add(enumMaterialCategory.Steel, 0);
			dictionary.Add(enumMaterialCategory.Bauxite, 0);
			Dictionary<enumMaterialCategory, int> dictionary2 = dictionary;
			List<Mst_slotitem> list = new List<Mst_slotitem>();
			foreach (int slot_rid in slot_rids)
			{
				Mem_slotitem value = null;
				if (Comm_UserDatas.Instance.User_slot.TryGetValue(slot_rid, out value))
				{
					if (value.Lock)
					{
						api_Result.state = Api_Result_State.Parameter_Error;
						return api_Result;
					}
					Mst_slotitem mst_slotitem = Mst_DataManager.Instance.Mst_Slotitem[value.Slotitem_id];
					Dictionary<enumMaterialCategory, int> dictionary3 = dictionary = dictionary2;
					enumMaterialCategory key;
					enumMaterialCategory key2 = key = enumMaterialCategory.Fuel;
					int num = dictionary[key];
					dictionary3[key2] = num + mst_slotitem.Broken1;
					Dictionary<enumMaterialCategory, int> dictionary4;
					Dictionary<enumMaterialCategory, int> dictionary5 = dictionary4 = dictionary2;
					enumMaterialCategory key3 = key = enumMaterialCategory.Bull;
					num = dictionary4[key];
					dictionary5[key3] = num + mst_slotitem.Broken2;
					Dictionary<enumMaterialCategory, int> dictionary6;
					Dictionary<enumMaterialCategory, int> dictionary7 = dictionary6 = dictionary2;
					enumMaterialCategory key4 = key = enumMaterialCategory.Steel;
					num = dictionary6[key];
					dictionary7[key4] = num + mst_slotitem.Broken3;
					Dictionary<enumMaterialCategory, int> dictionary8;
					Dictionary<enumMaterialCategory, int> dictionary9 = dictionary8 = dictionary2;
					enumMaterialCategory key5 = key = enumMaterialCategory.Bauxite;
					num = dictionary8[key];
					dictionary9[key5] = num + mst_slotitem.Broken4;
					list.Add(mst_slotitem);
				}
			}
			Comm_UserDatas.Instance.Remove_Slot(slot_rids);
			foreach (KeyValuePair<enumMaterialCategory, int> item in dictionary2)
			{
				Comm_UserDatas.Instance.User_material[item.Key].Add_Material(item.Value);
			}
			if (!questDestroyItemDisable)
			{
				QuestKousyou questKousyou = new QuestKousyou(list);
				questKousyou.ExecuteCheck();
			}
			api_Result.data = null;
			return api_Result;
		}

		public Api_Result<Mst_slotitem> CreateItem(Dictionary<enumMaterialCategory, int> items, int deck_rid)
		{
			Api_Result<Mst_slotitem> api_Result = new Api_Result<Mst_slotitem>();
			if (!validCreateItem(items, deck_rid))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (createItemGroup == null)
			{
				createItemGroup = Utils.Xml_Result("mst_stype_group", "mst_stype_group", "Id");
			}
			int createItemType = getCreateItemType(deck_rid);
			if (createItemTable == null)
			{
				createItemTable = Utils.Xml_Result("mst_slotitemget2", "mst_slotitemget2", "Id");
			}
			int createItem = getCreateItem(3, createItemType, items);
			Mst_slotitem value = null;
			Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(createItem, out value);
			bool flag = true;
			if (!isCreateItemRareCheck(value) || !isCreateItemMatCheck(items, value))
			{
				flag = false;
			}
			foreach (KeyValuePair<enumMaterialCategory, int> item in items)
			{
				Comm_UserDatas.Instance.User_material[item.Key].Sub_Material(item.Value);
			}
			if (flag)
			{
				Comm_UserDatas.Instance.User_material[enumMaterialCategory.Dev_Kit].Sub_Material(1);
				Comm_UserDatas.Instance.Add_Slot(new List<int>
				{
					createItem
				});
				api_Result.data = value;
			}
			QuestKousyou questKousyou = new QuestKousyou(items, flag);
			questKousyou.ExecuteCheck();
			return api_Result;
		}

		private bool validCreateItem(Dictionary<enumMaterialCategory, int> items, int deck_rid)
		{
			if (items.Count() != 4)
			{
				return false;
			}
			Dictionary<enumMaterialCategory, int[]> dictionary = new Dictionary<enumMaterialCategory, int[]>();
			dictionary.Add(enumMaterialCategory.Fuel, new int[2]
			{
				10,
				300
			});
			dictionary.Add(enumMaterialCategory.Bull, new int[2]
			{
				10,
				300
			});
			dictionary.Add(enumMaterialCategory.Steel, new int[2]
			{
				10,
				300
			});
			dictionary.Add(enumMaterialCategory.Bauxite, new int[2]
			{
				10,
				300
			});
			Dictionary<enumMaterialCategory, int[]> dictionary2 = dictionary;
			foreach (KeyValuePair<enumMaterialCategory, int[]> item in dictionary2)
			{
				if (!items.ContainsKey(item.Key))
				{
					return false;
				}
				if (items[item.Key] < item.Value[0] || items[item.Key] > item.Value[1])
				{
					return false;
				}
				if (items[item.Key] > Comm_UserDatas.Instance.User_material[item.Key].Value)
				{
					return false;
				}
			}
			if (Comm_UserDatas.Instance.User_material[enumMaterialCategory.Dev_Kit].Value < 1)
			{
				return false;
			}
			Mem_deck value = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, out value))
			{
				return false;
			}
			int key = value.Ship[0];
			if (!Comm_UserDatas.Instance.User_ship.ContainsKey(key))
			{
				return false;
			}
			return true;
		}

		private int getCreateItemType(int deckRid)
		{
			int key = Comm_UserDatas.Instance.User_deck[deckRid].Ship[0];
			Mem_ship ship = Comm_UserDatas.Instance.User_ship[key];
			var anon = (from item in createItemGroup
				where item.Element("Id").Value == ship.Stype.ToString()
				select new
				{
					type = item.Element("Createitem").Value
				}).First();
			return int.Parse(anon.type);
		}

		private int getCreateItem(int retryNum, int type, Dictionary<enumMaterialCategory, int> items)
		{
			int result = 0;
			for (int i = 0; i < retryNum; i++)
			{
				int createItemId = getCreateItemId(type, items);
				Mst_slotitem value = null;
				if (Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(createItemId, out value) && isCreateItemRareCheck(value) && isCreateItemMatCheck(items, value))
				{
					result = createItemId;
					break;
				}
			}
			return result;
		}

		private bool isCreateItemRareCheck(Mst_slotitem mst_slot)
		{
			if (mst_slot == null)
			{
				return false;
			}
			double randDouble = Utils.GetRandDouble(0.0, 100.0, 1.0, 1);
			int num = mst_slot.Rare;
			if (randDouble <= 20.0)
			{
				num -= 2;
			}
			else if (randDouble <= 50.0)
			{
				num--;
			}
			return (Comm_UserDatas.Instance.User_record.Level >= num * 10) ? true : false;
		}

		private bool isCreateItemMatCheck(Dictionary<enumMaterialCategory, int> items, Mst_slotitem mst_slot)
		{
			if (mst_slot == null)
			{
				return false;
			}
			if (items[enumMaterialCategory.Fuel] < mst_slot.Broken1 * 10 || items[enumMaterialCategory.Bull] < mst_slot.Broken2 * 10 || items[enumMaterialCategory.Steel] < mst_slot.Broken3 * 10 || items[enumMaterialCategory.Bauxite] < mst_slot.Broken4 * 10)
			{
				return false;
			}
			return true;
		}

		private int getCreateItemId(int typeNo, Dictionary<enumMaterialCategory, int> items)
		{
			int num = items.Values.Max();
			int layerNo = 1;
			if (items[enumMaterialCategory.Fuel] == num)
			{
				layerNo = 1;
			}
			else if (items[enumMaterialCategory.Steel] == num)
			{
				layerNo = 3;
			}
			else if (items[enumMaterialCategory.Bull] == num)
			{
				layerNo = 2;
			}
			else if (items[enumMaterialCategory.Bauxite] == num)
			{
				layerNo = 4;
			}
			Random random = new Random();
			int count = random.Next(50);
			var source = from item in createItemTable
				let table_id = int.Parse(item.Element("Id").Value)
				where item.Element("Type").Value == typeNo.ToString() && item.Element("Layer" + layerNo).Value == "1"
				orderby table_id
				select new
				{
					slot_id = item.Element("Slotitem_id").Value
				};
			var anon = source.Skip(count).First();
			return int.Parse(anon.slot_id);
		}

		public void Initialize_SlotitemRemodel()
		{
			mst_remodel = Mst_DataManager.Instance.Get_Mst_slotitem_remodel();
			mst_remodel_detail = Mst_DataManager.Instance.Get_Mst_slotitem_remodel_detail();
		}

		public Api_Result<Dictionary<int, List<Mst_slotitem_remodel>>> getSlotitemRemodelList(int deck_id)
		{
			Api_Result<Dictionary<int, List<Mst_slotitem_remodel>>> api_Result = new Api_Result<Dictionary<int, List<Mst_slotitem_remodel>>>();
			Mem_deck value = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_id, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			List<Mem_ship> memShip = value.Ship.getMemShip();
			if (memShip.Count == 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (memShip[0].Stype != 19)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mst_remodel == null)
			{
				mst_remodel = Mst_DataManager.Instance.Get_Mst_slotitem_remodel();
			}
			bool flag = false;
			if (mst_remodel_list == null)
			{
				mst_remodel_list = new Dictionary<int, Mst_slotitem_remodel>();
				flag = true;
			}
			Dictionary<int, List<Mst_slotitem_remodel>> dictionary = new Dictionary<int, List<Mst_slotitem_remodel>>();
			foreach (KeyValuePair<int, List<Mst_slotitem_remodel>> item in mst_remodel)
			{
				List<Mst_slotitem_remodel> list = new List<Mst_slotitem_remodel>();
				list.Add(null);
				list.Add(null);
				list.Add(null);
				list.Add(null);
				List<Mst_slotitem_remodel> list2 = list;
				List<Mst_slotitem_remodel> list3 = new List<Mst_slotitem_remodel>();
				dictionary.Add(item.Key, list2);
				foreach (Mst_slotitem_remodel item2 in item.Value)
				{
					if (flag)
					{
						mst_remodel_list.Add(item2.Id, item2);
					}
					if (item2.ValidShipId(memShip))
					{
						list2[0] = item2;
					}
					if (item2.ValidYomi(memShip))
					{
						list2[1] = item2;
					}
					if (item2.ValidStype(memShip))
					{
						list2[2] = item2;
					}
					if (item2.IsRemodelBase(memShip))
					{
						list2[3] = item2;
					}
				}
				list2.RemoveAll((Mst_slotitem_remodel x) => x == null);
				list3.Add(list2[0]);
				dictionary[item.Key] = list3;
			}
			if (dictionary.Count == 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			api_Result.data = dictionary;
			return api_Result;
		}

		public Api_Result<Mst_slotitem_remodel_detail> getSlotitemRemodelListDetail(int menu_id, int slot_id)
		{
			Api_Result<Mst_slotitem_remodel_detail> api_Result = new Api_Result<Mst_slotitem_remodel_detail>();
			if (!Comm_UserDatas.Instance.User_slot.TryGetValue(slot_id, out Mem_slotitem value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mst_remodel_detail == null)
			{
				mst_remodel_detail = Mst_DataManager.Instance.Get_Mst_slotitem_remodel_detail();
			}
			Mst_slotitem_remodel_detail mst_slotitem_remodel_detail = null;
			foreach (Mst_slotitem_remodel_detail item in mst_remodel_detail[menu_id])
			{
				if (value.Level >= item.Level_from && value.Level <= item.Level_to)
				{
					mst_slotitem_remodel_detail = item;
					break;
				}
			}
			if (mst_slotitem_remodel_detail == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
			}
			api_Result.data = mst_slotitem_remodel_detail;
			return api_Result;
		}

		public Api_Result<bool> RemodelSlot(Mst_slotitem_remodel_detail detail, int deck_id, int slot_id, bool certain_flag)
		{
			Api_Result<bool> api_Result = new Api_Result<bool>();
			if (!Comm_UserDatas.Instance.User_slot.TryGetValue(slot_id, out Mem_slotitem value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (value.Equip_flag == Mem_slotitem.enumEquipSts.Equip)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_deck value2 = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_id, out value2))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			List<Mem_ship> memShip = value2.Ship.getMemShip();
			if (memShip.Count == 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Dictionary<enumMaterialCategory, int> remodelUseMaterials = GetRemodelUseMaterials(detail, certain_flag);
			foreach (KeyValuePair<enumMaterialCategory, int> item in remodelUseMaterials)
			{
				if (Comm_UserDatas.Instance.User_material[item.Key].Value < item.Value)
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
			}
			List<Mem_slotitem> list = new List<Mem_slotitem>();
			if (detail.Req_slotitems >= 1)
			{
				list = (from mem_slot in Comm_UserDatas.Instance.User_slot.Values
					where mem_slot.Rid != slot_id
					where mem_slot.Level == 0
					where mem_slot.Slotitem_id == detail.Req_slotitem_id
					where mem_slot.Equip_flag == Mem_slotitem.enumEquipSts.Unset
					where !mem_slot.Lock
					select mem_slot).Take(detail.Req_slotitems).ToList();
				if (detail.Req_slotitems < list.Count())
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
			}
			int num = 100;
			if (!certain_flag)
			{
				num = ((memShip[0].Ship_id == 182) ? detail.Success_rate1 : ((memShip[0].Ship_id != 187) ? detail.Success_rate1 : detail.Success_rate2));
			}
			int num2 = new Random().Next(100);
			bool flag = (num2 < num) ? true : false;
			int slotitem_id = value.Slotitem_id;
			int num3 = slotitem_id;
			int level = 0;
			if (flag)
			{
				if (detail.New_slotitem_id > 0)
				{
					num3 = detail.New_slotitem_id;
					level = detail.New_slotitem_level;
				}
				else
				{
					level = value.Level + 1;
				}
			}
			foreach (KeyValuePair<enumMaterialCategory, int> item2 in remodelUseMaterials)
			{
				Comm_UserDatas.Instance.User_material[item2.Key].Sub_Material(item2.Value);
			}
			list.ForEach(delegate(Mem_slotitem x)
			{
				Comm_UserDatas.Instance.User_slot.Remove(x.Rid);
			});
			if (flag)
			{
				if (slotitem_id != num3)
				{
					ChangeSlotId(value, num3);
					Comm_UserDatas.Instance.Add_Book(2, num3);
				}
				value.SetLevel(level);
			}
			api_Result.data = flag;
			QuestKousyou questKousyou = new QuestKousyou(detail, value, flag);
			questKousyou.ExecuteCheck();
			return api_Result;
		}

		public void ChangeSlotId(Mem_slotitem obj, int changeId)
		{
			obj.ChangeSlotId(this, changeId);
		}

		private Dictionary<enumMaterialCategory, int> GetRemodelUseMaterials(Mst_slotitem_remodel_detail detailObj, bool cetain_flag)
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			Mst_slotitem_remodel mst_slotitem_remodel = mst_remodel_list[detailObj.Id];
			dictionary[enumMaterialCategory.Fuel] = mst_slotitem_remodel.Req_material1;
			dictionary[enumMaterialCategory.Bull] = mst_slotitem_remodel.Req_material2;
			dictionary[enumMaterialCategory.Steel] = mst_slotitem_remodel.Req_material3;
			dictionary[enumMaterialCategory.Bauxite] = mst_slotitem_remodel.Req_material4;
			if (cetain_flag)
			{
				dictionary[enumMaterialCategory.Dev_Kit] = detailObj.Req_material5_2;
				dictionary[enumMaterialCategory.Revamp_Kit] = detailObj.Req_material6_2;
			}
			else
			{
				dictionary[enumMaterialCategory.Dev_Kit] = detailObj.Req_material5_1;
				dictionary[enumMaterialCategory.Revamp_Kit] = detailObj.Req_material6_1;
			}
			return dictionary;
		}
	}
}
