using System;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
public class AllocMem : SingletonMonoBehaviour<AllocMem>
{
	public bool isShow = true;

	public bool isShowFPS;

	public bool isShowInEditor;

	private float fLastCollect;

	private float fLastCollectNum;

	private float fDelta;

	private float fLastDeltaTime;

	private int nAllocRate;

	private int nLastAllocMemory;

	private float fLastAllocSet = -9999f;

	private int nAllocMem;

	private int nCollectAlloc;

	private int nPeakAlloc;

	protected override void Awake()
	{
		base.Awake();
	}

	public void Start()
	{
		base.useGUILayout = false;
	}

	public void OnGUI()
	{
		if (!isShow || (!Application.isPlaying && !isShowInEditor))
		{
			return;
		}
		int num = GC.CollectionCount(0);
		if (fLastCollectNum != (float)num)
		{
			fLastCollectNum = num;
			fDelta = Time.realtimeSinceStartup - fLastCollect;
			fLastCollect = Time.realtimeSinceStartup;
			fLastDeltaTime = Time.deltaTime;
			nCollectAlloc = nAllocMem;
		}
		nAllocMem = (int)GC.GetTotalMemory(forceFullCollection: false);
		nPeakAlloc = ((nAllocMem <= nPeakAlloc) ? nPeakAlloc : nAllocMem);
		if (Time.realtimeSinceStartup - fLastAllocSet > 0.3f)
		{
			int num2 = nAllocMem - nLastAllocMemory;
			nLastAllocMemory = nAllocMem;
			fLastAllocSet = Time.realtimeSinceStartup;
			if (num2 >= 0)
			{
				nAllocRate = num2;
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[Currently allocated]\t");
		stringBuilder.Append(((float)nAllocMem / 1000000f).ToString("0"));
		stringBuilder.Append("mb\n");
		stringBuilder.Append("[Peak allocated]\t");
		stringBuilder.Append(((float)nPeakAlloc / 1000000f).ToString("0"));
		stringBuilder.Append("mb (last\tcollect ");
		stringBuilder.Append(((float)nCollectAlloc / 1000000f).ToString("0"));
		stringBuilder.Append(" mb)\n");
		stringBuilder.Append("[Allocation rate]\t");
		stringBuilder.Append(((float)nAllocRate / 1000000f).ToString("0.0"));
		stringBuilder.Append("mb\n");
		stringBuilder.Append("[Collection frequency]\t");
		stringBuilder.Append(fDelta.ToString("0.00"));
		stringBuilder.Append("s\n");
		stringBuilder.Append("[Last collect delta]\t");
		stringBuilder.Append(fLastDeltaTime.ToString("0.000"));
		stringBuilder.Append("s (");
		stringBuilder.Append((1f / fLastDeltaTime).ToString("0.0"));
		stringBuilder.Append(" fps)");
		stringBuilder.Append("mb\n");
		stringBuilder.Append($"[UseMemory]{Profiler.GetTotalAllocatedMemory() / 1048576u}/{Profiler.GetTotalReservedMemory() / 1048576u}MB");
		stringBuilder.Append("\n");
		stringBuilder.Append($"[UnUseMemory]{Profiler.GetTotalUnusedReservedMemory() / 1048576u}MB");
		if (isShowFPS)
		{
			stringBuilder.Append("\n" + (1f / Time.deltaTime).ToString("0.0") + " fps");
		}
		GUI.Box(new Rect(2f, 5f, 310f, 95 + ((!isShowFPS) ? 20 : 36)), string.Empty);
		GUI.Label(new Rect(10f, 5f, 1000f, 200f), stringBuilder.ToString());
	}

	public string GetText()
	{
		if (!isShow || (!Application.isPlaying && !isShowInEditor))
		{
			return string.Empty;
		}
		int num = GC.CollectionCount(0);
		if (fLastCollectNum != (float)num)
		{
			fLastCollectNum = num;
			fDelta = Time.realtimeSinceStartup - fLastCollect;
			fLastCollect = Time.realtimeSinceStartup;
			fLastDeltaTime = Time.deltaTime;
			nCollectAlloc = nAllocMem;
		}
		nAllocMem = (int)GC.GetTotalMemory(forceFullCollection: false);
		nPeakAlloc = ((nAllocMem <= nPeakAlloc) ? nPeakAlloc : nAllocMem);
		if (Time.realtimeSinceStartup - fLastAllocSet > 0.3f)
		{
			int num2 = nAllocMem - nLastAllocMemory;
			nLastAllocMemory = nAllocMem;
			fLastAllocSet = Time.realtimeSinceStartup;
			if (num2 >= 0)
			{
				nAllocRate = num2;
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Currently allocated\t\t\t");
		stringBuilder.Append(((float)nAllocMem / 1000000f).ToString("0"));
		stringBuilder.Append("mb\n");
		stringBuilder.Append("Peak allocated\t\t\t\t");
		stringBuilder.Append(((float)nPeakAlloc / 1000000f).ToString("0"));
		stringBuilder.Append("mb (last\tcollect ");
		stringBuilder.Append(((float)nCollectAlloc / 1000000f).ToString("0"));
		stringBuilder.Append(" mb)\n");
		stringBuilder.Append("Allocation rate\t\t\t\t");
		stringBuilder.Append(((float)nAllocRate / 1000000f).ToString("0.0"));
		stringBuilder.Append("mb\n");
		stringBuilder.Append("Collection frequency\t\t");
		stringBuilder.Append(fDelta.ToString("0.00"));
		stringBuilder.Append("s\n");
		stringBuilder.Append("Last collect delta\t\t\t");
		stringBuilder.Append(fLastDeltaTime.ToString("0.000"));
		stringBuilder.Append("s (");
		stringBuilder.Append((1f / fLastDeltaTime).ToString("0.0"));
		stringBuilder.Append(" fps)");
		if (isShowFPS)
		{
			stringBuilder.Append("\n" + (1f / Time.deltaTime).ToString("0.0") + " fps");
		}
		GUI.Box(new Rect(Screen.width / 2, 5f, 310f, 80 + (isShowFPS ? 16 : 0)), string.Empty);
		GUI.Label(new Rect(10f, 5f, 1000f, 200f), stringBuilder.ToString());
		return stringBuilder.ToString();
	}
}
