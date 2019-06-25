using UnityEngine;

public class _memtest : MonoBehaviour
{
	private int update_count;

	private int always;

	private UILabel actLabel;

	private string text;

	private void Start()
	{
		update_count = 0;
		always = 0;
		actLabel = GameObject.Find("actLabel").GetComponent<UILabel>();
		text = $"[LOG]\tMonoUsed\tMonoSize\tMonoUsed(%)\tTotalUsed\tTotalSize\tTotalUsed(%)\tUnUse\n";
		Debug.Log(text);
	}

	private void Update()
	{
		if (always++ % 2 == 0)
		{
			actLabel.SetActive(isActive: false);
		}
		else
		{
			actLabel.SetActive(isActive: true);
		}
		if (update_count++ >= 10)
		{
			update_count = 0;
			uint monoUsedSize = Profiler.GetMonoUsedSize();
			uint monoHeapSize = Profiler.GetMonoHeapSize();
			uint totalAllocatedMemory = Profiler.GetTotalAllocatedMemory();
			uint totalReservedMemory = Profiler.GetTotalReservedMemory();
			uint totalUnusedReservedMemory = Profiler.GetTotalUnusedReservedMemory();
			text = $"Mono :{monoUsedSize / 1024u}/{monoHeapSize / 1024u} kb({100.0 * (double)monoUsedSize / (double)monoHeapSize:f1}%)\nTotal:{totalAllocatedMemory / 1024u}/{totalReservedMemory / 1024u} kb({100.0 * (double)totalAllocatedMemory / (double)totalReservedMemory:f1}%)\nUnUse:{totalUnusedReservedMemory / 1024u}kb\n";
			base.gameObject.GetComponent<UILabel>().text = text;
			text = $"{monoUsedSize / 1024u}\t{monoHeapSize / 1024u}\t{100.0 * (double)monoUsedSize / (double)monoHeapSize:f1}%\t{totalAllocatedMemory / 1024u}\t{totalReservedMemory / 1024u}\t{100.0 * (double)totalAllocatedMemory / (double)totalReservedMemory:f1}%\t{totalUnusedReservedMemory / 1024u}\n";
			Debug.Log("[LOG]\t" + text);
		}
	}
}
