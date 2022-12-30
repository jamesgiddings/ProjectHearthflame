using GramophoneUtils.Stats;
using GramophoneUtils.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;
using GramophoneUtils.Events.CustomEvents;

public class Battler : MonoBehaviour
{
    [SerializeField] private Vector2 healthSliderPosOffset;
    [SerializeField] private SpriteRenderer targetCursor;
    [SerializeField] private SpriteRenderer currentActorHighlight;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshPro floatingText;
    [SerializeField] private Transform modifierPanel;
    [SerializeField] private GameObject statModifierImagePrefab;
    [SerializeField] private Canvas battlerStatsCanvas;
    [SerializeField] private CharacterEvent _onCharacterDeath;

    [SerializeField] private float healthSliderYOffset;

    private Character character;
    private BattleManager battleManager;
    private SpriteRenderer spriteRenderer;
    private AnimationPlayer animationPlayer;

    private bool isInitialised = false;

    public Action OnTurnComplete;

    #region Callbacks

    private void OnEnable()
    {
        if (ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.ExplorationState)
        {
            GetComponent<CharacterMovement>().enabled = true;
        }
    }

    private void OnDisable()
    {
        if (isInitialised) // TODO this is a hack
        {
            ServiceLocator.Instance.BattleDataModel.OnCurrentActorChanged -= UpdateCurrentActorHighlightState;
            ServiceLocator.Instance.TargetManager.OnCurrentTargetsChanged -= UpdateTargetCursor;
            character.HealthSystem.OnHealthChanged -= UpdateHealthSlider;
            character.HealthSystem.OnHealthChangedNotification -= DisplayFloatingTexts;
            character.StatSystem.OnStatSystemNotification -= DisplayFloatingTexts;
            character.HealthSystem.OnCharacterDeath -= KillCharacter;
            character.SkillSystem.OnSkillUsed -= animationPlayer.DisplayAnimation;
            isInitialised = false;
        }
    }

    #endregion

    #region API

    [Button]
    public void TestIncrementHealth(int increment)
    {
        Debug.Log(increment);
        Debug.Log(character.Name);
        character.HealthSystem.IncrementCurrentHealth(increment);
    }

    public void SetCharacterMovementIsLeader(int sortingGroupIndex)
    {
        CharacterMovement characterMovement = GetComponent<CharacterMovement>();
        SortingGroup sortingGroup = GetComponent<SortingGroup>();
        if (characterMovement == null)
        {
            Debug.LogError("no FollowerMove component on battler.");
            return;
        }
        if (sortingGroup == null)
        {
            Debug.LogError("no SortingGroup component on battler.");
            return;
        }
        characterMovement.SetIsFollower(false);
        sortingGroup.sortingOrder = sortingGroupIndex;
    }

    public void ConnectFollowerToLeader(CharacterMovement followee, float gap, int sortingGroupIndex)
    {
        CharacterMovement characterMovement = GetComponent<CharacterMovement>();
        SortingGroup sortingGroup = GetComponent<SortingGroup>();
        if (characterMovement == null)
        {
            Debug.LogError("no FollowerMove component on battler.");
            return;
        }
        if (sortingGroup == null)
        {
            Debug.LogError("no SortingGroup component on battler.");
            return;
        }
        characterMovement.SetIsFollower(true);
        characterMovement.SetFollowee(followee);
        characterMovement.SetGap(gap);
        sortingGroup.sortingOrder = sortingGroupIndex;
    }

    public void Initialise(BattleManager battleManager, Character character)
    {
        this.character = character;
        this.battleManager = battleManager;
        this.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        this.spriteRenderer.sprite = character.Sprite;
        this.animationPlayer = new AnimationPlayer(this, character, battleManager);

        ServiceLocator.Instance.BattleDataModel.OnCurrentActorChanged += UpdateCurrentActorHighlightState;
        ServiceLocator.Instance.TargetManager.OnCurrentTargetsChanged += UpdateTargetCursor;
        character.HealthSystem.OnHealthChanged += UpdateHealthSlider;
        character.HealthSystem.OnHealthChangedNotification += DisplayFloatingTexts;
        character.StatSystem.OnStatSystemNotification += DisplayFloatingTexts;
        character.HealthSystem.OnCharacterDeath += KillCharacter;
        character.SkillSystem.OnSkillUsed += animationPlayer.DisplayAnimation;

        UpdateHealthSlider(0);
        isInitialised = true;
    }

    public void SetHealthSliderPosition()
    {
        RectTransform canvasRect = healthSlider.gameObject.GetComponent<RectTransform>(); //.anchoredPosition = battlerStatsCanvas.WorldToCanvas(gameObject.transform.position + new Vector3(-2.2f, healthSliderYOffset)); // TODO FIX
        Vector2 uiOffset = new Vector2((float)canvasRect.sizeDelta.x / 2f, (float)canvasRect.sizeDelta.y / 2f);

        Vector2 pos = gameObject.transform.position;  // get the game object position
        pos += healthSliderPosOffset; // add the offset from the inspector
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(pos);  //convert game object position to VievportPoint

        // set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)
        canvasRect.anchorMin = viewportPoint;
        canvasRect.anchorMax = viewportPoint;
        gameObject.SetActive(true);
    }

    public void DisplayBattleUI(bool value)
    {
        healthSlider.gameObject.SetActive(value);
    }

    public void UpdateCurrentActorHighlightState()
    {
        currentActorHighlight.gameObject.SetActive(character.IsCurrentActor);
    }

    public void UpdateHealthSlider(int value)
    {
        if (character.HealthSystem.MaxHealth == 0)
        {
            healthSlider.value = 0;
        }
        else
        {
            healthSlider.value = (float)character.HealthSystem.CurrentHealth / (float)character.HealthSystem.MaxHealth;
        }
    }

    public void UpdateModifierPanel()
    {
        for (int i = 0; i < modifierPanel.childCount; i++)
        {
            Destroy(modifierPanel.GetChild(i));
        }

        foreach (KeyValuePair<IStatType, Stat> keyValuePair in character.StatSystem.Stats)
        {
            foreach (StatModifier statModifier in keyValuePair.Value.Modifiers)
            {
                UnityEngine.Object.Instantiate(statModifierImagePrefab, modifierPanel);
            }
        }
    }

    public void DisplayFloatingTexts(BattlerNotificationImpl notification)
    {
        StartCoroutine(DisplayFloatingTextsCoroutine(notification));
    }

    public IEnumerator DisplayFloatingTextsCoroutine(BattlerNotificationImpl notification)
    {
        while (character.GetIsAnyNotificationInQueue())
        {
            floatingText.gameObject.SetActive(true);
            DisplayFloatingText(character.DequeueBattlerNoticiation());
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(1f);
        floatingText.gameObject.SetActive(false);
    }

    public void DisplayFloatingText(BattlerNotificationImpl notification)
    {
        Color textColor = notification.GetColor();
        floatingText.color = textColor;
        floatingText.text = notification.GetMessage();
    }

    public void Uninitialise()
    {
        if (!isInitialised) { return; }
        ServiceLocator.Instance.BattleDataModel.OnCurrentActorChanged -= UpdateCurrentActorHighlightState;
        ServiceLocator.Instance.TargetManager.OnCurrentTargetsChanged -= UpdateTargetCursor;
        character.HealthSystem.OnHealthChanged -= UpdateHealthSlider;
        character.HealthSystem.OnHealthChangedNotification -= DisplayFloatingTexts;
        character.StatSystem.OnStatSystemNotification -= DisplayFloatingTexts;
        character.HealthSystem.OnCharacterDeath -= KillCharacter;
        character.SkillSystem.OnSkillUsed -= animationPlayer.DisplayAnimation;
        isInitialised = false;
    }

    #endregion

    #region Utilities

    private void UpdateTargetCursor(List<Character> targets)
    {
        if (targets.Contains(character))
        {
            targetCursor.gameObject.SetActive(true);
        }
        else
        {
            targetCursor.gameObject.SetActive(false);
        }
    }

    private void KillCharacter()
    {
        spriteRenderer.color = new Color32(255, 255, 225, 100); // greys out the characters image
        healthSlider.gameObject.SetActive(false);
        _onCharacterDeath?.Raise(character);
    }

    #endregion
}