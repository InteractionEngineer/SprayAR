namespace SprayAR
{
    /// <summary>
    /// Interface for the states of the spray can.
    /// </summary>
    public interface ISprayCanState
    {
        void EnterState();
        void Update();
        void ExitState();
        void OnSprayCanStateEvent(SprayCanStateEvent sprayCanStateEvent);
    }
}
