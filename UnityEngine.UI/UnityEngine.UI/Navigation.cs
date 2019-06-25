using System;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	[Serializable]
	public struct Navigation
	{
		[Flags]
		public enum Mode
		{
			None = 0x0,
			Horizontal = 0x1,
			Vertical = 0x2,
			Automatic = 0x3,
			Explicit = 0x4
		}

		[SerializeField]
		[FormerlySerializedAs("mode")]
		private Mode m_Mode;

		[SerializeField]
		[FormerlySerializedAs("selectOnUp")]
		private Selectable m_SelectOnUp;

		[FormerlySerializedAs("selectOnDown")]
		[SerializeField]
		private Selectable m_SelectOnDown;

		[SerializeField]
		[FormerlySerializedAs("selectOnLeft")]
		private Selectable m_SelectOnLeft;

		[FormerlySerializedAs("selectOnRight")]
		[SerializeField]
		private Selectable m_SelectOnRight;

		public Mode mode
		{
			get
			{
				return m_Mode;
			}
			set
			{
				m_Mode = value;
			}
		}

		public Selectable selectOnUp
		{
			get
			{
				return m_SelectOnUp;
			}
			set
			{
				m_SelectOnUp = value;
			}
		}

		public Selectable selectOnDown
		{
			get
			{
				return m_SelectOnDown;
			}
			set
			{
				m_SelectOnDown = value;
			}
		}

		public Selectable selectOnLeft
		{
			get
			{
				return m_SelectOnLeft;
			}
			set
			{
				m_SelectOnLeft = value;
			}
		}

		public Selectable selectOnRight
		{
			get
			{
				return m_SelectOnRight;
			}
			set
			{
				m_SelectOnRight = value;
			}
		}

		public static Navigation defaultNavigation
		{
			get
			{
				Navigation result = default(Navigation);
				result.m_Mode = Mode.Automatic;
				return result;
			}
		}
	}
}
