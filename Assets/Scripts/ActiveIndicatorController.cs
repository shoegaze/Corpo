using Battle;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class ActiveIndicatorController : MonoBehaviour {
  [Inject] private BattleManager battle;

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
