using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.Director
{
	public class Playable : IDisposable
	{
		internal IntPtr m_Ptr;

		internal int m_UniqueId;

		public double time
		{
			get;
			set;
		}

		public PlayState state
		{
			get;
			set;
		}

		public int inputCount
		{
			get;
		}

		public int outputCount
		{
			get;
		}

		public Playable()
		{
			m_Ptr = IntPtr.Zero;
			m_UniqueId = GenerateUniqueId();
			InstantiateEnginePlayable();
		}

		internal Playable(bool callCPPConstructor)
		{
			m_Ptr = IntPtr.Zero;
			m_UniqueId = GenerateUniqueId();
			if (callCPPConstructor)
			{
				InstantiateEnginePlayable();
			}
		}

		private void Dispose(bool disposing)
		{
			ReleaseEnginePlayable();
			m_Ptr = IntPtr.Zero;
		}

		internal int GetUniqueIDInternal() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool Connect(Playable source, Playable target)
		{
			return Connect(source, target, -1, -1);
		}

		public static bool Connect(Playable source, Playable target, int sourceOutputPort, int targetInputPort)
		{
			if (!CheckPlayableValidity(source, "source") && !CheckPlayableValidity(target, "target"))
			{
				return false;
			}
			if (source != null && !source.CheckInputBounds(sourceOutputPort, acceptAny: true))
			{
				return false;
			}
			if (!target.CheckInputBounds(targetInputPort, acceptAny: true))
			{
				return false;
			}
			return ConnectInternal(source, target, sourceOutputPort, targetInputPort);
		}

		public static void Disconnect(Playable target, int inputPort)
		{
			if (CheckPlayableValidity(target, "target") && target.CheckInputBounds(inputPort))
			{
				DisconnectInternal(target, inputPort);
			}
		}

		private void ReleaseEnginePlayable() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void InstantiateEnginePlayable() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private int GenerateUniqueId() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private bool SetInputWeightInternal(int inputIndex, float weight) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private float GetInputWeightInternal(int inputIndex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static bool ConnectInternal(Playable source, Playable target, int sourceOutputPort, int targetInputPort) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static void DisconnectInternal(Playable target, int inputPort) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Playable GetInput(int inputPort) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Playable[] GetInputs() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void ClearInputs() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Playable GetOutput(int outputPort) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Playable[] GetOutputs() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void GetInputsInternal(object list) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void GetOutputsInternal(object list) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		~Playable()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		public override bool Equals(object p)
		{
			return CompareIntPtr(this, p as Playable);
		}

		public override int GetHashCode()
		{
			return m_UniqueId;
		}

		internal static bool CompareIntPtr(Playable lhs, Playable rhs)
		{
			bool flag = (object)lhs == null || !IsNativePlayableAlive(lhs);
			bool flag2 = (object)rhs == null || !IsNativePlayableAlive(rhs);
			if (flag2 && flag)
			{
				return true;
			}
			if (flag2)
			{
				return !IsNativePlayableAlive(lhs);
			}
			if (flag)
			{
				return !IsNativePlayableAlive(rhs);
			}
			return lhs.GetUniqueIDInternal() == rhs.GetUniqueIDInternal();
		}

		internal static bool IsNativePlayableAlive(Playable p)
		{
			return p.m_Ptr != IntPtr.Zero;
		}

		internal static bool CheckPlayableValidity(Playable playable, string name)
		{
			if (playable == null)
			{
				throw new NullReferenceException("Playable " + name + "is null");
			}
			return true;
		}

		internal bool CheckInputBounds(int inputIndex)
		{
			return CheckInputBounds(inputIndex, acceptAny: false);
		}

		internal bool CheckInputBounds(int inputIndex, bool acceptAny)
		{
			if (inputIndex == -1 && acceptAny)
			{
				return true;
			}
			if (inputIndex < 0)
			{
				throw new IndexOutOfRangeException("Index must be greater than 0");
			}
			Playable[] inputs = GetInputs();
			if (inputs.Length <= inputIndex)
			{
				throw new IndexOutOfRangeException("inputIndex " + inputIndex + " is greater than the number of available inputs (" + inputs.Length + ").");
			}
			return true;
		}

		public float GetInputWeight(int inputIndex)
		{
			if (CheckInputBounds(inputIndex))
			{
				return GetInputWeightInternal(inputIndex);
			}
			return -1f;
		}

		public bool SetInputWeight(int inputIndex, float weight)
		{
			if (CheckInputBounds(inputIndex))
			{
				return SetInputWeightInternal(inputIndex, weight);
			}
			return false;
		}

		public void GetInputs(List<Playable> inputList)
		{
			inputList.Clear();
			GetInputsInternal(inputList);
		}

		public void GetOutputs(List<Playable> outputList)
		{
			outputList.Clear();
			GetOutputsInternal(outputList);
		}

		public virtual void PrepareFrame(FrameData info)
		{
		}

		public virtual void ProcessFrame(FrameData info, object playerData)
		{
		}

		public virtual void OnSetTime(float localTime)
		{
		}

		public virtual void OnSetPlayState(PlayState newState)
		{
		}

		public static bool operator ==(Playable x, Playable y)
		{
			return CompareIntPtr(x, y);
		}

		public static bool operator !=(Playable x, Playable y)
		{
			return !CompareIntPtr(x, y);
		}

		public static implicit operator bool(Playable exists)
		{
			return !CompareIntPtr(exists, null);
		}
	}
}
