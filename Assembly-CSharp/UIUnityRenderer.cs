using System;
using UnityEngine;

public class UIUnityRenderer : UIWidget
{
	public bool allowSharedMaterial;

	[SerializeField]
	[HideInInspector]
	private Renderer mRenderer;

	[HideInInspector]
	[SerializeField]
	private int renderQueue = -1;

	[HideInInspector]
	[SerializeField]
	private Material[] mMats;

	private static readonly Vector3 Verts = new Vector3(10000f, 10000f);

	public Renderer cachedRenderer
	{
		get
		{
			if ((UnityEngine.Object)mRenderer == null)
			{
				mRenderer = GetComponent<Renderer>();
			}
			return mRenderer;
		}
	}

	public override Material material
	{
		get
		{
			if (!ExistSharedMaterial0())
			{
				Debug.LogError("Renderer or Material is not found.");
				return null;
			}
			if (!allowSharedMaterial)
			{
				if (!CheckMaterial(mMats))
				{
					mMats = new Material[cachedRenderer.sharedMaterials.Length];
					for (int i = 0; i < cachedRenderer.sharedMaterials.Length; i++)
					{
						mMats[i] = new Material(cachedRenderer.sharedMaterials[i]);
						mMats[i].name = mMats[i].name + " (Copy)";
					}
				}
				if (CheckMaterial(mMats) && Application.isPlaying && cachedRenderer.materials != mMats)
				{
					cachedRenderer.materials = mMats;
				}
				return mMats[0];
			}
			return cachedRenderer.sharedMaterials[0];
		}
		set
		{
			throw new NotImplementedException(GetType() + " has no material setter");
		}
	}

	public override Shader shader
	{
		get
		{
			if (!allowSharedMaterial)
			{
				if (CheckMaterial(mMats))
				{
					return mMats[0].shader;
				}
			}
			else if (ExistSharedMaterial0())
			{
				return cachedRenderer.sharedMaterials[0].shader;
			}
			return null;
		}
		set
		{
			throw new NotImplementedException(GetType() + " has no shader setter");
		}
	}

	private bool ExistSharedMaterial0()
	{
		if ((UnityEngine.Object)cachedRenderer != null && CheckMaterial(cachedRenderer.sharedMaterials))
		{
			return true;
		}
		return false;
	}

	private bool CheckMaterial(Material[] mats)
	{
		if (mats != null && mats.Length > 0)
		{
			for (int i = 0; i < mats.Length; i++)
			{
				if (mats[i] == null)
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	private void OnDestroy()
	{
		mMats = null;
		mRenderer = null;
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (!allowSharedMaterial)
		{
			if (!CheckMaterial(mMats) || !(drawCall != null))
			{
				return;
			}
			renderQueue = drawCall.finalRenderQueue;
			for (int i = 0; i < mMats.Length; i++)
			{
				if (mMats[i].renderQueue != renderQueue)
				{
					mMats[i].renderQueue = renderQueue;
				}
			}
		}
		else if (ExistSharedMaterial0() && drawCall != null)
		{
			renderQueue = drawCall.finalRenderQueue;
			for (int j = 0; j < cachedRenderer.sharedMaterials.Length; j++)
			{
				cachedRenderer.sharedMaterials[j].renderQueue = renderQueue;
			}
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		verts.Add(Verts);
		verts.Add(Verts);
		verts.Add(Verts);
		verts.Add(Verts);
		uvs.Add(Vector2.zero);
		uvs.Add(Vector2.up);
		uvs.Add(Vector2.one);
		uvs.Add(Vector2.right);
		cols.Add(base.color);
		cols.Add(base.color);
		cols.Add(base.color);
		cols.Add(base.color);
	}
}
