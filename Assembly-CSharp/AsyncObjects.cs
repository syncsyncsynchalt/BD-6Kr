using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AsyncObjects : MonoBehaviour
{
	private delegate void InstantiatePrefab(List<ResourceRequest> Reqs);

	[Button("EditorCreate", "プレハブ生成", new object[]
	{

	})]
	public int CreateInstance;

	public GameObject[] objects;

	public string[] PrefabPaths;

	[NonSerialized]
	public List<GameObject> GOs;

	[NonSerialized]
	public Dictionary<string, GameObject> GOs_Dic;

	public bool isFinished;

	public bool isActive;

	public bool isAutoLoad = true;

	public bool isAutoActive;

	public float delay;

	public List<Action> Act;

	public string[] NameChange;

	private void Awake()
	{
		DebugUtils.SLog("AsyncObjects Awake" + Time.realtimeSinceStartup);
		Act = new List<Action>();
	}

	private void Start()
	{
		DebugUtils.SLog("AsyncObjects Start" + Time.realtimeSinceStartup);
		isFinished = false;
		if (isAutoLoad)
		{
			StartLoad();
		}
	}

	public void StartLoad()
	{
		if (objects.Length != 0)
		{
			InstantiateAsyncManager.InstanceAsync(objects, base.transform, this);
		}
		if (PrefabPaths != null && PrefabPaths.Length != 0)
		{
			ResourceLoadAsync(PrefabPaths);
		}
	}

	public void setActiveGOs()
	{
		StartCoroutine(ActiveGos());
	}

	private IEnumerator ActiveGos()
	{
		while (!isFinished)
		{
			yield return null;
		}
		foreach (GameObject go in GOs)
		{
			go.SetActive(true);
		}
		isActive = true;
		yield return null;
	}

	private void ResourceLoadAsync(string[] PrefabPaths)
	{
		List<ResourceRequest> list = new List<ResourceRequest>(PrefabPaths.Length);
		for (int i = 0; i < PrefabPaths.Length; i++)
		{
			list.Add(Resources.LoadAsync(PrefabPaths[i]));
		}
		InstantiatePrefab dlgt = StartInstantiate;
		StartCoroutine(WaitAllLoaded(list, dlgt));
	}

	private IEnumerator WaitAllLoaded(List<ResourceRequest> Reqs, InstantiatePrefab dlgt)
	{
		bool allReady = false;
		while (!allReady)
		{
			allReady = Reqs.All((ResourceRequest req) => req.isDone);
			yield return null;
		}
		dlgt(Reqs);
		yield return null;
	}

	private void StartInstantiate(List<ResourceRequest> Reqs)
	{
		GameObject[] array = new GameObject[Reqs.Count];
		for (int i = 0; i < Reqs.Count; i++)
		{
			array[i] = (Reqs[i].asset as GameObject);
		}
		InstantiateAsyncManager.InstanceAsync(array, base.transform, this);
	}

	private void EditorCreate()
	{
		GameObject[] array = objects;
		foreach (GameObject prefab in array)
		{
			Util.InstantiateGameObject(prefab, base.transform);
		}
	}

	private void OnDestroy()
	{
	}
}
