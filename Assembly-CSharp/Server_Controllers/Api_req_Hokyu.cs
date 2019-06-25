using System;
using Common.Enum;
using Server_Common;
using Server_Controllers.QuestLogic;
using Server_Models;
using System.Collections.Generic;

namespace Server_Controllers
{
    public class Api_req_Hokyu
    {
        public enum enumHokyuType
        {
            Fuel = 1,
            Bull,
            All
        }

        public int GetRequireUseBauxiteNum(Mem_ship ship, ref int haveBauxite, out List<int> afterOnslot)
        {
            afterOnslot = new List<int>(ship.Onslot);
            if (haveBauxite == 0)
            {
                return 0;
            }
            int num = haveBauxite;
            Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[ship.Ship_id];
            for (int i = 0; i < ship.Slotnum; i++)
            {
                if (haveBauxite == 0)
                {
                    break;
                }
                if (afterOnslot[i] < mst_ship.Maxeq[i])
                {
                    int num4 = ship.Slot[i];
                    int num2 = mst_ship.Maxeq[i] - ship.Onslot[i];
                    int num3 = num2 * 5;
                    if (haveBauxite >= num3)
                    {
                        afterOnslot[i] += num2;
                        haveBauxite -= num3;
                    }
                }
            }
            return num - haveBauxite;
        }

        public Api_Result<bool> Charge(List<int> ship_rids, enumHokyuType type)
        {
            Api_Result<bool> rslt = new Api_Result<bool>();
            rslt.data = false;
            if (ship_rids == null || ship_rids.Count == 0)
            {
                rslt.state = Api_Result_State.Parameter_Error;
                return rslt;
            }
            List<Mem_ship> ships = new List<Mem_ship>();
            ship_rids.ForEach(delegate (int x)
            {
                Mem_ship value = null;
                if (!Comm_UserDatas.Instance.User_ship.TryGetValue(x, out value))
                {
                    rslt.state = Api_Result_State.Parameter_Error;
                }
                else
                {
                    ships.Add(value);
                }
            });
            if (rslt.state == Api_Result_State.Parameter_Error)
            {
                ships.Clear();
                return rslt;
            }
            HashSet<int> hashSet = new HashSet<int>();
            int num = 0;
            foreach (Mem_ship item in ships)
            {
                if (Mst_DataManager.Instance.Mst_ship.ContainsKey(item.Ship_id))
                {
                    int fuel = item.Fuel;
                    int bull = item.Bull;
                    switch (type)
                    {
                        case enumHokyuType.Fuel:
                        case enumHokyuType.Bull:
                            {
                                bool flag3 = ChargeDataSet(type, item);
                                if (bull < item.Bull || fuel < item.Fuel)
                                {
                                    item.SumLovToCharge();
                                    num++;
                                }
                                HashSet<int> hashSet3 = ChargeDataSet_Onslot(enumHokyuType.All, item);
                                foreach (int item2 in hashSet3)
                                {
                                    hashSet.Add(item2);
                                }
                                if (flag3 || !hashSet3.Contains(-1))
                                {
                                    break;
                                }
                                throw new NotImplementedException("‚È‚É‚±‚ê");
                                // goto end_IL_00a0;
                            }
                        case enumHokyuType.All:
                            {
                                bool flag = ChargeDataSet(enumHokyuType.Bull, item);
                                bool flag2 = ChargeDataSet(enumHokyuType.Fuel, item);
                                HashSet<int> hashSet2 = ChargeDataSet_Onslot(enumHokyuType.All, item);
                                if (bull < item.Bull || fuel < item.Fuel)
                                {
                                    item.SumLovToCharge();
                                    num++;
                                }
                                foreach (int item3 in hashSet2)
                                {
                                    hashSet.Add(item3);
                                }
                                if (flag || flag2 || !hashSet2.Contains(-1))
                                {
                                    break;
                                }
                                throw new NotImplementedException("‚È‚É‚±‚ê");
                                // goto end_IL_00a0;
                            }
                    }
                }
            }
            if (hashSet.Contains(-2))
            {
                rslt.data = false;
            }
            else
            {
                rslt.data = true;
            }
            QuestSupply questSupply = new QuestSupply(num);
            questSupply.ExecuteCheck();
            return rslt;
        }

        private bool ChargeDataSet(enumHokyuType type, Mem_ship m_ship)
        {
            enumMaterialCategory key;
            int value;
            int num;
            int fuel;
            int bull;
            switch (type)
            {
                case enumHokyuType.Fuel:
                    {
                        key = enumMaterialCategory.Fuel;
                        int bull_max = Mst_DataManager.Instance.Mst_ship[m_ship.Ship_id].Fuel_max;
                        value = Comm_UserDatas.Instance.User_material[key].Value;
                        num = m_ship.GetRequireChargeFuel();
                        fuel = bull_max;
                        bull = m_ship.Bull;
                        break;
                    }
                case enumHokyuType.Bull:
                    {
                        key = enumMaterialCategory.Bull;
                        int bull_max = Mst_DataManager.Instance.Mst_ship[m_ship.Ship_id].Bull_max;
                        value = Comm_UserDatas.Instance.User_material[key].Value;
                        num = m_ship.GetRequireChargeBull();
                        fuel = m_ship.Fuel;
                        bull = bull_max;
                        break;
                    }
                default:
                    return true;
            }
            if (value <= 0)
            {
                return false;
            }
            if (num == 0)
            {
                return true;
            }
            if (num > value)
            {
                return true;
            }
            Comm_UserDatas.Instance.User_material[key].Sub_Material(num);
            m_ship.Set_ChargeData(bull, fuel, null);
            return true;
        }

        private HashSet<int> ChargeDataSet_Onslot(enumHokyuType type, Mem_ship m_ship)
        {
            enumMaterialCategory key = enumMaterialCategory.Bauxite;
            int haveBauxite = Comm_UserDatas.Instance.User_material[key].Value;
            HashSet<int> hashSet = new HashSet<int>();
            List<int> afterOnslot;
            if (haveBauxite == 0)
            {
                int haveBauxite2 = 100;
                int requireUseBauxiteNum = GetRequireUseBauxiteNum(m_ship, ref haveBauxite2, out afterOnslot);
                if (requireUseBauxiteNum > 0)
                {
                    hashSet.Add(-2);
                }
                hashSet.Add(-1);
                return hashSet;
            }
            int requireUseBauxiteNum2 = GetRequireUseBauxiteNum(m_ship, ref haveBauxite, out afterOnslot);
            m_ship.Set_ChargeData(m_ship.Bull, m_ship.Fuel, afterOnslot);
            Comm_UserDatas.Instance.User_material[key].Sub_Material(requireUseBauxiteNum2);
            List<int> maxeq = Mst_DataManager.Instance.Mst_ship[m_ship.Ship_id].Maxeq;
            for (int i = 0; i < m_ship.Slotnum; i++)
            {
                if (maxeq[i] > 0 && maxeq[i] != m_ship.Onslot[i])
                {
                    hashSet.Add(-2);
                }
            }
            return hashSet;
        }
    }
}
