using System;
using UnityEngine;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable EventNeverSubscribedTo.Global

namespace Battle.UI {
  public class BattleUI : MonoBehaviour {
    [SerializeField] private BattleController battle;
    
    // Menu
    public Actor.Actor ActiveActor { get; private set; }

    public event Action<Actor.Actor> OnActiveActorChanged;
    
    // Statusbar
    public BattleUIMode Mode { get; private set; }
    public ActorAlignment Team { get; private set; }
    public int Turn { get; private set; }
    
    public event Action<BattleUIMode> OnModeChanged;
    public event Action<ActorAlignment> OnTeamChanged;
    public event Action<int> OnTurnChanged;
    
    // Result
    public bool DoAlliesWin { get; private set; }
    public bool DoEnemiesWin { get; private set; }
    
    public event Action<bool> OnDoAlliesWinChanged;
    public event Action<bool> OnDoEnemiesWinChanged;

    protected void Start() {
      Mode = BattleUIMode.Grid;
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
      // Move input reading to BattleUIModeManager
      if (Input.GetButtonDown("Toggle")) {
        Mode = Mode.Next();
        OnModeChanged?.Invoke(Mode);
      }

      var activeActor = battle.ActiveActor;
      if (activeActor != ActiveActor) {
        ActiveActor = activeActor;
        OnActiveActorChanged?.Invoke(ActiveActor);

        Team = activeActor.Alignment;
        OnTeamChanged?.Invoke(Team);
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
