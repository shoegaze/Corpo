using UnityEngine;

public class BattleController : MonoBehaviour {
  [SerializeField] private uint width;
  [SerializeField] private uint height;
  
  public BattleGrid Grid { get; private set; }
  private BattleScreen screen;
  
  protected void Awake() {
    Grid = new BattleGrid(width, height);
    screen = GetComponent<BattleScreen>();
  }
  
  protected void OnEnable() {
    // TODO: Read level from presets?
    Grid.GenerateRandomWalls(10, 0.125f);
    screen.BuildViews(Grid);
  }
}
