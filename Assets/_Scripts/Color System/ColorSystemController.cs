using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SprayAR
{
    /// <summary>
    /// This class is responsible for showing and hiding the color system, which functions likea color picker as well as a refill system for the spray can.
    /// </summary>
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
