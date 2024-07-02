using SprayAR.General;

namespace SprayAR
{
    public struct FillStateEvent : IEvent
    {
        public FillStateType State;
        public FillStateEvent(FillStateType state)
        {
            State = state;
        }
    }

    public enum FillStateType
    {
        Empty,
        Full
    }
}
