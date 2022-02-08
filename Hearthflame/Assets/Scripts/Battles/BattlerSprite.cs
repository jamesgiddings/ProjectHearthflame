using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlerSprite : MonoBehaviour
{
    [SerializeField] private Image spriteImage;
    [SerializeField] private Image targetCursor;
    [SerializeField] private Image currentActorHighlight;
    
    private Character character;
    private BattleManager battleManager;

    private bool isBeingTargeted;

    public bool IsBeingTargeted { get { return isBeingTargeted; } set { isBeingTargeted = value; } }

    public void Initialise(BattleManager battleManager, Character character)
	{
        this.character = character;
        this.battleManager = battleManager;
        Debug.Log("battleManager == null: " + (battleManager == null));
        this.spriteImage.sprite = character.PartyCharacterTemplate.Icon;
        battleManager.OnCurrentActorChanged += UpdateDisplay;
	}

    public void UpdateDisplay()
	{
        currentActorHighlight.gameObject.SetActive(character.IsCurrentActor);
    }

	private void OnDestroy()
	{
        battleManager.OnCurrentActorChanged -= UpdateDisplay;
    }
}
