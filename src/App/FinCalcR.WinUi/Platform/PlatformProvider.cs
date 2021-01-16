namespace StEn.FinCalcR.WinUi.Platform
{
    public static class PlatformProvider
    {
        public static IPlatformProvider Current { get; set; } = new DefaultPlatformProvider();
    }
}
