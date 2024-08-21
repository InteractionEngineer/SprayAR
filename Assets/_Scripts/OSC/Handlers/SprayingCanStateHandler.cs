using extOSC;
using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
    /// <summary>
    /// Handles messages sent on the spray button OSC address for the spraying can.
    /// See <see cref="OSCRoutes"/> for the address.
    /// </summary>
    public class SprayingCanStateHandler : IOSCAdressHandler
    {
        /// <summary>
        /// Raises an event with the current state of the spray can. 
        /// State consists of the force applied to the nozzle head and whether the spray can is grabbed. 
        /// If the message does not contain 2 values, logs a warning.
        /// NOTE: isGrabbed is not used in the current implementation, due to hardware limitations. See documentation for more information.
        /// </summary>
        public void HandleMessage(OSCMessage message)
        {
            if (message.Values.Count != 2)
            {
                Debug.LogWarning($"SprayButtonPressedHandler:: Invalid message. Expected 2 values, got {message.Values.Count}");
                return;
            }
            else
            {
                var force = message.Values[1].IntValue;
                EventBus<SprayCanStateEvent>.Raise(new SprayCanStateEvent(force, true));
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