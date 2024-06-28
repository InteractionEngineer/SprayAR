using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SprayAR
{
    public class SprayColor : MonoBehaviour
    {
        [SerializeField] private ColorData _colorData;

        public ColorData ColorData { get => _colorData; private set => _colorData = value; }

        // Start is called before the first frame update
        void Start()
        {
            ConfigureColorSwatch();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void ConfigureColorSwatch()
        {
            GetComponent<Renderer>().material.color = ColorData.Color;
        }
    }
}
