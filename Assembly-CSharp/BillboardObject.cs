using UnityEngine;

[ExecuteInEditMode]
public class BillboardObject : MonoBehaviour
{
	[SerializeField]
	private Transform _billboardTarget;

	[SerializeField]
	private bool _isBillboard = true;

	[SerializeField]
	private bool _isEnableVerticalRotation;

	public Transform billboardTarget
	{
		get
		{
			return _billboardTarget;
		}
		set
		{
			_billboardTarget = value;
		}
	}

	public bool isBillboard
	{
		get
		{
			return _isBillboard;
		}
		set
		{
			_isBillboard = value;
		}
	}

	public bool isEnableVerticalRotation
	{
		get
		{
			return _isEnableVerticalRotation;
		}
		set
		{
			_isEnableVerticalRotation = value;
		}
	}

	private void LateUpdate()
	{
		if (_isBillboard && !(_billboardTarget == null))
		{
			if (isEnableVerticalRotation)
			{
				base.transform.LookAt(_billboardTarget.position);
				return;
			}
			Vector3 position = _billboardTarget.position;
			Vector3 position2 = base.transform.position;
			position.y = position2.y;
			base.transform.LookAt(position);
		}
	}
}
