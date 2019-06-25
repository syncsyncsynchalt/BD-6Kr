using local.models;
using UnityEngine;

namespace KCV.Production
{
	public class Test_BattleResultsReceiveShip : MonoBehaviour
	{
		private ProdReceiveShip _prodReceievShip;

		private KeyControl _clsInput;

		private void Awake()
		{
			_clsInput = new KeyControl();
		}

		private void Update()
		{
			_clsInput.Update();
			if (Input.GetKeyDown(KeyCode.B))
			{
				Reward_Ship rewardShip = new Reward_Ship(131);
				_prodReceievShip = ProdReceiveShip.Instantiate(PrefabFile.Load<ProdReceiveShip>(PrefabFileInfos.CommonProdReceiveShip), base.transform.parent, rewardShip, 1, _clsInput);
				_prodReceievShip.Play(delegate
				{
					Debug.Log("艦娘ドロップ演出終了");
				});
			}
		}

		private void _onFinished()
		{
		}
	}
}
