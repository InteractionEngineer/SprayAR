using UnityEngine;

namespace SprayAR
{
    public class SimpleCollisionPainter : MonoBehaviour
    {

        void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 3))
            {
                if (hit.collider.GetComponent<ShaderPainter>() != null)
                {
                    Vector2 pixelUV = hit.textureCoord;
                    float dist = Vector3.Distance(hit.point, transform.position);
                    hit.collider.GetComponent<ShaderPainter>().Paint(pixelUV, dist);
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
