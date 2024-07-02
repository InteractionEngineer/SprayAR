using System.Collections;
using System.Collections.Generic;
using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
    public struct FillColorEvent : IEvent
    {
        public FillColorEventType Type;
        public Color NewColor;
        public FillColorEvent(FillColorEventType type, Color newColor)
        {
            Type = type;
            NewColor = newColor;
        }

    }
    public enum FillColorEventType
    {
        Start,
        Stop
    }
}
