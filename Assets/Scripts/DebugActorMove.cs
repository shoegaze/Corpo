using UnityEngine;

public class DebugActorMove : MonoBehaviour {
  [SerializeField] private BattleController battle;
  
  // TODO: Initialize position in Awake
  private Vector2Int position = new Vector2Int(0, 0);

  private void Update() {
    int h = 0;
    
    if (Input.GetKeyDown(KeyCode.RightArrow)) {
      h += 1;
    }
    
    if (Input.GetKeyDown(KeyCode.LeftArrow)) {
      h -= 1;
    }
    
    int v = 0;
    
    if (Input.GetKeyDown(KeyCode.UpArrow)) {
      v += 1;
    }
    
    if (Input.GetKeyDown(KeyCode.DownArrow)) {
      v -= 1;
    }
    
    if (h != 0 && v != 0) {
      v = 0;
    }
    
    if (h == 0 && v == 0) {
      return;
    }
    
    var delta = new Vector2Int(h, v);
    var to = position + delta;
    
    if (battle.Grid.CanMove(position, to)) {
      transform.localPosition += new Vector3(delta.x, delta.y);
      position = to;
    }
    else {
      Debug.Log("bump");
    }
  }
}
