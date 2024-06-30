using System.Collections;
using System.Collections.Generic;
using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
    public struct FillColorEvent : IEvent
    {
        public FillColorEventType Type;
        public FillColorEvent(FillColorEventType type)
        {
            Type = type;
        }
        public enum FillColorEventType
        {
            Start,
            Stop
        }
    }
}
