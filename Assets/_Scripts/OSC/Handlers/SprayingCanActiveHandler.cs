using extOSC;
using General.EventBus;
using UnityEngine;

namespace SprayAR
{
    public class SprayingCanActiveHandler : IOSCAdressHandler
    {
        public void HandleMessage(OSCMessage message)
        {
            if (message.ToBool(out bool active))
            {
                EventBus<SprayingCanActiveEvent>.Raise(new SprayingCanActiveEvent(active));
            }
            else
            {
                Debug.LogWarning("SprayingCanActiveHandler:: Failed to parse message as float.");
            }
        }
    }


    public struct SprayingCanActiveEvent : IEvent
    {
        public bool Active { get; private set; }

        public SprayingCanActiveEvent(bool active)
        {
            Active = active;
        }
    }
}