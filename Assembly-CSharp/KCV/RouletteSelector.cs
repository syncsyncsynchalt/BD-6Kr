using System;
using UnityEngine;

namespace KCV
{
	public class RouletteSelector : MonoBehaviour
	{
		[SerializeField]
		private GameObject container;

		[SerializeField]
		private float horizontalRadius;

		[SerializeField]
		private float verticalRadius;

		[SerializeField]
		private float interval;

		[SerializeField]
		private float maxScale;

		[SerializeField]
		private float minScale;

		[SerializeField]
		private float maxAlpha;

		[SerializeField]
		private float minAlpha;

		[SerializeField]
		private int maxDepth;

		[SerializeField]
		private int minDepth;

		private int currentIdx;

		private bool moving;

		private float eachRadian;

		private float elapsedTime;

		private float moveRadian;

		private RouletteSelectorHandler handler;

		private KeyControl keyController;

		private bool dirty;

		private float PIx2 = (float)Math.PI * 2f;

		private float OFFSET_PI = (float)Math.PI / 2f;

		private float tmpRadian;

		private bool radiusScaling;

		private float scaleInterval;

		private float startRadiusScale;

		private float goalRadiusScale;

		private float radiusScaleElapsedTime;

		private float radiusScale = 1f;

		private int itemCount => container.transform.childCount;

		public bool controllable
		{
			get;
			set;
		}

		public float intervalTime
		{
			get
			{
				return interval;
			}
			set
			{
				if (value != interval)
				{
					interval = value;
				}
			}
		}

		public float scalaMax
		{
			get
			{
				return maxScale;
			}
			set
			{
				if (value != maxScale)
				{
					maxScale = value;
				}
			}
		}

		public float scalaMin
		{
			get
			{
				return minScale;
			}
			set
			{
				if (value != minScale)
				{
					minScale = value;
				}
			}
		}

		public float alphaMax
		{
			get
			{
				return maxAlpha;
			}
			set
			{
				if (value != maxAlpha)
				{
					maxAlpha = value;
				}
			}
		}

		public float alphaMin
		{
			get
			{
				return minAlpha;
			}
			set
			{
				if (value != minAlpha)
				{
					minAlpha = value;
				}
			}
		}

		public int depthMax
		{
			get
			{
				return maxDepth;
			}
			set
			{
				if (value != maxDepth)
				{
					maxDepth = value;
				}
			}
		}

		public int depthMin
		{
			get
			{
				return minDepth;
			}
			set
			{
				if (value != minDepth)
				{
					minDepth = value;
				}
			}
		}

		public void Init(RouletteSelectorHandler handler)
		{
			this.handler = handler;
			eachRadian = PIx2 / (float)itemCount;
			goalRadiusScale = 1f;
			CancelMoving();
			CancelRadiusScaling();
			controllable = true;
			for (int i = 0; i < itemCount; i++)
			{
				if (handler.IsSelectable(i))
				{
					SetCurrent(i);
					break;
				}
			}
			Reposition();
		}

		public void SetCurrent(int idx)
		{
			currentIdx = idx;
			handler.OnUpdateIndex(currentIdx, container.transform.GetChild(currentIdx));
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.keyController = keyController;
		}

		public void SetHorizontalRadius(int value)
		{
			horizontalRadius = value;
		}

		public void SetVerticalRadius(int value)
		{
			verticalRadius = value;
		}

		public void Scale(float interval, float scale)
		{
			if (!radiusScaling)
			{
				startRadiusScale = radiusScale;
				goalRadiusScale = scale;
				radiusScaleElapsedTime = 0f;
				scaleInterval = interval;
				radiusScaling = true;
			}
		}

		public void ScaleForce(float interval, float scale)
		{
			CancelRadiusScaling();
			Scale(interval, scale);
		}

		public void CancelMoving()
		{
			moving = false;
			dirty = true;
		}

		public void CancelRadiusScaling()
		{
			radiusScaling = false;
			radiusScale = goalRadiusScale;
			dirty = true;
		}

		public void Update()
		{
			if (base.enabled && controllable && !moving && keyController != null)
			{
				if (keyController.IsLeftDown())
				{
					MovePrev();
				}
				else if (keyController.IsRightDown())
				{
					MoveNext();
				}
				else if (keyController.IsMaruDown())
				{
					Determine();
				}
			}
			if (moving)
			{
				Move();
			}
			if (radiusScaling)
			{
				DoScale();
			}
			if (dirty)
			{
				Reposition();
			}
		}

		private void Move()
		{
			dirty = true;
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= interval)
			{
				moving = false;
				tmpRadian = 0f;
			}
			else
			{
				float num = elapsedTime / interval;
				tmpRadian = moveRadian * (1f - num);
			}
		}

		private void DoScale()
		{
			dirty = true;
			radiusScaleElapsedTime += Time.deltaTime;
			if (radiusScaleElapsedTime >= scaleInterval)
			{
				radiusScaling = false;
				radiusScale = goalRadiusScale;
			}
			else
			{
				float num = radiusScaleElapsedTime / scaleInterval;
				radiusScale = startRadiusScale * (1f - num) + goalRadiusScale * num;
			}
		}

		public void Reposition()
		{
			for (int i = 0; i < itemCount; i++)
			{
				float num = OFFSET_PI + eachRadian * (float)(currentIdx - i);
				if (moving)
				{
					num += tmpRadian;
				}
				float num2 = (float)Math.Sin(num);
				Transform child = container.transform.GetChild(i);
				child.localPositionX((float)((double)horizontalRadius * Math.Cos(num)) * radiusScale);
				child.localPositionY((0f - verticalRadius * num2) * radiusScale);
				float num3 = (1f + num2) * 0.5f;
				float num4 = minScale + (maxScale - minScale) * num3;
				child.localScaleX(num4);
				child.localScaleY(num4);
				float alpha = minAlpha + (maxAlpha - minAlpha) * num3;
				UISprite component = ((Component)child).GetComponent<UISprite>();
				if (component != null)
				{
					component.alpha = alpha;
				}
				UITexture component2 = ((Component)child).GetComponent<UITexture>();
				if (component2 != null)
				{
					component2.alpha = alpha;
				}
				UIPanel component3 = ((Component)child).GetComponent<UIPanel>();
				if (component3 != null)
				{
					component3.alpha = alpha;
				}
				UIWidget component4 = ((Component)child).GetComponent<UIWidget>();
				if (component4 != null)
				{
					component4.alpha = alpha;
				}
				int depth = minDepth + (int)((float)(maxDepth - minDepth) * num3);
				if (component4 != null)
				{
					component4.depth = depth;
				}
				if (component3 != null)
				{
					component3.depth = depth;
				}
			}
		}

		public void MovePrev()
		{
			PrepareMove(forward: false);
		}

		public void MoveNext()
		{
			PrepareMove(forward: true);
		}

		public void Determine()
		{
			if (handler.IsSelectable(currentIdx))
			{
				Debug.Log("currentIdx:" + currentIdx);
				handler.OnSelect(currentIdx, container.transform.GetChild(currentIdx));
			}
		}

		private void PrepareMove(bool forward)
		{
			if (moving)
			{
				return;
			}
			int num = currentIdx;
			int num2 = 0;
			int num3 = forward ? 1 : (-1);
			do
			{
				num += num3;
				if (num < 0)
				{
					num = itemCount - 1;
				}
				else if (num >= itemCount)
				{
					num = 0;
				}
				num2++;
				if (num2 >= itemCount)
				{
					throw new Exception("選択可能な項目がありません。");
				}
			}
			while (!handler.IsSelectable(num));
			SetCurrent(num);
			elapsedTime = 0f;
			moving = true;
			moveRadian = eachRadian * (float)num2;
			if (forward)
			{
				moveRadian = 0f - moveRadian;
			}
		}

		public GameObject GetContainer()
		{
			return container;
		}
	}
}
