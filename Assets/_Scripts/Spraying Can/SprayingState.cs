using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace SprayAR
{
    public class SprayingState : ISprayCanState
    {
        private SprayCanStateMachine _stateMachine;
        private SprayCanFeedbackSystem _feedbackSystem;

        private float _sprayForce = 0.0f;

        public SprayingState(SprayCanStateMachine stateMachine, SprayCanFeedbackSystem feedbackSystem)
        {
            _stateMachine = stateMachine;
            _feedbackSystem = feedbackSystem;
        }

        public void EnterState()
        {
            // TODO: Activate Particle System and play sound
            StartSpraying();
        }

        public void ExitState()
        {
            // TODO: Deactivate Particle System and stop sound
            StopSpraying();
        }

        public void OnSprayCanStateEvent(SprayCanStateEvent sprayCanStateEvent)
        {
            if (!sprayCanStateEvent.IsGrabbed)
            {
                _stateMachine.TransitionToState(_stateMachine.Standby);
            }
            else
            {
                if (sprayCanStateEvent.Force > 0.0f)
                {
                    _sprayForce = sprayCanStateEvent.Force;
                    if (_stateMachine.Can.IsEmpty)
                    {
                        // TODO: Transition to Empty state, implement Empty state first
                    }
                }
                else
                {
                    _stateMachine.TransitionToState(_stateMachine.Idle);
                }
            }
        }

        public void Update()
        {
            _stateMachine.Spray(_sprayForce);
        }

        private void StartSpraying()
        {
            _feedbackSystem.ActivateFeedback();

        }

        private void StopSpraying()
        {
            _feedbackSystem.DeactivateFeedback();
        }
    }


}