using System.Collections.Generic;
using extOSC;
using UnityEngine;

namespace SprayAR
{
    /// <summary>
    /// Handles the registration and dispatching of OSC messages to appropriate handlers.
    /// </summary>
    public class OSCHandlerDispatcher
    {
        public Dictionary<string, IOSCAdressHandler> Handlers { get; private set; } = new Dictionary<string, IOSCAdressHandler>();

        /// <summary>
        /// Registers a <see cref="IOSCAdressHandler"/> for a specific OSC address.
        /// </summary>
        /// <param name="address">The OSC address to register the handler for.</param>
        /// <param name="handler">The <see cref="IOSCAdressHandler"/> to register.</param>
        public void RegisterHandler(string address, IOSCAdressHandler handler)
        {
            if (!Handlers.ContainsKey(address))
            {
                Handlers.Add(address, handler);
                Debug.Log($"Registered handler for address: {address}");
            }
            else
            {
                Debug.LogWarning($"Handler already registered for address: {address}");
            }
        }

        /// <summary>
        /// Dispatches an incoming <see cref="OSCMessage"/> to the appropriate <see cref="IOSCAdressHandler"/>, if one is registered for the message's address.
        /// </summary>
        /// <param name="message">The <see cref="OSCMessage"/> to dispatch.</param> 
        public void Dispatch(OSCMessage message)
        {
            string address = message.Address;
            if (Handlers.TryGetValue(address, out IOSCAdressHandler handler))
            {
                handler.HandleMessage(message);
            }
            else
            {
                Debug.LogWarning($"No handler registered for address: {address}");
            }
        }
    }
}
