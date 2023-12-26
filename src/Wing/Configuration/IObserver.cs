namespace Wing.Configuration
{
    internal interface IObserver
    {
        void SetData(Dictionary<string, string> configData);
    }
}
