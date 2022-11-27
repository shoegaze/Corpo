using UnityEngine;

namespace Battle.UI {
  public class TeamIndicator : MonoBehaviour {
    [SerializeField] private BattleController battle;
    [SerializeField] private GameObject allyFrame;
    [SerializeField] private GameObject enemyFrame;
    
    protected void OnGUI() {
      // TODO: Set special indicator for when the battle has ended
      
      if (battle.ActiveActor.Alignment == ActorAlignment.Ally) {
        allyFrame.SetActive(true);
        enemyFrame.SetActive(false);
      }
      else { // Enemy
        allyFrame.SetActive(false);
        enemyFrame.SetActive(true);
      }
    }
  }
}
