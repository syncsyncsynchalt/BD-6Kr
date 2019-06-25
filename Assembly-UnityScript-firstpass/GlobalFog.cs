using System;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Rendering/Global Fog")]
[ExecuteInEditMode]
public class GlobalFog : PostEffectsBase
{
	[Serializable]
	public enum FogMode
	{
		AbsoluteYAndDistance,
		AbsoluteY,
		Distance,
		RelativeYAndDistance
	}

	public FogMode fogMode;

	private float CAMERA_NEAR;

	private float CAMERA_FAR;

	private float CAMERA_FOV;

	private float CAMERA_ASPECT_RATIO;

	public float startDistance;

	public float globalDensity;

	public float heightScale;

	public float height;

	public Color globalFogColor;

	public Shader fogShader;

	private Material fogMaterial;

	public GlobalFog()
	{
		fogMode = FogMode.AbsoluteYAndDistance;
		CAMERA_NEAR = 0.5f;
		CAMERA_FAR = 50f;
		CAMERA_FOV = 60f;
		CAMERA_ASPECT_RATIO = 1.333333f;
		startDistance = 200f;
		globalDensity = 1f;
		heightScale = 100f;
		globalFogColor = Color.grey;
	}

	public bool CheckResources()
	{
		CheckSupport(needDepth: true);
		fogMaterial = CheckShaderAndCreateMaterial(fogShader, fogMaterial);
		if (!isSupported)
		{
			ReportAutoDisable();
		}
		return isSupported;
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!CheckResources())
		{
			Graphics.Blit(source, destination);
			return;
		}
		CAMERA_NEAR = GetComponent<Camera>().nearClipPlane;
		CAMERA_FAR = GetComponent<Camera>().farClipPlane;
		CAMERA_FOV = GetComponent<Camera>().fieldOfView;
		CAMERA_ASPECT_RATIO = GetComponent<Camera>().aspect;
		Matrix4x4 identity = Matrix4x4.identity;
		float num = CAMERA_FOV * 0.5f;
		Vector3 b = GetComponent<Camera>().transform.right * CAMERA_NEAR * Mathf.Tan(num * ((float)Math.PI / 180f)) * CAMERA_ASPECT_RATIO;
		Vector3 b2 = GetComponent<Camera>().transform.up * CAMERA_NEAR * Mathf.Tan(num * ((float)Math.PI / 180f));
		Vector3 a = GetComponent<Camera>().transform.forward * CAMERA_NEAR - b + b2;
		float num2 = a.magnitude * CAMERA_FAR / CAMERA_NEAR;
		a.Normalize();
		a *= num2;
		Vector3 a2 = GetComponent<Camera>().transform.forward * CAMERA_NEAR + b + b2;
		a2.Normalize();
		a2 *= num2;
		Vector3 a3 = GetComponent<Camera>().transform.forward * CAMERA_NEAR + b - b2;
		a3.Normalize();
		a3 *= num2;
		Vector3 a4 = GetComponent<Camera>().transform.forward * CAMERA_NEAR - b - b2;
		a4.Normalize();
		a4 *= num2;
		identity.SetRow(0, a);
		identity.SetRow(1, a2);
		identity.SetRow(2, a3);
		identity.SetRow(3, a4);
		fogMaterial.SetMatrix("_FrustumCornersWS", identity);
		fogMaterial.SetVector("_CameraWS", GetComponent<Camera>().transform.position);
		fogMaterial.SetVector("_StartDistance", new Vector4(1f / startDistance, num2 - startDistance));
		fogMaterial.SetVector("_Y", new Vector4(height, 1f / heightScale));
		fogMaterial.SetFloat("_GlobalDensity", globalDensity * 0.01f);
		fogMaterial.SetColor("_FogColor", globalFogColor);
		CustomGraphicsBlit(source, destination, fogMaterial, (int)fogMode);
	}

	public static void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr)
	{
		RenderTexture.active = dest;
		fxMaterial.SetTexture("_MainTex", source);
		GL.PushMatrix();
		GL.LoadOrtho();
		fxMaterial.SetPass(passNr);
		GL.Begin(7);
		GL.MultiTexCoord2(0, 0f, 0f);
		GL.Vertex3(0f, 0f, 3f);
		GL.MultiTexCoord2(0, 1f, 0f);
		GL.Vertex3(1f, 0f, 2f);
		GL.MultiTexCoord2(0, 1f, 1f);
		GL.Vertex3(1f, 1f, 1f);
		GL.MultiTexCoord2(0, 0f, 1f);
		GL.Vertex3(0f, 1f, 0f);
		GL.End();
		GL.PopMatrix();
	}

	public void Main()
	{
	}
}
