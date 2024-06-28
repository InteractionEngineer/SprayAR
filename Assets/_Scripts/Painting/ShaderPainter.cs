using UnityEngine;

namespace SprayAR
{
    public class ShaderPainter : MonoBehaviour
    {
        public Shader paintShader;
        private Material paintMaterial;
        private RenderTexture renderTexture;
        private RenderTexture tempRenderTexture;
        private Texture2D brushTexture;
        public float brushSize = 0.1f;
        [SerializeField] private Color brushColor = Color.red;
        [SerializeField] private float brushStrength = 0.4f;
        [SerializeField] private float maxSprayDistance = 0.5f;
        [SerializeField] private AnimationCurve opacityCurve;

        void Start()
        {
            Debug.Log("Bounds size: " + GetComponent<Renderer>().bounds.size);
            int textureWidth = Mathf.Max(Mathf.ClosestPowerOfTwo((int)GetComponent<Renderer>().bounds.size.x * 1000), 2048);
            int textureHeight = Mathf.Max(Mathf.ClosestPowerOfTwo((int)GetComponent<Renderer>().bounds.size.z * 1000), 2048);
            Debug.Log("Texture size: " + textureWidth + "x" + textureHeight);

            renderTexture = new RenderTexture(textureWidth, textureHeight, 0, RenderTextureFormat.ARGB32);
            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.white);
            renderTexture.Create();
            RenderTexture.active = null;

            tempRenderTexture = new RenderTexture(textureWidth, textureHeight, 0, RenderTextureFormat.ARGB32);
            RenderTexture.active = tempRenderTexture;
            GL.Clear(true, true, Color.white);
            tempRenderTexture.Create();
            RenderTexture.active = null;

            // Create a temporary texture to use as the brush
            brushTexture = new Texture2D(1, 1);
            brushTexture.SetPixel(0, 0, brushColor);
            brushTexture.Apply();

            // Initialize the paint material with the custom shader
            paintMaterial = new Material(paintShader);
            Debug.Log("color opacity: " + brushColor.a);
        }

        public void Paint(Vector2 uv, float dist, Color brushColor)
        {
            // Set shader parameters
            float radius = CalculatePaintingRadius(dist);
            brushSize = radius;
            float opacity = CalculateOpacity(dist);
            //TODO: Add brush strength based on applied force to spray can
            brushStrength = opacity;
            paintMaterial.SetVector("_BrushUV", new Vector4(uv.x, uv.y, 0, 0));
            paintMaterial.SetFloat("_BrushSize", radius);
            paintMaterial.SetColor("_BrushColor", brushColor);
            paintMaterial.SetTexture("_BrushTex", brushTexture);
            paintMaterial.SetFloat("_BrushStrength", brushStrength);
            // Render to the tempRenderTexture
            Graphics.Blit(renderTexture, tempRenderTexture, paintMaterial);

            // Copy the tempRenderTexture back to the renderTexture
            Graphics.Blit(tempRenderTexture, renderTexture);

            // Apply the RenderTexture to the object's material
            GetComponent<Renderer>().material.mainTexture = renderTexture;
        }

        private float CalculatePaintingRadius(float dist)
        {
            float distanceRatio = dist / maxSprayDistance;
            Debug.Log("Distance ratio: " + distanceRatio);
            return Mathf.Lerp(0.005f, 0.1f, distanceRatio);

        }

        private float CalculateOpacity(float dist)
        {
            float distanceRatio = dist / maxSprayDistance;
            return opacityCurve.Evaluate(distanceRatio);
        }

        private void OnDestroy()
        {
            renderTexture.Release();
            tempRenderTexture.Release();
        }
    }
}