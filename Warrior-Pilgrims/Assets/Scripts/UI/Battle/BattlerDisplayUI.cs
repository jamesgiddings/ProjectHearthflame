using GramophoneUtils.Stats;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattlerDisplayUI : MonoBehaviour
{
	[SerializeField] private Transform battlerPrefab;
	
	private ICharacterModel _characterModel;

	private Battler[] playerBattlers;
	private Battler[] enemyBattlers;

	private Battler[] battlerGameObjects;

	private Dictionary<ICharacter, Battler> characterBattlerDictionary;

    public Battler[] BattlerGameObjects => battlerGameObjects;

    public Dictionary<ICharacter, Battler> CharacterBattlerDictionary => characterBattlerDictionary;

	#region Initialising
	public void Initialise(BattleManager battleManager)
	{
		_characterModel = ServiceLocator.Instance.CharacterModel;
		
		battlerGameObjects = new Battler[_characterModel.AllCharacters.Count];
		characterBattlerDictionary = new Dictionary<ICharacter, Battler>();

        playerBattlers = new Battler[_characterModel.PlayerCharacters.Count];
        enemyBattlers = new Battler[_characterModel.EnemyCharacters.Count];
		playerBattlers = InitialisePlayerBattlers(_characterModel.PlayerCharacters);

        battlerGameObjects = playerBattlers.Concat(enemyBattlers).ToArray();
	}

    private Battler[] InitialisePlayerBattlers(List<ICharacter> characters)
    {
        int newCharacterStartIndex = playerBattlers.Length;
        Battler[] newPlayerBattlers = new Battler[_characterModel.PlayerCharacters.Count];
        for (int i = newCharacterStartIndex; i < playerBattlers.Length; i++)
        {
            int j = 0;
            ICharacter character = characters[j];
            j++;
            if (character != null)
            {
                if (character.IsUnlocked && !character.IsRear)
                {
                    newPlayerBattlers[i] = ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary[character];
                    newPlayerBattlers[i].Initialise(character);
                    characterBattlerDictionary.Add(character, newPlayerBattlers[i]);
                }
            }
        }
		return newPlayerBattlers;
    }

	public void OnEnterBattle()
	{

	}

	public void RefreshBattlersOnOrderChange()
	{
		if (!(ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.BattleState))
		{
			return;
		}
		Debug.LogError("For some reason this isn't called when we add a character from the rear");
        battlerGameObjects = new Battler[_characterModel.AllCharacters.Count];

		for (int i = characterBattlerDictionary.Keys.Count - 1; i >= 0; i--)
		{
			ICharacter character = characterBattlerDictionary.ElementAt(i).Key;
            if (character.IsPlayer && !_characterModel.PlayerCharacters.Contains(character))
			{
				characterBattlerDictionary[character].Uninitialise();
                characterBattlerDictionary.Remove(character);
            }
        }

        IEnumerable<ICharacter> uninitialisedCharacters = from uninitialised in _characterModel.PlayerCharacters.Except(characterBattlerDictionary.Keys) select uninitialised;
        
        playerBattlers = new Battler[_characterModel.PlayerCharacters.Count];
        playerBattlers.Concat(InitialisePlayerBattlers(_characterModel.PlayerCharacters));
		Debug.Log("characterBattlerDictionary.Count: " + characterBattlerDictionary.Count);
        battlerGameObjects = playerBattlers.Concat(enemyBattlers).ToArray();
    }

    #endregion

    #region EndOfLife

    public void OnExitBattle()
    {
		for (int i = 0; i < battlerGameObjects.Length; i++)
		{
			if (battlerGameObjects[i] != null)
			{
                battlerGameObjects[i].DisplayBattleUI(false);
            }
		}

        for (int i = 0; i < enemyBattlers.Length; i++)
        {
            Destroy(enemyBattlers[i].gameObject);
        }
    }
    #endregion
}
