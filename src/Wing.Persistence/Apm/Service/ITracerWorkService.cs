namespace Wing.Persistence.Apm
{
    public interface ITracerWorkService
    {
        long HttpTimeoutTotal();

        long SqlTimeoutTotal();
    }
}
