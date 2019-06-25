using UnityEngine;

public class InstantiateObject<T> : MonoBehaviour where T : Component
{
	public static T Instantiate(T prefab)
	{
		return Object.Instantiate(prefab);
	}

	public static T Instantiate(T prefab, Transform parent)
	{
		T result = Instantiate(prefab);
		result.transform.parent = parent;
		result.transform.localScaleOne();
		result.transform.localPositionZero();
		return result;
	}

	public static T Instantiate(T prefab, Transform parent, params object[] param)
	{
		return Instantiate(prefab, parent);
	}
}
