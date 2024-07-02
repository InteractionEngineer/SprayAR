using System;
using extOSC;
using SprayAR.General;

namespace SprayAR
{
    public class FillColor : OSCSender
    {
        EventBinding<FillColorEvent> _fillColorEventBinding;
        public OSCTransmitter Transmitter { get; private set; }
        public FillColorEventType EventType { get; private set; }

        public FillColor(OSCTransmitter transmitter)
        {
            _fillColorEventBinding = new EventBinding<FillColorEvent>(OnFillColorEvent);
            EventBus<FillColorEvent>.Register(_fillColorEventBinding);
            Transmitter = transmitter;
        }

        private void OnFillColorEvent(FillColorEvent @event)
        {
            EventType = @event.Type;
            Send(Transmitter);
        }

        public override OSCMessage CreateMessage()
        {
            switch (EventType)
            {
                case FillColorEventType.Stop:
                    var message = new OSCMessage(OSCRoutes.OUTRefillStop);
                    message.AddValue(OSCValue.String("Stop"));
                    return message;
                case FillColorEventType.Start:
                    var message2 = new OSCMessage(OSCRoutes.OUTRefillStart);
                    message2.AddValue(OSCValue.String("Start"));
                    return message2;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}