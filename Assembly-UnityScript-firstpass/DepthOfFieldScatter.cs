using System;
using UnityEngine;
using Boo.Lang.Runtime;

[Serializable]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Camera/Depth of Field (Lens Blur, Scatter, DX11)")]
[ExecuteInEditMode]
public class DepthOfFieldScatter : PostEffectsBase
{
	[Serializable]
	public enum BlurType
	{
		DiscBlur,
		DX11
	}

	[Serializable]
	public enum BlurSampleCount
	{
		Low,
		Medium,
		High
	}

	public bool visualizeFocus;

	public float focalLength;

	public float focalSize;

	public float aperture;

	public Transform focalTransform;

	public float maxBlurSize;

	public bool highResolution;

	public BlurType blurType;

	public BlurSampleCount blurSampleCount;

	public bool nearBlur;

	public float foregroundOverlap;

	public Shader dofHdrShader;

	private Material dofHdrMaterial;

	public Shader dx11BokehShader;

	private Material dx11bokehMaterial;

	public float dx11BokehThreshhold;

	public float dx11SpawnHeuristic;

	public Texture2D dx11BokehTexture;

	public float dx11BokehScale;

	public float dx11BokehIntensity;

	private float focalDistance01;

	private ComputeBuffer cbDrawArgs;

	private ComputeBuffer cbPoints;

	private float internalBlurWidth;

	public DepthOfFieldScatter()
	{
		focalLength = 10f;
		focalSize = 0.05f;
		aperture = 11.5f;
		maxBlurSize = 2f;
		blurType = BlurType.DiscBlur;
		blurSampleCount = BlurSampleCount.High;
		foregroundOverlap = 1f;
		dx11BokehThreshhold = 0.5f;
		dx11SpawnHeuristic = 0.0875f;
		dx11BokehScale = 1.2f;
		dx11BokehIntensity = 2.5f;
		focalDistance01 = 10f;
		internalBlurWidth = 1f;
	}

	public bool CheckResources()
	{
		CheckSupport(needDepth: true);
		dofHdrMaterial = CheckShaderAndCreateMaterial(dofHdrShader, dofHdrMaterial);
		if (supportDX11 && blurType == BlurType.DX11)
		{
			dx11bokehMaterial = CheckShaderAndCreateMaterial(dx11BokehShader, dx11bokehMaterial);
			CreateComputeResources();
		}
		if (!isSupported)
		{
			ReportAutoDisable();
		}
		return isSupported;
	}

	public void OnEnable()
	{
		GetComponent<Camera>().depthTextureMode = (GetComponent<Camera>().depthTextureMode | DepthTextureMode.Depth);
	}

	public void OnDisable()
	{
		ReleaseComputeResources();
		if ((bool)dofHdrMaterial)
		{
			UnityEngine.Object.DestroyImmediate(dofHdrMaterial);
		}
		dofHdrMaterial = null;
		if ((bool)dx11bokehMaterial)
		{
			UnityEngine.Object.DestroyImmediate(dx11bokehMaterial);
		}
		dx11bokehMaterial = null;
	}

	public void ReleaseComputeResources()
	{
		if (cbDrawArgs != null)
		{
			cbDrawArgs.Release();
		}
		cbDrawArgs = null;
		if (cbPoints != null)
		{
			cbPoints.Release();
		}
		cbPoints = null;
	}

	public void CreateComputeResources()
	{
		if (RuntimeServices.EqualityOperator(cbDrawArgs, null))
		{
			cbDrawArgs = new ComputeBuffer(1, 16, ComputeBufferType.DrawIndirect);
			int[] data = new int[4]
			{
				0,
				1,
				0,
				0
			};
			cbDrawArgs.SetData(data);
		}
		if (RuntimeServices.EqualityOperator(cbPoints, null))
		{
			cbPoints = new ComputeBuffer(90000, 28, ComputeBufferType.Append);
		}
	}

	public float FocalDistance01(float worldDist)
	{
		Vector3 vector = GetComponent<Camera>().WorldToViewportPoint((worldDist - GetComponent<Camera>().nearClipPlane) * GetComponent<Camera>().transform.forward + GetComponent<Camera>().transform.position);
		return vector.z / (GetComponent<Camera>().farClipPlane - GetComponent<Camera>().nearClipPlane);
	}

	private void WriteCoc(RenderTexture fromTo, bool fgDilate)
	{
		dofHdrMaterial.SetTexture("_FgOverlap", null);
		if (nearBlur && fgDilate)
		{
			int width = fromTo.width / 2;
			int height = fromTo.height / 2;
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, fromTo.format);
			Graphics.Blit(fromTo, temporary, dofHdrMaterial, 4);
			float num = internalBlurWidth * foregroundOverlap;
			dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, num, 0f, num));
			RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, fromTo.format);
			Graphics.Blit(temporary, temporary2, dofHdrMaterial, 2);
			RenderTexture.ReleaseTemporary(temporary);
			dofHdrMaterial.SetVector("_Offsets", new Vector4(num, 0f, 0f, num));
			temporary = RenderTexture.GetTemporary(width, height, 0, fromTo.format);
			Graphics.Blit(temporary2, temporary, dofHdrMaterial, 2);
			RenderTexture.ReleaseTemporary(temporary2);
			dofHdrMaterial.SetTexture("_FgOverlap", temporary);
			fromTo.MarkRestoreExpected();
			Graphics.Blit(fromTo, fromTo, dofHdrMaterial, 13);
			RenderTexture.ReleaseTemporary(temporary);
		}
		else
		{
			Graphics.Blit(fromTo, fromTo, dofHdrMaterial, 0);
		}
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!CheckResources())
		{
			Graphics.Blit(source, destination);
			return;
		}
		if (!(aperture >= 0f))
		{
			aperture = 0f;
		}
		if (!(maxBlurSize >= 0.1f))
		{
			maxBlurSize = 0.1f;
		}
		focalSize = Mathf.Clamp(focalSize, 0f, 2f);
		internalBlurWidth = Mathf.Max(maxBlurSize, 0f);
		float num;
		if ((bool)focalTransform)
		{
			Vector3 vector = GetComponent<Camera>().WorldToViewportPoint(focalTransform.position);
			num = vector.z / GetComponent<Camera>().farClipPlane;
		}
		else
		{
			num = FocalDistance01(focalLength);
		}
		focalDistance01 = num;
		dofHdrMaterial.SetVector("_CurveParams", new Vector4(1f, focalSize, aperture / 10f, focalDistance01));
		RenderTexture renderTexture = null;
		RenderTexture renderTexture2 = null;
		float num3 = internalBlurWidth * foregroundOverlap;
		if (visualizeFocus)
		{
			WriteCoc(source, fgDilate: true);
			Graphics.Blit(source, destination, dofHdrMaterial, 16);
		}
		else if (blurType == BlurType.DX11 && (bool)dx11bokehMaterial)
		{
			if (highResolution)
			{
				internalBlurWidth = ((internalBlurWidth >= 0.1f) ? internalBlurWidth : 0.1f);
				float num2 = internalBlurWidth * foregroundOverlap;
				renderTexture = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
				RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
				WriteCoc(source, fgDilate: false);
				RenderTexture temporary2 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
				RenderTexture temporary3 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
				Graphics.Blit(source, temporary2, dofHdrMaterial, 15);
				dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, 1.5f, 0f, 1.5f));
				Graphics.Blit(temporary2, temporary3, dofHdrMaterial, 19);
				dofHdrMaterial.SetVector("_Offsets", new Vector4(1.5f, 0f, 0f, 1.5f));
				Graphics.Blit(temporary3, temporary2, dofHdrMaterial, 19);
				if (nearBlur)
				{
					Graphics.Blit(source, temporary3, dofHdrMaterial, 4);
				}
				dx11bokehMaterial.SetTexture("_BlurredColor", temporary2);
				dx11bokehMaterial.SetFloat("_SpawnHeuristic", dx11SpawnHeuristic);
				dx11bokehMaterial.SetVector("_BokehParams", new Vector4(dx11BokehScale, dx11BokehIntensity, Mathf.Clamp(dx11BokehThreshhold, 0.005f, 4f), internalBlurWidth));
				dx11bokehMaterial.SetTexture("_FgCocMask", (!nearBlur) ? null : temporary3);
				Graphics.SetRandomWriteTarget(1, cbPoints);
				Graphics.Blit(source, renderTexture, dx11bokehMaterial, 0);
				Graphics.ClearRandomWriteTargets();
				if (nearBlur)
				{
					dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, num2, 0f, num2));
					Graphics.Blit(temporary3, temporary2, dofHdrMaterial, 2);
					dofHdrMaterial.SetVector("_Offsets", new Vector4(num2, 0f, 0f, num2));
					Graphics.Blit(temporary2, temporary3, dofHdrMaterial, 2);
					Graphics.Blit(temporary3, renderTexture, dofHdrMaterial, 3);
				}
				Graphics.Blit(renderTexture, temporary, dofHdrMaterial, 20);
				dofHdrMaterial.SetVector("_Offsets", new Vector4(internalBlurWidth, 0f, 0f, internalBlurWidth));
				Graphics.Blit(renderTexture, source, dofHdrMaterial, 5);
				dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, internalBlurWidth, 0f, internalBlurWidth));
				Graphics.Blit(source, temporary, dofHdrMaterial, 21);
				Graphics.SetRenderTarget(temporary);
				ComputeBuffer.CopyCount(cbPoints, cbDrawArgs, 0);
				dx11bokehMaterial.SetBuffer("pointBuffer", cbPoints);
				dx11bokehMaterial.SetTexture("_MainTex", dx11BokehTexture);
				dx11bokehMaterial.SetVector("_Screen", new Vector3(1f / (1f * (float)source.width), 1f / (1f * (float)source.height), internalBlurWidth));
				dx11bokehMaterial.SetPass(2);
				Graphics.DrawProceduralIndirect(MeshTopology.Points, cbDrawArgs, 0);
				Graphics.Blit(temporary, destination);
				RenderTexture.ReleaseTemporary(temporary);
				RenderTexture.ReleaseTemporary(temporary2);
				RenderTexture.ReleaseTemporary(temporary3);
			}
			else
			{
				renderTexture = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
				renderTexture2 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
				float num2 = internalBlurWidth * foregroundOverlap;
				WriteCoc(source, fgDilate: false);
				source.filterMode = FilterMode.Bilinear;
				Graphics.Blit(source, renderTexture, dofHdrMaterial, 6);
				RenderTexture temporary2 = RenderTexture.GetTemporary(renderTexture.width >> 1, renderTexture.height >> 1, 0, renderTexture.format);
				RenderTexture temporary3 = RenderTexture.GetTemporary(renderTexture.width >> 1, renderTexture.height >> 1, 0, renderTexture.format);
				Graphics.Blit(renderTexture, temporary2, dofHdrMaterial, 15);
				dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, 1.5f, 0f, 1.5f));
				Graphics.Blit(temporary2, temporary3, dofHdrMaterial, 19);
				dofHdrMaterial.SetVector("_Offsets", new Vector4(1.5f, 0f, 0f, 1.5f));
				Graphics.Blit(temporary3, temporary2, dofHdrMaterial, 19);
				RenderTexture renderTexture3 = null;
				if (nearBlur)
				{
					renderTexture3 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
					Graphics.Blit(source, renderTexture3, dofHdrMaterial, 4);
				}
				dx11bokehMaterial.SetTexture("_BlurredColor", temporary2);
				dx11bokehMaterial.SetFloat("_SpawnHeuristic", dx11SpawnHeuristic);
				dx11bokehMaterial.SetVector("_BokehParams", new Vector4(dx11BokehScale, dx11BokehIntensity, Mathf.Clamp(dx11BokehThreshhold, 0.005f, 4f), internalBlurWidth));
				dx11bokehMaterial.SetTexture("_FgCocMask", renderTexture3);
				Graphics.SetRandomWriteTarget(1, cbPoints);
				Graphics.Blit(renderTexture, renderTexture2, dx11bokehMaterial, 0);
				Graphics.ClearRandomWriteTargets();
				RenderTexture.ReleaseTemporary(temporary2);
				RenderTexture.ReleaseTemporary(temporary3);
				if (nearBlur)
				{
					dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, num2, 0f, num2));
					Graphics.Blit(renderTexture3, renderTexture, dofHdrMaterial, 2);
					dofHdrMaterial.SetVector("_Offsets", new Vector4(num2, 0f, 0f, num2));
					Graphics.Blit(renderTexture, renderTexture3, dofHdrMaterial, 2);
					Graphics.Blit(renderTexture3, renderTexture2, dofHdrMaterial, 3);
				}
				dofHdrMaterial.SetVector("_Offsets", new Vector4(internalBlurWidth, 0f, 0f, internalBlurWidth));
				Graphics.Blit(renderTexture2, renderTexture, dofHdrMaterial, 5);
				dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, internalBlurWidth, 0f, internalBlurWidth));
				Graphics.Blit(renderTexture, renderTexture2, dofHdrMaterial, 5);
				Graphics.SetRenderTarget(renderTexture2);
				ComputeBuffer.CopyCount(cbPoints, cbDrawArgs, 0);
				dx11bokehMaterial.SetBuffer("pointBuffer", cbPoints);
				dx11bokehMaterial.SetTexture("_MainTex", dx11BokehTexture);
				dx11bokehMaterial.SetVector("_Screen", new Vector3(1f / (1f * (float)renderTexture2.width), 1f / (1f * (float)renderTexture2.height), internalBlurWidth));
				dx11bokehMaterial.SetPass(1);
				Graphics.DrawProceduralIndirect(MeshTopology.Points, cbDrawArgs, 0);
				dofHdrMaterial.SetTexture("_LowRez", renderTexture2);
				dofHdrMaterial.SetTexture("_FgOverlap", renderTexture3);
				dofHdrMaterial.SetVector("_Offsets", 1f * (float)source.width / (1f * (float)renderTexture2.width) * internalBlurWidth * Vector4.one);
				Graphics.Blit(source, destination, dofHdrMaterial, 9);
				if ((bool)renderTexture3)
				{
					RenderTexture.ReleaseTemporary(renderTexture3);
				}
			}
		}
		else
		{
			source.filterMode = FilterMode.Bilinear;
			if (highResolution)
			{
				internalBlurWidth *= 2f;
			}
			WriteCoc(source, fgDilate: true);
			renderTexture = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
			renderTexture2 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
			int pass = (blurSampleCount != BlurSampleCount.High && blurSampleCount != BlurSampleCount.Medium) ? 11 : 17;
			if (highResolution)
			{
				dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, internalBlurWidth, 0.025f, internalBlurWidth));
				Graphics.Blit(source, destination, dofHdrMaterial, pass);
			}
			else
			{
				dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, internalBlurWidth, 0.1f, internalBlurWidth));
				Graphics.Blit(source, renderTexture, dofHdrMaterial, 6);
				Graphics.Blit(renderTexture, renderTexture2, dofHdrMaterial, pass);
				dofHdrMaterial.SetTexture("_LowRez", renderTexture2);
				dofHdrMaterial.SetTexture("_FgOverlap", null);
				dofHdrMaterial.SetVector("_Offsets", Vector4.one * (1f * (float)source.width / (1f * (float)renderTexture2.width)) * internalBlurWidth);
				Graphics.Blit(source, destination, dofHdrMaterial, (blurSampleCount != BlurSampleCount.High) ? 12 : 18);
			}
		}
		if ((bool)renderTexture)
		{
			RenderTexture.ReleaseTemporary(renderTexture);
		}
		if ((bool)renderTexture2)
		{
			RenderTexture.ReleaseTemporary(renderTexture2);
		}
	}

	public void Main()
	{
	}
}
