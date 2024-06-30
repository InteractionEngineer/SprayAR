using UnityEngine;

namespace SprayAR
{
    public class FillColorState : ISprayCanState
    {

        private float _progress = 0.0f;

        private Color _color;

        private SprayCanStateMachine _stateMachine;

        public FillColorState(SprayCanStateMachine stateMachine, Color color)
        {
            _stateMachine = stateMachine;
            _color = color;
        }

        public void EnterState()
        {
            // _stateMachine.Can.EmptyCan();
            _stateMachine.Can.SetSprayColor(_color);
            _progress = 0.0f;
        }

        public void ExitState()
        {
            if (_progress >= 1.0f)
            {
            }
            _progress = 0.0f;
        }

        public void OnSprayCanStateEvent(SprayCanStateEvent sprayCanStateEvent)
        {
            if (!sprayCanStateEvent.IsGrabbed)
            {
                _stateMachine.TransitionToState(new StandbyState(_stateMachine));
            }
        }

        //TODO: Implement color change logic. Maybe needs some refactoring. 
        public void Update()
        {
            _progress += Time.deltaTime;
            _stateMachine.Can.Refill(Time.deltaTime * 5);

            if (_progress >= 1.0f)
            {
                _stateMachine.TransitionToState(new IdleState(_stateMachine));
            }
        }

    }
}
