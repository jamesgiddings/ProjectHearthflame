using GramophoneUtils.SavingLoading;
using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ServiceLocator : MonoBehaviour 
{
    
    [SerializeField] private DialogueUI _dialogueUI;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PlayerBehaviour _playerBehaviour;
    [SerializeField] private PlayerModel _playerModel;
    [SerializeField] private SavingSystem _savingSystem;
    [SerializeField] private UIServiceLocator _uiServiceLocator;

    public static ServiceLocator Instance { get; private set; }
    public UIServiceLocator UIServiceLocator => _uiServiceLocator;
    public DialogueUI DialogueUI => _dialogueUI;
    public EventSystem EventSystem => _eventSystem;
    public Camera MainCamera => _mainCamera;
    public PlayerBehaviour PlayerBehaviour => _playerBehaviour;
    public PlayerModel PlayerModel => _playerModel;
    public SavingSystem SavingSystem => _savingSystem;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}
