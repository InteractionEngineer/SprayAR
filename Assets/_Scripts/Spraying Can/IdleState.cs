
namespace SprayAR
{
    public class IdleState : ISprayCanState
    {
        private SprayCanStateMachine _stateMachine;

        public IdleState(SprayCanStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void EnterState()
        {
            // TODO: Ensure the spray can is visible, but not spraying

            // If can is empty, immediately transition to Empty state
        }

        public void ExitState()
        {

        }

        public void OnSprayCanStateEvent(SprayCanStateEvent sprayCanStateEvent)
        {
            if (!sprayCanStateEvent.IsGrabbed)
            {
                //TODO: Transition to Standby state
                _stateMachine.TransitionToState(_stateMachine.Idle);
            }
            else
            {
                if (sprayCanStateEvent.Force > 0.0f)
                {
                    _stateMachine.TransitionToState(_stateMachine.Spraying);
                }
            }
        }

        public void Update()
        {
            // Nothing to do here.
        }
    }
}