using TMPro;
using UnityEngine;
using GramophoneUtils.Stats;
using GramophoneUtils.Characters;

public class StatDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statLabel;
    [SerializeField] private TextMeshProUGUI statValue;
    [SerializeField] private StatType statType;
    private Character character;

	private void OnEnable()
	{
		UpdateStatDisplay();
	}

	public void Initialise(Character character)
	{
		this.character = character;
	}

	public void UpdateStatDisplay()
	{
		statLabel.text = statType.Name;
		statValue.text = character.StatSystem.GetStatValue(statType).ToString();
	}
}
