using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
    public class FillColorState : ISprayCanState
    {

        private float _progress = 0.0f;

        private float _lastFillTime = 0.0f;

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
            _lastFillTime = Time.time;
            _progress = 0.0f;
            _stateMachine.Can.EmptyCan();
            _stateMachine.FeedbackSystem.PlayCanRefillProgressSound();
            EventBus<FillColorEvent>.Raise(new FillColorEvent(FillColorEventType.Start, _color));
            EventBus<FillStateEvent>.Raise(new FillStateEvent(FillStateType.Empty));
        }

        public void ExitState()
        {
            EventBus<FillColorEvent>.Raise(new FillColorEvent(FillColorEventType.Stop, _color));
            EventBus<FillStateEvent>.Raise(new FillStateEvent(FillStateType.Full));
            _stateMachine.Can.SetSprayColor(_color);
            _stateMachine.FeedbackSystem.StopCanRefillProgressSound();
            if (_progress >= 1f)
            {
                _stateMachine.FeedbackSystem.PlayCanRefillSuccessSound();
            }
            _progress = 0.0f;
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
            if (Time.time - _lastFillTime > 1.0f)
            {
                _progress += 0.25f;
                // Add 25% of the max fill level per second
                _stateMachine.Can.Refill(25.0f);
                _lastFillTime = Time.time;
            }

            if (_progress >= 1.0f)
            {
                _stateMachine.TransitionToState(new IdleState(_stateMachine));
            }
        }

    }
}
