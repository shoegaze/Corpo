using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleScreen : MonoBehaviour {
  [SerializeField] private GameObject viewFloor;
  [SerializeField, Range(2, 5)] private int gridWidth = 5;
  [SerializeField, Range(2, 5)] private int gridHeight = 5;
  [SerializeField, Range(0f, 1f)] private float wallProbability = 0.125f;
  [SerializeField, Min(0)] private int maxWalls = 5;
  
  [ShowInInspector] private bool[,] edges;
  [ShowInInspector] private BattleActor[,] actors;

  protected void OnEnable() {
    BuildScene(Array.Empty<BattleActor>(), Array.Empty<BattleActor>());
  }

  public void BuildScene(BattleActor[] allies, BattleActor[] enemies) {
    BuildEdges();
    // PlaceActors(allies, enemies);
    
    BuildViews();
  }

  private void BuildEdges() {
    int n = gridWidth * gridHeight;
    edges = new bool[n, n];

    for (var x = 0; x < gridWidth; x++) {
      for (var y = 0; y < gridHeight; y++) {
        
        for (int dx = -1; dx <= +1; dx++) {
          for (int dy = -1; dy <= +1; dy++) {
            // Exclude self and only consider the bilinear neighborhood 
            if (dx == 0 && dy == 0 ||
                dx != 0 && dy != 0) { 
              continue;
            }
            
            // Coordinate Space (target cell)
            int s = x + dx; 
            int t = y + dy;
            
            // Ensure:
            //  s :: [0, w)
            //  t :: [0, h)
            if (s < 0 || s >= gridWidth ||
                t < 0 || t >= gridHeight) {
              continue;
            }
  
            // Index Space
            int i = y * gridWidth + x; // :: [0, n)
            int j = t * gridWidth + s; // :: [0, n)
            
            edges[i, j] = edges[j, i] = true;
          }
        }              
      }
    }

    // Build random walls
    var walls = 0;
    for (var x = 0; x < gridWidth; x++) {
      for (var y = 0; y < gridHeight; y++) {
        
        for (int dx = -1; dx <= +1; dx++) {
          for (int dy = -1; dy <= +1; dy++) {
            if (dx == 0 && dy == 0 ||
                dx != 0 && dy != 0) {
              continue;
            }

            int s = x + dx;
            int t = y + dy;

            if (s < 0 || s >= gridWidth ||
                t < 0 || t >= gridHeight) {
              continue;
            }

            int i = y * gridWidth + x; 
            int j = t * gridWidth + s;

            if (!edges[i, j]) {
              continue;
            }

            // TODO: Better wall placing algorithm
            if (walls < maxWalls && Random.value < wallProbability) {
              edges[i, j] = false;
              edges[j, i] = false;
              walls++;
            }
          }
        }
      }
    }
  }

  private void PlaceActors(BattleActor[] allies, BattleActor[] enemies) {
    // TODO
    actors = new BattleActor[gridHeight, gridWidth];
    
  }

  private void BuildViews() {
    BuildBackgroundView();
    BuildFloorViews();
    // BuildActorViews();
    BuildWallViews();
  }

  private void BuildBackgroundView() {
    var viewBackground = transform.Find("Background");
    viewBackground.localScale = new Vector3(gridWidth, gridHeight);
    viewBackground.localPosition = new Vector3(gridWidth - 5f, gridHeight - 5f) / 2f;
  }

  private void BuildFloorViews() {
    var viewFloorsRoot = transform.Find("Floors");
    var viewRow = transform.Find("Prototypes/Row");
    
    Debug.Assert(viewRow != null);

    for (var y = 0; y < gridHeight; y++) {
      var viewRowInstance = Instantiate(viewRow, viewFloorsRoot);
      viewRowInstance.gameObject.SetActive(true);
      viewRowInstance.name = $"Row__{y}";
      viewRowInstance.localPosition = y * Vector3.up;
      
      for (var x = 0; x < gridWidth; x++) {
        var floor = Instantiate(viewFloor, viewRowInstance);
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
    var viewEdgesRoot = transform.Find("Walls");
    var viewEdge = transform.Find("Prototypes/Wall");
    
    Debug.Assert(viewEdge != null);

    { // Build outer walls
      var viewOuterEdgesTop = viewEdgesRoot.Find("Outer/Top");
      viewOuterEdgesTop.localPosition = new Vector3(0.5f, gridHeight);
              
      var viewOuterEdgesBottom = viewEdgesRoot.Find("Outer/Bottom");

      for (var x = 0; x < gridWidth; x++) {
        var viewEdgeTop = Instantiate(viewEdge, viewOuterEdgesTop);
        viewEdgeTop.name = $"Wall__{x}";
        viewEdgeTop.localPosition = x * Vector3.right;
        viewEdgeTop.gameObject.SetActive(true);
        
        var viewEdgeBottom = Instantiate(viewEdge, viewOuterEdgesBottom);
        viewEdgeBottom.name = $"Wall__{x}";
        viewEdgeBottom.localPosition = x * Vector3.right;
        viewEdgeBottom.gameObject.SetActive(true);
      }
      
      var viewOuterEdgesRight = viewEdgesRoot.Find("Outer/Right");
      viewOuterEdgesRight.localPosition = new Vector3(gridWidth, 0.5f);
      
      var viewOuterEdgesLeft = viewEdgesRoot.Find("Outer/Left");

      for (var y = 0; y < gridHeight; y++) {
        var viewEdgeRight = Instantiate(viewEdge, viewOuterEdgesRight);
        viewEdgeRight.name = $"Wall__{y}";
        viewEdgeRight.localRotation = Quaternion.Euler(0f, 0f, 90f);
        viewEdgeRight.localPosition = y * Vector3.up;
        viewEdgeRight.gameObject.SetActive(true);
        
        var viewEdgeLeft = Instantiate(viewEdge, viewOuterEdgesLeft);
        viewEdgeLeft.name = $"Wall__{y}";
        viewEdgeLeft.localRotation = Quaternion.Euler(0f, 0f, 90f);
        viewEdgeLeft.localPosition = y * Vector3.up;
        viewEdgeLeft.gameObject.SetActive(true);
      }
    }

    { // Build inner walls
      var viewInnerEdgesRoot = viewEdgesRoot.Find("Inner");

      { // Instantiate walls
        var viewRow = transform.Find("Prototypes/Row");

        int rows = 2 * gridHeight - 1;
        for (var r = 0; r < rows; r++) {
          var viewRowInstance = Instantiate(viewRow, viewInnerEdgesRoot);
          viewRowInstance.gameObject.SetActive(true);
          viewRowInstance.name = $"Row__{r}";
          viewRowInstance.localPosition = new Vector3(0.5f + (r % 2 == 0 ? 0.5f : 0f), 0.5f * r + 0.5f);
          
          int cols = gridWidth - (r % 2 == 0 ? 1 : 0);
          for (var c = 0; c < cols; c++) {
            var viewEdgeInstance = Instantiate(viewEdge, viewRowInstance);
            viewEdgeInstance.gameObject.SetActive(false);
            viewEdgeInstance.name = $"Wall__{c}";
            viewEdgeInstance.localPosition = c * Vector3.right;

            if (r % 2 == 0) {
              viewEdgeInstance.localRotation = Quaternion.Euler(0f, 0f, 90f);
            }
          }
        }
      }

      { // Activate walls
        for (var x = 0; x < gridWidth; x++) {
          for (var y = 0; y < gridHeight; y++) {
            // Center cell
            int i = y * gridWidth + x;
            
            // Scan right
            if (x < gridWidth - 1) { 
              int j = y * gridWidth + (x+1);
              
              if (!edges[i, j] && !edges[j, i]) {
                // TODO
                int r = 2 * y;
                int c = x;
                var queryRight = $"Row__{r}/Wall__{c}";
                var wallRight = viewInnerEdgesRoot.Find(queryRight);
                wallRight.gameObject.SetActive(true);
              }
            }
            
            // Scan top
            if (y < gridHeight - 1) { 
              int j = (y+1) * gridWidth + x;
            
              if (!edges[i, j] && !edges[j, i]) {
                int r = 2 * y + 1;
                int c = x;
                var queryTop = $"Row__{r}/Wall__{c}";
                var wallTop = viewInnerEdgesRoot.Find(queryTop);
                wallTop.gameObject.SetActive(true);
              }
            }
          }
        }
      }
    }
  }
}
