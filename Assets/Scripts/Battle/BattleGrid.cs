using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Battle {
  public class BattleGrid {
    public uint Width { get; }
    public uint Height { get; }
  
    private bool[,] Edges { get; }
    public List<(Actor, (long x, long y))> Actors { get; }
  
    public BattleGrid(uint width, uint height) {
      Width = width;
      Height = height;
    
      uint n = width * height;
      Edges = new bool[n, n];
      BuildEdges();

      Actors = new List<(Actor, (long x, long y))>();
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

    public void RandomlyPlaceActors(IEnumerable<Actor> actors) {
      var placements = new HashSet<(long x, long y)>();
      var count = 0;
      
      foreach (var actor in actors) {
        var placed = false;
        
        count++;

        // Prevent infinite loop on filled grid
        while (!placed && placements.Count < count) {
          var pos = (
                  (long)(Width * Random.value),
                  (long)(Height * Random.value)
          );

          if (placements.Contains(pos)) {
            continue;
          }

          Actors.Add((actor, pos));
            
          placements.Add(pos);
          placed = true;
        }
      }
    }

    public bool AreConnected(long i, long j) {
      // TODO: Support one-way walls
      return !Edges[i, j] && !Edges[j, i];
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

      // Index Space
      long i = from.y * Width + from.x;
      long j = to.y * Width + to.x;
    
      return Edges[i, j];
    }
  }
}
