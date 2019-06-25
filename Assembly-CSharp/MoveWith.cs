using UnityEngine;

[ExecuteInEditMode]
public class MoveWith : MonoBehaviour
{
	[SerializeField]
	private Transform _target;

	public Transform target
	{
		get
		{
			return _target;
		}
		set
		{
			_target = value;
		}
	}

	private void OnDestroy()
	{
		_target = null;
	}

	private void LateUpdate()
	{
		base.transform.position = _target.position;
	}
}
