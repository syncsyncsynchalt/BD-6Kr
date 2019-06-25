using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Organize
{
	public class LockSwitch : MonoBehaviour
	{
		[SerializeField]
		private UISprite lockBtn;

		[SerializeField]
		private UISprite lockBg;

		private ShipModel ship;

		private Action ChangeListViewIcon;

		private void OnDestroy()
		{
			Mem.Del(ref lockBtn);
			Mem.Del(ref lockBg);
			Mem.Del(ref ship);
			Mem.Del(ref ChangeListViewIcon);
		}

		public void setIcon(ShipModel ship)
		{
			this.ship = ship;
			moveIcon(0f);
		}

		public void setChangeListViewIcon(Action act)
		{
			ChangeListViewIcon = act;
		}

		public void MoveLockBtn()
		{
			ChangeListViewIcon();
			moveIcon(0.2f);
		}

		private void moveIcon(float time)
		{
			if (time > 0f)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add("time", time);
				hashtable.Add("x", (!ship.IsLocked()) ? 47f : (-47f));
				hashtable.Add("easeType", iTween.EaseType.linear);
				hashtable.Add("isLocal", true);
				hashtable.Add("oncomplete", (!ship.IsLocked()) ? "compMoveUnLock" : "compMoveLock");
				hashtable.Add("oncompletetarget", base.gameObject);
				lockBtn.transform.gameObject.MoveTo(hashtable);
			}
			else
			{
				lockBtn.transform.localPositionX((!ship.IsLocked()) ? 47f : (-47f));
				if (ship.IsLocked())
				{
					compMoveLock();
				}
				else
				{
					compMoveUnLock();
				}
			}
		}

		private void compMoveLock()
		{
			lockBg.spriteName = "switch_lock_on";
			lockBtn.spriteName = "switch_lock_on_pin";
		}

		private void compMoveUnLock()
		{
			lockBg.spriteName = "switch_lock";
			lockBtn.spriteName = "switch_lock_pin";
		}
	}
}
