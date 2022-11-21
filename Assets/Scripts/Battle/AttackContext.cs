using UnityEngine;

namespace Battle {
  public readonly struct AttackContext {
    public Actor Target { get; }
    public BattleGrid Grid { get; }
    public Vector2Int From { get; }
    public Vector2Int To { get; }
    
    public AttackContext(Actor target, BattleGrid grid, Vector2Int from, Vector2Int to) {
      Grid = grid;
      Target = target;
      From = from;
      To = to;
    }
  }
}