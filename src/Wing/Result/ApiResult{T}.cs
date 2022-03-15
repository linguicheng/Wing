namespace Wing.Result
{
    public class ApiResult<T> : ApiResult
    {
        public T Data { get; set; }
    }
}
