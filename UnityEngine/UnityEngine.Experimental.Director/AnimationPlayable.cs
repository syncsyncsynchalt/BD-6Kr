using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.Director
{
	public class AnimationPlayable : Playable
	{
		public AnimationPlayable()
			: base(callCPPConstructor: false)
		{
			m_Ptr = IntPtr.Zero;
			InstantiateEnginePlayable();
		}

		public AnimationPlayable(bool final)
			: base(callCPPConstructor: false)
		{
			m_Ptr = IntPtr.Zero;
			if (final)
			{
				InstantiateEnginePlayable();
			}
		}

		private void InstantiateEnginePlayable() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public virtual int AddInput(AnimationPlayable source)
		{
			Playable.Connect(source, this, -1, -1);
			Playable[] inputs = GetInputs();
			return inputs.Length - 1;
		}

		public virtual bool SetInput(AnimationPlayable source, int index)
		{
			if (!CheckInputBounds(index))
			{
				return false;
			}
			Playable[] inputs = GetInputs();
			if (inputs[index] != null)
			{
				Playable.Disconnect(this, index);
			}
			return Playable.Connect(source, this, -1, index);
		}

		public virtual bool SetInputs(IEnumerable<AnimationPlayable> sources)
		{
			Playable[] inputs = GetInputs();
			int num = inputs.Length;
			for (int i = 0; i < num; i++)
			{
				Playable.Disconnect(this, i);
			}
			bool flag = false;
			int num2 = 0;
			foreach (AnimationPlayable source in sources)
			{
				flag = ((num2 >= num) ? (flag | Playable.Connect(source, this, -1, -1)) : (flag | Playable.Connect(source, this, -1, num2)));
				SetInputWeight(num2, 1f);
				num2++;
			}
			for (int j = num2; j < num; j++)
			{
				SetInputWeight(j, 0f);
			}
			return flag;
		}

		public virtual bool RemoveInput(int index)
		{
			if (!CheckInputBounds(index))
			{
				return false;
			}
			Playable.Disconnect(this, index);
			return true;
		}

		public virtual bool RemoveInput(AnimationPlayable playable)
		{
			if (!Playable.CheckPlayableValidity(playable, "playable"))
			{
				return false;
			}
			Playable[] inputs = GetInputs();
			for (int i = 0; i < inputs.Length; i++)
			{
				if (inputs[i] == playable)
				{
					Playable.Disconnect(this, i);
					return true;
				}
			}
			return false;
		}

		public virtual bool RemoveAllInputs()
		{
			Playable[] inputs = GetInputs();
			for (int i = 0; i < inputs.Length; i++)
			{
				RemoveInput(inputs[i] as AnimationPlayable);
			}
			return true;
		}
	}
}
