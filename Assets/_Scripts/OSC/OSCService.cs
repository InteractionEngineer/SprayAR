using System.Collections.Generic;
using extOSC;
using UnityEngine;

namespace SprayAR
{
    /// <summary>
    /// Represents a service for handling OSC (Open Sound Control) communication.
    /// </summary>
    public class OSCService : MonoBehaviour
    {
        private OSCReceiver _receiver;
        private OSCTransmitter _transmitter;
        [SerializeField] private string _sprayingCanIP;
        [SerializeField] private int _sprayingCanPort;
        [SerializeField] private int _receiverPort;

        private readonly OSCHandlerDispatcher _dispatcher = new();

        public List<OSCSender> Senders { get; private set; } = new List<OSCSender>();

        void Start()
        {
            InitializeOSCComponents();
        }

        /// <summary>
        /// Initializes the OSC components by adding an OSCReceiver and an OSCTransmitter to the current GameObject.
        /// Sets up the <see cref="OSCReceiver"/> to listen on the configured port and bind to the address schema.
        /// Sets up the <see cref="OSCTransmitter"/> to send messages to the configured IP and port of the spraying can.  
        /// </summary>
        private void InitializeOSCComponents()
        {
            _receiver = gameObject.GetOrAddComponent<OSCReceiver>();
            _transmitter = gameObject.GetOrAddComponent<OSCTransmitter>();

            _receiver.LocalPort = _receiverPort;
            _receiver.Bind(OSCRoutes.Root + "/*", OnMessageReceived);

            SprayingCanStateHandler sprayingCanStateHandler = new();
            SprayingCanPingHandler sprayingCanPingHandler = new();

            _dispatcher.RegisterHandler(OSCRoutes.INState, sprayingCanStateHandler);
            _dispatcher.RegisterHandler(OSCRoutes.INPing, sprayingCanPingHandler);

            _transmitter.RemoteHost = _sprayingCanIP;
            _transmitter.RemotePort = _sprayingCanPort;
        }

        /// <summary>
        /// Bindable Method for when an <see cref="OSCMessage"/> is received.
        /// Calls the <see cref="OSCHandlerDispatcher.Dispatch(OSCMessage)"/> method with the received message. 
        /// </summary>
        /// <param name="message">The received <see cref="OSCMessage"/></param>
        private void OnMessageReceived(OSCMessage message)
        {
            _dispatcher.Dispatch(message);
        }
    }
}
