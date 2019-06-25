using System;

using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine
{
	public sealed class ShaderVariantCollection : Object
	{
		public struct ShaderVariant
		{
			public Shader shader;

			public PassType passType;

			public string[] keywords;

			public ShaderVariant(Shader shader, PassType passType, params string[] keywords)
			{
				this.shader = shader;
				this.passType = passType;
				this.keywords = keywords;
				Internal_CheckVariant(shader, passType, keywords);
			}

			private static void Internal_CheckVariant(Shader shader, PassType passType, string[] keywords) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
		}

		public int shaderCount
		{
			get;
		}

		public int variantCount
		{
			get;
		}

		public bool isWarmedUp
		{
			get;
		}

		public ShaderVariantCollection()
		{
			Internal_Create(this);
		}

		private static void Internal_Create([Writable] ShaderVariantCollection mono) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool Add(ShaderVariant variant)
		{
			return AddInternal(variant.shader, variant.passType, variant.keywords);
		}

		private bool AddInternal(Shader shader, PassType passType, string[] keywords) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool Remove(ShaderVariant variant)
		{
			return RemoveInternal(variant.shader, variant.passType, variant.keywords);
		}

		private bool RemoveInternal(Shader shader, PassType passType, string[] keywords) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool Contains(ShaderVariant variant)
		{
			return ContainsInternal(variant.shader, variant.passType, variant.keywords);
		}

		private bool ContainsInternal(Shader shader, PassType passType, string[] keywords) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Clear() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void WarmUp() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
