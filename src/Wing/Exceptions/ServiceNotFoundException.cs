namespace Wing.Exceptions
{
    public class ServiceNotFoundException : SystemException
    {
        public ServiceNotFoundException(string serviceName)
            : base($"The \"{serviceName}\" service not found")
        {
        }

        public ServiceNotFoundException()
            : base("Service not found")
        {
        }
    }
}
