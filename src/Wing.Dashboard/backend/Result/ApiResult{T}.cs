namespace Wing.Dashboard.Result
{
    public class ApiResult<T> : ApiResult
    {
        public T Data { get; set; }
    }
}
