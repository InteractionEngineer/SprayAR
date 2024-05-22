using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SprayAR
{
    public class ShaderVertexPaint : MonoBehaviour
    {
        [SerializeField] private Transform sphere;
        private Material material;
        void Start()
        {
            material = GetComponent<Renderer>().material;
        }

        // Update is called once per frame
        void Update()
        {
            material.SetVector("_PaintingPos", sphere.position);
        }
    }
}
