using UnityEngine;

[ExecuteInEditMode]
public class OrderAdjuster : MonoBehaviour
{
	public enum OrderAdjusterMode
	{
		ZAxisOrder,
		DepthOrder
	}

	public OrderAdjusterMode AjusterMode = OrderAdjusterMode.DepthOrder;

	public int OderDepthOffs = 1;

	public bool isAdjust;

	private void Update()
	{
	}

	private void AdjustZ(Transform tf)
	{
		if (isAdjust)
		{
			foreach (Transform item in tf)
			{
				UIWidget component = ((Component)item).GetComponent<UIWidget>();
				UIPanel component2 = ((Component)item).GetComponent<UIPanel>();
				switch (AjusterMode)
				{
				case OrderAdjusterMode.DepthOrder:
					if ((bool)component)
					{
						Transform transform2 = item;
						Vector3 localPosition3 = item.localPosition;
						float x = localPosition3.x;
						Vector3 localPosition4 = item.localPosition;
						transform2.localPosition = new Vector3(x, localPosition4.y, (0f - (float)component.depth) * (float)OderDepthOffs);
					}
					else
					{
						AdjustZ(item);
					}
					if ((bool)component2)
					{
						Transform transform3 = item;
						Vector3 localPosition5 = item.localPosition;
						float x2 = localPosition5.x;
						Vector3 localPosition6 = item.localPosition;
						transform3.localPosition = new Vector3(x2, localPosition6.y, (0f - (float)component2.depth) * (float)OderDepthOffs);
					}
					else
					{
						AdjustZ(item);
					}
					break;
				case OrderAdjusterMode.ZAxisOrder:
					if ((bool)component)
					{
						UIWidget uIWidget = component;
						Vector3 localPosition = item.localPosition;
						uIWidget.depth = -(int)localPosition.z * OderDepthOffs;
					}
					else
					{
						AdjustZ(item);
					}
					if ((bool)component2)
					{
						UIPanel uIPanel = component2;
						Vector3 localPosition2 = item.localPosition;
						uIPanel.depth = -(int)localPosition2.z * OderDepthOffs;
					}
					else
					{
						AdjustZ(item);
					}
					break;
				}
			}
		}
	}
}
