using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class BitStream
	{
		internal IntPtr m_Ptr;

		public bool isReading
		{
			get;
		}

		public bool isWriting
		{
			get;
		}

		private void Serializeb(ref int value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Serializec(ref char value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Serializes(ref short value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Serializei(ref int value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Serializef(ref float value, float maximumDelta) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Serializeq(ref Quaternion value, float maximumDelta)
		{
			INTERNAL_CALL_Serializeq(this, ref value, maximumDelta);
		}

		private static void INTERNAL_CALL_Serializeq(BitStream self, ref Quaternion value, float maximumDelta) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Serializev(ref Vector3 value, float maximumDelta)
		{
			INTERNAL_CALL_Serializev(this, ref value, maximumDelta);
		}

		private static void INTERNAL_CALL_Serializev(BitStream self, ref Vector3 value, float maximumDelta) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Serializen(ref NetworkViewID viewID)
		{
			INTERNAL_CALL_Serializen(this, ref viewID);
		}

		private static void INTERNAL_CALL_Serializen(BitStream self, ref NetworkViewID viewID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Serialize(ref bool value)
		{
			int value2 = value ? 1 : 0;
			Serializeb(ref value2);
			value = ((value2 != 0) ? true : false);
		}

		public void Serialize(ref char value)
		{
			Serializec(ref value);
		}

		public void Serialize(ref short value)
		{
			Serializes(ref value);
		}

		public void Serialize(ref int value)
		{
			Serializei(ref value);
		}

		[ExcludeFromDocs]
		public void Serialize(ref float value)
		{
			float maxDelta = 1E-05f;
			Serialize(ref value, maxDelta);
		}

		public void Serialize(ref float value, [DefaultValue("0.00001F")] float maxDelta)
		{
			Serializef(ref value, maxDelta);
		}

		[ExcludeFromDocs]
		public void Serialize(ref Quaternion value)
		{
			float maxDelta = 1E-05f;
			Serialize(ref value, maxDelta);
		}

		public void Serialize(ref Quaternion value, [DefaultValue("0.00001F")] float maxDelta)
		{
			Serializeq(ref value, maxDelta);
		}

		[ExcludeFromDocs]
		public void Serialize(ref Vector3 value)
		{
			float maxDelta = 1E-05f;
			Serialize(ref value, maxDelta);
		}

		public void Serialize(ref Vector3 value, [DefaultValue("0.00001F")] float maxDelta)
		{
			Serializev(ref value, maxDelta);
		}

		public void Serialize(ref NetworkPlayer value)
		{
			int value2 = value.index;
			Serializei(ref value2);
			value.index = value2;
		}

		public void Serialize(ref NetworkViewID viewID)
		{
			Serializen(ref viewID);
		}

		private void Serialize(ref string value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
