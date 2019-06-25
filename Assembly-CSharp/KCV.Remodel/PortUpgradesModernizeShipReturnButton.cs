using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesModernizeShipReturnButton : MonoBehaviour
	{
		private PortUpgradesModernizeShipManager manager;

		private GameObject cog;

		private bool active;

		private bool hit;

		private bool done;

		private KeyControl keyController;

		public void Awake()
		{
			try
			{
				manager = ((Component)base.transform.parent).GetComponent<PortUpgradesModernizeShipManager>();
			}
			catch (NullReferenceException)
			{
				Debug.Log(".. not found in PortUpgradesModernizeShipReturnButton.cs");
			}
			if (manager == null)
			{
				Debug.Log("PortUpgradesModernizeShipManager.cs is not attached to ..");
			}
			cog = base.transform.GetChild(0).gameObject;
			if (cog == null)
			{
				Debug.Log("/Cog not found in PortUpgradesModernizeShipReturnButton.cs");
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
				((Component)base.transform.GetChild(1)).GetComponent<UISprite>().alpha = 0f;
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
			keyController.Update();
			if (active && !hit)
			{
				cog.transform.Rotate(50f * Time.deltaTime * Vector3.forward);
			}
			if (active && !done && keyController.IsMaruDown())
			{
				done = true;
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					manager.isFinished = true;
				});
			}
		}

		public void OnHover(bool isOver)
		{
			hit = isOver;
		}

		public void OnClick()
		{
			SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
			{
				manager.isFinished = true;
			});
			manager.Finish();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
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
				((Component)base.transform.GetChild(1)).GetComponent<UISprite>().alpha = 1f;
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
		}
	}
}
