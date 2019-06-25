using Common.Enum;
using local.models;
using System.Collections.Generic;

namespace local.utils
{
	public static class Logging
	{
		public static void log(params object[] messages)
		{
			string text = string.Empty;
			for (int i = 0; i < messages.Length; i++)
			{
				text = text + messages[i].ToString() + "  ";
			}
			NGUIDebug.Log(text);
		}

		public static void log(DeckModel[] decks)
		{
			string empty = string.Empty;
			for (int i = 0; i < decks.Length; i++)
			{
				log($"{decks[i]}");
			}
			log(empty);
		}

		public static void log(ShipModel[] ships)
		{
			string text = string.Empty;
			for (int i = 0; i < ships.Length; i++)
			{
				ShipModel shipModel = ships[i];
				if (shipModel == null)
				{
					text += $"[{i}] - \n";
					continue;
				}
				text += $"[{i}]{shipModel.ShipTypeName} {shipModel.ShortName} Lv:{shipModel.Level}";
				text += $" {shipModel.NowHp}/{shipModel.MaxHp}({shipModel.TaikyuRate:F}% - {shipModel.DamageStatus})";
				text += $" 疲労度:{shipModel.Condition}";
				DeckModelBase deck = shipModel.getDeck();
				if (deck != null && deck is DeckModel)
				{
					text += string.Format(" [艦隊ID:{0}{1}に編成中]", ((DeckModel)deck).Id, (!shipModel.IsInActionEndDeck()) ? string.Empty : "(行動終了済)");
				}
				else if (deck != null && deck is EscortDeckModel)
				{
					text += $" [護衛艦隊{((EscortDeckModel)deck).Id}に編成中]";
				}
				text += string.Format(" {0}", (!shipModel.IsLocked()) ? string.Empty : "[ロック]");
				text += string.Format(" {0}", (!shipModel.IsInRepair()) ? string.Empty : "[入渠中]");
				text += string.Format(" {0}", (!shipModel.IsInMission()) ? string.Empty : "[遠征中]");
				text += string.Format(" {0}", (!shipModel.IsBling()) ? string.Empty : "[回航中]");
				if (shipModel.IsBlingWait())
				{
					text += $"[回航待ち中(Area:{shipModel.AreaIdBeforeBlingWait})]";
				}
				text += string.Format(" {0}", (!shipModel.IsTettaiBling()) ? string.Empty : "[撤退中]");
				text += $" mstID:{shipModel.MstId} memID:{shipModel.MemId}";
				text += $"\n";
			}
			log(text);
		}

		public static void log(SlotitemModel[] items)
		{
			string text = string.Empty;
			for (int i = 0; i < items.Length; i++)
			{
				SlotitemModel slotitemModel = items[i];
				text = ((slotitemModel != null) ? (text + $"[{i}]{slotitemModel.ShortName}\n") : (text + $"[{i}] - \n"));
			}
			log(text);
		}

		public static void log(ItemlistModel[] items)
		{
			string text = string.Empty;
			foreach (ItemlistModel itemlistModel in items)
			{
				text = ((itemlistModel == null) ? (text + " - \n") : (text + string.Format("{0}{1}\n", itemlistModel, (!itemlistModel.IsUsable()) ? string.Empty : "[使用可能]")));
			}
			log(text);
		}

		public static void log(ItemStoreModel[] items)
		{
			string text = string.Empty;
			foreach (ItemStoreModel itemStoreModel in items)
			{
				text = ((itemStoreModel == null) ? (text + " - \n") : (text + $"{itemStoreModel}\n"));
			}
			log(text);
		}

		public static void log(IAlbumModel[] data)
		{
			for (int i = 0; i < data.Length; i++)
			{
				if (data[i] != null)
				{
					log($"[{i}]{data[i]}");
				}
			}
		}

		public static void log(List<IReward> rewards)
		{
			foreach (IReward reward in rewards)
			{
				log(reward);
			}
		}

		public static string ToString(List<IsGoCondition> conds)
		{
			string text = string.Empty;
			for (int i = 0; i < conds.Count; i++)
			{
				text += conds[i];
				if (i < conds.Count)
				{
					text += ", ";
				}
			}
			return text;
		}
	}
}
