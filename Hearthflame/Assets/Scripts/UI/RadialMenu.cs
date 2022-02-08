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

	private BattleManager battleManager;
	private Character character;

	private List<Item> items = new List<Item>();
	private List<Skill> skills = new List<Skill>();

    public Character Character
	{
		get { return character; }
		set { character = value; UpdateDisplay(); Debug.Log("Setting character" ); }
	}

	public void InitialiseRadialMenu(BattleManager battleManager)
	{
		this.battleManager = battleManager;
		this.character = battleManager.CurrentActor;

		InitialiseItemSubMenu();
		InitialiseSkillsSubMenu();
		UpdateDisplay();
		//InitialisePositionSubMenu();

		battleManager.OnCurrentActorChanged += UpdateDisplay;
	}

	private RMF_RadialMenu InitialiseItemSubMenu()
	{
		RMF_RadialMenu subMenu = UnityEngine.Object.Instantiate(subMenuPrefab, itemsElementsParent).GetComponent<RMF_RadialMenu>();

		Transform parent = subMenu.gameObject.transform.GetChild(0); // get the parent transform for the elements

		Button button = itemsElementsParent.GetComponent<Button>(); // get the button

		Debug.Log("button is null: " + (button == null));

		button.onClick.AddListener(delegate { subMenu.gameObject.SetActive(!subMenu.gameObject.activeInHierarchy); });

		foreach (ItemSlot itemSlot in character.PartyInventory.ItemSlots) // add the items from the partyInventory
		{
			Debug.LogWarning("This should actually add from that characters quickbar, not the partyInventory");
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

		Button button = skillsElementsParent.GetComponent<Button>(); // get the button

		Debug.Log("button == null: " + (button == null));

		button.onClick.AddListener(delegate { subMenu.gameObject.SetActive(!subMenu.gameObject.activeInHierarchy); });

		foreach (Skill skill in character.SkillSystem.UnlockedSkills) // add the unlocked skills from the SkillSystem
		{
			Debug.LogWarning("This should actually add from that characters quickbar, not the partyInventory");
			skills.Add(skill);
			RMF_RadialMenuElement rMF_RadialMenuElement = UnityEngine.Object.Instantiate(elementPrefab, parent).GetComponent<RMF_RadialMenuElement>();
			rMF_RadialMenuElement.text.text = skill.Name;
			subMenu.GetComponent<RMF_RadialMenu>().elements.Add(rMF_RadialMenuElement);

			rMF_RadialMenuElement.button.onClick.AddListener(delegate { battleManager.GetTargets(); });

		}

		return subMenu;
	}

	private void InitialisePositionSubMenu()
	{
		throw new NotImplementedException();
	}

	public void UpdateDisplay()
	{
		holder.SetActive(battleManager.CurrentActor.IsPlayer);
				
		if (battleManager.CurrentActor != this.character)
		{
			this.character = battleManager.CurrentActor;
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
}
