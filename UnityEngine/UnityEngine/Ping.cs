using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Ping
	{
		private IntPtr pingWrapper;

		public bool isDone
		{
			get;
		}

		public int time
		{
			get;
		}

		public string ip
		{
			get;
		}

		public Ping(string address) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void DestroyPing() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		~Ping()
		{
			DestroyPing();
		}
	}
}
