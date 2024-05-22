using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SprayAR
{

    public class VertexPainter : MonoBehaviour
    {
        [SerializeField] private Color color;
        [SerializeField] private ParticleSystem _particleSystem;

        // Start is called before the first frame update
        void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            var main = _particleSystem.main;
            main.startColor = color;

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnParticleCollision(GameObject other)
        {
            List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
            ParticlePhysicsExtensions.GetCollisionEvents(_particleSystem, other, collisionEvents);
            for (int i = 0; i < collisionEvents.Count; i++)
            {
                Paint(collisionEvents[i].intersection, other);
            }
        }

        void Paint(Vector3 position, GameObject other)
        {
            Mesh mesh = other.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            Color[] colors = mesh.colors;

            if (colors.Length == 0)
            {
                colors = new Color[vertices.Length];
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = Color.white;
                }
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 worldPosition = other.transform.TransformPoint(vertices[i]);
                if (Vector3.Distance(worldPosition, position) < 0.5f)
                {
                    colors[i] = color;
                    Debug.Log($"Painted vertex {i} at {worldPosition}");

                }
            }
            mesh.colors = colors;
        }
    }
}
