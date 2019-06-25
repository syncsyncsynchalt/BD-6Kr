using Common.Struct;
using local.managers;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class DayAnimation : MonoBehaviour
	{
		private enum AnimType
		{
			Day,
			Week,
			Month,
			Choco,
			EndDay
		}

		private Animation anim;

		[SerializeField]
		private Transform[] AnimationTypes;

		[SerializeField]
		private StrategyMonthWeekBonus MonthBonus;

		[SerializeField]
		private StrategyMonthWeekBonus WeekBonus;

		[SerializeField]
		private UILabel dayText;

		[SerializeField]
		private UILabel EndDayText;

		private KeyControl key;

		private void Awake()
		{
			anim = GetComponent<Animation>();
			key = new KeyControl();
		}

		private IEnumerator Start()
		{
			yield return Util.WaitEndOfFrames(5);
			this.SetActive(isActive: false);
		}

		public void StartWait(string AnimName)
		{
			StartCoroutine(WaitForItemView(AnimName));
		}

		public IEnumerator WaitForItemView(string AnimName)
		{
			anim[AnimName].speed = 0f;
			float time = 0f;
			while (time < 2f && !key.IsMaruDown())
			{
				key.Update();
				time += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			anim[AnimName].speed = 1f;
			yield return null;
		}

		public IEnumerator StartDayAnimation(StrategyMapManager LogicMng, bool isDebug)
		{
			SetActiveAnimType(AnimType.Day);
			string[] array = new string[6];
			TurnString datetimeString = LogicMng.DatetimeString;
			array[0] = datetimeString.Year;
			array[1] = "の年 ";
			TurnString datetimeString2 = LogicMng.DatetimeString;
			array[2] = datetimeString2.Month;
			array[3] = " ";
			TurnString datetimeString3 = LogicMng.DatetimeString;
			array[4] = datetimeString3.Day;
			array[5] = "日";
			string day = string.Concat(array);
			switch (DateTime.DaysInMonth(LogicMng.Datetime.Year, LogicMng.Datetime.Month) - LogicMng.Datetime.Day)
			{
			case 4:
				dayText.color = new Color32(190, 230, 190, byte.MaxValue);
				break;
			case 3:
				dayText.color = new Color32(150, 230, 150, byte.MaxValue);
				break;
			case 2:
				dayText.color = new Color32(110, 230, 110, byte.MaxValue);
				break;
			case 1:
				dayText.color = new Color32(70, 230, 70, byte.MaxValue);
				break;
			case 0:
				dayText.color = new Color32(30, 230, 30, byte.MaxValue);
				break;
			default:
				dayText.color = Color.white;
				break;
			}
			dayText.text = day;
			anim.Play("DayAnimation");
			int count = 0;
			while (anim.isPlaying && !isDebug)
			{
				count++;
				if (count > 100)
				{
					break;
				}
				yield return new WaitForEndOfFrame();
			}
		}

		public IEnumerator StartMonthAnimation(StrategyMapManager LogicMng, UserPreActionPhaseResultModel userPreAction, bool isDebug)
		{
			MaterialInfo materialInfo = userPreAction.GetMonthlyBonus();
			if (!materialInfo.HasPositive())
			{
				yield break;
			}
			SetActiveAnimType(AnimType.Month);
			StrategyMonthWeekBonus monthBonus = MonthBonus;
			TurnString datetimeString = LogicMng.DatetimeString;
			monthBonus.SetLabels(datetimeString.Month, materialInfo);
			anim.Play("MonthAnimation");
			int count = 0;
			while (anim.isPlaying && !isDebug)
			{
				count++;
				if (count > 100)
				{
					break;
				}
				yield return new WaitForEndOfFrame();
			}
		}

		public IEnumerator StartWeekAnimation(StrategyMapManager LogicMng, UserPreActionPhaseResultModel userPreAction, bool isDebug)
		{
			MaterialInfo materialInfo = userPreAction.GetWeeklyBonus();
			if (!materialInfo.HasPositive())
			{
				yield break;
			}
			SetActiveAnimType(AnimType.Week);
			WeekBonus.SetLabelsWeek(materialInfo);
			anim.Play("WeekAnimation");
			int count = 0;
			while (anim.isPlaying && !isDebug)
			{
				count++;
				if (count > 100)
				{
					break;
				}
				yield return new WaitForEndOfFrame();
			}
		}

		public IEnumerator StartSendChocoAnimation(StrategyMapManager LogicMng, UserPreActionPhaseResultModel userPreAction, bool isDebug)
		{
			if (userPreAction.Rewards.Count == 0)
			{
				yield break;
			}
			SetActiveAnimType(AnimType.Choco);
			anim.Play("SendChoco");
			int count = 0;
			while (anim.isPlaying && !isDebug)
			{
				count++;
				if (count > 100)
				{
					break;
				}
				yield return new WaitForEndOfFrame();
			}
		}

		public IEnumerator EndDayAnimation(StrategyMapManager LogicMng, bool isDebug)
		{
			int leaveTurn = 3601 - LogicMng.Turn;
			if (CheckEndDay(leaveTurn))
			{
				SetActiveAnimType(AnimType.EndDay);
				string empty = string.Empty;
				string color = (leaveTurn > 50) ? "[F9C66CFF]" : ((leaveTurn <= 10) ? "[FF1000FF]" : "[FF6E00FF]");
				string day = "残り継戦時間 " + color + leaveTurn + "[-]日";
				EndDayText.text = day;
				anim.Play("DayAnimation");
				while (anim.isPlaying && !isDebug)
				{
					yield return new WaitForEndOfFrame();
				}
			}
		}

		private bool CheckEndDay(int leaveTurn)
		{
			if (leaveTurn > 100)
			{
				return false;
			}
			return true;
		}

		private void SetActiveAnimType(AnimType type)
		{
			Transform[] animationTypes = AnimationTypes;
			foreach (Transform component in animationTypes)
			{
				component.SetActive(isActive: false);
			}
			AnimationTypes[(int)type].SetActive(isActive: true);
		}

		private void OnDestroy()
		{
			anim = null;
			AnimationTypes = null;
			MonthBonus = null;
			WeekBonus = null;
			dayText = null;
			key = null;
		}
	}
}
