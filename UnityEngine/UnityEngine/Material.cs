using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public class Material : Object
	{
		public Shader shader
		{
			get;
			set;
		}

		public Color color
		{
			get
			{
				return GetColor("_Color");
			}
			set
			{
				SetColor("_Color", value);
			}
		}

		public Texture mainTexture
		{
			get
			{
				return GetTexture("_MainTex");
			}
			set
			{
				SetTexture("_MainTex", value);
			}
		}

		public Vector2 mainTextureOffset
		{
			get
			{
				return GetTextureOffset("_MainTex");
			}
			set
			{
				SetTextureOffset("_MainTex", value);
			}
		}

		public Vector2 mainTextureScale
		{
			get
			{
				return GetTextureScale("_MainTex");
			}
			set
			{
				SetTextureScale("_MainTex", value);
			}
		}

		public int passCount
		{
			get;
		}

		public int renderQueue
		{
			get;
			set;
		}

		public string[] shaderKeywords
		{
			get;
			set;
		}

		public MaterialGlobalIlluminationFlags globalIlluminationFlags
		{
			get;
			set;
		}

		[Obsolete("Creating materials from shader source string will be removed in the future. Use Shader assets instead.")]
		public Material(string contents)
		{
			Internal_CreateWithString(this, contents);
		}

		public Material(Shader shader)
		{
			Internal_CreateWithShader(this, shader);
		}

		public Material(Material source)
		{
			Internal_CreateWithMaterial(this, source);
		}

		public void SetColor(string propertyName, Color color)
		{
			SetColor(Shader.PropertyToID(propertyName), color);
		}

		public void SetColor(int nameID, Color color)
		{
			INTERNAL_CALL_SetColor(this, nameID, ref color);
		}

		private static void INTERNAL_CALL_SetColor(Material self, int nameID, ref Color color) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Color GetColor(string propertyName)
		{
			return GetColor(Shader.PropertyToID(propertyName));
		}

		public Color GetColor(int nameID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetVector(string propertyName, Vector4 vector)
		{
			SetColor(propertyName, new Color(vector.x, vector.y, vector.z, vector.w));
		}

		public void SetVector(int nameID, Vector4 vector)
		{
			SetColor(nameID, new Color(vector.x, vector.y, vector.z, vector.w));
		}

		public Vector4 GetVector(string propertyName)
		{
			Color color = GetColor(propertyName);
			return new Vector4(color.r, color.g, color.b, color.a);
		}

		public Vector4 GetVector(int nameID)
		{
			Color color = GetColor(nameID);
			return new Vector4(color.r, color.g, color.b, color.a);
		}

		public void SetTexture(string propertyName, Texture texture)
		{
			SetTexture(Shader.PropertyToID(propertyName), texture);
		}

		public void SetTexture(int nameID, Texture texture) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Texture GetTexture(string propertyName)
		{
			return GetTexture(Shader.PropertyToID(propertyName));
		}

		public Texture GetTexture(int nameID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_GetTextureOffset(Material mat, string name, out Vector2 output) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_GetTextureScale(Material mat, string name, out Vector2 output) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetTextureOffset(string propertyName, Vector2 offset)
		{
			INTERNAL_CALL_SetTextureOffset(this, propertyName, ref offset);
		}

		private static void INTERNAL_CALL_SetTextureOffset(Material self, string propertyName, ref Vector2 offset) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector2 GetTextureOffset(string propertyName)
		{
			Internal_GetTextureOffset(this, propertyName, out Vector2 output);
			return output;
		}

		public void SetTextureScale(string propertyName, Vector2 scale)
		{
			INTERNAL_CALL_SetTextureScale(this, propertyName, ref scale);
		}

		private static void INTERNAL_CALL_SetTextureScale(Material self, string propertyName, ref Vector2 scale) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector2 GetTextureScale(string propertyName)
		{
			Internal_GetTextureScale(this, propertyName, out Vector2 output);
			return output;
		}

		public void SetMatrix(string propertyName, Matrix4x4 matrix)
		{
			SetMatrix(Shader.PropertyToID(propertyName), matrix);
		}

		public void SetMatrix(int nameID, Matrix4x4 matrix)
		{
			INTERNAL_CALL_SetMatrix(this, nameID, ref matrix);
		}

		private static void INTERNAL_CALL_SetMatrix(Material self, int nameID, ref Matrix4x4 matrix) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Matrix4x4 GetMatrix(string propertyName)
		{
			return GetMatrix(Shader.PropertyToID(propertyName));
		}

		public Matrix4x4 GetMatrix(int nameID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetFloat(string propertyName, float value)
		{
			SetFloat(Shader.PropertyToID(propertyName), value);
		}

		public void SetFloat(int nameID, float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetFloat(string propertyName)
		{
			return GetFloat(Shader.PropertyToID(propertyName));
		}

		public float GetFloat(int nameID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetInt(string propertyName, int value)
		{
			SetFloat(propertyName, value);
		}

		public void SetInt(int nameID, int value)
		{
			SetFloat(nameID, value);
		}

		public int GetInt(string propertyName)
		{
			return (int)GetFloat(propertyName);
		}

		public int GetInt(int nameID)
		{
			return (int)GetFloat(nameID);
		}

		public void SetBuffer(string propertyName, ComputeBuffer buffer) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool HasProperty(string propertyName)
		{
			return HasProperty(Shader.PropertyToID(propertyName));
		}

		public bool HasProperty(int nameID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public string GetTag(string tag, bool searchFallbacks, [DefaultValue("\"\"")] string defaultValue) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public string GetTag(string tag, bool searchFallbacks)
		{
			string empty = string.Empty;
			return GetTag(tag, searchFallbacks, empty);
		}

		public void SetOverrideTag(string tag, string val) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Lerp(Material start, Material end, float t) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool SetPass(int pass) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("Creating materials from shader source string will be removed in the future. Use Shader assets instead.")]
		public static Material Create(string scriptContents)
		{
			return new Material(scriptContents);
		}

		private static void Internal_CreateWithString([Writable] Material mono, string contents) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_CreateWithShader([Writable] Material mono, Shader shader) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_CreateWithMaterial([Writable] Material mono, Material source) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void CopyPropertiesFromMaterial(Material mat) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void EnableKeyword(string keyword) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void DisableKeyword(string keyword) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool IsKeywordEnabled(string keyword) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
