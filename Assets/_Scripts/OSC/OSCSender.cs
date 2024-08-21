using System;
using System.Collections;
using System.Collections.Generic;
using extOSC;
using UnityEngine;

namespace SprayAR
{
    public abstract class OSCSender
    {
        /// <summary>
        /// Factory Method that creates an <see cref="OSCMessage"/> to be sent, containing address and data.
        /// </summary>
        /// <returns>The created <see cref="OSCMessage"/></returns> 
        public abstract OSCMessage CreateMessage();

        /// <summary>
        /// Sends an OSC message using the specified transmitter.
        /// </summary>
        /// <param name="transmitter">The OSCTransmitter to use for sending the message.</param>
        public void Send(OSCTransmitter transmitter)
        {
            OSCMessage message = CreateMessage();
            Debug.Log($"Sending message on route: {message.Address}");
            transmitter.Send(message);
        }
    }
}

