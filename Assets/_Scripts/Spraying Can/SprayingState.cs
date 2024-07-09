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
        private const float Duration = 0.2f;
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
                    _feedbackSystem.TransitionSpraySoundVolume(Mathf.Clamp(_sprayForce / 3.0f, 0.4f, 1.0f), Duration);

                    if (_stateMachine.Can.IsEmpty)
                    {
                        _stateMachine.TransitionToState(new EmptyState(_stateMachine));
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