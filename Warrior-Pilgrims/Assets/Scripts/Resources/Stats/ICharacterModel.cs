using GramophoneUtils.Characters;
using GramophoneUtils.Items.Containers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GramophoneUtils.Stats
{
    public interface ICharacterModel
    {
        List<Character> AllCharacters { get; }
        List<Character> DeadEnemyCharacters { get; }
        List<Character> DeadEnemyCharactersList { get; }
        List<Character> DeadPlayerCharacters { get; }
        CharacterOrder EnemyCharacterOrder { get; set; }
        List<Character> EnemyCharacters { get; }
        Inventory EnemyInventory { get; }
        Inventory PartyInventory { get; }
        Character[] PlayerCharacterBlueprints { get; }
        CharacterOrder PlayerCharacterOrder { get; set; }
        List<Character> PlayerCharacters { get; }
        List<Character> ReserveEnemyCharacters { get; }
        Character Slot1EnemyCharacter { get; }
        Character Slot1PlayerCharacter { get; }
        Character Slot2EnemyCharacter { get; }
        Character Slot2PlayerCharacter { get; }
        Character Slot3EnemyCharacter { get; }
        Character Slot3PlayerCharacter { get; }
        Character Slot4EnemyCharacter { get; }
        Character Slot4PlayerCharacter { get; }

        void AddEnemyCharacters(List<Character> charactersToAdd);
        void AddEnemyToDeadEnemyCharactersList(Character character);
        void AddPlayerToDeadPlayerCharactersList(Character character);
        Task AwaitCharacterDeathSequence();
        object CaptureState();
        void ClearDeadEnemyCharactersList();
        void ClearDeadPlayerCharactersList();
        List<Character> InstanceCharacters();
        void RegisterCharacterDeath(Character character);
        void RemoveEnemyCharacter(Character characterToRemove);
        void RemovePlayerCharacter(Character characterToRemove);
        void ReplaceEnemyCharacters(List<Character> charactersToReplaceWith);
        void ResetEnemyCharacterOrder();
        void ResetPlayerCharacterOrder();
        void RestoreState(object state);
        void UpdateEnemyCharacterOrder(CharacterOrder characterOrder);
        void UpdatePlayerCharacterOrder(CharacterOrder characterOrder);
    }
}