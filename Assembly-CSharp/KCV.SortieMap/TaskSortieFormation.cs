using Common.Enum;
using KCV.Battle.Formation;
using KCV.SortieBattle;
using Librarys.State;
using local.managers;
using System.Collections;
using UnityEngine;

namespace KCV.SortieMap
{
	public class TaskSortieFormation : Task
	{
		private UIBattleFormationKindSelectManager _uiBattleFormationSelector;

		protected override void Dispose(bool isDisposing)
		{
			Mem.Del(ref _uiBattleFormationSelector);
		}

		protected override bool Init()
		{
			App.TimeScale(1f);
			UIShortCutSwitch shortCutSwitch = SortieMapTaskManager.GetShortCutSwitch();
			shortCutSwitch.Hide();
			SortieMapTaskManager.GetUIShipCharacter().ShowInFormation(50, null);
			BattleFormationKinds1[] formationArray = SortieUtils.GetFormationArray(SortieBattleTaskManager.GetMapManager().Deck);
			if (1 < formationArray.Length)
			{
				_uiBattleFormationSelector = Util.Instantiate(SortieMapTaskManager.GetPrefabFile().prefabUIBattleFormationKindSelectManager.gameObject, SortieMapTaskManager.GetSharedPlace().gameObject).GetComponent<UIBattleFormationKindSelectManager>();
				SortieMapTaskManager.GetUIAreaMapFrame().SetMessage("陣形を選択してください。");
				_uiBattleFormationSelector.Initialize(GameObject.Find("SortieAreaCamera").GetComponent<Camera>(), formationArray);
				_uiBattleFormationSelector.SetOnUIBattleFormationKindSelectManagerAction(UIBattleFormationKindSelectManagerActionCallBack);
				_uiBattleFormationSelector.SetKeyController(SortieBattleTaskManager.GetKeyControl());
			}
			else
			{
				OnFormationSelected(BattleFormationKinds1.TanJuu);
			}
			return true;
		}

		protected override bool UnInit()
		{
			Mem.DelComponentSafe(ref _uiBattleFormationSelector);
			return true;
		}

		protected override bool Update()
		{
			if (_uiBattleFormationSelector != null)
			{
				_uiBattleFormationSelector.OnUpdatedKeyController();
			}
			if (SortieMapTaskManager.GetMode() != SortieMapTaskManagerMode.SortieMapTaskManagerMode_BEF)
			{
				return (SortieMapTaskManager.GetMode() == SortieMapTaskManagerMode.Formation) ? true : false;
			}
			return true;
		}

		private void UIBattleFormationKindSelectManagerActionCallBack(UIBattleFormationKindSelectManager.ActionType actionType, UIBattleFormationKindSelectManager calledObject, UIBattleFormationKind centerView)
		{
			calledObject.OnReleaseKeyController();
			BattleFormationKinds1 iFormation = (actionType != 0) ? BattleFormationKinds1.TanJuu : centerView.Category;
			OnFormationSelected(iFormation);
		}

		private void OnFormationSelected(BattleFormationKinds1 iFormation)
		{
			SortieMapTaskManager.GetUIShipCharacter().Hide(null);
			SortieMapTaskManager.GetUIAreaMapFrame().Hide();
			ProdSortieTransitionToBattle prodSortieTransitionToBattle = SortieBattleTaskManager.GetSortieBattlePrefabFile().prodSortieTransitionToBattle;
			SortieBattleTaskManager.GetTransitionCamera().enabled = true;
			prodSortieTransitionToBattle.Play(delegate
			{
				Hashtable hashtable = new Hashtable();
				if (SortieBattleTaskManager.GetMapManager().GetType().Equals(typeof(RebellionMapManager)))
				{
					hashtable.Add("rootType", Generics.BattleRootType.Rebellion);
					hashtable.Add("rebellionMapManager", SortieBattleTaskManager.GetMapManager());
				}
				else
				{
					hashtable.Add("rootType", Generics.BattleRootType.SortieMap);
					hashtable.Add("sortieMapManager", SortieBattleTaskManager.GetMapManager());
				}
				hashtable.Add("formation", iFormation);
				RetentionData.SetData(hashtable);
				SortieBattleTaskManager.ReqMode(SortieBattleMode.Battle);
			});
		}
	}
}
