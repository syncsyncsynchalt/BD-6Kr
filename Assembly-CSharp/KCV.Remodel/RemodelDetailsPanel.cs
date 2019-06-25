using local.models;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace KCV.Remodel
{
	public class RemodelDetailsPanel : MonoBehaviour
	{
		private UIPanel myPanel;

		[SerializeField]
		private UITexture nowItemBase;

		[SerializeField]
		private UITexture nextItemBase;

		[SerializeField]
		private UILabel nowItemParams;

		[SerializeField]
		private UILabel nextItemParams;

		private StringBuilder sb;

		private List<int> paramList;

		private Vector3 enterPos;

		private Vector3 exitPos;

		private readonly string[] itemTypeName = new string[9]
		{
			"火力",
			"雷撃",
			"爆装",
			"対空",
			"対潜",
			"索敵",
			"装甲",
			"回避",
			"命中"
		};

		private void Start()
		{
			sb = new StringBuilder();
			paramList = new List<int>();
			enterPos = new Vector3(270f, -40f, 0f);
			exitPos = new Vector3(690f, -40f, 0f);
		}

		private void OnDestroy()
		{
			Mem.Del(ref myPanel);
			Mem.Del(ref nowItemBase);
			Mem.Del(ref nextItemBase);
			Mem.Del(ref nowItemParams);
			Mem.Del(ref nextItemBase);
			Mem.Del(ref sb);
			Mem.DelListSafe(ref paramList);
			Mem.Del(ref enterPos);
			Mem.Del(ref exitPos);
		}

		public void Initialize(SlotitemModel nowItem, SlotitemModel nextItem)
		{
			EnterPanel();
		}

		public void EnterPanel()
		{
			Util.MoveTo(base.gameObject, 0.2f, enterPos, iTween.EaseType.easeOutQuint);
		}

		public void ExitPanel()
		{
			Util.MoveTo(base.gameObject, 0.2f, exitPos, iTween.EaseType.easeOutQuint);
		}

		public string makeParamText(SlotitemModel item)
		{
			sb.Length = 0;
			paramList.Clear();
			if (0 < item.Syatei)
			{
				sb.Append("射程：" + App.SYATEI_TEXT[item.Syatei]);
				sb.AppendLine();
			}
			paramList.Add(item.Hougeki);
			paramList.Add(item.Raigeki);
			paramList.Add(item.Bakugeki);
			paramList.Add(item.Taikuu);
			paramList.Add(item.Taisen);
			paramList.Add(item.Sakuteki);
			paramList.Add(item.Soukou);
			paramList.Add(item.Kaihi);
			paramList.Add(item.HouMeityu);
			int num = 0;
			for (int i = 0; i < paramList.Count; i++)
			{
				if (0 < paramList[i])
				{
					string text = (paramList[i] <= 0) ? " -" : " +";
					sb.Append(itemTypeName[i] + text + paramList[i] + "\u3000");
					num++;
					if (num % 3 == 0)
					{
						sb.AppendLine();
					}
				}
			}
			return sb.ToString();
		}
	}
}
