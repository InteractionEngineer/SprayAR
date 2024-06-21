// Purpose: Contains the OSC routes for the SprayAR project.
namespace SprayAR
{
    public static class OSCRoutes
    {
        public static readonly string Root = "/sprayar/microcontroller";
        public static readonly string INState = $"{Root}/state";
        public static readonly string INPing = $"{Root}/ping";
        public static readonly string OUTStartRefill = $"{Root}/startRefill";
        public static readonly string OUTEndRefill = $"{Root}/endRefill";
        public static readonly string OUTAbortRefill = $"{Root}/abortRefill";
        public static readonly string OUTCanIsEmpty = $"{Root}/canIsEmpty";
        public static readonly string OUTCanIsFull = $"{Root}/canIsFull";
    }
}
