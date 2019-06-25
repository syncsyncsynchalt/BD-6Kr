using System;
using System.Text.RegularExpressions;
using UnityEngine.Serialization;

namespace UnityEngine.Events
{
	[Serializable]
	internal class ArgumentCache : ISerializationCallbackReceiver
	{
		private const string kVersionString = ", Version=\\d+.\\d+.\\d+.\\d+";

		private const string kCultureString = ", Culture=\\w+";

		private const string kTokenString = ", PublicKeyToken=\\w+";

		[FormerlySerializedAs("objectArgument")]
		[SerializeField]
		private Object m_ObjectArgument;

		[FormerlySerializedAs("objectArgumentAssemblyTypeName")]
		[SerializeField]
		private string m_ObjectArgumentAssemblyTypeName;

		[SerializeField]
		[FormerlySerializedAs("intArgument")]
		private int m_IntArgument;

		[FormerlySerializedAs("floatArgument")]
		[SerializeField]
		private float m_FloatArgument;

		[SerializeField]
		[FormerlySerializedAs("stringArgument")]
		private string m_StringArgument;

		[SerializeField]
		private bool m_BoolArgument;

		public Object unityObjectArgument
		{
			get
			{
				return m_ObjectArgument;
			}
			set
			{
				m_ObjectArgument = value;
				m_ObjectArgumentAssemblyTypeName = ((!(value != null)) ? string.Empty : value.GetType().AssemblyQualifiedName);
			}
		}

		public string unityObjectArgumentAssemblyTypeName => m_ObjectArgumentAssemblyTypeName;

		public int intArgument
		{
			get
			{
				return m_IntArgument;
			}
			set
			{
				m_IntArgument = value;
			}
		}

		public float floatArgument
		{
			get
			{
				return m_FloatArgument;
			}
			set
			{
				m_FloatArgument = value;
			}
		}

		public string stringArgument
		{
			get
			{
				return m_StringArgument;
			}
			set
			{
				m_StringArgument = value;
			}
		}

		public bool boolArgument
		{
			get
			{
				return m_BoolArgument;
			}
			set
			{
				m_BoolArgument = value;
			}
		}

		private void TidyAssemblyTypeName()
		{
			if (!string.IsNullOrEmpty(m_ObjectArgumentAssemblyTypeName))
			{
				m_ObjectArgumentAssemblyTypeName = Regex.Replace(m_ObjectArgumentAssemblyTypeName, ", Version=\\d+.\\d+.\\d+.\\d+", string.Empty);
				m_ObjectArgumentAssemblyTypeName = Regex.Replace(m_ObjectArgumentAssemblyTypeName, ", Culture=\\w+", string.Empty);
				m_ObjectArgumentAssemblyTypeName = Regex.Replace(m_ObjectArgumentAssemblyTypeName, ", PublicKeyToken=\\w+", string.Empty);
			}
		}

		public void OnBeforeSerialize()
		{
			TidyAssemblyTypeName();
		}

		public void OnAfterDeserialize()
		{
			TidyAssemblyTypeName();
		}
	}
}
