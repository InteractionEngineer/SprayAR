using System.Collections;
using System.Collections.Generic;
using extOSC;
using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
    /// <summary>
    /// Handles messages sent on the battery OSC address for the spraying can.
    /// See <see cref="OSCRoutes"/> for the address.
    /// </summary>
    public class SprayingCanBatteryHandler : IOSCAdressHandler
    {
        /// <summary>
        /// Raises an event with the battery level of the spraying can.
        /// If the message cannot be parsed as a float, logs a warning.
        /// </summary>
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

    /// <summary>
    /// Event for when the spraying can receives a battery message.
    /// </summary>
    public struct SprayingCanBatteryEvent : IEvent
    {
        public float BatteryLevel { get; private set; }
        public SprayingCanBatteryEvent(float batteryLevel)
        {
            BatteryLevel = batteryLevel;
        }
    }
}
