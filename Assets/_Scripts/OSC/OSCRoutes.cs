// Purpose: Contains the OSC routes for the SprayAR project.
namespace SprayAR
{
    public static class OSCRoutes
    {
        public static readonly string Root = "/sprayar/microcontroller";
        public static readonly string INState = $"{Root}/state";
        public static readonly string INPing = $"{Root}/ping";
        public static readonly string OUTRefill = $"{Root}/Refill";
        public static readonly string INBattery = $"{Root}/batterycharge";
    }
}
