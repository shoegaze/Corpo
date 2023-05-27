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

    protected void Awake() {
      Mode = BattleUIMode.Grid;
      Team = battle.ActiveActor.Alignment;
      Turn = battle.Turn;
    }

    protected void Update() {
      if (Input.GetButtonDown("Toggle")) {
        Mode = Mode.Next();
        OnModeChanged?.Invoke(Mode);
      }

      var team = battle.ActiveActor.Alignment;
      if (team != Team) {
        Team = team;
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
