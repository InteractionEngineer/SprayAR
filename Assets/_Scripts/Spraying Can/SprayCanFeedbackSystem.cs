using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SprayAR
{
    public class SprayCanFeedbackSystem : MonoBehaviour
    {
        private ParticleSystem _sprayParticles;
        private AudioSource _spraySound;
        private bool _isInStandbyMode = false;
        [SerializeField] private GameObject _sprayCanVisual;
        [SerializeField] private Transform _particleOrientationProvider;
        [SerializeField] private GameObject _statsMenu;
        [SerializeField] private Image _colorIndicator;
        private TextMeshProUGUI _fillIndicator;
        private CanvasGroup _statsMenuCanvasGroup;

        void Awake()
        {
            _sprayParticles = _sprayCanVisual.GetComponentInChildren<ParticleSystem>();
            _spraySound = _sprayCanVisual.GetComponentInChildren<AudioSource>();
            _fillIndicator = _statsMenu.GetComponentInChildren<TextMeshProUGUI>();
            _statsMenuCanvasGroup = _statsMenu.GetComponent<CanvasGroup>();
        }

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
            _colorIndicator.color = newColor;
        }

        public void UpdateFillIndicator(float fillAmount)
        {
            _fillIndicator.text = $"{fillAmount * 100}%";
        }

        public void SetToStandby()
        {
            _isInStandbyMode = true;
            DeactivateFeedback();
            DeactivateStatsMenu();
        }

        public void ActivateStatsMenu()
        {
            if (_isInStandbyMode) return;
            _statsMenu.SetActive(true);
            _statsMenuCanvasGroup.DOFade(1, 0.5f);
        }

        public void DeactivateStatsMenu()
        {
            _statsMenuCanvasGroup.DOFade(0, 0.5f).OnComplete(() => _statsMenu.SetActive(false));
        }

        void Update()
        {
            // RealignParticles();
        }

        // Finger tip is too dynamic, using the palm as a reference point. The up vector of the palm faces away from the back of the hand, therefore we need to invert it.
        // Assuming that the palm is oriented away from the body while spraying, since this will be the natural position for the user when they are spraying.
        void RealignParticles()
        {
            _sprayParticles.transform.forward = -_particleOrientationProvider.up;
        }
    }
}
