using Sony.NP;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class TrophyManager : SingletonMonoBehaviour<TrophyManager>
	{
		private class TrophyData
		{
			public int Id;

			public bool unlocked;

			public TrophyData(Trophies.TrophyData data)
			{
				Id = data.trophyId;
				unlocked = data.unlocked;
			}

			public override string ToString()
			{
				string empty = string.Empty;
				return string.Format("[Trophy{0:D2}{1} {2}]{3}", Id, (!unlocked) ? "(未)" : "(済)", empty, (Id % 2 != 1) ? "  " : "\n");
			}
		}

		private List<TrophyData> _data;

		private Action<bool> _callback_initialize;

		private Action<bool> _callback_update;

		public Action<string> LogMethod;

		public bool Available => Trophies.TrophiesAreAvailable;

		public int Count => (_data != null) ? _data.Count : 0;

		public void Initialize(Action<bool> callback)
		{
			if (_callback_initialize == null)
			{
				if (Available)
				{
					callback(obj: true);
					return;
				}
				_callback_initialize = callback;
				Main.enableInternalLogging = true;
				Main.OnLog += __EventHandler_Log__;
				Main.OnLogWarning += __EventHandler_Log__;
				Main.OnLogError += __EventHandler_Log__;
				Main.OnNPInitialized += __EventHandler_Initialized__;
				int creationFlags = Main.kNpToolkitCreate_CacheTrophyIcons | Main.kNpToolkitCreate_NoRanking;
				Main.Initialize(creationFlags);
			}
		}

		public void UpdateTrophyInfo(Action<bool> callback)
		{
			if (!Available)
			{
				callback(obj: false);
				return;
			}
			if (Trophies.RequestTrophyInfoIsBusy())
			{
				callback(obj: false);
				return;
			}
			if (_callback_update != null)
			{
				callback(obj: false);
				return;
			}
			_callback_update = callback;
			if (Trophies.RequestTrophyInfo() != 0)
			{
				_callback_update = null;
				callback(obj: false);
			}
		}

		public void UnlockTrophies(List<int> trophy_ids)
		{
			if (!Available || _data == null || _callback_update != null || trophy_ids == null)
			{
				return;
			}
			for (int i = 0; i < trophy_ids.Count; i++)
			{
				int num = trophy_ids[i];
				_Log($"ID:{num} 解除開始");
				TrophyData trophyData = _GetTrophyData(num);
				trophyData.unlocked = true;
				int num2 = _IDtoIndex(trophyData);
				if (num2 > 0 && Trophies.AwardTrophy(num2) != 0)
				{
					trophyData.unlocked = false;
				}
			}
		}

		public bool IsUnlocked(int trophy_id)
		{
			return _GetTrophyData(trophy_id)?.unlocked ?? true;
		}

		private int _IDtoIndex(int trophy_id)
		{
			TrophyData trophyData = _GetTrophyData(trophy_id);
			return _IDtoIndex(trophyData);
		}

		private int _IDtoIndex(TrophyData trophyData)
		{
			if (_data == null)
			{
				return -1;
			}
			if (trophyData == null)
			{
				return -1;
			}
			return _data.IndexOf(trophyData);
		}

		private TrophyData _GetTrophyData(int id)
		{
			if (_data == null)
			{
				return null;
			}
			return _data.Find((TrophyData t) => t.Id == id);
		}

		private void __EventHandler_Initialized__(Messages.PluginMessage msg)
		{
			Trophies.OnGotTrophyInfo += __EventHandler_UpdateTrophyInfo__;
			Action<bool> callback_initialize = _callback_initialize;
			_callback_initialize = null;
			UpdateTrophyInfo(callback_initialize);
		}

		private void __EventHandler_UpdateTrophyInfo__(Messages.PluginMessage msg)
		{
			Trophies.TrophyData[] cachedTrophyData = Trophies.GetCachedTrophyData();
			List<Trophies.TrophyData> list = new List<Trophies.TrophyData>(cachedTrophyData);
			_data = list.ConvertAll((Trophies.TrophyData item) => new TrophyData(item));
			Action<bool> callback_update = _callback_update;
			_callback_update = null;
			callback_update?.Invoke(obj: true);
		}

		private void __EventHandler_UnlockSuccess__(Messages.PluginMessage msg)
		{
		}

		private void __EventHandler_Log__(Messages.PluginMessage msg)
		{
			_Log("[NP Log] " + msg.Text);
		}

		private void Update()
		{
			Main.Update();
		}

		private void OnDestroy()
		{
			_callback_initialize = null;
			_callback_update = null;
			LogMethod = null;
			if (_data != null)
			{
				_data.Clear();
			}
			_data = null;
		}

		private void _Log(string message)
		{
			if (LogMethod != null)
			{
				LogMethod(message);
			}
		}

		public override string ToString()
		{
			string text = "== トロフィ\u30fcデ\u30fcタ ==\n";
			if (_data != null)
			{
				for (int i = 0; i < _data.Count; i++)
				{
					text += _data[i].ToString();
				}
			}
			else
			{
				text += "なし";
			}
			return text;
		}
	}
}
