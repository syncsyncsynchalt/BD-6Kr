using KCV.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(Animation))]
	public class ProdTranscendenceSlots : MonoBehaviour
	{
		private Animation _anim;

		private List<UISlotItemHexButton> _listHexBtn;

		public List<UISlotItemHexButton> hexButtonList => _listHexBtn;

		private void OnDestroy()
		{
			Mem.Del(ref _anim);
			Mem.DelListSafe(ref _listHexBtn);
		}

		public bool Init()
		{
			_anim = GetComponent<Animation>();
			_anim.Stop();
			_listHexBtn = new List<UISlotItemHexButton>();
			for (int i = 0; i < 3; i++)
			{
				_listHexBtn.Add(((Component)base.transform.FindChild($"TAHexBtn{i + 1}")).GetComponent<UISlotItemHexButton>());
			}
			return true;
		}

		public void Play(ProdTranscendenceCutIn.AnimationList iList)
		{
			_anim.Play($"{iList.ToString()}Slots");
		}

		private void playSlotItem(int nSlotNum)
		{
			_listHexBtn[nSlotNum].SetActive(isActive: true);
			_listHexBtn[nSlotNum].Play(UIHexButton.AnimationList.ProdTranscendenceAttackHex, null);
		}

		private void playProdTATorpedox2Slots()
		{
			_listHexBtn.ForEach(delegate(UISlotItemHexButton x)
			{
				x.SetActive(isActive: true);
				x.Play(UIHexButton.AnimationList.ProdTranscendenceAttackHex, null);
			});
		}

		private void PlaySlotSE()
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_939a);
		}
	}
}
