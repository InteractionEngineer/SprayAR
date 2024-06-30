using System;
using System.Collections;
using System.Collections.Generic;
using extOSC;
using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
    public class FillColor : OSCSender
    {
        EventBinding<FillColorEvent> _fillColorEventBinding;
        String oscMessage = "";
        public OSCTransmitter Transmitter { get; private set; }

        public FillColor(OSCTransmitter transmitter)
        {
            _fillColorEventBinding = new EventBinding<FillColorEvent>(OnFillColorEvent);
            EventBus<FillColorEvent>.Register(_fillColorEventBinding);
            Transmitter = transmitter;
        }

        private void OnFillColorEvent(FillColorEvent @event)
        {
            switch (@event.Type)
            {
                case FillColorEvent.FillColorEventType.Start:
                    oscMessage = "Start";
                    break;
                case FillColorEvent.FillColorEventType.Stop:
                    oscMessage = "Stop";
                    break;
            }
            Send(Transmitter);
        }

        public override OSCMessage CreateMessage()
        {
            OSCMessage message = new OSCMessage(OSCRoutes.OUTRefill);
            message.AddValue(OSCValue.String(oscMessage));
            return message;
        }
    }
}
