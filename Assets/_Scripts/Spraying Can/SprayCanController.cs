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

        void Paint()
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 0.75f))
            {
                if (hit.collider.GetComponent<ShaderPainter>() != null)
                {
                    Vector2 pixelUV = hit.textureCoord;
                    float dist = Vector3.Distance(hit.point, transform.position);
                    hit.collider.GetComponent<ShaderPainter>().Paint(pixelUV, dist, _brushColor, 1f);
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
