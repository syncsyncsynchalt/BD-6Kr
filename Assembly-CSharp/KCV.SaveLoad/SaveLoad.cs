using Common.SaveManager;
using local.managers;
using Server_Common;
using System.Collections;
using UnityEngine;

namespace KCV.SaveLoad
{
	public class SaveLoad : MonoBehaviour, ISaveDataOperator
	{
		public enum Execute_Mode
		{
			Load_Mode = 1,
			Save_Mode,
			Delete_Mode
		}

		public enum Now_State
		{
			Idle,
			Saving,
			Loading,
			Deleting,
			Exiting
		}

		private SoundManager _SM;

		private VitaSaveManager _instance;

		private string _SceneName;

		private UILabel _Label_status;

		private KeyControl ItemSelectController;

		private Execute_Mode _Execute_Mode;

		private Now_State _now_status;

		private Generics.Scene BackScene;

		private void Start()
		{
			_SM = SingletonMonoBehaviour<SoundManager>.Instance;
			_instance = VitaSaveManager.Instance;
			_instance.Open(this);
			_Label_status = GameObject.Find("Label_status").GetComponent<UILabel>();
			BackScene = Generics.Scene.PortTop;
			_Set_Status(Now_State.Idle);
			_SceneName = Application.loadedLevelName;
			Debug.Log("Application.loadedLevelName: " + _SceneName);
			Hashtable hashtable = null;
			if (RetentionData.GetData() != null)
			{
				hashtable = RetentionData.GetData();
			}
			if (hashtable == null || (int)hashtable["rootType"] != 1)
			{
				_Set_Execute_Mode(Execute_Mode.Save_Mode);
				if ((int)hashtable["rootType"] == 21)
				{
					BackScene = Generics.Scene.Strategy;
				}
			}
			else
			{
				_Set_Execute_Mode(Execute_Mode.Load_Mode);
			}
			if (hashtable != null)
			{
				RetentionData.Release();
			}
			if (_Execute_Mode == Execute_Mode.Load_Mode)
			{
				DebugUtils.SLog("ロードを実行します");
				_DO_LOAD();
			}
			else if (_Execute_Mode == Execute_Mode.Save_Mode)
			{
				Debug.Log("セーブを実行します");
				_DO_SAVE();
				SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(null);
			}
		}

		private void _Set_Status(Now_State stat)
		{
			_now_status = stat;
		}

		public void _Set_Execute_Mode(Execute_Mode mode)
		{
			_Execute_Mode = mode;
		}

		private void _DO_SAVE()
		{
			Debug.Log("saveする");
			if (_now_status == Now_State.Idle)
			{
				_Set_Status(Now_State.Saving);
				GameObject.Find("Label_status").GetComponent<UILabel>().text = string.Empty;
				_instance.Save();
			}
		}

		private void _DO_LOAD()
		{
			if (_now_status == Now_State.Idle)
			{
				_Set_Status(Now_State.Loading);
				GameObject.Find("Label_status").GetComponent<UILabel>().text = string.Empty;
				DebugUtils.SLog("Loadする");
				_instance.Load();
			}
		}

		private void _DO_DELETE()
		{
			Debug.Log("Deleteする");
			if (_now_status == Now_State.Idle)
			{
				_Set_Status(Now_State.Deleting);
				GameObject.Find("Label_status").GetComponent<UILabel>().text = "デリートdialog";
				_instance.Delete();
			}
		}

		private void back_to_port()
		{
			back_to_port(saveloadSuccess: true);
		}

		private void back_to_port(bool saveloadSuccess)
		{
			if (_now_status != 0)
			{
				return;
			}
			_Set_Status(Now_State.Exiting);
			_instance.Close();
			if (_Execute_Mode == Execute_Mode.Save_Mode)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(BackScene);
			}
			else if (saveloadSuccess)
			{
				DebugUtils.SLog("AppInitializeManager.IsInitialize = true;");
				AppInitializeManager.IsInitialize = true;
				DebugUtils.SLog("ManagerBase.initialize();");
				ManagerBase.initialize();
				DebugUtils.SLog("引継ぎ判定");
				if (Server_Common.Utils.IsValidNewGamePlus() && Server_Common.Utils.IsGameClear())
				{
					LoadedInheritData();
					return;
				}
				DebugUtils.SLog("通常データ");
				LoadedNormalData();
			}
			else
			{
				Debug.Log("タイトルに戻ります。");
				Application.LoadLevel(Generics.Scene.Title.ToString());
			}
		}

		private void LoadedNormalData()
		{
			SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(Resources.Load("Sounds/Voice/kc9999/" + $"{XorRandom.GetILim(206, 211):D2}") as AudioClip, 0);
			DebugUtils.SLog("戦略マップへ進みます。");
			SingletonMonoBehaviour<AppInformation>.Instance.NextLoadType = AppInformation.LoadType.Ship;
			SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
			Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
		}

		private void LoadedInheritData()
		{
			Application.LoadLevel(Generics.Scene.InheritLoad.ToString());
		}

		public void Canceled()
		{
			Debug.Log("Save/Load/Delete dialog Cancelled.");
			_Set_Status(Now_State.Idle);
			_Label_status.text = string.Empty;
			back_to_port(saveloadSuccess: false);
		}

		public void SaveError()
		{
			Debug.Log("Save Error.");
			_Set_Status(Now_State.Idle);
			StartCoroutine(RightDown_Message("セーブ時にエラーが発生しました。"));
			back_to_port(saveloadSuccess: false);
		}

		public void SaveComplete()
		{
			Debug.Log("Save Complete.");
			_Set_Status(Now_State.Idle);
			_Label_status.text = string.Empty;
			_instance.Save();
		}

		public void LoadError()
		{
			DebugUtils.SLog("Load Error.");
			_Set_Status(Now_State.Idle);
			StartCoroutine(RightDown_Message("ロード時に内部エラーが発生しました。"));
			back_to_port(saveloadSuccess: false);
		}

		public void LoadComplete()
		{
			Debug.Log("Load Complete.");
			_Set_Status(Now_State.Idle);
			_Label_status.text = string.Empty;
			back_to_port();
		}

		public void LoadNothing()
		{
			Debug.Log("Data not found or empty.");
			_Set_Status(Now_State.Idle);
			StartCoroutine(RightDown_Message("ロードデータがありません。"));
			back_to_port(saveloadSuccess: false);
		}

		public void DeleteComplete()
		{
			Debug.Log("Delete Complete.");
			_Set_Status(Now_State.Idle);
			StartCoroutine(RightDown_Message("データを削除しました。"));
			back_to_port();
		}

		public void SaveManOpen()
		{
			Debug.Log("S/L SaveManOpen");
		}

		public void SaveManClose()
		{
			Debug.Log("S/L SaveManClose");
		}

		public IEnumerator RightDown_Message(string msg)
		{
			_Label_status.text = msg;
			yield return new WaitForSeconds(2f);
			_Label_status.text = string.Empty;
		}
	}
}
