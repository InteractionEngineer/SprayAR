using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
    /// <summary>
    /// The state of the spray can when it is filling up with a new color.
    /// </summary>
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

        /// <summary>
        /// First, check if the can is full and the color is the same as the one being filled.
        /// If so, transition to the idle state.
        /// Otherwise, start filling the can with the new color.
        /// </summary>
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

        /// <summary>
        /// First, stop filling the can with the new color.
        /// Then, set the spray color of the can to the new color.
        /// Finally, stop the can refill progress sound and play the can refill success sound if the can is full.
        /// </summary>
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

        /// <summary>
        /// Update the fill progress of the can.
        /// Currently, the can fills up 25% of the max fill level per second.
        /// </summary>
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
