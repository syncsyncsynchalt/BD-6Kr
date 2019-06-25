using UnityEngine;

namespace KCV.Battle.Production
{
	[ExecuteInEditMode]
	public class SearchLight : MonoBehaviour
	{
		[Range(0f, 1f)]
		public float _brightness = 1f;

		public Color _baseColor = Color.white;

		public LensFlare _flare;

		private void OnDestroy()
		{
			Mem.Del(ref _brightness);
			Mem.Del(ref _baseColor);
			Mem.Del(ref _flare);
		}

		private void Update()
		{
			_flare.color = _baseColor * _brightness;
		}
	}
}
