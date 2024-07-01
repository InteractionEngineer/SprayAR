using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
    public class FillColorState : ISprayCanState
    {

        private float _progress = 0.0f;

        private Color _color;

        private SprayCanStateMachine _stateMachine;

        public FillColorState(SprayCanStateMachine stateMachine, Color color)
        {
            _stateMachine = stateMachine;
            _color = color;
        }

        public void EnterState()
        {
            if (_stateMachine.Can.IsFull && _color == _stateMachine.Can.SprayColor)
            {
                _stateMachine.TransitionToState(new IdleState(_stateMachine));
                return;
            }
            _progress = 0.0f;
            EventBus<FillColorEvent>.Raise(new FillColorEvent(FillColorEvent.FillColorEventType.Start, _color));
        }

        public void ExitState()
        {
            if (_progress >= 1.0f)
            {
                EventBus<FillColorEvent>.Raise(new FillColorEvent(FillColorEvent.FillColorEventType.Stop, _color));
                _stateMachine.Can.SetSprayColor(_color);
                _progress = 0.0f;
            }
        }

        public void OnSprayCanStateEvent(SprayCanStateEvent sprayCanStateEvent)
        {
            if (!sprayCanStateEvent.IsGrabbed)
            {
                _stateMachine.TransitionToState(new StandbyState(_stateMachine));
            }
        }

        public void Update()
        {
            _progress += Time.deltaTime * 0.25f;
            _stateMachine.Can.Refill(Time.deltaTime * 5);

            if (_progress >= 1.0f)
            {
                _stateMachine.TransitionToState(new IdleState(_stateMachine));
            }
        }

    }
}
