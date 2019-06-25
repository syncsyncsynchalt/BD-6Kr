using DG.Tweening;
using local.utils;
using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyHaveMaterials : MonoBehaviour
	{
		public UILabel[] MaterialNum;

		public GameObject ParentObject;

		private int[] MaterialsNumInt;

		private int[] PrevMaterialsNumInt;

		private bool isInitialize;

		private Coroutine[] coroutines;

		private void Awake()
		{
			MaterialsNumInt = new int[MaterialNum.Length];
			PrevMaterialsNumInt = new int[MaterialNum.Length];
			coroutines = new Coroutine[MaterialNum.Length];
		}

		public void Initialize()
		{
			UpdateNum();
			UpdateLabel();
			for (int i = 0; i < 4; i++)
			{
				UpdateColor(i);
			}
			isInitialize = true;
		}

		public void UpdateFooterMaterials()
		{
			if (isInitialize)
			{
				UpdateNumAnimation();
			}
			else
			{
				Initialize();
			}
		}

		private void UpdateNumAnimation()
		{
			for (int i = 0; i < MaterialNum.Length; i++)
			{
				PrevMaterialsNumInt[i] = MaterialsNumInt[i];
			}
			UpdateNum();
			for (int j = 0; j < MaterialNum.Length; j++)
			{
				if (MaterialsNumInt[j] != PrevMaterialsNumInt[j] && coroutines[j] == null)
				{
					coroutines[j] = StartCoroutine(ChangeNumAnimation(MaterialNum[j], j));
				}
			}
		}

		private void UpdateNum()
		{
			MaterialsNumInt[0] = StrategyTopTaskManager.GetLogicManager().Material.Fuel;
			MaterialsNumInt[1] = StrategyTopTaskManager.GetLogicManager().Material.Steel;
			MaterialsNumInt[2] = StrategyTopTaskManager.GetLogicManager().Material.Ammo;
			MaterialsNumInt[3] = StrategyTopTaskManager.GetLogicManager().Material.Baux;
			MaterialsNumInt[4] = StrategyTopTaskManager.GetLogicManager().Material.Devkit;
			MaterialsNumInt[5] = StrategyTopTaskManager.GetLogicManager().Material.RepairKit;
		}

		private void UpdateLabel()
		{
			for (int i = 0; i < MaterialNum.Length; i++)
			{
				MaterialNum[i].text = MaterialsNumInt[i].ToString();
			}
		}

		private IEnumerator ChangeNumAnimation(UILabel label, int LabelNo)
		{
			int changeValue = MaterialsNumInt[LabelNo] - PrevMaterialsNumInt[LabelNo];
			string PlusMinus;
			if (changeValue < 0)
			{
				PlusMinus = string.Empty;
				label.color = Color.red;
			}
			else
			{
				PlusMinus = "+";
				label.color = Color.cyan;
				TrophyUtil.Unlock_Material();
			}
			label.text = PlusMinus + changeValue;
			yield return new WaitForSeconds(1f);
			label.color = Color.white;
			label.text = MaterialsNumInt[LabelNo].ToString();
			TweenLabel(LabelNo, PrevMaterialsNumInt[LabelNo], MaterialsNumInt[LabelNo]);
			coroutines[LabelNo] = null;
		}

		private void UpdateColor(int LabelNo)
		{
			int materialMaxNum = StrategyTopTaskManager.GetLogicManager().UserInfo.GetMaterialMaxNum();
			MaterialNum[LabelNo].color = ((materialMaxNum > MaterialsNumInt[LabelNo]) ? Color.white : Color.yellow);
		}

		private void TweenLabel(int LabelNo, int from, int to)
		{
			Tween t = new UINumberCounter(MaterialNum[LabelNo]).SetFrom(from).SetTo(to).SetDuration(0.5f)
				.SetAnimationType(UINumberCounter.AnimationType.Count)
				.SetOnFinishedCallBack(delegate
				{
					if (LabelNo < 4)
					{
						UpdateColor(LabelNo);
					}
				})
				.Buld();
			t.Play();
		}
	}
}
