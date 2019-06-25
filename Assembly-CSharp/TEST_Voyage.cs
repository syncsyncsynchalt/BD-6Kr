using Common.Enum;
using KCV;
using KCV.Battle;
using KCV.Strategy;
using local.managers;
using local.models;
using Server_Controllers;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class TEST_Voyage : SingletonMonoBehaviour<TEST_Voyage>
{
	private Stopwatch mStopWatch;

	private IEnumerator mStartVoyageCoroutine;

	private int mCycleCount;

	public void StartVoyage()
	{
		if (mStartVoyageCoroutine == null)
		{
			SuppressTutorials();
			mStopWatch = new Stopwatch();
			mStopWatch.Reset();
			mStopWatch.Start();
			mStartVoyageCoroutine = StartVoyageCoroutine();
			StartCoroutine(mStartVoyageCoroutine);
		}
	}

	private void SuppressTutorials()
	{
		TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
		for (int i = 0; i < 20; i++)
		{
			tutorial.SetStepTutorialFlg(i);
		}
		for (int j = 0; j < 99; j++)
		{
			tutorial.SetKeyTutorialFlg(j);
		}
	}

	private IEnumerator StartVoyageCoroutine()
	{
		while (true)
		{
			UnityEngine.Debug.Log("StartVoyage [TotalCount]:" + mCycleCount + "  [Total Time]:" + mStopWatch.Elapsed.TotalSeconds);
			StrategyTopTaskManager.GetTurnEnd().DebugTurnEndAuto();
			DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
			yield return StartCoroutine(WaitForCount("Next Start VoyageDebug:", 5));
			yield return StartCoroutine(DeckTeaTime(currentDeck));
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			yield return StartCoroutine(GoFrontMenues());
			yield return StartCoroutine(GoBackMenues());
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			yield return StartCoroutine(WaitForCount("Next GoSortie", 5));
			yield return StartCoroutine(GoSortie());
			mCycleCount++;
			if (Input.GetKey(KeyCode.Joystick1Button6) || Input.GetKey(KeyCode.LeftShift))
			{
				break;
			}
			UnityEngine.Debug.Log("EndOfVoyage [TotalCount]:" + mCycleCount + "  [Total Time]:" + mStopWatch.Elapsed.TotalSeconds);
		}
		if (CommonPopupDialog.Instance != null)
		{
			CommonPopupDialog.Instance.StartPopup("オ\u30fcト戦闘キャンセル");
		}
		mStartVoyageCoroutine = null;
	}

	private IEnumerator DeckTeaTime(DeckModel deckModel)
	{
		yield return StartCoroutine(WaitForCount("Next (ง \u02d8ω\u02d8 )ว", 5));
		new Debug_Mod();
		Debug_Mod.DeckRefresh(deckModel.Id);
	}

	private IEnumerator GoBackMenues()
	{
		yield return StartCoroutine(WaitForCount("Next Record", 5));
		SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.Record);
		yield return StartCoroutine(WaitForCount("Next Album", 5));
		SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.Album);
		yield return StartCoroutine(WaitForCount("Next Item", 5));
		SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.Item);
		yield return StartCoroutine(WaitForCount("Next Interior", 5));
		SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.Interior);
		yield return StartCoroutine(WaitForCount("SaveLoad", 5));
		SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.SaveLoad);
	}

	private IEnumerator WaitForCount(string message, int seconds)
	{
		for (int i = 0; i < seconds; i++)
		{
			if (CommonPopupDialog.Instance != null)
			{
				CommonPopupDialog.Instance.StartPopup(message + ":" + (seconds - i));
			}
			yield return new WaitForSeconds(1f);
		}
	}

	private IEnumerator GoFrontMenues()
	{
		yield return StartCoroutine(WaitForCount("Next Organize", 5));
		SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.Organize);
		yield return StartCoroutine(WaitForCount("Next Remodel", 5));
		SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.Remodel);
		yield return StartCoroutine(WaitForCount("Next Arsenal", 5));
		SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.Arsenal);
		yield return StartCoroutine(WaitForCount("Next Duty", 5));
		SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.Duty);
		yield return StartCoroutine(WaitForCount("Next Repair", 5));
		SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.Repair);
		yield return StartCoroutine(WaitForCount("Next Supply", 5));
		SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.Supply);
	}

	private IEnumerator GoSortie()
	{
		SortieManager sortieManager = new SortieManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
		MapModel mapModel = sortieManager.Maps[0];
		RetentionData.SetData(new Hashtable
		{
			{
				"sortieMapManager",
				sortieManager.GoSortie(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id, mapModel.MstId)
			},
			{
				"rootType",
				0
			},
			{
				"shipRecoveryType",
				ShipRecoveryType.None
			},
			{
				"escape",
				false
			}
		});
		Object.Destroy(SingletonMonoBehaviour<PortObjectManager>.Instance.gameObject);
		Object.Destroy(GameObject.Find("Information Root"));
		Object.Destroy(GameObject.Find("OverView"));
		yield return Application.LoadLevelAsync(Generics.Scene.SortieAreaMap.ToString());
		while (Application.loadedLevelName != "SortieAreaMap")
		{
			yield return StartCoroutine(WaitForCount("Wait For sortieareamap", 5));
		}
		while (Application.loadedLevelName != "Strategy")
		{
			if ((bool)GameObject.Find("BattleStartBtn"))
			{
				GameObject.Find("BattleStartBtn").GetComponent<UIButton>().onClick[0].Execute();
			}
			if ((bool)GameObject.Find("SelectTouchesArea"))
			{
				GameObject.Find("SelectTouchesArea").SendMessage("OnClick");
			}
			if ((bool)GameObject.Find("NextBtn"))
			{
				GameObject.Find("NextBtn").SendMessage("OnClick");
			}
			if ((bool)GameObject.Find("GearBtn"))
			{
				GameObject.Find("GearBtn").SendMessage("OnClick");
			}
			if ((bool)GameObject.Find("WithdrawalHexExBtn"))
			{
				GameObject.Find("WithdrawalHexExBtn").GetComponent<UIWithdrawalButton>().OnDecide();
			}
			if ((bool)GameObject.Find("WithdrawalExBtn"))
			{
				GameObject.Find("WithdrawalExBtn").GetComponent<UIAdvancingWithDrawalButton>().OnDecide();
			}
			yield return StartCoroutine(WaitForCount("Wait For Strategy", 3));
		}
	}
}
