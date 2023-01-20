using UnityEngine;
using System.Linq;
using GramophoneUtils.Utilities;
using GramophoneUtils.Events.CustomEvents;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Battle Manager", menuName = "Battles/Systems/Battle Manager")]
public class BattleManager : ScriptableObjectThatCanRunCoroutines
{
    #region Attributes/Fields/Properties

    [SerializeField] private GameObject _rearBattlePrefab;
    public GameObject RearBattlePrefab => _rearBattlePrefab;

    [SerializeField] private GameObject _turnOrderPrefab;
    public GameObject TurnOrderPrefab => _turnOrderPrefab;

    [SerializeField] private VoidEvent _onCharactersUpdated;
    public VoidEvent OnCharactersUpdated => _onCharactersUpdated;

    [SerializeField] private GameObject _radialMenuPrefab;

    private RadialMenu _radialMenu;

    [SerializeField] private GameObject _battleRewardsPrefab;

    private BattleRewardsDisplayUI _battleRewardsDisplayUI;

    private Battle _battle;
	
	private BattleDataModel _battleDataModel;

	private ITargetManager targetManager;

	private TurnOrderUI turnOrderUI;

    private RearBattleUI _rearBattleUI;
	public ITargetManager TargetManager => targetManager; // getter
	public BattleDataModel BattleDataModel => _battleDataModel; // getter

	public Battle Battle => _battle;

    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public void InitialiseBattleManager()
	{
        _battleDataModel = ServiceLocator.Instance.BattleDataModel;
        targetManager = ServiceLocator.Instance.TargetManager;
        ServiceLocator.Instance.CharacterModel.AddEnemyCharacters(_battle.BattleCharacters);
        _battleDataModel.InitialiseBattleModel();

        InitialiseTurnOrderUI();
		InitialiseRearBattleUI();
        InitialiseBattleRewardsDisplayUI();
        InitialiseRadialMenu();
        _battleDataModel.OnCurrentActorChanged?.Invoke();

        // Move to next state:
        //_battleDataModel.UpdateState(0.5f);
    }

    public void SetBattle(Battle battle)
    {
		this._battle = battle;
    }

    public void InitialiseRadialMenu()
    {
        _radialMenu = Instantiate(_radialMenuPrefab, ServiceLocator.Instance.BattleUITransform).GetComponent<RadialMenu>();
        //_radialMenu.InitialiseRadialMenu();
    }

    [Button]
    public void UpdateRadialMenu()
    {
        _radialMenu.UpdateDisplay();
    }

    [Button]
    public void DestroyRadialMenu()
    {
        _radialMenu.Destroy();
    }

    [Button]
    public void GetTargets(ISkill skill)
    {
        targetManager.GetCurrentlyTargeted(skill, BattleDataModel.CurrentActor);
        Debug.Log("GetCurrentlyTargeted(skill, BattleDataModel.CurrentActor).Count: " + targetManager.GetCurrentlyTargeted(skill, BattleDataModel.CurrentActor).Count);
    }

    public void ChangeTargets()
    {
        targetManager.ChangeTargeted(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
    }

    public void EndBattle()
    {
        Destroy(turnOrderUI.gameObject);
        Destroy(_battleRewardsDisplayUI.gameObject);
        DestroyRadialMenu();
        _rearBattlePrefab.SetActive(false);
        ServiceLocator.Instance.GameStateManager.ChangeState(ServiceLocator.Instance.ExplorationState);
    }

    public void AddBattleReward()
    {
        _battle.BattleReward.AddBattleReward(ServiceLocator.Instance.CharacterModel);
        _battleDataModel.OnBattleRewardsEarned?.Invoke(_battle.BattleReward);
    }

    #endregion


    #region Utilities


    private void InitialiseBattleRewardsDisplayUI()
    {
        _battleRewardsDisplayUI = Instantiate(_battleRewardsPrefab, ServiceLocator.Instance.BattleUITransform).GetComponent<BattleRewardsDisplayUI>();
        _battleRewardsDisplayUI.Initialise(this);
    }

    private void InitialiseTurnOrderUI()
    {
        //turnOrderUI = Instantiate(_turnOrderPrefab, ServiceLocator.Instance.BattleUITransform).GetComponent<TurnOrderUI>();
        turnOrderUI = Instantiate(_turnOrderPrefab, ServiceLocator.Instance.transform).GetComponent<TurnOrderUI>(); // TODO fix this, it only works when it isn't on a canvas
        turnOrderUI.Initialise();
    }

    private void InitialiseRearBattleUI()
    {
        _rearBattleUI = Instantiate(_rearBattlePrefab, ServiceLocator.Instance.BattleUITransform).GetComponent<RearBattleUI>();
        //_rearBattlePrefab.SetActive(true);
    }


    private void UninitialiseBattlers()
	{
		Battler[] battlers = ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary.Values.ToArray();

        foreach (Battler battler in battlers)
		{
			if (battler != null)
			{
                battler.Uninitialise();
            }
		}
	}

	#endregion

}
