using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateAsyncManager : SingletonMonoBehaviour<InstantiateAsyncManager>
{
	public GameObject test;

	public GameObject parent;

	public static void InstanceAsync(GameObject[] objects, Transform self, AsyncObjects AsyncObj)
	{
		SingletonMonoBehaviour<InstantiateAsyncManager>.Instance.StartCoroutine(SingletonMonoBehaviour<InstantiateAsyncManager>.Instance.InstanceObjects(objects, self, AsyncObj));
	}

	public IEnumerator InstanceObjects(GameObject[] objects, Transform parent, AsyncObjects AsyncObj)
	{
		if (AsyncObj.GOs == null)
		{
			AsyncObj.GOs = new List<GameObject>();
			AsyncObj.GOs_Dic = new Dictionary<string, GameObject>();
		}
		for (int j = 0; j < objects.Length; j++)
		{
			if (AsyncObj.GOs_Dic.ContainsKey(objects[j].name))
			{
				Debug.Log("AsyncObject " + objects[j].name + " □□□ 既に存在しているため生成しません □□□");
				continue;
			}
			float time = Time.realtimeSinceStartup;
			GameObject go = Util.Instantiate(objects[j], parent.gameObject);
			Debug.Log(objects[j].name + " AsyncLoad " + (Time.realtimeSinceStartup - time));
			AsyncObj.GOs.Add(go);
			AsyncObj.GOs_Dic.Add(go.name, go);
			if (AsyncObj.NameChange != null && AsyncObj.NameChange.Length > j)
			{
				AsyncObj.GOs[j].name = AsyncObj.NameChange[j];
			}
			if (AsyncObj.isAutoActive)
			{
				AsyncObj.GOs[j].SetActive(true);
			}
			yield return null;
		}
		yield return new WaitForSeconds(AsyncObj.delay);
		AsyncObj.isFinished = true;
		if (AsyncObj.Act != null)
		{
			for (int i = 0; i < AsyncObj.Act.Count; i++)
			{
				AsyncObj.Act[i]();
			}
		}
		yield return null;
	}
}
