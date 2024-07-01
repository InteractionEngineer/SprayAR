using System;
using DG.Tweening;
using SprayAR.General;
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
        private float _colorIndicatorMaxHeight;
        private CanvasGroup _statsMenuCanvasGroup;
        private EventBinding<SprayingCanBatteryEvent> _batteryLevelBinding;
        [SerializeField] private GameObject _sprayCanVisual;
        [SerializeField] private Transform _particleOrientationProvider;
        [SerializeField] private GameObject _statsMenu;
        [SerializeField] private Image _colorIndicator;
        [SerializeField] private Image _colorBackground;
        [SerializeField] private TextMeshProUGUI _batteryLevelText;
        [SerializeField] private float _colorBackgroundOpacity = 0.4f;

        void Awake()
        {
            _sprayParticles = _sprayCanVisual.GetComponentInChildren<ParticleSystem>();
            _spraySound = _sprayCanVisual.GetComponentInChildren<AudioSource>();
            // _statsMenuCanvasGroup = _statsMenu.GetComponent<CanvasGroup>();
            _colorIndicatorMaxHeight = _colorIndicator.rectTransform.sizeDelta.y;
        }

        void Start()
        {
            _batteryLevelBinding = new EventBinding<SprayingCanBatteryEvent>(OnBatteryLevelEvent);
        }

        private void OnBatteryLevelEvent(SprayingCanBatteryEvent @event)
        {
            // Voltage is below 2V, indicating empty battery.
            if (@event.BatteryLevel < 2f)
            {
                _batteryLevelText.gameObject.SetActive(true);
            }
            else
            {
                _batteryLevelText.gameObject.SetActive(false);
            }
        }

        public void ActivateFeedback()
        {
            _spraySound.Play();
        }

        public void DeactivateFeedback()
        {
            _spraySound.Stop();
        }

        public void UpdateSprayColor(Color newColor)
        {
            var mainModule = _sprayParticles.main;
            mainModule.startColor = newColor;
            _colorIndicator.color = newColor;
            _colorBackground.color = new Color(newColor.r, newColor.g, newColor.b, _colorBackgroundOpacity);
        }

        public void UpdateFillIndicator(float percentage)
        {
            _colorIndicator.rectTransform.sizeDelta = new Vector2(_colorIndicator.rectTransform.sizeDelta.x, _colorIndicatorMaxHeight * percentage);
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

        }

        public void DeactivateStatsMenu()
        {
            // _statsMenuCanvasGroup.DOFade(0, 0.5f).OnComplete(() => _statsMenu.SetActive(false));
            _statsMenu.SetActive(false);
        }
    }
}
