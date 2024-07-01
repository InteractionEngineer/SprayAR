using Unity.VisualScripting;
using UnityEngine;

namespace SprayAR
{
    [RequireComponent(typeof(SprayCanFeedbackSystem))]
    public class SprayCan : MonoBehaviour
    {
        private float _currentFillLevel;
        [SerializeField] private Color _sprayColor = Color.blue;
        private SprayCanStateMachine _stateMachine;
        [SerializeField] private float _maxFillLevel;
        [SerializeField] private Transform _nozzleOrigin;
        private SprayCanFeedbackSystem _feedbackSystem;

        private RaycastHit[] raycastHits = new RaycastHit[1];

        [SerializeField] private LayerMask mask;

        public float CurrentFillLevel => _currentFillLevel;

        public int FillLevelPercentage => (int)(_currentFillLevel / _maxFillLevel * 100);

        public float MaxFillLevel => _maxFillLevel;

        public bool IsEmpty => _currentFillLevel <= 0;
        public bool IsFull => _currentFillLevel >= _maxFillLevel;

        public Color SprayColor { get => _sprayColor; private set => _sprayColor = value; }

        public void InitiateColorFill(Color color)
        {
            if (_stateMachine.CurrentState is IdleState or EmptyState)
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
            _feedbackSystem.UpdateFillIndicator(FillLevelPercentage);
        }

        public void Refill(float percentageAmount)
        {
            _currentFillLevel = Mathf.Min(_currentFillLevel + _maxFillLevel * percentageAmount / 100, _maxFillLevel);
            _feedbackSystem.UpdateFillIndicator(FillLevelPercentage);
        }

        public void EmptyCan()
        {
            _currentFillLevel = 0;
            _feedbackSystem.UpdateFillIndicator(0);
        }

        void Start()
        {
            _feedbackSystem = GetComponent<SprayCanFeedbackSystem>();
            _currentFillLevel = _maxFillLevel;
            _stateMachine = new SprayCanStateMachine(this, _feedbackSystem);
            _feedbackSystem.UpdateSprayColor(SprayColor);
        }

        void Update()
        {
            _stateMachine.ExecuteStateUpdate();
        }

        public void Paint(float force)
        {
            int hitCount = Physics.RaycastNonAlloc(_nozzleOrigin.position, _nozzleOrigin.forward, raycastHits, 0.8f, mask);
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
                    float dist = Vector3.Distance(hit.point, _nozzleOrigin.position);
                    hit.collider.GetComponent<ShaderPainter>().Paint(pixelUV, dist, SprayColor, force);
                }
            }
        }
    }
}
