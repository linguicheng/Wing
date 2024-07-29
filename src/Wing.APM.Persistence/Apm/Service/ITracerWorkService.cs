namespace Wing.APM.Persistence
{
    public interface ITracerWorkService
    {
        long HttpTimeoutTotal();

        long SqlTimeoutTotal();
    }
}
