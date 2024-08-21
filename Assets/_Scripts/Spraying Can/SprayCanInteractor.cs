using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SprayAR
{
    /// <summary>
    /// Triggers the spray can to fill with a color when the player enters the trigger.
    /// Is attached to the spray can.
    /// </summary>
    public class SprayCanInteractor : MonoBehaviour
    {
        [SerializeField] private SprayCan _sprayCan;

        void Start()
        {
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<SprayColor>() != null)
            {
                _sprayCan.InitiateColorFill(other.GetComponent<SprayColor>().ColorData);
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
