namespace SprayAR
{
    /// <summary>
    ///  Contains the OSC routes for the SprayAR project.
    /// </summary>
    public static class OSCRoutes
    {
        public static readonly string Root = "/sprayar/microcontroller";
        public static readonly string INState = $"{Root}/state";
        public static readonly string INPing = $"{Root}/ping";
        public static readonly string OUTRefillStart = $"{Root}/Refill/Start";
        public static readonly string OUTRefillStop = $"{Root}/Refill/Stop";
        public static readonly string OUTCanEmpty = $"{Root}/FillState/Empty";
        public static readonly string OUTCanFull = $"{Root}/FillState/Full";
        public static readonly string INBattery = $"{Root}/charge";

        public static string OUTFillEmpty { get; internal set; }
    }
}
