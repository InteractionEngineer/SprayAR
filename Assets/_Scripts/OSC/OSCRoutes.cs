// Purpose: Contains the OSC routes for the SprayAR project.
namespace SprayAR
{
    public static class OSCRoutes
    {
        public static readonly string Root = "/sprayar";
        public static readonly string INSpray = $"{Root}/spray";
        public static readonly string INSprayCanActive = $"{Root}/sprayCanActive";
        public static readonly string OUTStartRefill = $"{Root}/startRefill";
        public static readonly string OUTEndRefill = $"{Root}/endRefill";
        public static readonly string OUTAbortRefill = $"{Root}/abortRefill";
        public static readonly string OUTCanIsEmpty = $"{Root}/canIsEmpty";
        public static readonly string OUTCanIsFull = $"{Root}/canIsFull";
    }
}
