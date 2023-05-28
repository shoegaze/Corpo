using System;
using UnityEngine;

namespace Battle.UI {
  public class BattleUI : MonoBehaviour {
    [SerializeField] private BattleController battle;
    
    public BattleUIMode Mode { get; private set; }
    public ActorAlignment Team { get; private set; }
    public int Turn { get; private set; }

    public event Action<BattleUIMode> OnModeChanged;
    public event Action<ActorAlignment> OnTeamChanged;
    public event Action<int> OnTurnChanged;

    protected void Start() {
      Mode = BattleUIMode.Grid;
      // HACK:
      Team = ActorAlignment.Ally;
      Turn = battle.Turn;
      
      OnModeChanged?.Invoke(Mode);
      OnTeamChanged?.Invoke(Team);
      OnTurnChanged?.Invoke(Turn);
    }

    protected void Update() {
      // Move input management to BattleUIModeManager
      if (Input.GetButtonDown("Toggle")) {
        Mode = Mode.Next();
        OnModeChanged?.Invoke(Mode);
      }

      var activeActor = battle.ActiveActor;
      if (activeActor != null && activeActor.Alignment != Team) {
        Team = activeActor.Alignment;
        OnTeamChanged?.Invoke(Team);
      }

      var turn = battle.Turn;
      if (turn != Turn) {
        Turn = turn;
        OnTurnChanged?.Invoke(Turn);
      }
    }
  }
}
