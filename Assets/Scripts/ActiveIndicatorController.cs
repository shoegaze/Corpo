using Battle;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class ActiveIndicatorController : MonoBehaviour {
  [SerializeField] private BattleController battle;

  private new SpriteRenderer renderer;
  private Animator animator;
  
  protected void Awake() {
    renderer = GetComponent<SpriteRenderer>();
    animator = GetComponent<Animator>();
  }

  protected void Update() {
    var actor = battle.ActiveActor;
    var isValid = actor != null && actor.IsAlive;
    
    renderer.enabled = isValid;

    if (!isValid) {
      return;
    }

    transform.position = actor.View.transform.position;
  }
}
