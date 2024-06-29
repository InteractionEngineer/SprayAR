using UnityEngine;

namespace SprayAR
{
    public class SprayCanFeedbackSystem : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _sprayParticles;
        [SerializeField] private GameObject _activeColorIndicator;
        [SerializeField] private AudioSource _spraySound;
        [SerializeField] private Transform _particleOrientationProvider;

        public void ActivateFeedback()
        {
            _sprayParticles.Play();
            _spraySound.Play();
        }

        public void DeactivateFeedback()
        {
            _sprayParticles.Stop();
            _spraySound.Stop();
        }

        public void UpdateSprayColor(Color newColor)
        {
            var mainModule = _sprayParticles.main;
            mainModule.startColor = newColor;
            _activeColorIndicator.GetComponent<Renderer>().material.color = newColor;
        }

        void Update()
        {
            RealignParticles();
        }

        // Finger tip is too dynamic, using the palm as a reference point. The up vector of the palm faces away from the back of the hand, therefore we need to invert it.
        // Assuming that the palm is oriented away from the body while spraying, since this will be the natural position for the user when they are spraying.
        void RealignParticles()
        {
            _sprayParticles.transform.forward = -_particleOrientationProvider.up;
        }
    }
}
