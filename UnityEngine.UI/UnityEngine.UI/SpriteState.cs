using System;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	[Serializable]
	public struct SpriteState
	{
		[SerializeField]
		[FormerlySerializedAs("m_SelectedSprite")]
		[FormerlySerializedAs("highlightedSprite")]
		private Sprite m_HighlightedSprite;

		[SerializeField]
		[FormerlySerializedAs("pressedSprite")]
		private Sprite m_PressedSprite;

		[SerializeField]
		[FormerlySerializedAs("disabledSprite")]
		private Sprite m_DisabledSprite;

		public Sprite highlightedSprite
		{
			get
			{
				return m_HighlightedSprite;
			}
			set
			{
				m_HighlightedSprite = value;
			}
		}

		public Sprite pressedSprite
		{
			get
			{
				return m_PressedSprite;
			}
			set
			{
				m_PressedSprite = value;
			}
		}

		public Sprite disabledSprite
		{
			get
			{
				return m_DisabledSprite;
			}
			set
			{
				m_DisabledSprite = value;
			}
		}
	}
}
