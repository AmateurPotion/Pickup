using System;
using UnityEngine;

namespace Pickup.Graphics.UI
{
    [ExecuteInEditMode]
    public class Pixelator : MonoBehaviour
    {
        [Range(1, 100)] public int pixelate;

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            src.filterMode = FilterMode.Point;
            var resultTexture = RenderTexture.GetTemporary(src.width / pixelate, src.height / pixelate, 0, src.format);
            resultTexture.filterMode = FilterMode.Point;
            UnityEngine.Graphics.Blit(src, resultTexture);
            UnityEngine.Graphics.Blit(resultTexture, dest);
            RenderTexture.ReleaseTemporary(resultTexture);
        }
    }
}