using System.Collections.Generic;
using UnityEngine;

namespace KCV.Supply
{
	public abstract class AbstractUISupplyIconManager : MonoBehaviour
	{
		protected const int ORIGIN_DEPTH = 6;

		protected const int HIDE_ANIMATION_DEPTH = 20;

		protected double amount;

		private int lastIconObjCount;

		private UIPanel panel;

		protected List<GameObject> iconObjList = new List<GameObject>();

		protected abstract Vector3 getStartPos();

		protected abstract int calculateIconCount();

		protected abstract GameObject createIconObj(int currentIconObjCount, int i);

		protected abstract string getIconObjName();

		protected abstract int getMaxIconObjCount();

		public void Awake()
		{
			panel = GetComponent<UIPanel>();
		}

		public void init(int amount)
		{
			this.amount = amount;
			panel.depth = 6;
			base.transform.localPosition(getStartPos());
			processIcons();
		}

		private void processIcons()
		{
			int num = calculateIconCount();
			if (num > lastIconObjCount)
			{
				int num2 = num - lastIconObjCount;
				for (int i = 0; i < num2; i++)
				{
					if (iconObjList.Count < getMaxIconObjCount())
					{
						iconObjList.Add(createIconObj(iconObjList.Count, i));
					}
				}
			}
			else if (num < lastIconObjCount)
			{
				for (int j = num; j < lastIconObjCount; j++)
				{
					if (j < getMaxIconObjCount())
					{
						int index = iconObjList.Count - 1;
						GameObject iconObj = iconObjList[index];
						ProcessEachIconCancelAnimation(iconObj);
						iconObjList.RemoveAt(index);
					}
				}
			}
			lastIconObjCount = num;
		}

		protected GameObject InstantiateIconObj()
		{
			GameObject gameObject = Util.InstantiateGameObject(Resources.Load("Prefabs/Supply/SupplyIcon") as GameObject, base.transform);
			gameObject.name = getIconObjName() + iconObjList.Count;
			return gameObject;
		}

		protected GameObject ResetSmoke(GameObject iconObj)
		{
			UISprite component = ((Component)iconObj.transform.FindChild("IconObject/Smoke")).GetComponent<UISprite>();
			component.alpha = 0f;
			return iconObj;
		}

		protected void SetIconAnimation(GameObject iconObj, int num)
		{
			Animation component = iconObj.GetComponent<Animation>();
			component.Stop();
			component.Play("SupplyIcon" + (num + 1));
		}

		public void ProcessConsumingAnimation()
		{
			panel.depth = 20;
			base.Invoke("OnCompleteConsumingAnimation", 0f);
			iconObjList.ForEach(delegate(GameObject iconObj)
			{
				ProcessEachIconConsumingAnimation(iconObj);
			});
			iconObjList.Clear();
			lastIconObjCount = 0;
		}

		protected void ProcessEachIconConsumingAnimation(GameObject iconObj)
		{
			Animation component = iconObj.GetComponent<Animation>();
			component.Play("SupplyIconEnd");
			UISprite component2 = ((Component)iconObj.transform.FindChild("IconObject/Icon")).GetComponent<UISprite>();
			GameObjectExtensionMethods.RotatoTo(rot: (new Vector3[2]
			{
				new Vector3(0f, 0f, XorRandom.GetILim(180, 270)),
				new Vector3(0f, 0f, XorRandom.GetILim(-270, -180))
			})[XorRandom.GetILim(0, 1)], obj: component2.transform.gameObject, time: 1f, callback: null);
		}

		public void ProcessCancelAnimation()
		{
			iconObjList.ForEach(delegate(GameObject iconObj)
			{
				ProcessEachIconCancelAnimation(iconObj);
			});
			iconObjList.Clear();
			lastIconObjCount = 0;
		}

		protected void ProcessEachIconCancelAnimation(GameObject iconObj)
		{
			Animation component = iconObj.GetComponent<Animation>();
			component.Play("SupplyIconLost");
		}

		public void OnCompleteConsumingAnimation()
		{
			SupplyMainManager.Instance.change_2_SHIP_RECOVERY_ANIMATION();
		}
	}
}
