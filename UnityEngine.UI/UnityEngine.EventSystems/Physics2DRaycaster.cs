using System.Collections.Generic;

namespace UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("Event/Physics 2D Raycaster")]
public class Physics2DRaycaster : PhysicsRaycaster
{
	protected Physics2DRaycaster()
	{
	}

	public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
	{
		if (eventCamera == null)
		{
			return;
		}
		Ray ray = eventCamera.ScreenPointToRay(eventData.position);
		float distance = eventCamera.farClipPlane - eventCamera.nearClipPlane;
		RaycastHit2D[] array = Physics2D.RaycastAll(ray.origin, ray.direction, distance, base.finalEventMask);
		if (array.Length != 0)
		{
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				SpriteRenderer component = array[i].collider.gameObject.GetComponent<SpriteRenderer>();
				RaycastResult item = new RaycastResult
				{
					gameObject = array[i].collider.gameObject,
					module = this,
					distance = Vector3.Distance(eventCamera.transform.position, array[i].transform.position),
					worldPosition = array[i].point,
					worldNormal = array[i].normal,
					screenPosition = eventData.position,
					index = resultAppendList.Count,
					sortingLayer = ((component != null) ? component.sortingLayerID : 0),
					sortingOrder = ((component != null) ? component.sortingOrder : 0)
				};
				resultAppendList.Add(item);
			}
		}
	}
}
