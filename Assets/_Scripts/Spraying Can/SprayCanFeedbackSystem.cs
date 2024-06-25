using UnityEngine;

namespace SprayAR
{
    public class SprayCanFeedbackSystem : MonoBehaviour
    {
        [SerializeField] private ParticleSystem sprayParticles;
        [SerializeField] private AudioSource spraySound;
        [SerializeField] private Transform _particleOrientationProvider;

        public void ActivateFeedback()
        {
            sprayParticles.Play();
            spraySound.Play();
        }

        public void DeactivateFeedback()
        {
            sprayParticles.Stop();
            spraySound.Stop();
        }

        void Update()
        {
            RealignParticles();
        }

        // Finger tip is too dynamic, using the palm as a reference point. The up vector of the palm faces away from the back of the hand, therefore we need to invert it.
        // Assuming that the palm is oriented away from the body while spraying, since this will be the natural position for the user when they are spraying.
        void RealignParticles()
        {
            sprayParticles.transform.forward = -_particleOrientationProvider.up;
        }
    }
}
