using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesConvertShipReturnButton : MonoBehaviour
	{
		private PortUpgradesConvertShipManager manager;

		private GameObject cog;

		private bool active;

		private bool hit;

		private KeyControl keyController;

		public void Awake()
		{
			try
			{
				manager = ((Component)base.transform.parent.parent).GetComponent<PortUpgradesConvertShipManager>();
			}
			catch (NullReferenceException)
			{
				Debug.Log("../.. not found in PortUpgradesConvertShipReturnButton.cs");
			}
			if (manager == null)
			{
				Debug.Log("PortUpgradesConvertShipManager.cs is not attached to ../..");
			}
			cog = base.transform.GetChild(1).gameObject;
			if (cog == null)
			{
				Debug.Log("/Cog not found in ReceiveShipNextButton.cs");
			}
			try
			{
				cog.GetComponent<UISprite>().alpha = 0f;
			}
			catch (NullReferenceException)
			{
				Debug.Log("UISprite.cs is not attached to /Cog");
			}
			try
			{
				((Component)base.transform.GetChild(0)).GetComponent<UISprite>().alpha = 0f;
			}
			catch (NullReferenceException)
			{
				Debug.Log("UISprite.cs is not attached to /Inner");
			}
			active = false;
			keyController = new KeyControl();
		}

		public void Update()
		{
			if (active && !hit)
			{
				cog.transform.Rotate(50f * Time.deltaTime * Vector3.forward);
			}
		}

		public void OnHover(bool isOver)
		{
			hit = isOver;
		}

		public void OnClick()
		{
			manager.Finish();
		}

		public void Activate()
		{
			NGUITools.AddWidgetCollider(base.gameObject);
			try
			{
				cog.GetComponent<UISprite>().alpha = 1f;
			}
			catch (NullReferenceException)
			{
				Debug.Log("UISprite.cs is not attached to /Cog");
			}
			try
			{
				((Component)base.transform.GetChild(0)).GetComponent<UISprite>().alpha = 1f;
			}
			catch (NullReferenceException)
			{
				Debug.Log("UISprite.cs is not attached to /Inner");
			}
			active = true;
		}

		private void OnDestroy()
		{
			manager = null;
			cog = null;
			keyController = null;
		}
	}
}
