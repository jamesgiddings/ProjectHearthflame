using GramophoneUtils.Characters;
using GramophoneUtils.Items.Containers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GramophoneUtils.Stats
{
    public interface ICharacterModel
    {
        IAnimationService AnimationService { get; }
        List<ICharacter> AllCharacters { get; }
        List<ICharacter> DeadEnemyCharacters { get; }
        List<ICharacter> DeadEnemyCharactersList { get; }
        List<ICharacter> DeadPlayerCharacters { get; }
        CharacterOrder EnemyCharacterOrder { get; set; }
        List<ICharacter> EnemyCharacters { get; }
        Inventory EnemyInventory { get; }
        Inventory PartyInventory { get; }
        ICharacter[] PlayerCharacterBlueprints { get; }
        CharacterOrder PlayerCharacterOrder { get; set; }
        List<ICharacter> PlayerCharacters { get; }
        List<ICharacter> ReserveEnemyCharacters { get; }
        ICharacter Slot1EnemyCharacter { get; }
        ICharacter Slot1PlayerCharacter { get; }
        ICharacter Slot2EnemyCharacter { get; }
        ICharacter Slot2PlayerCharacter { get; }
        ICharacter Slot3EnemyCharacter { get; }
        ICharacter Slot3PlayerCharacter { get; }
        ICharacter Slot4EnemyCharacter { get; }
        ICharacter Slot4PlayerCharacter { get; }

        Task PerformAction();
        void AddEnemyCharacters(List<ICharacter> charactersToAdd);
        void AddEnemyToDeadEnemyCharactersList(ICharacter character);
        void AddPlayerToDeadPlayerCharactersList(ICharacter character);
        Task AwaitCharacterDeathSequence();
        object CaptureState();
        void ClearDeadEnemyCharactersList();
        void ClearDeadPlayerCharactersList();
        List<ICharacter> InstanceCharacters();
        void RegisterCharacterDeath(ICharacter character);
        void RemoveEnemyCharacter(ICharacter characterToRemove);
        void RemovePlayerCharacter(ICharacter characterToRemove);
        void ReplaceEnemyCharacters(List<ICharacter> charactersToReplaceWith);
        void ResetEnemyCharacterOrder();
        void ResetPlayerCharacterOrder();
        void RestoreState(object state);
        void UpdateEnemyCharacterOrder(CharacterOrder characterOrder);
        void UpdatePlayerCharacterOrder(CharacterOrder characterOrder);
    }
}