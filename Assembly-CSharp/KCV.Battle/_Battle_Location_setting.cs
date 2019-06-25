using UnityEngine;

namespace KCV.Battle
{
	public class _Battle_Location_setting : MonoBehaviour
	{
		private int ShipState;

		private int NowShip;

		private bool isDamaged;

		private GameObject TargetObject;

		private mst_shipgraphbattle ShipOffset;

		private float offsetX;

		private float offsetY;

		private UIBattleShip to;

		private void Start()
		{
			NowShip = 140;
			isDamaged = false;
			ShipState = 9;
		}

		private void _draw()
		{
			TargetObject = GameObject.Find("/BattleTaskManager/Stage/BattleField/FriendFleetAnchor/吹雪/ShipTexture/Object3D");
			to = GameObject.Find("/BattleTaskManager/Stage/BattleField/FriendFleetAnchor/吹雪").GetComponent<UIBattleShip>();
			ShipOffset = Resources.Load<mst_shipgraphbattle>("Data/mst_shipgraphbattle");
			if (!isDamaged)
			{
				offsetX = ShipOffset.param[NowShip].foot_x;
				offsetY = ShipOffset.param[NowShip].foot_y;
				ShipState = 9;
			}
			else
			{
				offsetX = ShipOffset.param[NowShip].foot_d_x;
				offsetY = ShipOffset.param[NowShip].foot_d_y;
				ShipState = 10;
			}
			TargetObject.transform.localPosition = new Vector3(offsetX, offsetY, 0f);
			Debug.Log("ShipNo: " + NowShip + " /ShipState =" + ShipState + "  offset(x,y)=" + offsetX + "," + offsetY);
			to.object3D.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(NowShip, ShipState);
			if (to.object3D.mainTexture == null)
			{
				Debug.Log("Null Texture.");
			}
			to.object3D.MakePixelPerfect();
		}

		private void Update()
		{
			if (Input.GetKeyDown("3"))
			{
				if (isDamaged)
				{
					isDamaged = false;
				}
				else
				{
					isDamaged = true;
				}
				_draw();
			}
			if (Input.GetKeyDown("1"))
			{
				if (NowShip != 1)
				{
					NowShip--;
				}
				isDamaged = false;
				_draw();
			}
			if (Input.GetKeyDown("2"))
			{
				if (NowShip != 999)
				{
					NowShip++;
				}
				isDamaged = false;
				_draw();
			}
		}
	}
}
