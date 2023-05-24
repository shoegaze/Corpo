using System;
using UnityEngine;

namespace Intro {
  [RequireComponent(typeof(Camera))]
  [ExecuteAlways]
  public class IntroCinematic : MonoBehaviour {
    [SerializeField] private Material material;
    
    protected void OnRenderImage(RenderTexture source, RenderTexture destination) {
      Graphics.Blit(source, destination, material);
    }
  }
}
