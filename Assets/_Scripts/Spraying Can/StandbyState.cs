using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprayAR
{
    public class StandbyState : ISprayCanState
    {
        private SprayCanStateMachine _stateMachine;

        public StandbyState(SprayCanStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void EnterState()
        {
            // TODO: Deactivate the spray can (visuals, etc.)
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