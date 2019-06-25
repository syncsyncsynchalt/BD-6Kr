using local.models;
using Server_Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	public class Test_Marriage : MonoBehaviour
	{
		private GameObject go;

		public void Start()
		{
			StartCoroutine(Initialize());
		}

		public IEnumerator Initialize()
		{
			yield return new WaitForSeconds(0.5f);
			go = (Object.Instantiate(Resources.Load("Prefabs/Remodel/MarriageCut")) as GameObject);
			if (go == null)
			{
				Debug.Log("Failed to properly instantiate and initialize MarriageCut");
				yield break;
			}
			go.name = "MarriageCut";
			go.transform.localPosition = new Vector3(-1000f, -1000f, 0f);
			MarriageManager manager = go.GetComponent<MarriageManager>();
			if (manager == null)
			{
				manager = go.AddComponent<MarriageManager>();
			}
			if (manager == null)
			{
				Debug.Log("Failed to properly instantiate and initialize MarriageCut");
				yield break;
			}
			manager.enabled = true;
			List<int> tmpList2 = new List<int>
			{
				54
			};
			Debug_Mod debug = new Debug_Mod();
			tmpList2 = debug.Add_Ship(tmpList2);
			ShipModel ship = new ShipModel(tmpList2[0]);
			if (ship == null)
			{
				Debug.Log("Failed to properly instantiate and initialize MarriageCut");
			}
			else
			{
				manager.Initialize(ship, KeyControlManager.Instance.KeyController, DeleteGO);
			}
		}

		public void DeleteGO()
		{
			Object.Destroy(go);
		}
	}
}
