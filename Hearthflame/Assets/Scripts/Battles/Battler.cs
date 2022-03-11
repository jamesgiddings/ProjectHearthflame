using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

public class Battler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer targetCursor;
    [SerializeField] private SpriteRenderer currentActorHighlight;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI floatingTextNotification;
    [SerializeField] private Transform modifierPanel;
    [SerializeField] private GameObject statModifierImagePrefab;
    [SerializeField] private Canvas battlerStatsCanvas;

    [SerializeField] private float healthSliderYOffset;

    private Character character;
    private BattleManager battleManager;
    private SpriteRenderer spriteRenderer;
    private BattlerDisplayUI battlerDisplayUI;

    private Camera battleCamera;

    private bool isBeingTargeted;

    public bool IsBeingTargeted { get { return isBeingTargeted; } set { isBeingTargeted = value; } }

    public Action OnTurnComplete;

    public void Initialise(BattleManager battleManager, Character character, Camera battleCamera)
    {
        this.character = character;
        this.battleManager = battleManager;
        this.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        this.spriteRenderer.sprite = character.CharacterTemplate.Icon;
        this.battleCamera = battleCamera;
        this.battlerDisplayUI = battleManager.BattleBehaviour.BattlerDisplayUI;
        battleManager.BattleDataModel.OnCurrentActorChanged += UpdateCurrentActorHighlightState;
        battleManager.TargetManager.OnCurrentTargetsChanged += UpdateTargetCursor;
        character.HealthSystem.OnHealthChanged += UpdateHealthSlider;
        character.HealthSystem.OnHealthChanged += DisplayFloatingTexts;
        character.HealthSystem.OnCharacterDeath += KillCharacter;
        character.SkillSystem.OnSkillUsed += DisplayAnimation;

        UpdateHealthSlider(0);

        healthSlider.gameObject.GetComponent<RectTransform>().anchoredPosition = battlerStatsCanvas.WorldToCanvas(gameObject.transform.position + new Vector3(0f, healthSliderYOffset), battleCamera);
    }

    private void DisplayAnimation(Skill skill, List<Character> targets)
    {
        switch (skill.SkillAnimType)
        {
            case (SkillAnimType.MeleePhysical):
                foreach (Character target in targets)
                {
                    Vector3 startPos = gameObject.transform.position;
                    Sequence sequence = DOTween.Sequence();

                    //Vector3 targetPos = battlerDisplayUI.CharacterBattlerDictionary[target].transform.position + (gameObject.transform.position / 5);

                    sequence.Append(gameObject.transform.DOMove(battlerDisplayUI.CharacterBattlerDictionary[target].transform.position, 1.5f));
                    sequence.Append(gameObject.transform.DOShakeRotation(0.5f, 20f).OnComplete(() => skill.DoNextBit(targets, character)));
                    sequence.Append(gameObject.transform.DOMove(startPos, 0.4f)).WaitForCompletion();
                    sequence.Play().OnComplete(() => OnTurnComplete?.Invoke());
                }
                break;
            case (SkillAnimType.MagicalProjectile):
                gameObject.transform.DOShakeRotation(0.5f, 20f).WaitForCompletion();

                Sequence projectileSequence = DOTween.Sequence();

                List<GameObject> projectiles = new List<GameObject>();

                foreach (Character target in targets)
                {
                    GameObject projectile = Instantiate(skill.ProjectilePrefab, gameObject.transform);
                    projectile.transform.localPosition = new Vector3(0, 0, 0);
                    projectile.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    projectiles.Add(projectile);
                    projectileSequence.Join(projectile.transform.DOMove(battlerDisplayUI.CharacterBattlerDictionary[target].transform.position, 0.4f));
                    projectileSequence.Join(projectile.transform.DOScale(2f, 0.4f));

                }
                projectileSequence.Play().OnComplete(() =>
                {
                    DestroyProjectiles(projectiles);
                    skill.DoNextBit(targets, character);
                    OnTurnComplete?.Invoke();
                });

                break;
            case (SkillAnimType.BuffOrDebuff):
                foreach (Character target in targets)
                {
                    Sequence sequence = DOTween.Sequence();

                    sequence.Append(gameObject.transform.DOShakeRotation(0.5f, 20f).OnComplete(() => skill.DoNextBit(targets, character))).WaitForCompletion();
                    sequence.Play().OnComplete(() => OnTurnComplete?.Invoke());
                }
                break;
        }
    }

    private void DestroyProjectiles(List<GameObject> projectiles)
	{
		foreach (GameObject projectile in projectiles)
		{
            Destroy(projectile);
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

    public void DisplayFloatingTexts(int value)
    {
        StartCoroutine(DisplayFloatingTextsCoroutine(value));
    }

    public IEnumerator DisplayFloatingTextsCoroutine(int value)
    {
        while (character.GetIsAnyNotificationInQueue())
        {
            floatingTextNotification.gameObject.SetActive(true);
            DisplayFloatingText(character.DequeueBattlerNoticiation());
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(1f);
        floatingTextNotification.gameObject.SetActive(false);
    }

    public void DisplayFloatingText(int value)
    {
        Color textColor = (value >= 0) ? Color.green : Color.red;
        floatingTextNotification.color = textColor;
        floatingTextNotification.text = value.ToString();
    }

    private void KillCharacter()
    {
        spriteRenderer.color = new Color32(255, 255, 225, 100); // greys out the characters image
        healthSlider.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        battleManager.BattleDataModel.OnCurrentActorChanged -= UpdateCurrentActorHighlightState;
        battleManager.TargetManager.OnCurrentTargetsChanged -= UpdateTargetCursor;
        character.HealthSystem.OnHealthChanged -= UpdateHealthSlider;
        character.HealthSystem.OnHealthChanged -= DisplayFloatingTexts;
        character.HealthSystem.OnCharacterDeath -= KillCharacter;
    }
}