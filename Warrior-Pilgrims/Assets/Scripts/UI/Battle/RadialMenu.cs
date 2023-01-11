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
	[SerializeField] private GameObject holder;

	[SerializeField] private GameObject subMenuPrefab;
	[SerializeField] private GameObject elementPrefab;

	[SerializeField] private Transform itemsElementsParent;
	[SerializeField] private Transform skillsElementsParent;
	[SerializeField] private Transform positionElementsParent;

	RMF_RadialMenu subMenu;

	private BattleManager battleManager;
	private Character character;

	private List<Item> items = new List<Item>();
	private List<ISkill> skills = new List<ISkill>();

    #region API

    public Character Character
	{
		get { return character; }
		set { character = value; UpdateDisplay(); Debug.Log("Setting character" ); }
	}

	public void InitialiseRadialMenu(BattleManager battleManager)
	{
		this.battleManager = battleManager;
		this.character = battleManager.BattleDataModel.CurrentActor;

		InitialiseItemSubMenu();
		InitialiseSkillsSubMenu();
		UpdateDisplay();
		//InitialisePositionSubMenu();

		battleManager.BattleDataModel.OnCurrentActorChanged += UpdateDisplay;
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

		button.onClick.AddListener(delegate { subMenu.gameObject.SetActive(!subMenu.gameObject.activeInHierarchy); });

		foreach (ItemSlot itemSlot in character.PartyInventory.ItemSlots) // add the items from the partyInventory
		{
			//Debug.LogWarning("This should actually add from that characters quickbar, not the partyInventory"); // TODO
			if (itemSlot.item != null)
			{
				items.Add(itemSlot.item);
				RMF_RadialMenuElement rMF_RadialMenuElement = UnityEngine.Object.Instantiate(elementPrefab, parent).GetComponent<RMF_RadialMenuElement>();
				rMF_RadialMenuElement.text.text = itemSlot.item.Name;
				subMenu.GetComponent<RMF_RadialMenu>().elements.Add(rMF_RadialMenuElement);
			}
		}

		return subMenu;
	}

	private RMF_RadialMenu InitialiseSkillsSubMenu()
	{
		RMF_RadialMenu subMenu = UnityEngine.Object.Instantiate(subMenuPrefab, skillsElementsParent).GetComponent<RMF_RadialMenu>();

		Transform parent = subMenu.gameObject.transform.GetChild(0); // get the parent transform for the elements

		Button button = skillsElementsParent.GetComponent<Button>(); // get the buttons

		button.onClick.AddListener(delegate { subMenu.gameObject.SetActive(!subMenu.gameObject.activeInHierarchy); if (!subMenu.gameObject.activeInHierarchy) { battleManager.TargetManager.ClearTargets(); } });

		foreach (ISkill skill in character.SkillSystem.UnlockedSkills) // add the unlocked skills from the SkillSystem
		{
			//Debug.LogWarning("This should actually add from that characters quickbar, not the partyInventory"); //todo
			skills.Add(skill);
			RMF_RadialMenuElement rMF_RadialMenuElement = UnityEngine.Object.Instantiate(elementPrefab, parent).GetComponent<RMF_RadialMenuElement>();
			rMF_RadialMenuElement.text.text = skill.Name;
			subMenu.GetComponent<RMF_RadialMenu>().elements.Add(rMF_RadialMenuElement);

			rMF_RadialMenuElement.button.onClick.AddListener(delegate { battleManager.GetTargets(skill); });

		}

		return subMenu;
	}

	private void InitialisePositionSubMenu()
	{
		throw new NotImplementedException();
	}

	public void UpdateDisplay()
	{
		holder.SetActive(battleManager.BattleDataModel.CurrentActor.IsPlayer);
				
		if (battleManager.BattleDataModel.CurrentActor != this.character)
		{
			this.character = battleManager.BattleDataModel.CurrentActor;
			if (character.IsPlayer)
			{
				DestroyOldMenu(itemsElementsParent);
				DestroyOldMenu(skillsElementsParent);

				InitialiseItemSubMenu();
				InitialiseSkillsSubMenu();  
			}
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
