namespace UnityEngine
{
	public struct Touch
	{
		private int m_FingerId;

		private Vector2 m_Position;

		private Vector2 m_RawPosition;

		private Vector2 m_PositionDelta;

		private float m_TimeDelta;

		private int m_TapCount;

		private TouchPhase m_Phase;

		private float m_Pressure;

		private float m_maximumPossiblePressure;

		public int fingerId => m_FingerId;

		public Vector2 position => m_Position;

		public Vector2 rawPosition => m_RawPosition;

		public Vector2 deltaPosition => m_PositionDelta;

		public float deltaTime => m_TimeDelta;

		public int tapCount => m_TapCount;

		public TouchPhase phase => m_Phase;

		public float pressure => m_Pressure;

		public float maximumPossiblePressure => m_maximumPossiblePressure;
	}
}
