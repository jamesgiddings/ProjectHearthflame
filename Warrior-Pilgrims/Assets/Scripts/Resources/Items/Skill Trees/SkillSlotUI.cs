using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GramophoneUtils.Items.Containers
{
	public class SkillSlotUI : ResourceSlotUI, IDragHandler
	{
		[SerializeField] private ISkill slotSkill;

		public override IResource SlotResource => (IResource) slotSkill;

		public void OnDrag(PointerEventData eventData)
		{
			return;
		}

		private void OnEnable()
		{
			UpdateSlotUI();
		}

		protected override void EnableSlotUI(bool enable)
		{
			resourceIconImage.enabled = enable;
			//itemQuantityText.enabled = enable;
		}

		public override void OnDrop(PointerEventData eventData)
		{
			return;
		}

		public override void UpdateSlotUI()
		{
			resourceIconImage.sprite = SlotResource.Sprite;

			EnableSlotUI(true);

			//SetItemQuantityUI();
		}
	}
}

