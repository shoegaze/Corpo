using UnityEngine;

namespace Battle {
  public readonly struct AttackContext {
    public Actor Source { get; }
    public Actor Target { get; }
    public BattleGrid Grid { get; }
    public Vector2Int From { get; }
    public Vector2Int To { get; }
    
    public AttackContext(Actor source, Actor target, BattleGrid grid, Vector2Int from, Vector2Int to) {
      Source = source;
      Target = target;
      Grid = grid;
      From = from;
      To = to;
    }
  }
}