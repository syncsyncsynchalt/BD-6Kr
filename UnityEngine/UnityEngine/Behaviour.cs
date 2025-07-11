using System.Runtime.CompilerServices;

namespace UnityEngine;

public class Behaviour : Component
{
	private bool _enabled = true;

	public bool enabled
	{
		get { return _enabled; }
		set { _enabled = value; }
	}

	public bool isActiveAndEnabled
	{
		get { return _enabled; }
	}
}
