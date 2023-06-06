using MoonSharp.Interpreter;
using UnityEngine;
// ReSharper disable UnusedMember.Global

namespace Lua.Proxy {
  public class ActorProxy {
    private Actor.Actor Actor { get; }

    [MoonSharpHidden]
    public ActorProxy(Actor.Actor actor) {
      Actor = actor;
    }
    
    public Vector3 ViewPosition {
      get => Actor.View.transform.localPosition;
      set => Actor.View.transform.localPosition = value;
    }

    public string Name => Actor.Name;
    public uint MaxHealth => Actor.MaxHealth;
    public ActorAlignment Alignment => Actor.Alignment;
    // TODO:
    // public IEnumerable<Ability> Abilities => Actor.Abilities;
    public uint Health => Actor.Health;
    public bool IsAlive => Actor.IsAlive;

    public void TakeHealth(uint damage) {
      Actor.TakeHealth(damage);
    }

    // public void GiveHealth(uint heal) {
    //   Actor.GiveHealth(heal);
    // }
  }
}