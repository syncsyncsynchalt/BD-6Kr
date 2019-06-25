using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TransformViewportToWorld : MonoBehaviour
{
	public Vector3 lb;

	public Vector3 rt;

	public Vector3 lt;

	public Vector3 rb;

	public Transform center;

	private GameObject mapBG;

	private float mapX1;

	private float mapX2;

	private float mapY1;

	private float mapY2;

	private Vector3[] worldPosBG;

	private void Start()
	{
		mapBG = GameObject.Find("Map_BG");
		Vector3 position = mapBG.transform.position;
		UITexture component = mapBG.GetComponent<UITexture>();
		float num = component.width;
		float num2 = component.height;
		mapX1 = position.x - num / 2f;
		mapX2 = position.x + num / 2f;
		mapY1 = position.y - num2 / 2f;
		mapY2 = position.y + num2 / 2f;
		worldPosBG = new Vector3[4];
		worldPosBG[0] = component.worldCorners[0];
		worldPosBG[1] = component.worldCorners[1];
		worldPosBG[2] = component.worldCorners[2];
		worldPosBG[3] = component.worldCorners[3];
	}

	private void Update()
	{
		float z = Vector3.Distance(base.transform.position, center.position);
		lb = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0f, 0f, z));
		rt = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1f, 1f, z));
		lt = new Vector3(lb.x, rt.y, lb.z);
		rb = new Vector3(rt.x, lb.y, rt.z);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawSphere(lb, 0.3f);
		Gizmos.DrawSphere(rt, 0.3f);
		Gizmos.DrawSphere(rb, 0.3f);
		Gizmos.DrawSphere(lt, 0.3f);
	}
}
