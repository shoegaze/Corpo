using MoonSharp.Interpreter;
using UnityEngine;

namespace Lua.Proxy {
  public class ActorProxy {
    static ActorProxy() {
      UserData.RegisterProxyType<ActorProxy, Actor.Actor>(
              a => new ActorProxy(a));
    }
    
    public Actor.Actor Actor { get; }

    [MoonSharpHidden]
    public ActorProxy(Actor.Actor actor) {
      Actor = actor;
    }
    
    // Manipulate Actor.View instead?
    public Transform Transform => Actor.transform; 
    
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