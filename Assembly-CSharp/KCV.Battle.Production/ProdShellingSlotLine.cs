using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdShellingSlotLine : MonoBehaviour
	{
		[SerializeField]
		private ProdShellingLine _prodShellingLine;

		[SerializeField]
		private ProdShellingSlot _prodShellingSlot;

		public int baseDepth
		{
			get
			{
				return _prodShellingLine.panel.depth;
			}
			set
			{
				_prodShellingLine.panel.depth = value;
				_prodShellingSlot.panel.depth = value + 1;
			}
		}

		public static ProdShellingSlotLine Instantiate(ProdShellingSlotLine prefab, Transform parent)
		{
			ProdShellingSlotLine prodShellingSlotLine = UnityEngine.Object.Instantiate(prefab);
			prodShellingSlotLine.transform.parent = parent;
			prodShellingSlotLine.transform.localScaleOne();
			prodShellingSlotLine.transform.localPositionZero();
			return prodShellingSlotLine;
		}

		private void Awake()
		{
			_prodShellingLine.transform.localScaleZero();
			_prodShellingSlot.transform.localScaleZero();
			baseDepth = 0;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _prodShellingLine);
			Mem.Del(ref _prodShellingSlot);
		}

		public void SetSlotData(SlotitemModel_Battle model, bool isFriend)
		{
			_prodShellingSlot.SetSlotData(model);
		}

		public void SetSlotData(List<SlotitemModel_Battle> models, bool isFriend)
		{
			_prodShellingSlot.SetSlotData(models.ToArray());
		}

		public void SetSlotData(SlotitemModel_Battle[] models, ProdTranscendenceCutIn.AnimationList iList)
		{
			_prodShellingSlot.SetSlotData(models, iList);
		}

		public void Play(BaseProdLine.AnimationName iName, bool isFriend, Action callback)
		{
			base.transform.localScaleOne();
			if (iName != BaseProdLine.AnimationName.ProdSuccessiveLine)
			{
				_prodShellingLine.Play(isFriend);
			}
			_prodShellingSlot.Play(iName, isFriend, callback);
		}

		public void PlayTranscendenceLine(BaseProdLine.AnimationName iName, bool isFriend, Action callback)
		{
			base.transform.localScaleOne();
			_prodShellingSlot.Play(iName, isFriend, callback);
		}
	}
}
