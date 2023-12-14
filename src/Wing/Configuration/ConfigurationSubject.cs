namespace Wing.Configuration
{
    internal static class ConfigurationSubject
    {
        private static IObserver _observer;

        public static void Add(IObserver observer)
        {
            _observer = observer;
        }

        public static void Notify(Dictionary<string, string> configData)
        {
            _observer.SetData(configData);
        }
    }
}
