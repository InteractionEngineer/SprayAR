using UnityEngine;

namespace SprayAR
{
    public class SimpleCollisionPainter : MonoBehaviour
    {
        Vector3 _lastPaintPosition;
        void Update()
        {

            float distanceToLastPaint = Vector3.Distance(_lastPaintPosition, transform.position);

            if (distanceToLastPaint > 0.2f)
            {
                int steps = Mathf.CeilToInt(distanceToLastPaint / 0.5f);
                for (int i = 0; i < steps; i++)
                {
                    Vector3 pos = Vector3.Lerp(_lastPaintPosition, transform.position, i / (float)steps);
                    RaycastHit interpolatedHit;
                    if (Physics.Raycast(pos, transform.forward, out interpolatedHit, 0.75f))
                    {
                        if (interpolatedHit.collider.GetComponent<ShaderPainter>() != null)
                        {
                            Vector2 pixelUV = interpolatedHit.textureCoord;
                            float dist = Vector3.Distance(interpolatedHit.point, transform.position);
                            interpolatedHit.collider.GetComponent<ShaderPainter>().Paint(pixelUV, dist, Color.red);
                        }
                    }
                }
            }
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f))
            {
                if (hit.collider.GetComponent<ShaderPainter>() != null)
                {
                    _lastPaintPosition = transform.position;
                    Vector2 pixelUV = hit.textureCoord;
                    float dist = Vector3.Distance(hit.point, hit.transform.position);
                    hit.collider.GetComponent<ShaderPainter>().Paint(pixelUV, dist, Color.red);
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
