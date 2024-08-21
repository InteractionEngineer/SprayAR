using DG.Tweening;
using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
    public class HandleFill : MonoBehaviour
    {
        [SerializeField] private SprayCan _sprayCan;
        private SprayColor _sprayColor;
        EventBinding<FillColorEvent> _fillColorEventBinding;
        bool _isFilling = false;
        Vector3 _initialScale;

        void Start()
        {
            _initialScale = transform.localScale;
            _sprayColor = GetComponent<SprayColor>();
            _fillColorEventBinding = new EventBinding<FillColorEvent>(OnFillColorEvent);
            EventBus<FillColorEvent>.Register(_fillColorEventBinding);
            DOTween.Init();
        }

        private void OnFillColorEvent(FillColorEvent @event)
        {
            switch (@event.Type)
            {
                case FillColorEventType.Stop:
                    if (_isFilling)
                    {
                        _isFilling = false;
                        StopFillingAnimation();
                    }
                    break;
                case FillColorEventType.Start:
                    if (@event.NewColor == _sprayColor.ColorData)
                    {
                        _isFilling = true;
                        StartFillingAnimation();
                    }
                    break;
            }
        }

        private void StartFillingAnimation()
        {
            // Return if not filling
            if (!_isFilling) return;

            // Scale the color box up and down in a loop
            transform.DOScale(_initialScale * 1.1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }

        private void StopFillingAnimation()
        {
            transform.DOKill();
            transform.localScale = _initialScale;
        }
    }
}