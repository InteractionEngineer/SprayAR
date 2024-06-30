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
       
        void Start()
        {
            execute();
        }

        void Update()
        {
            if (_continuously)
            {
                execute();
            }
        }

        void execute()
        {
         transform.position = _reference.position + (_reference.forward * _reachDistance);   
        }
    }
}
