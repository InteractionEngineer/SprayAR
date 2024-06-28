using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SprayAR
{
    public class ChangeColorState : ISprayCanState
    {

        private float _progress = 0.0f;

        private SprayCanStateMachine _stateMachine;

        public ChangeColorState(SprayCanStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void EnterState()
        {
            _progress = 0.0f;
        }

        public void ExitState()
        {
            _progress = 0.0f;
        }

        public void OnSprayCanStateEvent(SprayCanStateEvent sprayCanStateEvent)
        {
            if (!sprayCanStateEvent.IsGrabbed)
            {
                _stateMachine.TransitionToState(_stateMachine.Standby);
            }
        }

        //TODO: Implement color change logic. Maybe needs some refactoring. 
        public void Update()
        {
        }

    }
}
