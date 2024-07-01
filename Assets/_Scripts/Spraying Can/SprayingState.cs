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

        private float _lastSprayTime = 0.0f;
        private float _sprayForce = 0.0f;

        public SprayingState(SprayCanStateMachine stateMachine, SprayCanFeedbackSystem feedbackSystem)
        {
            _stateMachine = stateMachine;
            _feedbackSystem = feedbackSystem;
        }

        public void EnterState()
        {
            _lastSprayTime = Time.time;
            StartSpraying();
        }

        public void ExitState()
        {
            StopSpraying();
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
                    _lastSprayTime = Time.time;
                    _sprayForce = sprayCanStateEvent.Force;
                    if (_stateMachine.Can.IsEmpty)
                    {
                        // TODO: Transition to Empty state, implement Empty state first
                    }
                }
                else
                {
                    _stateMachine.TransitionToState(new IdleState(_stateMachine));
                }
            }
        }

        public void Update()
        {
            if (Time.time - _lastSprayTime > 0.2f)
            {
                _stateMachine.TransitionToState(new IdleState(_stateMachine));
            }
            _stateMachine.Spray(_sprayForce * 10);
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