using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KCV;
using Server_Common;
using Server_Models;
using Server_Controllers;
using local.models;
using local.managers;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            Mst_DataManager.Instance.LoadStartMaster(delegate
            {
                Mst_DataManager.Instance.SetStartMasterData();
            });

            // var bgm = Mst_DataManager.Instance.GetMstBgm();
            // var cabinet = Mst_DataManager.Instance.GetMstCabinet();
            // var payitem = Mst_DataManager.Instance.GetPayitem();
            // var furnitureText = Mst_DataManager.Instance.GetFurnitureText();

            Console.WriteLine("hajimarimasu");
        }
    }
}
