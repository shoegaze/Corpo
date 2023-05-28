using UnityEngine;

namespace Battle.UI {
  [RequireComponent(typeof(CanvasRenderer))]
  public class ResultIndicator : MonoBehaviour {
    [SerializeField] private BattleController battle;
    [SerializeField] private GameObject winLabel;
    [SerializeField] private GameObject loseLabel;

    private new CanvasRenderer renderer;

    protected void Awake() {
      renderer = GetComponent<CanvasRenderer>();
      renderer.SetAlpha(0f);
    }

    protected void OnGUI() {
      var battleEnded = battle.DoAlliesWin || battle.DoEnemiesWin;
      renderer.SetAlpha(battleEnded ? 1f: 0f);
      
      winLabel.SetActive(battle.DoAlliesWin);     
      loseLabel.SetActive(battle.DoEnemiesWin);
    }
  }
}
