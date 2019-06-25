using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class MaterialPropertyBlock
	{
		internal IntPtr m_Ptr;

		public bool isEmpty
		{
			get;
		}

		public MaterialPropertyBlock()
		{
			InitBlock();
		}

		internal void InitBlock() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void DestroyBlock() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		~MaterialPropertyBlock()
		{
			DestroyBlock();
		}

		public void SetFloat(string name, float value)
		{
			SetFloat(Shader.PropertyToID(name), value);
		}

		public void SetFloat(int nameID, float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetVector(string name, Vector4 value)
		{
			SetVector(Shader.PropertyToID(name), value);
		}

		public void SetVector(int nameID, Vector4 value)
		{
			INTERNAL_CALL_SetVector(this, nameID, ref value);
		}

		private static void INTERNAL_CALL_SetVector(MaterialPropertyBlock self, int nameID, ref Vector4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetColor(string name, Color value)
		{
			SetColor(Shader.PropertyToID(name), value);
		}

		public void SetColor(int nameID, Color value)
		{
			INTERNAL_CALL_SetColor(this, nameID, ref value);
		}

		private static void INTERNAL_CALL_SetColor(MaterialPropertyBlock self, int nameID, ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetMatrix(string name, Matrix4x4 value)
		{
			SetMatrix(Shader.PropertyToID(name), value);
		}

		public void SetMatrix(int nameID, Matrix4x4 value)
		{
			INTERNAL_CALL_SetMatrix(this, nameID, ref value);
		}

		private static void INTERNAL_CALL_SetMatrix(MaterialPropertyBlock self, int nameID, ref Matrix4x4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetTexture(string name, Texture value)
		{
			SetTexture(Shader.PropertyToID(name), value);
		}

		public void SetTexture(int nameID, Texture value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void AddFloat(string name, float value)
		{
			AddFloat(Shader.PropertyToID(name), value);
		}

		public void AddFloat(int nameID, float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void AddVector(string name, Vector4 value)
		{
			AddVector(Shader.PropertyToID(name), value);
		}

		public void AddVector(int nameID, Vector4 value)
		{
			INTERNAL_CALL_AddVector(this, nameID, ref value);
		}

		private static void INTERNAL_CALL_AddVector(MaterialPropertyBlock self, int nameID, ref Vector4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void AddColor(string name, Color value)
		{
			AddColor(Shader.PropertyToID(name), value);
		}

		public void AddColor(int nameID, Color value)
		{
			INTERNAL_CALL_AddColor(this, nameID, ref value);
		}

		private static void INTERNAL_CALL_AddColor(MaterialPropertyBlock self, int nameID, ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void AddMatrix(string name, Matrix4x4 value)
		{
			AddMatrix(Shader.PropertyToID(name), value);
		}

		public void AddMatrix(int nameID, Matrix4x4 value)
		{
			INTERNAL_CALL_AddMatrix(this, nameID, ref value);
		}

		private static void INTERNAL_CALL_AddMatrix(MaterialPropertyBlock self, int nameID, ref Matrix4x4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void AddTexture(string name, Texture value)
		{
			AddTexture(Shader.PropertyToID(name), value);
		}

		public void AddTexture(int nameID, Texture value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetFloat(string name)
		{
			return GetFloat(Shader.PropertyToID(name));
		}

		public float GetFloat(int nameID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector4 GetVector(string name)
		{
			return GetVector(Shader.PropertyToID(name));
		}

		public Vector4 GetVector(int nameID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Matrix4x4 GetMatrix(string name)
		{
			return GetMatrix(Shader.PropertyToID(name));
		}

		public Matrix4x4 GetMatrix(int nameID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Texture GetTexture(string name)
		{
			return GetTexture(Shader.PropertyToID(name));
		}

		public Texture GetTexture(int nameID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Clear() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
