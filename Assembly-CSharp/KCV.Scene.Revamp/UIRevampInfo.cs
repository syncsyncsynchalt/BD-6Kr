using local.managers;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIRevampInfo : MonoBehaviour
	{
		private class RevampMaterial
		{
			private Transform _traMaterial;

			private UILabel _uiDevKit;

			private UILabel _uiRevKit;

			public RevampMaterial(Transform parent, string objName)
			{
				Util.FindParentToChild(ref _traMaterial, parent, objName);
				Util.FindParentToChild(ref _uiDevKit, _traMaterial, "DevKit");
				Util.FindParentToChild(ref _uiRevKit, _traMaterial, "RevKit");
			}

			public void SetMaterial(RevampManager manager)
			{
				_uiDevKit.textInt = manager.Material.Devkit;
				_uiRevKit.textInt = manager.Material.Revkit;
			}
		}

		private RevampMaterial _clsMaterial;

		private void Awake()
		{
			_clsMaterial = new RevampMaterial(base.transform, "RevampMaterial");
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void SetMaterial(RevampManager manager)
		{
			_clsMaterial.SetMaterial(manager);
		}
	}
}
