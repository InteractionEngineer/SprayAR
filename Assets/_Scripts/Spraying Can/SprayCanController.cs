using System;
using SprayAR.General;
using UnityEngine;
namespace SprayAR
{
    public class SprayCanController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _sprayParticleSystem;
        private bool _isCanGrabbed = false;
        private float _force = 0.0f;
        private Color _brushColor = Color.blue;
        private EventBinding<SprayCanStateEvent> _sprayCanStateEventBinding;

        public float Force
        {
            get => _force;
            set => _force = value;
        }

        private void OnEnable()
        {
            _sprayCanStateEventBinding = new EventBinding<SprayCanStateEvent>(OnSprayCanStateEvent);
            EventBus<SprayCanStateEvent>.Register(_sprayCanStateEventBinding);
        }

        private void OnDisable()
        {
            EventBus<SprayCanStateEvent>.Unregister(_sprayCanStateEventBinding);
        }

        private void OnSprayCanStateEvent(SprayCanStateEvent data)
        {
            _isCanGrabbed = data.IsGrabbed;
            Force = data.Force;
        }

        // TODO: Implement Painting method. This method should be called when the spray can is grabbed and the user is applying force to the nozzle head.
        // Mock this first, until spraying can is built.
        // Try to make it as generic as possible, so that the mock can be easily replaced with the actual spraying can.
        void Paint()
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 0.75f))
            {
                if (hit.collider.GetComponent<ShaderPainter>() != null)
                {
                    Vector2 pixelUV = hit.textureCoord;
                    float dist = Vector3.Distance(hit.point, transform.position);
                    hit.collider.GetComponent<ShaderPainter>().Paint(pixelUV, dist, _brushColor);
                }
            }
        }

        void Update()
        {
            if (_isCanGrabbed)
            {
                if (Force > 0.5f)
                {
                    _sprayParticleSystem.Play();
                    Paint();
                }
                else
                {
                    _sprayParticleSystem.Stop();
                }
            }
            else
            {
                _sprayParticleSystem.Stop();
            }
        }
    }
}
