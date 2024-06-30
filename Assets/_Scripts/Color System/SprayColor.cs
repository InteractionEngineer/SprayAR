using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SprayAR
{
    public class SprayColor : MonoBehaviour
    {
        [SerializeField] private Color _colorData;

        public Color ColorData { get => _colorData; private set => _colorData = value; }

        private void Start()
        {
            ConfigureColorSwatch();
        }

        private void ConfigureColorSwatch()
        {
            GetComponent<Renderer>().material.color = ColorData;
        }
    }
}
