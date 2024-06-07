using UnityEngine;

namespace SprayAR
{
    public class ShaderPainter : MonoBehaviour
    {
        private Camera cam;
        public Shader paintShader;
        private Material paintMaterial;
        private RenderTexture renderTexture;
        private RenderTexture tempRenderTexture;
        private Texture2D brushTexture;
        public float brushSize = 0.1f;
        public Color brushColor = Color.red;
        public float brushStrength = 0.4f;

        void Start()
        {
            cam = Camera.main;
            Debug.Log("Bounds size: " + GetComponent<Renderer>().bounds.size);
            int textureWidth = Mathf.Max(Mathf.ClosestPowerOfTwo((int)GetComponent<Renderer>().bounds.size.x * 1000), 2048);
            int textureHeight = Mathf.Max(Mathf.ClosestPowerOfTwo((int)GetComponent<Renderer>().bounds.size.z * 1000), 2048);
            Debug.Log("Texture size: " + textureWidth + "x" + textureHeight);

            renderTexture = new RenderTexture(textureWidth, textureHeight, 0, RenderTextureFormat.ARGB32);
            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.white);
            renderTexture.Create();
            RenderTexture.active = null;

            // Initialize the temporary RenderTexture
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

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Renderer renderer = hit.collider.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        // Get texture coordinates
                        float dist = Vector3.Distance(hit.point, transform.position);
                        Vector2 uv = hit.textureCoord;
                        Paint(uv, dist);
                    }
                }
            }
        }

        public void Paint(Vector2 uv, float dist)
        {
            // Set shader parameters
            float radius = CalculatePaintingRadius(dist);
            brushSize = radius;
            float opacity = CalculateOpacity(dist);
            brushStrength = opacity;
            paintMaterial.SetVector("_BrushUV", new Vector4(uv.x, uv.y, 0, 0));
            paintMaterial.SetFloat("_BrushSize", radius);
            paintMaterial.SetColor("_BrushColor", brushColor);
            paintMaterial.SetTexture("_BrushTex", brushTexture);
            paintMaterial.SetFloat("_BrushStrength", opacity);
            // Render to the tempRenderTexture
            Graphics.Blit(renderTexture, tempRenderTexture, paintMaterial);

            // Copy the tempRenderTexture back to the renderTexture
            Graphics.Blit(tempRenderTexture, renderTexture);

            // Apply the RenderTexture to the object's material
            GetComponent<Renderer>().material.mainTexture = renderTexture;
        }

        private float CalculatePaintingRadius(float dist)
        {
            float maxDist = 3.0f;
            float distanceRatio = dist / maxDist;
            Debug.Log("Distance ratio: " + distanceRatio);
            return Mathf.Lerp(0.0001f, 0.3f, distanceRatio);

        }

        private float CalculateOpacity(float dist)
        {
            float maxDist = 3.0f;
            float distanceRatio = dist / maxDist;
            return Mathf.Lerp(1, 0.01f, distanceRatio);
        }

        void OnDestroy()
        {
            renderTexture.Release();
            tempRenderTexture.Release();
        }
    }
}