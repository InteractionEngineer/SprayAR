using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SprayAR
{
    public class ShaderVertexPaint : MonoBehaviour
    {
        [SerializeField] private Transform sphere;
        [SerializeField] private RenderTexture paintingTexture;
        [SerializeField] private Texture2D brushTexture;
        [SerializeField] private float brushSize = 0.1f;
        [SerializeField] private Color brushColor = Color.red;

        private static readonly int BrushPos = Shader.PropertyToID("_BrushPos");
        private static readonly int BrushSize = Shader.PropertyToID("_BrushSize");
        private static readonly int BrushTex = Shader.PropertyToID("_BrushTex");
        private static readonly int PaintTex = Shader.PropertyToID("_PaintTex");
        private static readonly int PaintColor = Shader.PropertyToID("_PaintingColor");

        private Material material;
        void Awake()
        {
            material = GetComponent<Renderer>().material;
            material.SetTexture(BrushTex, brushTexture);
            material.SetColor(PaintColor, brushColor);
            material.SetTexture(PaintTex, paintingTexture);
        }

        void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collision");
            Paint(collision.GetContact(0).point);
        }

        void Paint(Vector3 worldPos)
        {
            Vector3 localPos = transform.InverseTransformPoint(worldPos);
            material.SetVector(BrushPos, new Vector4(localPos.x, localPos.y, localPos.z, 0));
            material.SetFloat(BrushSize, brushSize);
            RenderTexture temp = RenderTexture.GetTemporary(paintingTexture.width, paintingTexture.height, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(paintingTexture, temp, material);
            Graphics.Blit(temp, paintingTexture);
            RenderTexture.ReleaseTemporary(temp);
        }
    }
}
