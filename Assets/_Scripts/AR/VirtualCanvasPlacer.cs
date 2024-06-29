using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

namespace SprayAR
{
    public class VirtualCanvasPlacer : MonoBehaviour
    {
        [SerializeField] private GameObject _virtualCanvasPrefab;

        [SerializeField] private ARPlaneManager _arPlaneManager;
        [SerializeField] private InputAction _placeCanvasAction;

        [SerializeField] private Transform _controllerTransform;

        // Start is called before the first frame update
        void OnEnable()
        {
            _placeCanvasAction.performed += PlaceCanvas;
            _placeCanvasAction.Enable();
        }

        void OnDisable()
        {
            _placeCanvasAction.performed -= PlaceCanvas;
            _placeCanvasAction.Disable();
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            foreach (var plane in _arPlaneManager.trackables)
            {
                PlaceVirtualPlane(plane);
            }
        }

        private void PlaceCanvas(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (Physics.Raycast(_controllerTransform.position, _controllerTransform.forward, out RaycastHit hit))
                {
                    ARPlane arPlane = hit.transform.GetComponent<ARPlane>();
                    if (arPlane != null)
                    {
                        PlaceVirtualPlane(arPlane);
                    }
                }
            }
        }

        void PlaceVirtualPlane(ARPlane plane)
        {
            Vector3 position = plane.transform.position;
            Quaternion rotation = plane.transform.rotation;

            // Instantiate the virtual plane at the detected wall's position and orientation
            GameObject virtualPlane = Instantiate(_virtualCanvasPrefab, position, rotation);
            Vector3 newScale = new Vector3(virtualPlane.transform.localScale.x * plane.size.x, virtualPlane.transform.localScale.y, plane.size.y * virtualPlane.transform.localScale.z);
            virtualPlane.transform.localScale = newScale;
            Debug.Log($"New scale: {newScale}");
            virtualPlane.transform.SetParent(plane.transform);

            virtualPlane.GetComponent<ShaderPainter>().InitializeCanvas();
        }

    }
}
