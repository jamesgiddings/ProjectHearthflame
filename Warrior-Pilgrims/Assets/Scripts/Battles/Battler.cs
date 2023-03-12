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
using System.Threading;

public class Battler : MonoBehaviour
{
    #region Attributes/Fields/Properties

    [SerializeField] private SpriteRenderer targetCursor;
    [SerializeField] private SpriteRenderer currentActorHighlight;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private StatusEffectBarUI statusEffectBarUI;
    [SerializeField] private TextMeshPro floatingText;
    [SerializeField] private Transform modifierPanel;
    [SerializeField] private GameObject statModifierImagePrefab;
    [SerializeField] private Canvas battlerStatsCanvas;
    [SerializeField] private CharacterEvent _onCharacterDeath;
    [SerializeField] private Animator _animatorOverride;
    public Animator Animator => GetAnimator();

    private Character character;
    private SpriteRenderer spriteRenderer;
    private AnimationPlayer animationPlayer;
    private CharacterAnimation _characterAnimation;
    private CharacterMovement _characterMovement;

    private bool isInitialised = false;


    public Action OnTurnComplete;

    #endregion

    #region Callbacks

    private void OnEnable()
    {
        if (ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.ExplorationState)
        {
            GetComponent<CharacterMovement>().enabled = true;
        }
        if (ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.BattleState)
        {
            healthSlider.enabled = true;
            statModifierImagePrefab.SetActive(true);
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

    #region Public Functions

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

    public void ConnectFollowerToLeader(CharacterMovement followee, float battlerGap, int positionIndex, int sortingGroupIndex)
    {
        if (_characterMovement == null)
        {
            _characterMovement = GetComponent<CharacterMovement>();
        }
        
        SortingGroup sortingGroup = GetComponent<SortingGroup>();
        if (_characterMovement == null)
        {
            Debug.LogError("no FollowerMove component on battler.");
            return;
        }
        if (sortingGroup == null)
        {
            Debug.LogError("no SortingGroup component on battler.");
            return;
        }
        _characterMovement.SetIsFollower(true);
        _characterMovement.SetFollowee(followee);

        float stateDependentBattlerGap = ServiceLocatorObject.Instance.GameStateManager.State == ServiceLocatorObject.Instance.GameBattleState ? battlerGap + (battlerGap * positionIndex) : battlerGap * positionIndex;

        _characterMovement.SetGap(stateDependentBattlerGap);
        sortingGroup.sortingOrder = sortingGroupIndex;
    }

    [Button]
    public void PointFollowerToLeader(CharacterMovement followee = null)
    {
        if (_characterMovement == null)
        {
            _characterMovement = GetComponent<CharacterMovement>();
        }
        if (_characterAnimation == null)
        {
            _characterAnimation = GetComponent<CharacterAnimation>();
        }
        if (_characterMovement.IsFollower || followee != null)
        {
            CharacterMovement characterToPointTo = followee == null ? _characterMovement.Followee : followee;
            _characterAnimation.SetLastX(GetOrientation(this.gameObject.transform.position.x, characterToPointTo.gameObject.transform.position.x));
        }
    }

    public void Initialise(Character character)
    {
        this.character = character;
        this.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        this.spriteRenderer.sprite = character.Sprite;
        this.animationPlayer = new AnimationPlayer(this, character);

        ServiceLocator.Instance.BattleDataModel.OnCurrentActorChanged += UpdateCurrentActorHighlightState;
        ServiceLocator.Instance.TargetManager.OnCurrentTargetsChanged += UpdateTargetCursor;
        character.HealthSystem.OnHealthChanged += UpdateHealthSlider;
        character.HealthSystem.OnHealthChangedNotification += DisplayFloatingTexts;
        character.StatSystem.OnStatSystemNotification += DisplayFloatingTexts;
        character.HealthSystem.OnCharacterDeath += KillCharacter;
        character.SkillSystem.OnSkillUsed += animationPlayer.DisplayAnimation;

        statusEffectBarUI.Initialise(character);

        UpdateHealthSlider(0);

        DisplayBattleUI(ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.BattleState);

        isInitialised = true;
    }

    IStatusEffect statusEffect;
    IStatusEffect statusEffect1;

    public void DisplayBattleUI(bool value)
    {
        healthSlider.gameObject.SetActive(value);
        statusEffectBarUI.gameObject.SetActive(value);
        statModifierImagePrefab.SetActive(value);
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

        foreach (KeyValuePair<IStatType, IStat> keyValuePair in character.StatSystem.Stats)
        {
            foreach (StatModifier statModifier in keyValuePair.Value.StatModifiers)
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

    #region Private Functions

    private Animator GetAnimator()
    {
        return _animatorOverride == null ? GetComponent<Animator>() : _animatorOverride;
    }

    /// <summary>
    /// Find out which way sprite should face baced on difference in position between them.
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="x2"></param>
    /// <returns></returns>
    private int GetOrientation(float x1, float x2)
    {
        if (x2 - x1 > 0)
        {
            return 1;
        }
        else if (x2 - x1 < 0)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

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