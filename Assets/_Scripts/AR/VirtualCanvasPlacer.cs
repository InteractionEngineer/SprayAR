using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

namespace SprayAR
{
    public class VirtualCanvasPlacer : MonoBehaviour
    {
        [SerializeField] private GameObject _virtualCanvasPrefab;

        [SerializeField] private ARPlaneManager _arPlaneManager;

        private List<ShaderPainter> _virtualCanvases = new();

        IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            foreach (var plane in _arPlaneManager.trackables)
            {
                PlaceVirtualPlane(plane);
            }
        }

        public void ResetAllCanvases()
        {
            foreach (var canvas in _virtualCanvases)
            {
                canvas.ResetCanvas();
            }
        }


        void PlaceVirtualPlane(ARPlane plane)
        {
            Vector3 position = plane.transform.position;
            Quaternion rotation = plane.transform.rotation;

            // Instantiate the virtual plane at the detected wall's position and orientation
            GameObject virtualPlane = Instantiate(_virtualCanvasPrefab, position, rotation);
            Vector3 newScale = new(virtualPlane.transform.localScale.x * plane.size.x, virtualPlane.transform.localScale.y, plane.size.y * virtualPlane.transform.localScale.z);
            virtualPlane.transform.localScale = newScale;
            Debug.Log($"New scale: {newScale}");
            virtualPlane.transform.SetParent(plane.transform);

            virtualPlane.GetComponent<ShaderPainter>().InitializeCanvas();

            _virtualCanvases.Add(virtualPlane.GetComponent<ShaderPainter>());
        }
    }
}
