using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Battle {
  public class BattleGrid {
    public uint Width { get; }
    public uint Height { get; }
  
    private bool[,] Edges { get; }
    // Synchronize order with parameters concat order
    public List<(Actor.Actor actor, Vector2Int position)> GridActors { get; }
  
    public BattleGrid(uint width, uint height) {
      Width = width;
      Height = height;
      
      uint n = width * height;
      Edges = new bool[n, n];
      BuildEdges();
      
      GridActors = new List<(Actor.Actor, Vector2Int)>();
    }
  
    private void BuildEdges() {
      for (var x = 0; x < Width; x++) {
        for (var y = 0; y < Height; y++) {
        
          for (int dx = -1; dx <= +1; dx++) {
            for (int dy = -1; dy <= +1; dy++) {
              // Exclude self and only consider the bilinear neighborhood 
              if (dx == 0 && dy == 0 ||
                  dx != 0 && dy != 0) { 
                continue;
              }
            
              // Coordinate Space (target cell)
              int s = x + dx; 
              int t = y + dy;
            
              // Ensure:
              //  s :: [0, w)
              //  t :: [0, h)
              if (s < 0 || s >= Width ||
                  t < 0 || t >= Height) {
                continue;
              }
  
              // Index Space
              long i = y * Width + x; // :: [0, n)
              long j = t * Width + s; // :: [0, n)
            
              Edges[i, j] = Edges[j, i] = true;
            }
          }              
        }
      }
    }
  
    public void GenerateRandomWalls(uint maxWalls, float probability) {
      // TODO: Make sure there is a main island where all actors preside
      uint walls = 0;
      for (var x = 0; x < Width; x++) {
        for (var y = 0; y < Height; y++) {
        
          for (int dx = -1; dx <= +1; dx++) {
            for (int dy = -1; dy <= +1; dy++) {
              if (dx == 0 && dy == 0 ||
                  dx != 0 && dy != 0) {
                continue;
              }

              int s = x + dx;
              int t = y + dy;

              if (s < 0 || s >= Width ||
                  t < 0 || t >= Height) {
                continue;
              }

              long i = y * Width + x; 
              long j = t * Height + s;

              if (!Edges[i, j]) {
                continue;
              }

              // TODO: Better wall placing algorithm
              if (walls < maxWalls && Random.value < probability) {
                Edges[i, j] = false;
                Edges[j, i] = false;
                walls++;
              }
            }
          }
        }
      }
    }

    public void RandomlyPlaceActors(IEnumerable<Actor.Actor> actors) {
      // BUG: Overflow error on sufficiently large Width, Height
      var max = (int)(Width * Height);
      var candidates = new List<Vector2Int>(max);

      // Populate candidates
      for (var x = 0; x < Width; x++) {
        for (var y = 0; y < Height; y++) {
          candidates.Add(new Vector2Int(x, y));
        }
      }

      foreach (var actor in actors) {
        // No more available spots!
        if (max == 0) {
          Debug.LogWarning("No more available spots to place an actor.\nIgnoring...");
          return;
        }

        var j = (int)(max * Random.value);
        var position = candidates[j];
        
        { // Swap items to end of window
          var tmp = candidates[j];
          candidates[j] = candidates[max - 1];
          candidates[max - 1] = tmp;
        }

        GridActors.Add((actor, position));

        max--;
      }
    }

    private bool AreConnected(long i, long j) {
      // TODO: Support one-way walls
      return !Edges[i, j] && !Edges[j, i];
    }

    public bool AreConnected(Vector2Int from, Vector2Int to) {
      // Convert to index space
      long i = from.y * Width + from.x;
      long j = to.y * Width + to.x;

      return AreConnected(i, j);
    }
  
    // TODO: Check if to-position has an actor
    public bool TryMoveActor(Actor.Actor actor, Vector2Int to) {
      var i = GridActors.FindIndex(v => v.actor == actor);
      var (target, position) = GridActors[i];

      if (target == null) {
        return false;
      }

      var from = position;
      
      if (!CanMove(from, to)) {
        return false;
      }
      
      GridActors[i] = (target, to);

      return true;
    }

    public Actor.Actor GetActor(Vector2Int position) {
      // TODO: Compare by instance ID
      var i = GridActors.FindIndex(v => v.position == position);

      if (i < 0) {
        return null;
      }

      return GridActors[i].actor;
    }

    public bool TryRemoveActor(Actor.Actor actor) {
      var i = GridActors.FindIndex(v => v.actor == actor);

      if (i < 0) {
        return false;
      }
      
      GridActors.RemoveAt(i);

      return true;
    }
    
    public Vector2Int? GetPosition(Actor.Actor actor) {
      var i = GridActors.FindIndex(v => v.actor == actor);

      if (i < 0) {
        return null;
      }

      return GridActors[i].position;
    }
            
    public bool HasActor(Vector2Int position) {
      return GridActors.Exists(v => v.position == position);
    }
    
    public bool CanMove(Vector2Int from, Vector2Int to) {
      if (from.x < 0 || from.x >= Width ||
          from.y < 0 || from.y >= Height) {
        return false;
      }
      
      if (to.x < 0 || to.x >= Width ||
          to.y < 0 || to.y >= Height) {
        return false;
      }

      // Assert `from` and `to` are adjacent
      if ((to - from).sqrMagnitude != 1) {
        return false;
      }
      
      if (HasActor(to)) {
        return false;
      }

      // Index Space
      long i = from.y * Width + from.x;
      long j = to.y * Width + to.x;
    
      return Edges[i, j];
    }
  }
}
