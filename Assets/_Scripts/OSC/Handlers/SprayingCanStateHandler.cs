using extOSC;
using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
    public class SprayingCanStateHandler : IOSCAdressHandler
    {
        public void HandleMessage(OSCMessage message)
        {
            if (message.Values.Count != 2)
            {
                Debug.LogWarning($"SprayButtonPressedHandler:: Invalid message. Expected 2 values, got {message.Values.Count}");
                return;
            }
            else
            {
                // Debug.Log("SprayingCanStateHandler:: Received message: " + message.Values[0].BoolValue + " " + message.Values[1].IntValue);
                var isGrabbed = message.Values[0].BoolValue;
                Debug.Log("Is Grabbed: " + isGrabbed + "Timestamp: " + Time.time);
                var force = Mathf.Ceil(message.Values[1].IntValue);
                EventBus<SprayCanStateEvent>.Raise(new SprayCanStateEvent(force, isGrabbed));
            }
        }

    }

    /// <summary>
    /// Represents an event that holds the state of the spray can.
    /// </summary>
    public struct SprayCanStateEvent : IEvent
    {
        /// <summary>
        /// Gets a value indicating whether the spray can is grabbed.
        /// </summary>
        public bool IsGrabbed { get; private set; }

        /// <summary>
        /// Gets the force the user is applying to the spray can nozzle head.
        /// </summary>
        public float Force { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SprayCanStateEvent"/> struct.
        /// </summary>
        /// <param name="force">The force applied to the nozzle head in Newton.</param>
        /// <param name="isGrabbed">A value indicating whether the spray can is grabbed.</param>
        public SprayCanStateEvent(float force, bool isGrabbed = false)
        {
            IsGrabbed = isGrabbed;
            Force = force;
        }
    }
}