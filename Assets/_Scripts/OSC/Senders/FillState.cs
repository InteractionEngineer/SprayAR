using System;
using System.Collections;
using System.Collections.Generic;
using extOSC;
using SprayAR.General;
using UnityEngine;

namespace SprayAR
{
    public class FillState : OSCSender
    {
        private EventBinding<FillStateEvent> _fillStateEventBinding;
        public FillStateType State { get; private set; }
        public OSCTransmitter Transmitter { get; private set; }

        public FillState(OSCTransmitter transmitter)
        {
            _fillStateEventBinding = new EventBinding<FillStateEvent>(OnFillStateEvent);
            EventBus<FillStateEvent>.Register(_fillStateEventBinding);
            Transmitter = transmitter;
        }

        private void OnFillStateEvent(FillStateEvent @event)
        {
            State = @event.State;
            Send(Transmitter);
        }

        public override OSCMessage CreateMessage()
        {
            switch (State)
            {
                case FillStateType.Empty:
                    var message = new OSCMessage(OSCRoutes.OUTCanEmpty);
                    message.AddValue(OSCValue.String("Empty"));
                    return message;
                case FillStateType.Full:
                    var message2 = new OSCMessage(OSCRoutes.OUTCanFull);
                    message2.AddValue(OSCValue.String("Full"));
                    return message2;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }


}
