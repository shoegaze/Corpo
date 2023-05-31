using System.Collections.Generic;
using Shapes;
using UnityEngine;

namespace Battle.UI {
  public class CandidateCells : ImmediateModeShapeDrawer {
    private readonly List<Vector2Int> cells = new();

    public override void DrawShapes(Camera cam) {
      using (Draw.Command(cam)) {
        foreach (var cell in cells) {
          var position = new Vector2(cell.x, cell.y);
          var size = Vector2.one;
          var rect = new Rect(position, size);
          var color = new Color(1f, 1f, 1f, 0.5f);
          
          Draw.Rectangle(rect, color);
          Draw.ResetStyle();
        }
      }
    }

    public void Queue(/*BattleGrid grid,*/ IEnumerable<Vector2Int> candidates) {
      cells.RemoveAll(_ => true);
      
      foreach (var candidate in candidates) {
        // TODO: Cull invalid candidates:
        //  1. OOB
        //  2. Wall collisions
        
        // if (!grid.IsValidCell(candidate)) {
        //   return;
        // }
        
        // ...       
        
        cells.Add(candidate);
      }
    } 
  }
}