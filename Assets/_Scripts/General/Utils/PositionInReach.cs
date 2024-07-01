using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SprayAR
{
    public class PositionInReach : MonoBehaviour
    {
        [SerializeField] private Transform _reference;
        [SerializeField] private float _reachDistance = 0.3f;
        [SerializeField] private bool _continuously = false;

        void OnEnable()
        {
            Execute();
        }

        void Update()
        {
            if (_continuously)
            {
                Execute();
            }
        }

        void Execute()
        {
            PositionSelfInReach();
            OrientateSelfToReference();
        }

        void PositionSelfInReach()
        {
            transform.position = _reference.position + (_reference.forward * _reachDistance);
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
        }

        void OrientateSelfToReference()
        {
            transform.LookAt(_reference);
        }
    }
}
