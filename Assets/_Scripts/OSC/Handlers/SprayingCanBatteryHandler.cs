using System.Collections;
using System.Collections.Generic;
using extOSC;
using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
    public class SprayingCanBatteryHandler : IOSCAdressHandler
    {
        public void HandleMessage(OSCMessage message)
        {
            if (message.ToFloat(out float batteryLevel))
            {
                EventBus<SprayingCanBatteryEvent>.Raise(new SprayingCanBatteryEvent(batteryLevel));
            }
            else
            {
                Debug.LogWarning("SprayingCanBatteryHandler:: Failed to parse message as float.");
            }
        }
    }

    public struct SprayingCanBatteryEvent : IEvent
    {
        public float BatteryLevel { get; private set; }
        public SprayingCanBatteryEvent(float batteryLevel)
        {
            BatteryLevel = batteryLevel;
        }
    }
}
