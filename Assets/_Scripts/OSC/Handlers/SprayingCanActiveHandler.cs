using System;
using extOSC;
using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
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