using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
    public class SprayCanStateMachine
    {
        private EventBinding<SprayCanStateEvent> _sprayCanStateEventBinding;
        private ISprayCanState _currentState;

        public SprayCan Can { get; private set; }
        private SprayCanFeedbackSystem _feedbackSystem;

        public readonly ISprayCanState Idle;
        public readonly ISprayCanState Spraying;
        public readonly ISprayCanState Standby;

        public SprayCanStateMachine(SprayCan can, SprayCanFeedbackSystem sprayCanFeedbackSystem)
        {
            Can = can;
            _feedbackSystem = sprayCanFeedbackSystem;
            Idle = new IdleState(this);
            Spraying = new SprayingState(this, _feedbackSystem);
            Standby = new StandbyState(this);

            _sprayCanStateEventBinding = new EventBinding<SprayCanStateEvent>(OnSprayCanStateEvent);
            EventBus<SprayCanStateEvent>.Register(_sprayCanStateEventBinding);
            _currentState = Standby;
            Debug.Log($"Initial state: {_currentState.GetType().Name}");
        }

        private void OnSprayCanStateEvent(SprayCanStateEvent sprayCanStateEvent)
        {
            _currentState?.OnSprayCanStateEvent(sprayCanStateEvent);
        }

        public void ExecuteStateUpdate()
        {
            _currentState?.Update();
        }

        public void Spray(float force)
        {
            Can.UseSpray(Time.deltaTime * force);
            Can.Paint();
        }

        public void TransitionToState(ISprayCanState newState)
        {
            Debug.Log($"Transitioning from {_currentState.GetType().Name} to {newState.GetType().Name}");
            _currentState?.ExitState();
            _currentState = newState;
            _currentState.EnterState();
        }
    }
}
