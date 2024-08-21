namespace SprayAR
{
    /// <summary>
    /// The state of the spraying can when it isn't grabbed.
    /// This is currently not used due to hardware limitations. See documentation for further information.
    /// </summary>
    public class StandbyState : ISprayCanState
    {
        private SprayCanStateMachine _stateMachine;

        public StandbyState(SprayCanStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void EnterState()
        {
            _stateMachine.FeedbackSystem.DeactivateFeedback();
        }

        public void ExitState()
        {

        }

        public void OnSprayCanStateEvent(SprayCanStateEvent sprayCanStateEvent)
        {
            if (sprayCanStateEvent.IsGrabbed) _stateMachine.TransitionToState(new IdleState(_stateMachine));
        }

        public void Update()
        {
            // Nothing to do here.
        }
    }
}