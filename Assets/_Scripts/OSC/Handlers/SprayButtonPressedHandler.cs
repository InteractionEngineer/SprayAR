using System.Collections;
using System.Collections.Generic;
using extOSC;
using General.EventBus;
using UnityEngine;

namespace SprayAR
{
    public class SprayButtonPressedHandler : IOSCAdressHandler
    {
        public void HandleMessage(OSCMessage message)
        {
            if (message.ToFloat(out float force))
            {
                EventBus<SprayButtonEvent>.Raise(new SprayButtonEvent(force));
            }
            else
            {
                Debug.LogWarning("SprayButtonPressedHandler:: Failed to parse message as float.");
            }
        }

    }

    public struct SprayButtonEvent : IEvent
    {
        public float Force { get; private set; }

        public SprayButtonEvent(float force)
        {
            Force = force;
        }
    }
}