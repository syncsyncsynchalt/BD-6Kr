using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mem
{
	public static void ZeroMemory<T>(IList<T> destination)
	{
		for (int i = 0; i < destination.Count; i++)
		{
			destination[i] = default(T);
		}
	}

	public static void ZeroMemory<T>(T[] o, int offset, int length)
	{
		for (int i = 0; i < length; i++)
		{
			o[offset + i] = default(T);
		}
	}

	public static void Del<T>(ref T p)
	{
		p = default(T);
	}

	public static void Del(ref UISprite p)
	{
		if (p != null)
		{
			p.Clear();
		}
		p = null;
	}

	public static void Del(ref ParticleSystem p)
	{
		if ((UnityEngine.Object)p != null)
		{
			Renderer component = ((Component)p).GetComponent<Renderer>();
			if ((UnityEngine.Object)component != null)
			{
				Material[] materials = component.materials;
				if (materials != null)
				{
					for (int i = 0; i < materials.Length; i++)
					{
						materials[i] = null;
					}
				}
				component.material = (Material) null;
			}
			UnityEngine.Object.Destroy((UnityEngine.Object)p);
		}
		p = null;
	}

	public static void DelAry<T>(ref T[] p)
	{
		p = null;
	}

	public static void DelAry<T>(ref T[,] p)
	{
		p = null;
	}

	public static void DelList<T>(ref List<T> p)
	{
		p.Clear();
		p = null;
	}

	public static void DelQueue<T>(ref Queue<T> p)
	{
		p.Clear();
		p = null;
	}

	public static void DelDictionary<T, V>(ref Dictionary<T, V> p)
	{
		p.Clear();
		p = null;
	}

	public static void DelHashtable(ref Hashtable p)
	{
		p.Clear();
		p = null;
	}

	public static void DelComponent<T>(ref T p) where T : Component
	{
		UnityEngine.Object.Destroy(p.gameObject);
		Del(ref p);
	}

	public static void DelIDisposable<T>(ref T p) where T : IDisposable
	{
		p.Dispose();
		Del(ref p);
	}

	public static void DelSafe<T>(ref T p)
	{
		if (p != null)
		{
			Del(ref p);
			p = default(T);
		}
	}

	public static void DelArySafe<T>(ref T[] p)
	{
		if (p != null)
		{
			DelAry(ref p);
			p = null;
		}
	}

	public static void DelListSafe<T>(ref List<T> p)
	{
		if (p != null)
		{
			DelList(ref p);
		}
	}

	public static void DelQueueSafe<T>(ref Queue<T> p)
	{
		if (p != null)
		{
			DelQueue(ref p);
		}
	}

	public static void DelDictionarySafe<T, V>(ref Dictionary<T, V> p)
	{
		if (p != null)
		{
			DelDictionary(ref p);
		}
	}

	public static void DelHashtableSafe(ref Hashtable p)
	{
		if (p != null)
		{
			DelHashtable(ref p);
		}
	}

	public static void DelComponentSafe<T>(ref T p) where T : Component
	{
		if ((UnityEngine.Object)p != (UnityEngine.Object)null && p.gameObject != null)
		{
			DelComponent(ref p);
		}
		else
		{
			Del(ref p);
		}
	}

	public static void DelIDisposableSafe<T>(ref T p) where T : IDisposable
	{
		if (p != null)
		{
			DelIDisposable(ref p);
		}
	}

	public static void DelMeshSafe(ref Transform p)
	{
		if (!(p != null))
		{
			return;
		}
		if (((Component)p).GetComponent<MeshFilter>() != null)
		{
			Mesh mesh = ((Component)p).GetComponent<MeshFilter>().mesh;
			if (!(mesh != null))
			{
			}
		}
		MeshRenderer component = ((Component)p).GetComponent<MeshRenderer>();
		if (!((UnityEngine.Object)component != null))
		{
			return;
		}
		if (component.material != null)
		{
			component.material.mainTexture = null;
			component.material.shader = null;
			UnityEngine.Object.DestroyImmediate(component.material, allowDestroyingAssets: true);
			component.material = (Material) null;
		}
		if (component.materials != null)
		{
			for (int i = 0; i < component.materials.Length; i++)
			{
				component.materials[i].mainTexture = null;
				component.materials[i].shader = null;
				UnityEngine.Object.DestroyImmediate(component.materials[i], allowDestroyingAssets: true);
				component.materials[i] = null;
			}
		}
	}

	public static void DelSkyboxSafe(ref Skybox p)
	{
		if (p != null && p.material != null)
		{
			p.material = null;
		}
		Del(ref p);
	}

	public static void New<T>(ref object p) where T : new()
	{
		p = new T();
	}

	public static void NewAry<T>(ref T[] p, int n)
	{
		p = new T[n];
	}
}
