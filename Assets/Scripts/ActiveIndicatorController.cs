using Battle;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ActiveIndicatorController : MonoBehaviour {
  [SerializeField] private BattleController battle;

  private Animator animator;
  
  protected void Awake() {
    animator = GetComponent<Animator>();
  }

  protected void Update() {
    var actor = battle.ActiveActor;

    if (actor == null || !actor.IsAlive) {
      return;
    }

    transform.position = actor.View.transform.position;
  }
}
