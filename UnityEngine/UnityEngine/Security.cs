using System;
using System.Reflection;
using System.Security;
using UnityEngine.Internal;

namespace UnityEngine;

public sealed class Security
{
	[ExcludeFromDocs]
	public static bool PrefetchSocketPolicy(string ip, int atPort)
	{
		int timeout = 3000;
		return PrefetchSocketPolicy(ip, atPort, timeout);
	}

	public static bool PrefetchSocketPolicy(string ip, int atPort, [DefaultValue("3000")] int timeout)
	{
		return true;
	}

	private static MethodInfo GetUnityCrossDomainHelperMethod(string methodname)
	{
		Type type = Types.GetType("UnityEngine.UnityCrossDomainHelper", "CrossDomainPolicyParser, Version=1.0.0.0, Culture=neutral");
		if (type == null)
		{
			throw new SecurityException("Cant find type UnityCrossDomainHelper");
		}
		MethodInfo method = type.GetMethod(methodname);
		if (method == null)
		{
			throw new SecurityException("Cant find " + methodname);
		}
		return method;
	}

	internal static string TokenToHex(byte[] token)
	{
		if (token == null || 8 > token.Length)
		{
			return string.Empty;
		}
		return $"{token[0]:x2}{token[1]:x2}{token[2]:x2}{token[3]:x2}{token[4]:x2}{token[5]:x2}{token[6]:x2}{token[7]:x2}";
	}

	[SecuritySafeCritical]
	public static Assembly LoadAndVerifyAssembly(byte[] assemblyData, string authorizationKey)
	{
		return null;
	}

	[SecuritySafeCritical]
	public static Assembly LoadAndVerifyAssembly(byte[] assemblyData)
	{
		return null;
	}

	[SecuritySafeCritical]
	private static Assembly LoadAndVerifyAssemblyInternal(byte[] assemblyData)
	{
		return null;
	}
}
