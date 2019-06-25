namespace UnityEngine
{
	public struct GradientColorKey
	{
		public Color color;

		public float time;

		public GradientColorKey(Color col, float time)
		{
			color = col;
			this.time = time;
		}
	}
}
