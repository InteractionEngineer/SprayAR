using System.Collections;
using System.Collections.Generic;
using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
    /// <summary>
    /// The state of the spray can when it is empty.
    /// Doesn't perform any actions at the moment.
    /// </summary>
    public class EmptyState : ISprayCanState
    {
        private SprayCanStateMachine _stateMachine;

        public EmptyState(SprayCanStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void EnterState()
        {
            EventBus<FillStateEvent>.Raise(new FillStateEvent(FillStateType.Empty));
        }

        public void ExitState()
        {
        }

        public void OnSprayCanStateEvent(SprayCanStateEvent sprayCanStateEvent)
        {
        }

        public void Update()
        {

        }
    }
}
