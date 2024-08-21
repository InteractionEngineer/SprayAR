using extOSC;
using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
    /// <summary>
    /// Handles messages sent on the ping OSC address for the spraying can.
    /// See <see cref="OSCRoutes"/> for the address.
    /// </summary>
    public class SprayingCanPingHandler : IOSCAdressHandler
    {
        public void HandleMessage(OSCMessage message)
        {
            if (message.ToString(out string pingMessage))
            {
                if (pingMessage == "PING")
                {
                    EventBus<SprayingCanPingEvent>.Raise(new SprayingCanPingEvent(pingMessage));
                }
                else
                {
                    Debug.LogWarning("SprayingCanActiveHandler:: Received unexpected message: " + pingMessage);
                }
            }
            else
            {
                Debug.LogWarning("SprayingCanActiveHandler:: Failed to parse message as float.");
            }
        }


        /// <summary>
        /// Event for when the spraying can receives a ping message.
        /// </summary>
        public struct SprayingCanPingEvent : IEvent
        {
            public string Active { get; private set; }

            public SprayingCanPingEvent(string pingMessage)
            {
                Active = pingMessage;
            }
        }
    }
}