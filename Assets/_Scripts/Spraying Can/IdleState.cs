

using UnityEngine;

namespace SprayAR
{
    /// <summary>
    /// The state of the spray can when no action is being performed and it isn't empty.
    /// </summary>
    public class IdleState : ISprayCanState
    {
        private SprayCanStateMachine _stateMachine;

        public IdleState(SprayCanStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void EnterState()
        {
            // If can is empty, immediately transition to Empty state
            if (_stateMachine.Can.IsEmpty)
            {
                _stateMachine.TransitionToState(new EmptyState(_stateMachine));
            }
        }

        public void ExitState()
        {

        }

        public void OnSprayCanStateEvent(SprayCanStateEvent sprayCanStateEvent)
        {
            if (!sprayCanStateEvent.IsGrabbed)
            {
                _stateMachine.TransitionToState(new StandbyState(_stateMachine));
            }
            else
            {
                if (sprayCanStateEvent.Force > 0.0f)
                {
                    _stateMachine.TransitionToState(new SprayingState(_stateMachine, _stateMachine.FeedbackSystem));
                }
            }
        }

        public void Update()
        {
            // Nothing to do here.
        }
    }
}