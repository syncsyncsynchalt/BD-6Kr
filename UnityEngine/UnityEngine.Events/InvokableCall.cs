using System;
using System.Reflection;
using UnityEngineInternal;

namespace UnityEngine.Events;

internal class InvokableCall : BaseInvokableCall
{
	private event UnityAction Delegate;

	public InvokableCall(object target, MethodInfo theFunction)
		: base(target, theFunction)
	{
		this.Delegate = (UnityAction)System.Delegate.Combine(this.Delegate, (UnityAction)theFunction.CreateDelegate(typeof(UnityAction), target));
	}

	public InvokableCall(UnityAction action)
	{
		this.Delegate = (UnityAction)System.Delegate.Combine(this.Delegate, action);
	}

	public override void Invoke(object[] args)
	{
		if (BaseInvokableCall.AllowInvoke(this.Delegate))
		{
			this.Delegate();
		}
	}

	public override bool Find(object targetObj, MethodInfo method)
	{
		return this.Delegate.Target == targetObj && System.Reflection.RuntimeReflectionExtensions.GetMethodInfo(this.Delegate) == method;
	}
}
internal class InvokableCall<T1> : BaseInvokableCall
{
	protected event UnityAction<T1> Delegate;

	public InvokableCall(object target, MethodInfo theFunction)
		: base(target, theFunction)
	{
		this.Delegate = (UnityAction<T1>)System.Delegate.Combine(this.Delegate, (UnityAction<T1>)theFunction.CreateDelegate(typeof(UnityAction<T1>), target));
	}

	public InvokableCall(UnityAction<T1> action)
	{
		this.Delegate = (UnityAction<T1>)System.Delegate.Combine(this.Delegate, action);
	}

	public override void Invoke(object[] args)
	{
		if (args.Length != 1)
		{
			throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
		}
		BaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
		if (BaseInvokableCall.AllowInvoke(this.Delegate))
		{
			this.Delegate((T1)args[0]);
		}
	}

	public override bool Find(object targetObj, MethodInfo method)
	{
		return this.Delegate.Target == targetObj && System.Reflection.RuntimeReflectionExtensions.GetMethodInfo(this.Delegate) == method;
	}
}
internal class InvokableCall<T1, T2> : BaseInvokableCall
{
	protected event UnityAction<T1, T2> Delegate;

	public InvokableCall(object target, MethodInfo theFunction)
		: base(target, theFunction)
	{
		this.Delegate = (UnityAction<T1, T2>)theFunction.CreateDelegate(typeof(UnityAction<T1, T2>), target);
	}

	public InvokableCall(UnityAction<T1, T2> action)
	{
		this.Delegate = (UnityAction<T1, T2>)System.Delegate.Combine(this.Delegate, action);
	}

	public override void Invoke(object[] args)
	{
		if (args.Length != 2)
		{
			throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
		}
		BaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
		BaseInvokableCall.ThrowOnInvalidArg<T2>(args[1]);
		if (BaseInvokableCall.AllowInvoke(this.Delegate))
		{
			this.Delegate((T1)args[0], (T2)args[1]);
		}
	}

	public override bool Find(object targetObj, MethodInfo method)
	{
		return this.Delegate.Target == targetObj && System.Reflection.RuntimeReflectionExtensions.GetMethodInfo(this.Delegate) == method;
	}
}
internal class InvokableCall<T1, T2, T3> : BaseInvokableCall
{
	protected event UnityAction<T1, T2, T3> Delegate;

	public InvokableCall(object target, MethodInfo theFunction)
		: base(target, theFunction)
	{
		this.Delegate = (UnityAction<T1, T2, T3>)theFunction.CreateDelegate(typeof(UnityAction<T1, T2, T3>), target);
	}

	public InvokableCall(UnityAction<T1, T2, T3> action)
	{
		this.Delegate = (UnityAction<T1, T2, T3>)System.Delegate.Combine(this.Delegate, action);
	}

	public override void Invoke(object[] args)
	{
		if (args.Length != 3)
		{
			throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
		}
		BaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
		BaseInvokableCall.ThrowOnInvalidArg<T2>(args[1]);
		BaseInvokableCall.ThrowOnInvalidArg<T3>(args[2]);
		if (BaseInvokableCall.AllowInvoke(this.Delegate))
		{
			this.Delegate((T1)args[0], (T2)args[1], (T3)args[2]);
		}
	}

	public override bool Find(object targetObj, MethodInfo method)
	{
		return this.Delegate.Target == targetObj && System.Reflection.RuntimeReflectionExtensions.GetMethodInfo(this.Delegate) == method;
	}
}
internal class InvokableCall<T1, T2, T3, T4> : BaseInvokableCall
{
	protected event UnityAction<T1, T2, T3, T4> Delegate;

	public InvokableCall(object target, MethodInfo theFunction)
		: base(target, theFunction)
	{
		this.Delegate = (UnityAction<T1, T2, T3, T4>)theFunction.CreateDelegate(typeof(UnityAction<T1, T2, T3, T4>), target);
	}

	public InvokableCall(UnityAction<T1, T2, T3, T4> action)
	{
		this.Delegate = (UnityAction<T1, T2, T3, T4>)System.Delegate.Combine(this.Delegate, action);
	}

	public override void Invoke(object[] args)
	{
		if (args.Length != 4)
		{
			throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
		}
		BaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
		BaseInvokableCall.ThrowOnInvalidArg<T2>(args[1]);
		BaseInvokableCall.ThrowOnInvalidArg<T3>(args[2]);
		BaseInvokableCall.ThrowOnInvalidArg<T4>(args[3]);
		if (BaseInvokableCall.AllowInvoke(this.Delegate))
		{
			this.Delegate((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]);
		}
	}

	public override bool Find(object targetObj, MethodInfo method)
	{
		return this.Delegate.Target == targetObj && System.Reflection.RuntimeReflectionExtensions.GetMethodInfo(this.Delegate) == method;
	}
}
