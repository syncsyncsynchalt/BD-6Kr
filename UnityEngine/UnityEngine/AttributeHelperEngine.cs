using System;
using System.Collections.Generic;

namespace UnityEngine;

internal class AttributeHelperEngine
{
	private static Type GetParentTypeDisallowingMultipleInclusion(Type type)
	{
		Stack<Type> stack = new Stack<Type>();
		while (type != null && type != typeof(MonoBehaviour))
		{
			stack.Push(type);
			type = type.BaseType;
		}
		Type type2 = null;
		while (stack.Count > 0)
		{
			type2 = stack.Pop();
			object[] customAttributes = type2.GetCustomAttributes(typeof(DisallowMultipleComponent), inherit: false);
			if (customAttributes.Length != 0)
			{
				return type2;
			}
		}
		return null;
	}

	private static Type[] GetRequiredComponents(Type klass)
	{
		List<Type> list = null;
		while (klass != null && klass != typeof(MonoBehaviour))
		{
			RequireComponent[] array = (RequireComponent[])klass.GetCustomAttributes(typeof(RequireComponent), inherit: false);
			Type baseType = klass.BaseType;
			RequireComponent[] array2 = array;
			foreach (RequireComponent requireComponent in array2)
			{
				if (list == null && array.Length == 1 && baseType == typeof(MonoBehaviour))
				{
					return new Type[3] { requireComponent.m_Type0, requireComponent.m_Type1, requireComponent.m_Type2 };
				}
				if (list == null)
				{
					list = new List<Type>();
				}
				if (requireComponent.m_Type0 != null)
				{
					list.Add(requireComponent.m_Type0);
				}
				if (requireComponent.m_Type1 != null)
				{
					list.Add(requireComponent.m_Type1);
				}
				if (requireComponent.m_Type2 != null)
				{
					list.Add(requireComponent.m_Type2);
				}
			}
			klass = baseType;
		}
		return list?.ToArray();
	}

	private static bool CheckIsEditorScript(Type klass)
	{
		while (klass != null && klass != typeof(MonoBehaviour))
		{
			object[] customAttributes = klass.GetCustomAttributes(typeof(ExecuteInEditMode), inherit: false);
			if (customAttributes.Length != 0)
			{
				return true;
			}
			klass = klass.BaseType;
		}
		return false;
	}
}
