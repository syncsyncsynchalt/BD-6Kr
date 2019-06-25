using System;

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Profiler
	{
		public static bool supported
		{
			get;
		}

		public static string logFile
		{
			get;
			set;
		}

		public static bool enableBinaryLog
		{
			get;
			set;
		}

		public static bool enabled
		{
			get;
			set;
		}

		public static int maxNumberOfSamplesPerFrame
		{
			get;
			set;
		}

		public static uint usedHeapSize
		{
			get;
		}

		[Conditional("ENABLE_PROFILER")]
		public static void AddFramesFromFile(string file) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Conditional("ENABLE_PROFILER")]
		public static void BeginSample(string name)
		{
			BeginSampleOnly(name);
		}

		[Conditional("ENABLE_PROFILER")]
		public static void BeginSample(string name, Object targetObject) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void BeginSampleOnly(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Conditional("ENABLE_PROFILER")]
		public static void EndSample() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int GetRuntimeMemorySize(Object o) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static uint GetMonoHeapSize() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static uint GetMonoUsedSize() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static uint GetTotalAllocatedMemory() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static uint GetTotalUnusedReservedMemory() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static uint GetTotalReservedMemory() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
