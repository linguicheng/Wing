namespace Wing.APM.Listeners
{
    public interface IDiagnosticListener : IObserver<KeyValuePair<string, object>>
    {
        string Name { get; }
    }
}
