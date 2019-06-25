using System;

using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public class MonoBehaviour : Behaviour
	{
		public bool useGUILayout
		{
			get;
			set;
		}

		// public MonoBehaviour() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Internal_CancelInvokeAll() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private bool Internal_IsInvokingAll() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Invoke(string methodName, float time) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InvokeRepeating(string methodName, float time, float repeatRate) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void CancelInvoke()
		{
			Internal_CancelInvokeAll();
		}

		public void CancelInvoke(string methodName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool IsInvoking(string methodName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool IsInvoking()
		{
			return Internal_IsInvokingAll();
		}

		public Coroutine StartCoroutine(IEnumerator routine)
		{
			return StartCoroutine_Auto(routine);
		}

		public Coroutine StartCoroutine_Auto(IEnumerator routine) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public Coroutine StartCoroutine(string methodName)
		{
			object value = null;
			return StartCoroutine(methodName, value);
		}

		public void StopCoroutine(string methodName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void StopCoroutine(IEnumerator routine)
		{
			StopCoroutineViaEnumerator_Auto(routine);
		}

		public void StopCoroutine(Coroutine routine)
		{
			StopCoroutine_Auto(routine);
		}

		internal void StopCoroutineViaEnumerator_Auto(IEnumerator routine) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void StopCoroutine_Auto(Coroutine routine) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void StopAllCoroutines() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void print(object message)
		{
			Debug.Log(message);
		}
	}
}
