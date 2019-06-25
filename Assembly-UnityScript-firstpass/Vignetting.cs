using System;
using UnityEngine;

[Serializable]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camera/Vignette and Chromatic Aberration")]
[RequireComponent(typeof(Camera))]
public class Vignetting : PostEffectsBase
{
	[Serializable]
	public enum AberrationMode
	{
		Simple,
		Advanced
	}

	public AberrationMode mode;

	public float intensity;

	public float chromaticAberration;

	public float axialAberration;

	public float blur;

	public float blurSpread;

	public float luminanceDependency;

	public float blurDistance;

	public Shader vignetteShader;

	private Material vignetteMaterial;

	public Shader separableBlurShader;

	private Material separableBlurMaterial;

	public Shader chromAberrationShader;

	private Material chromAberrationMaterial;

	public Vignetting()
	{
		mode = AberrationMode.Simple;
		intensity = 0.375f;
		chromaticAberration = 0.2f;
		axialAberration = 0.5f;
		blurSpread = 0.75f;
		luminanceDependency = 0.25f;
		blurDistance = 2.5f;
	}

	public bool CheckResources()
	{
		CheckSupport(needDepth: false);
		vignetteMaterial = CheckShaderAndCreateMaterial(vignetteShader, vignetteMaterial);
		separableBlurMaterial = CheckShaderAndCreateMaterial(separableBlurShader, separableBlurMaterial);
		chromAberrationMaterial = CheckShaderAndCreateMaterial(chromAberrationShader, chromAberrationMaterial);
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
		int width = source.width;
		int height = source.height;
		int num = (Mathf.Abs(blur) > 0f) ? 1 : 0;
		if (num == 0)
		{
			num = ((Mathf.Abs(intensity) > 0f) ? 1 : 0);
		}
		bool flag = (byte)num != 0;
		float num2 = 1f * (float)width / (1f * (float)height);
		float num3 = 0.001953125f;
		RenderTexture renderTexture = null;
		RenderTexture renderTexture2 = null;
		if (flag)
		{
			renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
			if (!(Mathf.Abs(blur) <= 0f))
			{
				renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
				Graphics.Blit(source, renderTexture2, chromAberrationMaterial, 0);
				for (int i = 0; i < 2; i++)
				{
					separableBlurMaterial.SetVector("offsets", new Vector4(0f, blurSpread * num3, 0f, 0f));
					RenderTexture temporary = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
					Graphics.Blit(renderTexture2, temporary, separableBlurMaterial);
					RenderTexture.ReleaseTemporary(renderTexture2);
					separableBlurMaterial.SetVector("offsets", new Vector4(blurSpread * num3 / num2, 0f, 0f, 0f));
					renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
					Graphics.Blit(temporary, renderTexture2, separableBlurMaterial);
					RenderTexture.ReleaseTemporary(temporary);
				}
			}
			vignetteMaterial.SetFloat("_Intensity", intensity);
			vignetteMaterial.SetFloat("_Blur", blur);
			vignetteMaterial.SetTexture("_VignetteTex", renderTexture2);
			Graphics.Blit(source, renderTexture, vignetteMaterial, 0);
		}
		chromAberrationMaterial.SetFloat("_ChromaticAberration", chromaticAberration);
		chromAberrationMaterial.SetFloat("_AxialAberration", axialAberration);
		chromAberrationMaterial.SetVector("_BlurDistance", new Vector2(0f - blurDistance, blurDistance));
		chromAberrationMaterial.SetFloat("_Luminance", 1f / Mathf.Max(Mathf.Epsilon, luminanceDependency));
		if (flag)
		{
			renderTexture.wrapMode = TextureWrapMode.Clamp;
		}
		else
		{
			source.wrapMode = TextureWrapMode.Clamp;
		}
		Graphics.Blit((!flag) ? source : renderTexture, destination, chromAberrationMaterial, (mode != AberrationMode.Advanced) ? 1 : 2);
		RenderTexture.ReleaseTemporary(renderTexture);
		RenderTexture.ReleaseTemporary(renderTexture2);
	}

	public void Main()
	{
	}
}
