using Unity.VisualScripting;
using UnityEngine;

namespace SprayAR
{
    [RequireComponent(typeof(SprayCanFeedbackSystem))]
    public class SprayCan : MonoBehaviour
    {
        private float _currentFillLevel;
        private Color _sprayColor = Color.blue;
        private SprayCanStateMachine _stateMachine;
        [SerializeField] private float _maxFillLevel;
        [SerializeField] private Transform _sprayCanBody;
        private SprayCanFeedbackSystem _feedbackSystem;

        private RaycastHit[] raycastHits = new RaycastHit[1];

        [SerializeField] private LayerMask mask;

        public float CurrentFillLevel => _currentFillLevel;

        public float MaxFillLevel => _maxFillLevel;

        public bool IsEmpty => _currentFillLevel <= 0;
        public bool IsFull => _currentFillLevel >= _maxFillLevel;

        public Color SprayColor { get => _sprayColor; private set => _sprayColor = value; }

        public void InitiateColorFill(Color color)
        {
            // TODO: Add empty state as well, remove check for StandbyState (currently necessary for testing purposes)
            if (_stateMachine.CurrentState is IdleState or StandbyState)
                _stateMachine.TransitionToState(new FillColorState(_stateMachine, color));
        }

        public void AbortColorFill()
        {
            if (_stateMachine.CurrentState is FillColorState)
                _stateMachine.TransitionToState(new IdleState(_stateMachine));
        }

        public void SetSprayColor(Color newColor)
        {
            if (newColor == SprayColor)
            {
                return;
            }
            SprayColor = newColor;
            _feedbackSystem.UpdateSprayColor(newColor);
        }

        public void UseSpray(float amount)
        {
            _currentFillLevel = Mathf.Max(_currentFillLevel - amount, 0);
            Debug.Log($"Current fill level: {_currentFillLevel}");
            _feedbackSystem.UpdateFillIndicator(_currentFillLevel / _maxFillLevel);
        }

        public void Refill(float amount)
        {
            _currentFillLevel = Mathf.Min(_currentFillLevel + amount, _maxFillLevel);
            _feedbackSystem.UpdateFillIndicator(_currentFillLevel / _maxFillLevel);
        }

        void Start()
        {
            _feedbackSystem = GetComponent<SprayCanFeedbackSystem>();
            _currentFillLevel = _maxFillLevel;
            _stateMachine = new SprayCanStateMachine(this, _feedbackSystem);
        }

        void Update()
        {
            _stateMachine.ExecuteStateUpdate();
        }

        public void Paint()
        {
            int hitCount = Physics.RaycastNonAlloc(_sprayCanBody.position, _sprayCanBody.forward, raycastHits, 0.75f, mask);
            if (hitCount == 0)
            {
                return;
            }
            RaycastHit hit = raycastHits[0];
            Debug.Log(hit.collider);
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<ShaderPainter>() != null)
                {
                    Vector2 pixelUV = hit.textureCoord;
                    float dist = Vector3.Distance(hit.point, _sprayCanBody.position);
                    hit.collider.GetComponent<ShaderPainter>().Paint(pixelUV, dist, SprayColor);
                }
            }
        }
    }
}
