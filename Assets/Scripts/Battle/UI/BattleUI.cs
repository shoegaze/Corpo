using System;
using UnityEngine;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable EventNeverSubscribedTo.Global

namespace Battle.UI {
  [RequireComponent(typeof(Menu))]
  public class BattleUI : MonoBehaviour {
    [SerializeField] private BattleController battle;

    private Menu menu;
    
    // Menu
    public Actor.Actor ActiveActor { get; private set; }
    public int AbilityIndex { get; private set; }
    
    public event Action<Actor.Actor> OnActiveActorChanged;
    public event Action<int> OnAbilityIndexChanged;
    
    // Statusbar
    public Mode Mode { get; private set; }
    public ActorAlignment Team { get; private set; }
    public int Turn { get; private set; }
    
    public event Action<Mode> OnModeChanged;
    public event Action<ActorAlignment> OnTeamChanged;
    public event Action<int> OnTurnChanged;
    
    // Result
    public bool DoAlliesWin { get; private set; }
    public bool DoEnemiesWin { get; private set; }
    
    public event Action<bool> OnDoAlliesWinChanged;
    public event Action<bool> OnDoEnemiesWinChanged;

    protected void Awake() {
      menu = GetComponent<Menu>();
    }

    protected void Start() {
      ActiveActor = null;
      AbilityIndex = 0;
      
      OnActiveActorChanged?.Invoke(ActiveActor);
      OnAbilityIndexChanged?.Invoke(AbilityIndex);
    
      Mode = Mode.Grid;
      // HACK:
      Team = ActorAlignment.Ally;
      Turn = battle.Turn;
      
      OnModeChanged?.Invoke(Mode);
      OnTeamChanged?.Invoke(Team);
      OnTurnChanged?.Invoke(Turn);
      
      DoAlliesWin = false;
      DoEnemiesWin = false;
      
      OnDoAlliesWinChanged?.Invoke(DoAlliesWin);
      OnDoEnemiesWinChanged?.Invoke(DoEnemiesWin);
    }

    protected void Update() {
      var mode = menu.Mode;
      if (mode != Mode) {
        Mode = mode;
        OnModeChanged?.Invoke(Mode);
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
