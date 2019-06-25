using local.models;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KCV.Remodel
{
	public class Test_PortUpgradesModernizeShip : MonoBehaviour
	{
		private GameObject go;

		public void Awake()
		{
			go = (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/ModernizeShip")) as GameObject);
			try
			{
				go.name = "ModernizeShip";
				go.transform.parent = base.transform.parent;
				go.transform.localScale = new Vector3(1f, 1f, 1f);
				go.GetComponent<PortUpgradesModernizeShipManager>().enabled = true;

                throw new NotImplementedException("‚È‚É‚±‚ê");
                // RuntimeHelpers.InitializeArray(new bool[5, 4], (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);

				go.GetComponent<PortUpgradesModernizeShipManager>().Initialize(new ShipModelMst(80), 5, fail: true, SuperSuccessed: false, 4);
			}
			catch (Exception)
			{
				go.AddComponent<PortUpgradesModernizeShipManager>();

                throw new NotImplementedException("‚È‚É‚±‚ê");
                // RuntimeHelpers.InitializeArray(new bool[5, 4], (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);

				go.GetComponent<PortUpgradesModernizeShipManager>().Initialize(new ShipModelMst(80), 5, fail: true, SuperSuccessed: false, 4);
			}
		}
	}
}
