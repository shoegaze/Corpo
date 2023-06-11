using System;
using Actor;
using Battle.Model;
using UnityEngine;
using Zenject;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable EventNeverSubscribedTo.Global

namespace Battle.View {
  [RequireComponent(typeof(StateManager), 
                    typeof(MenuModel))]
  public class BattleView : MonoBehaviour {
    [Inject] private BattleManager battle;
    // TODO: Get rid of this injection via MenuModel.StateManager:
    // [Inject] private StateManager stateManager;

    private MenuModel menuModel;
    
    public StateManager StateManager { get; private set; }

    // MenuModel
    public Actor.Actor ActiveActor { get; private set; }
    public int AbilityIndex { get; private set; }
    
    public event Action<Actor.Actor> OnActiveActorChanged;
    public event Action<int> OnAbilityIndexChanged;
    
    // Statusbar
    public FocusState FocusState { get; private set; }
    public SelectState SelectState { get; private set; }
    public ActorAlignment Team { get; private set; }
    public int Turn { get; private set; }
    
    public event Action<FocusState> OnPanelStateChanged;
    public event Action<SelectState> OnFocusStateChanged;
    public event Action<ActorAlignment> OnTeamChanged;
    public event Action<int> OnTurnChanged;
    
    // Result
    public bool DoAlliesWin { get; private set; }
    public bool DoEnemiesWin { get; private set; }
    
    public event Action<bool> OnDoAlliesWinChanged;
    public event Action<bool> OnDoEnemiesWinChanged;

    protected void Awake() {
      StateManager = GetComponent<StateManager>();
      menuModel = GetComponent<MenuModel>();
    }

    protected void Start() {
      ActiveActor = null;
      AbilityIndex = 0;
      
      OnActiveActorChanged?.Invoke(ActiveActor);
      OnAbilityIndexChanged?.Invoke(AbilityIndex);
    
      FocusState = FocusState.Grid;
      SelectState = SelectState.Free;
      // HACK: Should probably add ActorAlignment.None
      Team = ActorAlignment.Ally;
      Turn = battle.Turn;
      
      OnPanelStateChanged?.Invoke(FocusState);
      OnFocusStateChanged?.Invoke(SelectState);
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
      var panelState = StateManager.FocusState;
      if (panelState != FocusState) {
        FocusState = panelState;
        OnPanelStateChanged?.Invoke(FocusState);
      }

      var focusState = StateManager.SelectState;
      if (focusState != SelectState) {
        SelectState = focusState;
        OnFocusStateChanged?.Invoke(SelectState);
      }

      var activeActor = battle.ActiveActor;
      if (activeActor != ActiveActor) {
        ActiveActor = activeActor;
        OnActiveActorChanged?.Invoke(ActiveActor);

        Team = activeActor.Alignment;
        OnTeamChanged?.Invoke(Team);
      }

      int abilityIndex = menuModel.AbilityIndex;
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
