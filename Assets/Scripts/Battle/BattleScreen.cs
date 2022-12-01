using UnityEngine;

namespace Battle {
  public class BattleScreen : MonoBehaviour {
    [SerializeField] private GameObject[] viewsFloor;
    
    public static void UpdateActorView(Actor.Actor actor, BattleGrid grid) {
      var view = actor.View;
      var position = grid.GridActors
                         .Find(v => v.actor == actor)
                         .position;

      view.transform.localPosition = new Vector3(position.x, position.y);
    }
    
    public void BuildViews(BattleGrid grid, ResourcesCache cache) {
      BuildBackgroundView(grid);
      BuildFloorViews(grid);
      BuildActorViews(grid, cache);
      BuildWallViews(grid);
    }
    
    private void BuildBackgroundView(BattleGrid grid) {
      const float w = 5f;
      const float h = 5f;
      
      var viewBackground = transform.Find("Background");
      viewBackground.localScale = new Vector3(grid.Width, grid.Height);
      viewBackground.localPosition = new Vector3(grid.Width - w, grid.Height - h) / 2f;
    }
    
    private void BuildFloorViews(BattleGrid grid) {
      var viewFloorsRoot = transform.Find("Floors");
      var viewRow = transform.Find("Prototypes/Row");

      for (var y = 0; y < grid.Height; y++) {
        var viewRowInstance = Instantiate(viewRow, viewFloorsRoot);
        viewRowInstance.name = $"Row.{y}";
        viewRowInstance.localPosition = y * Vector3.up;
        viewRowInstance.gameObject.SetActive(true);
      
        for (var x = 0; x < grid.Width; x++) {
          var viewFloor = viewsFloor[(int)(Random.value * viewsFloor.Length)];
          var floor = Instantiate(viewFloor, viewRowInstance);
          floor.name = $"Floor.{x}";
          floor.transform.localPosition = x * Vector3.right;
        }
      }
    }

    private void BuildActorViews(BattleGrid grid, ResourcesCache cache) {
      var viewAllies = transform.Find("Actors/Allies");
      var viewEnemies = transform.Find("Actors/Enemies");
      
      foreach (var (actor, pos) in grid.GridActors) {
        // TODO: Collapse Allies/Enemies transforms into parent
        var viewRoot = actor.Alignment == ActorAlignment.Ally ? viewAllies : viewEnemies;
        
        var viewActor = actor.CreateView(viewRoot);
        viewActor.name = viewActor.GetComponent<Actor.Actor>().Name;
        viewActor.transform.localPosition = new Vector3(pos.x, pos.y);
        viewActor.SetActive(true);
      }
    }
  
    private void BuildWallViews(BattleGrid grid) {
      var viewEdgesRoot = transform.Find("Walls");
      var viewEdge = transform.Find("Prototypes/Wall");
      
      { // Build outer walls
        var viewOuterEdgesTop = viewEdgesRoot.Find("Outer/Top");
        viewOuterEdgesTop.localPosition = new Vector3(0.5f, grid.Height);
        
        var viewOuterEdgesBottom = viewEdgesRoot.Find("Outer/Bottom");
        
        for (var x = 0; x < grid.Width; x++) {
          var viewEdgeTop = Instantiate(viewEdge, viewOuterEdgesTop);
          viewEdgeTop.name = $"Wall.{x}";
          viewEdgeTop.localPosition = x * Vector3.right;
          viewEdgeTop.gameObject.SetActive(true);
          
          var viewEdgeBottom = Instantiate(viewEdge, viewOuterEdgesBottom);
          viewEdgeBottom.name = $"Wall.{x}";
          viewEdgeBottom.localPosition = x * Vector3.right;
          viewEdgeBottom.gameObject.SetActive(true);
        }
      
        var viewOuterEdgesRight = viewEdgesRoot.Find("Outer/Right");
        viewOuterEdgesRight.localPosition = new Vector3(grid.Width, 0.5f);
        
        var viewOuterEdgesLeft = viewEdgesRoot.Find("Outer/Left");
        
        for (var y = 0; y < grid.Height; y++) {
          var viewEdgeRight = Instantiate(viewEdge, viewOuterEdgesRight);
          viewEdgeRight.name = $"Wall.{y}";
          viewEdgeRight.localRotation = Quaternion.Euler(0f, 0f, 90f);
          viewEdgeRight.localPosition = y * Vector3.up;
          viewEdgeRight.gameObject.SetActive(true);
          
          var viewEdgeLeft = Instantiate(viewEdge, viewOuterEdgesLeft);
          viewEdgeLeft.name = $"Wall.{y}";
          viewEdgeLeft.localRotation = Quaternion.Euler(0f, 0f, 90f);
          viewEdgeLeft.localPosition = y * Vector3.up;
          viewEdgeLeft.gameObject.SetActive(true);
        }
      }

      { // Build inner walls
        var viewInnerEdgesRoot = viewEdgesRoot.Find("Inner");
        
        { // Instantiate walls
          var viewRow = transform.Find("Prototypes/Row");
          
          long rows = 2 * grid.Height - 1;
          for (var r = 0; r < rows; r++) {
            var viewRowInstance = Instantiate(viewRow, viewInnerEdgesRoot);
            viewRowInstance.name = $"Row.{r}";
            viewRowInstance.localPosition = new Vector3(0.5f + (r % 2 == 0 ? 0.5f : 0f), 0.5f * r + 0.5f);
            viewRowInstance.gameObject.SetActive(true);
            
            long cols = grid.Width - (r % 2 == 0 ? 1 : 0);
            for (var c = 0; c < cols; c++) {
              var viewEdgeInstance = Instantiate(viewEdge, viewRowInstance);
              viewEdgeInstance.name = $"Wall.{c}";
              viewEdgeInstance.localPosition = c * Vector3.right;
              viewEdgeInstance.gameObject.SetActive(false);
              
              if (r % 2 == 0) {
                viewEdgeInstance.localRotation = Quaternion.Euler(0f, 0f, 90f);
              }
            }
          }
        }

        { // Activate walls
          for (var x = 0; x < grid.Width; x++) {
            for (var y = 0; y < grid.Height; y++) {
              // Center cell
              var from = new Vector2Int(x, y);
              
              // Scan right
              if (x < grid.Width - 1) { 
                var to = new Vector2Int(x + 1, y);
                
                if (grid.AreConnected(from, to)) {
                  int r = 2 * y;
                  int c = x;
                  var queryRight = $"Row.{r}/Wall.{c}";
                  var wallRight = viewInnerEdgesRoot.Find(queryRight);
                  wallRight.gameObject.SetActive(true);
                }
              }
            
              // Scan top
              if (y < grid.Height - 1) { 
                var to = new Vector2Int(x, y + 1);
                
                if (grid.AreConnected(from, to)) {
                  int r = 2 * y + 1;
                  int c = x;
                  var queryTop = $"Row.{r}/Wall.{c}";
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
}
