using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SprayAR.General;
using DG.Tweening;

namespace SprayAR
{
    public class HandleFill : MonoBehaviour
    {
        [SerializeField] private SprayCan _sprayCan;
        EventBinding<FillColorEvent> _fillColorEventBinding;
        bool _isFilling = false;


        void Start()
        {
            _fillColorEventBinding = new EventBinding<FillColorEvent>(OnFillColorEvent);
            EventBus<FillColorEvent>.Register(_fillColorEventBinding);
            DOTween.Init();
        }

        void Update()
        {
            if (_isFilling)
            {
                transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 1f, _sprayCan.FillLevelPercentage/10, 0.5f);
            }
        }

        private void OnFillColorEvent(FillColorEvent @event)
        {
            switch (@event.Type)
            {
                case FillColorEvent.FillColorEventType.Start:
                    _isFilling = true;
                    break;
                case FillColorEvent.FillColorEventType.Stop:
                    _isFilling = false;
                    break;
            }
        }
    }
}
