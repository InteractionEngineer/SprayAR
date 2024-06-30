using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SprayAR
{
    public class SprayCanInteractor : MonoBehaviour
    {
        [SerializeField] private SprayCan _sprayCan;

        void Start()
        {
            // GetComponent<Renderer>().material.color = _sprayCan.SprayColor;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<SprayColor>() != null)
            {
                _sprayCan.InitiateColorFill(other.GetComponent<SprayColor>().ColorData.Color);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<SprayColor>() != null)
            {
                _sprayCan.AbortColorFill();
            }
        }
    }
}
