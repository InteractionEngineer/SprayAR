using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SprayAR
{
    public class ControllerPaintingTest : MonoBehaviour
    {
        [SerializeField] private InputAction _paintAction;
        void Update()
        {
            Paint();
        }

        private void Paint()
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1))
            {
                if (hit.collider.GetComponent<ShaderPainter>() != null)
                {
                    Vector2 pixelUV = hit.textureCoord;
                    float dist = Vector3.Distance(hit.point, transform.position);
                    hit.collider.GetComponent<ShaderPainter>().Paint(pixelUV, dist);
                }
            }
        }
    }
}
