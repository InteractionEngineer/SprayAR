using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
    /// <summary>
    /// The main driver of the spray can.
    /// Manages the state of the spray can and transitions between states.
    /// </summary>
    public class SprayCanStateMachine
    {
        private EventBinding<SprayCanStateEvent> _sprayCanStateEventBinding;
        private ISprayCanState _currentState;

        public SprayCan Can { get; private set; }
        public ISprayCanState CurrentState { get => _currentState; private set => _currentState = value; }

        public readonly SprayCanFeedbackSystem FeedbackSystem;


        public SprayCanStateMachine(SprayCan can, SprayCanFeedbackSystem sprayCanFeedbackSystem)
        {
            Can = can;
            FeedbackSystem = sprayCanFeedbackSystem;

            _sprayCanStateEventBinding = new EventBinding<SprayCanStateEvent>(OnSprayCanStateEvent);
            EventBus<SprayCanStateEvent>.Register(_sprayCanStateEventBinding);
            CurrentState = new IdleState(this);
            CurrentState.EnterState();
            Debug.Log($"Initial state: {CurrentState.GetType().Name}");
        }

        private void OnSprayCanStateEvent(SprayCanStateEvent sprayCanStateEvent)
        {
            CurrentState?.OnSprayCanStateEvent(sprayCanStateEvent);
        }

        public void ExecuteStateUpdate()
        {
            CurrentState?.Update();
        }

        public void Spray(float force)
        {
            Can.UseSpray(Time.deltaTime / 2);
            Can.Paint(force);
        }

        public void TransitionToState(ISprayCanState newState)
        {
            Debug.Log($"Transitioning from {CurrentState.GetType().Name} to {newState.GetType().Name}");
            CurrentState?.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }
    }
}
