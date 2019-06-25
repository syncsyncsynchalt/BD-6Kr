using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class ComputeShader : Object
	{
		public int FindKernel(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetFloat(string name, float val) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetInt(string name, int val) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetVector(string name, Vector4 val)
		{
			INTERNAL_CALL_SetVector(this, name, ref val);
		}

		private static void INTERNAL_CALL_SetVector(ComputeShader self, string name, ref Vector4 val) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetFloats(string name, params float[] values)
		{
			Internal_SetFloats(name, values);
		}

		private void Internal_SetFloats(string name, float[] values) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetInts(string name, params int[] values)
		{
			Internal_SetInts(name, values);
		}

		private void Internal_SetInts(string name, int[] values) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetTexture(int kernelIndex, string name, Texture texture) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetBuffer(int kernelIndex, string name, ComputeBuffer buffer) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Dispatch(int kernelIndex, int threadsX, int threadsY, int threadsZ) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
