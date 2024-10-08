using SprayAR.General;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SprayAR
{
    public class ControllerPaintingTest : MonoBehaviour
    {
        [SerializeField] private InputAction _paintAction;

        private void OnEnable()
        {
            _paintAction.performed += OnButtonPressed;
            _paintAction.Enable();
        }

        void OnButtonPressed(InputAction.CallbackContext value)
        {
            if (value.ReadValue<float>() > 0.1f)
            {
                float force = value.ReadValue<float>();
                EventBus<SprayCanStateEvent>.Raise(new SprayCanStateEvent(force * 3, true));
            }
            else
            {
                EventBus<SprayCanStateEvent>.Raise(new SprayCanStateEvent(0, false));
            }
        }

        private void OnDisable()
        {
            _paintAction.Disable();
        }


    }
}
