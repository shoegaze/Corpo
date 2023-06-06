using System;
using UnityEngine;
using Zenject;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable EventNeverSubscribedTo.Global

namespace Battle.UI {
  [RequireComponent(typeof(StateManager), 
                    typeof(Menu))]
  public class BattleUI : MonoBehaviour {
    [Inject] private BattleController battle;
    // [Inject] private StateManager stateManager;

    private Menu menu;
    
    public StateManager StateManager { get; private set; }

    // Menu
    public Actor.Actor ActiveActor { get; private set; }
    public int AbilityIndex { get; private set; }
    
    public event Action<Actor.Actor> OnActiveActorChanged;
    public event Action<int> OnAbilityIndexChanged;
    
    // Statusbar
    public PanelState PanelState { get; private set; }
    public FocusState FocusState { get; private set; }
    public ActorAlignment Team { get; private set; }
    public int Turn { get; private set; }
    
    public event Action<PanelState> OnPanelStateChanged;
    public event Action<FocusState> OnFocusStateChanged;
    public event Action<ActorAlignment> OnTeamChanged;
    public event Action<int> OnTurnChanged;
    
    // Result
    public bool DoAlliesWin { get; private set; }
    public bool DoEnemiesWin { get; private set; }
    
    public event Action<bool> OnDoAlliesWinChanged;
    public event Action<bool> OnDoEnemiesWinChanged;

    protected void Awake() {
      StateManager = GetComponent<StateManager>();
      menu = GetComponent<Menu>();
    }

    protected void Start() {
      ActiveActor = null;
      AbilityIndex = 0;
      
      OnActiveActorChanged?.Invoke(ActiveActor);
      OnAbilityIndexChanged?.Invoke(AbilityIndex);
    
      PanelState = PanelState.Grid;
      FocusState = FocusState.Free;
      // HACK: Should probably add ActorAlignment.None
      Team = ActorAlignment.Ally;
      Turn = battle.Turn;
      
      OnPanelStateChanged?.Invoke(PanelState);
      OnFocusStateChanged?.Invoke(FocusState);
      OnTeamChanged?.Invoke(Team);
      OnTurnChanged?.Invoke(Turn);
      
      DoAlliesWin = false;
      DoEnemiesWin = false;
      
      OnDoAlliesWinChanged?.Invoke(DoAlliesWin);
      OnDoEnemiesWinChanged?.Invoke(DoEnemiesWin);
    }

    protected void Update() {
      UpdateStates();
    }

    private void UpdateStates() {
      var panelState = StateManager.PanelState;
      if (panelState != PanelState) {
        PanelState = panelState;
        OnPanelStateChanged?.Invoke(PanelState);
      }

      var focusState = StateManager.FocusState;
      if (focusState != FocusState) {
        FocusState = focusState;
        OnFocusStateChanged?.Invoke(FocusState);
      }

      var activeActor = battle.ActiveActor;
      if (activeActor != ActiveActor) {
        ActiveActor = activeActor;
        OnActiveActorChanged?.Invoke(ActiveActor);

        Team = activeActor.Alignment;
        OnTeamChanged?.Invoke(Team);
      }

      int abilityIndex = menu.AbilityIndex;
      if (abilityIndex != AbilityIndex) {
        AbilityIndex = abilityIndex;
        OnAbilityIndexChanged?.Invoke(AbilityIndex);
      }

      int turn = battle.Turn;
      if (turn != Turn) {
        Turn = turn;
        OnTurnChanged?.Invoke(Turn);
      }

      bool doAlliesWin = battle.DoAlliesWin;
      if (doAlliesWin != DoAlliesWin) {
        DoAlliesWin = doAlliesWin;
        OnDoAlliesWinChanged?.Invoke(DoAlliesWin);
      }
      
      bool doEnemiesWin = battle.DoEnemiesWin;
      if (doEnemiesWin != DoEnemiesWin) {
        DoEnemiesWin = doEnemiesWin;
        OnDoEnemiesWinChanged?.Invoke(DoEnemiesWin);
      }     
    }
  }
}
