using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SprayAR
{
    [CreateAssetMenu(fileName = "ColorData", menuName = "Color System/Color Data")]
    public class ColorData : ScriptableObject
    {
        [SerializeField] private Color color;

        public Color Color { get => color; set => color = value; }
    }
}
