using System.Collections;
using UnityEngine;

public class RebellionArrow : MonoBehaviour
{
	private UITexture Arrow;

	public Vector3 FromTilePos;

	public Vector3 TargetTilePos;

	[Button("DebugAnimation", "START", new object[]
	{

	})]
	public int button1;

	[Button("EndAnimation", "END", new object[]
	{

	})]
	public int button2;

	private float movedValue;

	public float speed = 0.1f;

	public float moveDistance = 5f;

	private bool isEnd;

	private IEnumerator update()
	{
		while (!isEnd)
		{
			Arrow.transform.LookAt2D(TargetTilePos, Vector2.up);
			moveArrow();
			yield return new WaitForEndOfFrame();
		}
		TweenAlpha ta = TweenAlpha.Begin(Arrow.gameObject, 0.2f, 0f);
		ta.SetOnFinished(delegate
		{
			Object.Destroy(this.gameObject);
		});
	}

	private void moveArrow()
	{
		Arrow.transform.position += Arrow.transform.TransformDirection(Vector2.up) * speed;
		movedValue += speed;
		if (movedValue > moveDistance)
		{
			Arrow.transform.position = FromTilePos;
			movedValue = 0f;
		}
		float num = movedValue / moveDistance;
		Arrow.alpha = num * 1.5f;
	}

	public void StartAnimation(Vector3 fromTile, Vector3 targetTile)
	{
		movedValue = 0f;
		Arrow = GetComponent<UITexture>();
		isEnd = false;
		FromTilePos = fromTile;
		TargetTilePos = targetTile;
		Arrow.transform.position = FromTilePos;
		StartCoroutine(update());
	}

	public void EndAnimation()
	{
		isEnd = true;
	}

	private void DebugAnimation()
	{
		StartAnimation(FromTilePos, TargetTilePos);
	}
}
