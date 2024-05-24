using System.Collections;
using System.Collections.Generic;
using extOSC;
using TMPro;
using UnityEngine;

namespace SprayAR
{
    public class OSCTest : MonoBehaviour
    {
        public OSCTransmitter transmitter;

        public OSCReceiver oSCReceiver;
        public TextMeshProUGUI text;
        public float TimeStampLastMessage;
        // Start is called before the first frame update
        void Start()
        {
            var message = new OSCMessage("/test");
            message.AddValue(OSCValue.Float(0.5f));
            transmitter.Send(message);
            oSCReceiver.Bind("/test", ReceiveMessage);

        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time - TimeStampLastMessage > 1)
            {
                TimeStampLastMessage = Time.time;
                var message = new OSCMessage("/test");
                message.AddValue(OSCValue.Float(0.5f));
                transmitter.Send(message);
            }
        }

        private void ReceiveMessage(OSCMessage message)
        {
            Debug.Log("Received message: " + message);
            text.text = message.Values[0].FloatValue.ToString();
        }
    }
}
