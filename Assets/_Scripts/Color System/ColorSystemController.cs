using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SprayAR
{
    public class ColorSystemController : MonoBehaviour
    {
        [SerializeField] private GameObject _colorPicker;
        [SerializeField] private SprayCan _sprayCan;
        public void ShowColorSystem()
        {
            _colorPicker.SetActive(true);
        }

        public void HideColorSystem()
        {
            _colorPicker.SetActive(false);
            _sprayCan.AbortColorFill();
        }
    }
}
