using Sirenix.OdinInspector;
using UnityEngine;

public class BattleScreen : MonoBehaviour {
  [SerializeField] private GameObject viewRow;
  [SerializeField] private GameObject viewFloor;
  [SerializeField, Range(2, 5)] private int gridWidth = 5;
  [SerializeField, Range(2, 5)] private int gridHeight = 5;
  
  private bool[,] edges;
  private bool[,] walls;
  private BattleActor[,] actors;
  
  [Button("Build Scene")]  
  public void BuildScene(BattleActor[] allies, BattleActor[] enemies) {
    BuildEdges();
    BuildWalls();
    PlaceActors(allies, enemies);
    
    BuildViews();
  }
  
  private void BuildEdges() {
    // TODO
    edges = new bool[gridHeight+1, gridWidth+1];
    
  }
  
  private void BuildWalls() {
    // TODO
    walls = new bool[gridHeight, gridWidth];
    
  }

  private void PlaceActors(BattleActor[] allies, BattleActor[] enemies) {
    // TODO
    actors = new BattleActor[gridHeight, gridWidth];
    
  }

  [Button("Build Views")]
  private void BuildViews() {
    BuildFloorViews();
    // BuildActorViews();
    // BuildWallViews();
    // BuildEdgeViews();
  }

  private void BuildFloorViews() {
    var viewFloors = transform.Find("Floors");
    viewFloors.DetachChildren();

    for (int y = 0; y < gridHeight; y++) {
      var row = Instantiate(viewRow, viewFloors);
      row.name = $"Row__{y}";
      row.transform.localPosition = y * Vector3.up;
          
      for (int x = 0; x < gridWidth; x++) {
        var floor = Instantiate(viewFloor, row.transform);
        floor.name = $"Floor__{x}";
        floor.transform.localPosition = x * Vector3.right;
      }
    }
  }

  private void BuildActorViews() {
    // TODO
    var viewActors = transform.Find("Actors");
    viewActors.DetachChildren();
    
  }

  private void BuildWallViews() {
    // TODO
    var viewWalls = transform.Find("Walls");
    viewWalls.DetachChildren();
    
  }
  
  private void BuildEdgeViews() {
    // TODO
    var viewEdges = transform.Find("Edges");
    viewEdges.DetachChildren();
    
  }
}
