namespace SprayAR
{
    public interface ISprayCanState
    {
        void EnterState();
        void Update();
        void ExitState();
        void OnSprayCanStateEvent(SprayCanStateEvent sprayCanStateEvent);
    }
}
