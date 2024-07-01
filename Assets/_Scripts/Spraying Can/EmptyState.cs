using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SprayAR
{
    public class EmptyState : ISprayCanState
    {
        private SprayCanStateMachine _stateMachine;

        public EmptyState(SprayCanStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void EnterState()
        {

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
