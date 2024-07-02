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
        private float _aspectRatio;
        private Renderer _renderer;

        private bool isContinuousBrushStroke = false;

        private float lastBrushTime;
        private Vector2 lastBrushPos;

        [SerializeField] private Color brushColor = Color.red;
        [SerializeField] private float brushStrength = 0.4f;
        [SerializeField] private float maxSprayDistance = 0.8f;
        [SerializeField] private AnimationCurve opacityCurve;

        void Awake()
        {
            _renderer = GetComponent<Renderer>();
            // InitializeCanvas();
        }

        public void InitializeCanvas()
        {
            int textureWidth;
            int textureHeight;
            if (_renderer.bounds.size.x / _renderer.bounds.size.z > 1)
            {
                Debug.Log("Width is greater than height");
                textureWidth = Mathf.Max(Mathf.ClosestPowerOfTwo((int)_renderer.bounds.size.x * 1000), 2048);
                textureHeight = Mathf.Max(Mathf.ClosestPowerOfTwo((int)_renderer.bounds.size.z * 1000), 2048);
            }
            else if (_renderer.bounds.size.z / _renderer.bounds.size.x > 1)
            {
                Debug.Log("Height is greater than width");
                textureWidth = Mathf.Max(Mathf.ClosestPowerOfTwo((int)_renderer.bounds.size.z * 1000), 2048);
                textureHeight = Mathf.Max(Mathf.ClosestPowerOfTwo((int)_renderer.bounds.size.x * 1000), 2048);
            }
            else
            {
                Debug.Log("Width and height are equal");
                textureWidth = 2048;
                textureHeight = 2048;
            }

            _aspectRatio = textureWidth / (float)textureHeight;
            Debug.Log("Texture size: " + textureWidth + "x" + textureHeight);
            Debug.Log("Aspect ratio: " + _aspectRatio);

            renderTexture = new RenderTexture(textureWidth, textureHeight, 0, RenderTextureFormat.ARGB32);
            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.clear);
            renderTexture.Create();
            RenderTexture.active = null;

            tempRenderTexture = new RenderTexture(textureWidth, textureHeight, 0, RenderTextureFormat.ARGB32);
            RenderTexture.active = tempRenderTexture;
            GL.Clear(true, true, Color.clear);
            tempRenderTexture.Create();
            RenderTexture.active = null;

            // Create a temporary texture to use as the brush
            brushTexture = new Texture2D(1, 1);
            brushTexture.SetPixel(0, 0, brushColor);
            brushTexture.Apply();

            // Initialize the paint material with the custom shader
            paintMaterial = new Material(paintShader);
            _renderer.material.mainTexture = renderTexture;
        }

        public void Paint(Vector2 uv, float dist, Color brushColor, float force)
        {
            if (Time.time - lastBrushTime > 0.2f)
            {
                isContinuousBrushStroke = false;
            }
            else
            {
                isContinuousBrushStroke = true;
            }
            // Set shader parameters
            float radius = CalculatePaintingRadius(dist);
            Vector2 adjustedBrushSize = AdjustBrushSizeForAspectRatio(radius);
            brushStrength = CalculateOpacity(dist, force);

            paintMaterial.SetVector("_BrushUV", new Vector4(uv.x, uv.y, 0, 0));
            paintMaterial.SetFloat("_BrushSizeX", adjustedBrushSize.x);
            paintMaterial.SetFloat("_BrushSizeY", adjustedBrushSize.y);
            paintMaterial.SetColor("_BrushColor", brushColor);
            paintMaterial.SetTexture("_BrushTex", brushTexture);
            paintMaterial.SetFloat("_BrushStrength", brushStrength);
            paintMaterial.SetFloat("_IsContinuousBrushStroke", isContinuousBrushStroke ? 1 : 0);
            paintMaterial.SetVector("_PrevBrushUV", new Vector4(lastBrushPos.x, lastBrushPos.y, 0, 0));
            lastBrushPos = uv;
            lastBrushTime = Time.time;
            // Render to the tempRenderTexture
            Graphics.Blit(renderTexture, tempRenderTexture, paintMaterial);

            // Copy the tempRenderTexture back to the renderTexture
            Graphics.Blit(tempRenderTexture, renderTexture);

            // Apply the RenderTexture to the object's material
            _renderer.material.mainTexture = renderTexture;
        }

        private float CalculatePaintingRadius(float dist)
        {
            float distanceRatio = dist / maxSprayDistance;
            // Debug.Log("Distance ratio: " + distanceRatio);
            return Mathf.Lerp(0.001f, 0.07f, distanceRatio);
        }

        private Vector2 AdjustBrushSizeForAspectRatio(float radius)
        {

            if (_aspectRatio > 1)
            {
                return new Vector2(radius / _aspectRatio, radius);
            }
            else if (_aspectRatio < 1)
            {
                return new Vector2(radius, radius * _aspectRatio);
            }
            else
            {
                return new Vector2(radius, radius);
            }
        }

        private float CalculateOpacity(float dist, float force)
        {
            float distanceRatio = dist / maxSprayDistance;
            float opacity = opacityCurve.Evaluate(distanceRatio);
            // Spraying Can force is in range of 1 to 3, so we divide it by 3 to get a value between 0 and 1.
            float forceAdjustedOpacity = opacity * (force / 3f);
            Debug.Log("Force adjusted opacity: " + forceAdjustedOpacity);
            return Mathf.Clamp(forceAdjustedOpacity, 0.1f, 1f);
        }

        private void OnDestroy()
        {
            renderTexture.Release();
            tempRenderTexture.Release();
        }
    }
}