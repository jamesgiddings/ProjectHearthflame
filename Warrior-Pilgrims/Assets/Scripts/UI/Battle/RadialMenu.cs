using AYellowpaper;
using GramophoneUtils.Characters;
using GramophoneUtils.Items;
using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
	[SerializeField] private BattleManager _battleManager;
	[SerializeField] private BattleDataModel battleDataModel;

	[SerializeField] private GameObject holder;

	[SerializeField] private GameObject subMenuPrefab;
	[SerializeField] private GameObject elementPrefab;

	[SerializeField] private Transform itemsElementsParent;
	[SerializeField] private Transform skillsElementsParent;
	[SerializeField] private Transform positionElementsParent;

	[SerializeField] private List<InterfaceReference<ISkill>> moveToSlotsSkills;

	RMF_RadialMenu subMenu;

	private Character character;

	private List<Item> items = new List<Item>();
	private List<ISkill> skills = new List<ISkill>();

    #region API

    public Character Character
	{
		get { return character; }
		set { character = value; UpdateDisplay(); Debug.Log("Setting character" ); }
	}

	public void InitialiseRadialMenu()
	{
		this.character = _battleManager.BattleDataModel.CurrentActor;

		InitialiseItemSubMenu();
		InitialiseSkillsSubMenu();
		UpdateDisplay();

		//InitialisePositionSubMenu();

		//battleManager.BattleDataModel.OnCurrentActorChanged += UpdateDisplay;
	}

	public void Destroy()
	{
		Destroy(this.gameObject);
	}

    #endregion

    private RMF_RadialMenu InitialiseItemSubMenu()
	{
		subMenu = UnityEngine.Object.Instantiate(subMenuPrefab, itemsElementsParent).GetComponent<RMF_RadialMenu>();

		Transform parent = subMenu.gameObject.transform.GetChild(0); // get the parent transform for the elements

		Button button = itemsElementsParent.GetComponent<Button>(); // get the button

        button.onClick.AddListener(delegate { subMenu.gameObject.SetActive(!subMenu.gameObject.activeInHierarchy); if (!subMenu.gameObject.activeInHierarchy) { _battleManager.TargetManager.ClearTargets(); } });

        foreach (ItemSlot itemSlot in character.PartyInventory.ItemSlots) // add the items from the partyInventory
		{
			//Debug.LogWarning("This should actually add from that characters quickbar, not the partyInventory"); // TODO
			if (itemSlot.item != null && itemSlot.item is ConsumableItem)
			{
                ConsumableItem consumableItem = (ConsumableItem)itemSlot.item;
                if (consumableItem.Skill.UseFromSlot.CanUseFromSlot(character))
				{ 
					items.Add(itemSlot.item);
					RMF_RadialMenuElement rMF_RadialMenuElement = Instantiate(elementPrefab, parent).GetComponent<RMF_RadialMenuElement>();
					rMF_RadialMenuElement.text.text = itemSlot.item.Name;
					subMenu.GetComponent<RMF_RadialMenu>().elements.Add(rMF_RadialMenuElement);

					rMF_RadialMenuElement.button.onClick.AddListener(delegate { _battleManager.GetTargets(consumableItem.Skill); });
				}
            }
		}

		return subMenu;
	}

	private RMF_RadialMenu InitialiseSkillsSubMenu()
	{
		RMF_RadialMenu subMenu = Instantiate(subMenuPrefab, skillsElementsParent).GetComponent<RMF_RadialMenu>();

		Transform parent = subMenu.gameObject.transform.GetChild(0); // get the parent transform for the elements

		Button button = skillsElementsParent.GetComponent<Button>(); // get the buttons

		button.onClick.AddListener(delegate { subMenu.gameObject.SetActive(!subMenu.gameObject.activeInHierarchy); if (!subMenu.gameObject.activeInHierarchy) { _battleManager.TargetManager.ClearTargets(); } });

		foreach (ISkill skill in character.SkillSystem.UnlockedSkills) // add the unlocked skills from the SkillSystem
		{
			//Debug.LogWarning("This should actually add from that characters quickbar, not the partyInventory"); //todo
			if (skill.UseFromSlot.CanUseFromSlot(character))
			{
                skills.Add(skill);

                RMF_RadialMenuElement rMF_RadialMenuElement = Instantiate(elementPrefab, parent).GetComponent<RMF_RadialMenuElement>();
                rMF_RadialMenuElement.text.text = skill.Name;
                subMenu.GetComponent<RMF_RadialMenu>().elements.Add(rMF_RadialMenuElement);

                rMF_RadialMenuElement.button.onClick.AddListener(delegate { _battleManager.GetTargets(skill); });
            }
		}

		return subMenu;
	}

	private RMF_RadialMenu InitialisePositionSubMenu()
	{
        RMF_RadialMenu subMenu = Instantiate(subMenuPrefab, positionElementsParent).GetComponent<RMF_RadialMenu>();

        Transform parent = subMenu.gameObject.transform.GetChild(0); // get the parent transform for the elements

        Button button = positionElementsParent.GetComponent<Button>(); // get the buttons

        button.onClick.AddListener(delegate { subMenu.gameObject.SetActive(!subMenu.gameObject.activeInHierarchy); if (!subMenu.gameObject.activeInHierarchy) { _battleManager.TargetManager.ClearTargets(); } });

        foreach (InterfaceReference<ISkill> skill in moveToSlotsSkills) // add the unlocked skills from the SkillSystem
        {
			// Todo, we know the character, so we can create a skill which targets an ally and moves them to the current slot, rather than a skill which 
			// moves that character? Or we need another option which is swap to slot in move, anyway, there should be a better option
            if (skill.Value.UseFromSlot.CanUseFromSlot(character))
            {
                skills.Add(skill.Value);

                RMF_RadialMenuElement rMF_RadialMenuElement = Instantiate(elementPrefab, parent).GetComponent<RMF_RadialMenuElement>();
                rMF_RadialMenuElement.text.text = skill.Value.Name;
                subMenu.GetComponent<RMF_RadialMenu>().elements.Add(rMF_RadialMenuElement);

                rMF_RadialMenuElement.button.onClick.AddListener(delegate { _battleManager.GetTargets(skill.Value); });
            }
        }

        return subMenu;
    }

	public void UpdateDisplay()
	{
		if (_battleManager.BattleDataModel.CurrentActor == null) { return; }
		if (holder == null) { return; } // TODO, tidy up, this is a messy way to prevent problems in testing
		if (ServiceLocator.Instance.ServiceLocatorObject.GameStateManager.State != ServiceLocator.Instance.ServiceLocatorObject.GameBattleState) { return; }

		holder.SetActive(_battleManager.BattleDataModel.CurrentActor.IsPlayer);
		
		this.character = _battleManager.BattleDataModel.CurrentActor;
		if (character.IsPlayer)
		{
			DestroyOldMenu(itemsElementsParent);
			DestroyOldMenu(skillsElementsParent);
			DestroyOldMenu(positionElementsParent);

			InitialiseItemSubMenu();
			InitialiseSkillsSubMenu();
			InitialisePositionSubMenu();

        }
	}


	private void DestroyOldMenu(Transform oldElementsParent)
	{
		RMF_RadialMenu[] oldItemSubMenus = oldElementsParent.GetComponentsInChildren<RMF_RadialMenu>(true);
		foreach (RMF_RadialMenu menu in oldItemSubMenus)
		{
			oldElementsParent.GetComponent<Button>().onClick.RemoveAllListeners();
			Destroy(menu.gameObject);
		}
	}

	public void RemoveMenu()
	{
        DestroyOldMenu(itemsElementsParent);
        DestroyOldMenu(skillsElementsParent);
		holder.SetActive(false);
    }

	private void OnDisable()
	{
		if (subMenu != null)
		{
            foreach (var element in subMenu.GetComponent<RMF_RadialMenu>().elements)
            {
                element.button.onClick.RemoveAllListeners();
            }
        }
	}
}
