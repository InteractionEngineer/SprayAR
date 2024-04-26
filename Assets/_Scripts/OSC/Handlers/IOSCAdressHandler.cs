using System.Collections;
using System.Collections.Generic;
using extOSC;
using UnityEngine;

namespace SprayAR
{
    public interface IOSCAdressHandler
    {
        /// <summary>
        /// Handles messages sent to an OSC address. 
        /// Intended to be configured with a dedicated OSC address. 
        /// </summary>
        /// <param name="message">The OSC message to handle.</param>
        void HandleMessage(OSCMessage message);
    }
}
