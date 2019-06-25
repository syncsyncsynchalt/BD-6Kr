using Common.Enum;
using local.models;
using System.Collections.Generic;

namespace KCV.Scene.Practice
{
	public class BattlePracticeContext
	{
		public enum PlayType
		{
			Battle,
			ShortCutBattle
		}

		public PlayType BattleStartType
		{
			get;
			private set;
		}

		public List<IsGoCondition> Conditions
		{
			get;
			private set;
		}

		public DeckModel FriendDeck
		{
			get;
			private set;
		}

		public DeckModel TargetDeck
		{
			get;
			private set;
		}

		public BattleFormationKinds1 FormationType
		{
			get;
			private set;
		}

		public BattlePracticeContext()
		{
			BattleStartType = PlayType.Battle;
		}

		public void SetBattleType(PlayType battleType)
		{
			BattleStartType = battleType;
		}

		public void SetTargetDeck(DeckModel deck)
		{
			TargetDeck = deck;
		}

		public void SetFriendDeck(DeckModel deck)
		{
			FriendDeck = deck;
		}

		public void SetConditions(List<IsGoCondition> conditions)
		{
			Conditions = conditions;
		}

		public void SetFormationType(BattleFormationKinds1 kind)
		{
			FormationType = kind;
		}
	}
}
