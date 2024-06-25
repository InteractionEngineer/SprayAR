using UnityEngine;

namespace SprayAR
{
    [RequireComponent(typeof(SprayCanFeedbackSystem))]
    public class SprayCan : MonoBehaviour
    {
        private float currentFillLevel;
        private Color _sprayColor = Color.blue;
        private SprayCanStateMachine _stateMachine;
        [SerializeField] private float maxFillLevel;
        [SerializeField] private Transform _sprayCanBody;
        [SerializeField] private SprayCanFeedbackSystem _feedbackSystem;

        public float CurrentFillLevel => currentFillLevel;

        public float MaxFillLevel => maxFillLevel;

        public bool IsEmpty => currentFillLevel <= 0;
        public bool IsFull => currentFillLevel >= maxFillLevel;

        public void UseSpray(float amount)
        {
            currentFillLevel = Mathf.Max(currentFillLevel - amount, 0);
        }

        public void Refill(float amount)
        {
            currentFillLevel = Mathf.Min(currentFillLevel + amount, maxFillLevel);
        }

        void Start()
        {
            currentFillLevel = maxFillLevel;
            _stateMachine = new SprayCanStateMachine(this, _feedbackSystem);
        }

        void Update()
        {
            _stateMachine.ExecuteStateUpdate();
        }

        public void Paint()
        {
            if (Physics.Raycast(_sprayCanBody.position, _sprayCanBody.forward, out RaycastHit hit, 0.75f))
            {
                if (hit.collider.GetComponent<ShaderPainter>() != null)
                {
                    Vector2 pixelUV = hit.textureCoord;
                    float dist = Vector3.Distance(hit.point, _sprayCanBody.position);
                    hit.collider.GetComponent<ShaderPainter>().Paint(pixelUV, dist, _sprayColor);
                }
            }
        }
    }
}
