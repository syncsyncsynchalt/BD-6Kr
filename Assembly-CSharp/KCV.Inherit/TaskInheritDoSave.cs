using Common.SaveManager;
using System;
using UnityEngine;

namespace KCV.Inherit
{
	public class TaskInheritDoSave : SceneTaskMono, ISaveDataOperator
	{
		private VitaSaveManager vitaSaveManager;

		public Action OnSavedCallBack;

		[SerializeField]
		private UILabel Message;

		protected override void Start()
		{
			vitaSaveManager = VitaSaveManager.Instance;
		}

		protected override bool Run()
		{
			return true;
		}

		protected override bool Init()
		{
			vitaSaveManager.Open(this);
			Debug.Log("OpenSaveManager");
			vitaSaveManager.Save();
			return true;
		}

		public void SaveManOpen()
		{
			Debug.Log("SaveManOpen");
		}

		public void SaveManClose()
		{
			Debug.Log("SaveManClose");
		}

		public void Canceled()
		{
			Debug.Log("Save Cancel");
			vitaSaveManager.Close();
			InheritSaveTaskManager.ReqMode(InheritSaveTaskManager.InheritTaskManagerMode.InheritTaskManagerMode_ST);
			Close();
		}

		public void SaveError()
		{
			Debug.Log("SaveError");
		}

		public void SaveComplete()
		{
			Debug.Log("OnSaved");
			OnSavedCallBack();
			vitaSaveManager.Save();
		}

		public void LoadError()
		{
		}

		public void LoadComplete()
		{
		}

		public void LoadNothing()
		{
		}

		public void DeleteComplete()
		{
		}
	}
}
