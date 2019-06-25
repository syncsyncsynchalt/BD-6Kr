using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
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
					RaycastResult raycastResult = default(RaycastResult);
					raycastResult.gameObject = array[i].collider.gameObject;
					raycastResult.module = this;
					raycastResult.distance = Vector3.Distance(eventCamera.transform.position, array[i].transform.position);
					raycastResult.worldPosition = array[i].point;
					raycastResult.worldNormal = array[i].normal;
					raycastResult.screenPosition = eventData.position;
					raycastResult.index = resultAppendList.Count;
					raycastResult.sortingLayer = ((component != null) ? component.sortingLayerID : 0);
					raycastResult.sortingOrder = ((component != null) ? component.sortingOrder : 0);
					RaycastResult item = raycastResult;
					resultAppendList.Add(item);
				}
			}
		}
	}
}
