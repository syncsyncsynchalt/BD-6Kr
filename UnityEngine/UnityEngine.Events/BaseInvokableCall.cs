using System;
using System.Reflection;

namespace UnityEngine.Events;

internal abstract class BaseInvokableCall
{
	protected BaseInvokableCall()
	{
	}

	protected BaseInvokableCall(object target, MethodInfo function)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (function == null)
		{
			throw new ArgumentNullException("function");
		}
	}

	public abstract void Invoke(object[] args);

	protected static void ThrowOnInvalidArg<T>(object arg)
	{
		if (arg != null && !(arg is T))
		{
			throw new ArgumentException(UnityString.Format("Passed argument 'args[0]' is of the wrong type. Type:{0} Expected:{1}", arg.GetType(), typeof(T)));
		}
	}

	protected static bool AllowInvoke(Delegate @delegate)
	{
		object target = @delegate.Target;
		if (target == null)
		{
			return true;
		}
		Object obj = target as Object;
		if (!object.ReferenceEquals(obj, null))
		{
			return obj != null;
		}
		return true;
	}

	public abstract bool Find(object targetObj, MethodInfo method);
}
