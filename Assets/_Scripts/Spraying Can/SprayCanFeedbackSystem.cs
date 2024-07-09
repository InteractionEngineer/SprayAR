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
        private EventBinding<SprayingCanBatteryEvent> _batteryLevelBinding;
        [SerializeField] private GameObject _sprayCanVisual;
        [SerializeField] private GameObject _statsMenu;
        [SerializeField] private Image _colorIndicator;
        [SerializeField] private Image _colorBackground;
        [SerializeField] private TextMeshProUGUI _batteryLevelText;
        [SerializeField] private float _colorBackgroundOpacity = 0.4f;
        [SerializeField] private AudioClip _canRefillSuccessSound;
        [SerializeField] private AudioSource _canRefillProgressSound;
        [SerializeField] private Sprite[] _batteryLevelSprites;
        [SerializeField] private Image _batteryLevelImage;

        void Awake()
        {
            _sprayParticles = _sprayCanVisual.GetComponentInChildren<ParticleSystem>();
            _spraySound = _sprayCanVisual.GetComponentInChildren<AudioSource>();
            // _statsMenuCanvasGroup = _statsMenu.GetComponent<CanvasGroup>();
        }

        void Start()
        {
            _colorIndicatorMaxHeight = _colorIndicator.rectTransform.sizeDelta.y;
            _batteryLevelBinding = new EventBinding<SprayingCanBatteryEvent>(OnBatteryLevelEvent);
            EventBus<SprayingCanBatteryEvent>.Register(_batteryLevelBinding);
        }

        private void OnBatteryLevelEvent(SprayingCanBatteryEvent @event)
        {
            // Voltage is below 3V, indicating empty battery.
            switch (@event.BatteryLevel)
            {
                case < 3f:
                    _batteryLevelText.gameObject.SetActive(true);
                    break;
                case < 3.2f:
                    _batteryLevelImage.sprite = _batteryLevelSprites[0];
                    _batteryLevelImage.color = Color.red; // Almost empty battery
                    _batteryLevelText.gameObject.SetActive(false);
                    break;
                case < 3.3f:
                    _batteryLevelImage.sprite = _batteryLevelSprites[1];
                    _batteryLevelImage.color = Color.yellow; // Low battery
                    _batteryLevelText.gameObject.SetActive(false);
                    break;
                case < 3.4f:
                    _batteryLevelImage.sprite = _batteryLevelSprites[2];
                    _batteryLevelImage.color = Color.green; // Full battery
                    _batteryLevelText.gameObject.SetActive(false);
                    break;
                default:
                    break;
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

        public void UpdateSprayColor(Color newColor)
        {
            var mainModule = _sprayParticles.main;
            mainModule.startColor = newColor;
            _colorIndicator.color = newColor;
            newColor.a = _colorBackgroundOpacity;
            _colorBackground.color = newColor;
        }

        public void UpdateFillIndicator(float percentage)
        {
            _colorIndicator.rectTransform.sizeDelta = new Vector2(_colorIndicator.rectTransform.sizeDelta.x, _colorIndicatorMaxHeight * percentage / 100);
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
