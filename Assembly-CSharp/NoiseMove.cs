using UnityEngine;

[ExecuteInEditMode]
public class NoiseMove : MonoBehaviour
{
	public float _noiseSpeed = 8f;

	public float _noisePower;

	private void OnDestroy()
	{
		Mem.Del(ref _noiseSpeed);
		Mem.Del(ref _noisePower);
	}

	private void Update()
	{
		float time = Time.time;
		float num = time;
		float x = (Mathf.PerlinNoise(num * _noiseSpeed, 0f) * 2f - 1f) * _noisePower;
		float y = (Mathf.PerlinNoise(num * _noiseSpeed, 40f) * 2f - 1f) * _noisePower;
		base.transform.localPosition = new Vector3(x, y);
	}
}
