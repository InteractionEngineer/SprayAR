using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SprayAR
{
    public class OrientateTowards : MonoBehaviour
    {
        [SerializeField] private Transform _reference;
        [SerializeField] private bool _continuously = false;

        void OnEnable()
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
            transform.LookAt(_reference);
        }
    }
}
