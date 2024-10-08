using UnityEngine;

namespace SprayAR
{
    public class SimpleCollisionPainter : MonoBehaviour
    {
        Vector3 _lastPaintPosition;
        void Update()
        {
            RaycastHit[] hits = new RaycastHit[1];
            Physics.RaycastNonAlloc(transform.position, transform.forward, hits, 0.8f);
            RaycastHit hit = hits[0];
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<ShaderPainter>() != null)
                {
                    Vector2 pixelUV = hit.textureCoord;
                    float dist = Vector3.Distance(hit.point, hit.transform.position);
                    hit.collider.GetComponent<ShaderPainter>().Paint(pixelUV, dist, Color.red, 1f);
                }
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<ShaderPainter>() != null)
            {
                Debug.Log("Collision");
                Vector3 collisionPoint = collision.contacts[0].point;
                RaycastHit hit;
                if (Physics.Raycast(collisionPoint, Vector3.down, out hit))
                {
                    Vector2 pixelUV = hit.textureCoord;
                    // collision.gameObject.GetComponent<ShaderPainter>().Paint(pixelUV);
                }
            }
        }

    }
}
