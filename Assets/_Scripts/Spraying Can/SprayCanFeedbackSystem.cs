using System;
using DG.Tweening;
using SprayAR.General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SprayAR
{
    /// <summary>
    /// This class is responsible for providing feedback to the user when using the spray can.
    /// </summary>
    public class SprayCanFeedbackSystem : MonoBehaviour
    {
        private ParticleSystem _sprayParticles;
        private AudioSource _spraySound;
        private bool _isInStandbyMode = false;
        private float _colorIndicatorMaxHeight;
        private EventBinding<SprayingCanBatteryEvent> _batteryLevelBinding;
        [SerializeField] private GameObject _sprayCanVisual;
        [SerializeField] private GameObject _statsMenu;
        [SerializeField] private Image _colorIndicator;
        [SerializeField] private Image _colorBackground;
        [SerializeField] private TextMeshProUGUI _batteryLevelEmptyText;
        [SerializeField] private float _colorBackgroundOpacity = 0.4f;
        [SerializeField] private AudioClip _canRefillSuccessSound;
        [SerializeField] private AudioSource _canRefillProgressSound;
        [SerializeField] private Sprite[] _batteryLevelSprites;
        [SerializeField] private TextMeshProUGUI _batteryLevelText;

        void Awake()
        {
            _sprayParticles = _sprayCanVisual.GetComponentInChildren<ParticleSystem>();
            _spraySound = _sprayCanVisual.GetComponentInChildren<AudioSource>();
        }

        void Start()
        {
            _colorIndicatorMaxHeight = _colorIndicator.rectTransform.sizeDelta.y;
            _batteryLevelBinding = new EventBinding<SprayingCanBatteryEvent>(OnBatteryLevelEvent);
            EventBus<SprayingCanBatteryEvent>.Register(_batteryLevelBinding);
        }

        private void OnBatteryLevelEvent(SprayingCanBatteryEvent @event)
        {
            // Voltage is below 3.5V, indicating empty battery.
            if (@event.BatteryLevel < 3.5f)
            {
                _batteryLevelText.text = $"Niedriger Batteriestand: {@event.BatteryLevel:F2}V";
                _batteryLevelEmptyText.gameObject.SetActive(true);
            }
            else
            {
                _batteryLevelEmptyText.gameObject.SetActive(false);
                _batteryLevelText.text = $"Batteriestand: {@event.BatteryLevel:F2}V";
            }
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

        /// <summary>
        /// Transitions the volume of the spray sound to allow for smooth volume changes.
        /// </summary>
        public void TransitionSpraySoundVolume(float targetVolume, float duration)
        {
            _spraySound.DOFade(targetVolume, duration);
        }

        /// <summary>
        /// Updates the visuals to reflect the new color of the spray can.
        /// </summary>
        /// <param name="newColor">The new color to spray.</param>
        public void UpdateSprayColor(Color newColor)
        {
            var mainModule = _sprayParticles.main;
            mainModule.startColor = newColor;
            _colorIndicator.color = newColor;
            newColor.a = _colorBackgroundOpacity;
            _colorBackground.color = newColor;
        }

        /// <summary>
        /// Updates the fill indicator on the hand attached UI to reflect the current fill level of the spray can.
        public void UpdateFillIndicator(float percentage)
        {
            _colorIndicator.rectTransform.sizeDelta = new Vector2(_colorIndicator.rectTransform.sizeDelta.x, _colorIndicatorMaxHeight * percentage / 100);
        }

        /// <summary>
        /// Sets the feedback system to standby mode, where no feedback is provided.
        /// This is supposed to be used in case the isGrabbed value is incorporated in the future.
        /// </summary>
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
            _statsMenu.SetActive(false);
        }

        public void PlayCanRefillSuccessSound()
        {
            AudioSource.PlayClipAtPoint(_canRefillSuccessSound, _sprayCanVisual.transform.position);
        }

        public void PlayCanRefillProgressSound()
        {
            _canRefillProgressSound.Play();
        }

        public void StopCanRefillProgressSound()
        {
            _canRefillProgressSound.Stop();
        }
    }
}
