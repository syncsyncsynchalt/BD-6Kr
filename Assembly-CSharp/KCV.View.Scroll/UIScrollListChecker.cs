using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.View.Scroll
{
	public class UIScrollListChecker : MonoBehaviour
	{
		public UIScrollShipListParent mUIScrollShipListParent;

		[SerializeField]
		private UILabel mLabel_Debug;

		[SerializeField]
		private UIScrollListShipInfo mUIScrollListShipInfo;

		private KeyControl mKeyController;

		private bool mGCSwitch;

		private float UPDATE_GC_COLLECT_MILLI_SEC = 2f;

		private void Start()
		{
			StartCoroutine(ManualGCCollection());
			ShipModel[] shipList = new OrganizeManager(1).GetShipList();
			List<ShipModel> list = new List<ShipModel>();
			list.AddRange(shipList);
			mUIScrollShipListParent.Initialize(list.ToArray());
			UpdateDebugLabel();
			mUIScrollShipListParent.SetOnUIScrollListParentAction(delegate(ActionType actionType, UIScrollListParent<ShipModel, UIScrollShipListChild> calledOjbect, UIScrollListChild<ShipModel> child)
			{
				switch (actionType)
				{
				case ActionType.OnTouch:
					Debug.Log("Called ListItemTouch Name:" + child.Model.ToString());
					break;
				case ActionType.OnButtonSelect:
					Debug.Log("Called ListItemSelect" + child.Model.ToString());
					break;
				case ActionType.OnChangeFocus:
				case ActionType.OnChangeFirstFocus:
					Debug.Log("Called OnChangeFocus" + child.Model.ToString());
					mUIScrollListShipInfo.Initialize(child.Model);
					break;
				}
			});
			mKeyController = new KeyControl();
			mUIScrollShipListParent.SetKeyController(mKeyController);
		}

		private void Update()
		{
			if (mKeyController != null)
			{
				mKeyController.Update();
				if (mKeyController.keyState[8].down)
				{
					mGCSwitch = true;
					UpdateDebugLabel();
				}
				else if (mKeyController.keyState[12].down)
				{
					mGCSwitch = false;
					UpdateDebugLabel();
				}
				else if (mKeyController.keyState[3].down)
				{
					UPDATE_GC_COLLECT_MILLI_SEC -= 0.1f;
					UpdateDebugLabel();
				}
				else if (mKeyController.keyState[0].down)
				{
					UPDATE_GC_COLLECT_MILLI_SEC += 0.1f;
					UpdateDebugLabel();
				}
				else if (mKeyController.keyState[6].down)
				{
					Application.LoadLevel("Strategy");
				}
			}
		}

		private IEnumerator ManualGCCollection()
		{
			float lastGcTime = 0f;
			while (true)
			{
				lastGcTime += Time.deltaTime;
				if (UPDATE_GC_COLLECT_MILLI_SEC < lastGcTime && mGCSwitch)
				{
					GC.Collect();
					lastGcTime = 0f;
					Debug.Log(" == Called GC == ");
				}
				yield return null;
			}
		}

		private void UpdateDebugLabel()
		{
			mLabel_Debug.text = "GC:" + ((!mGCSwitch) ? "Off" : "On") + " GCSpeed::" + UPDATE_GC_COLLECT_MILLI_SEC;
		}
	}
}
