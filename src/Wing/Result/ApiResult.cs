namespace Wing.Result
{
    public class ApiResult
    {
        public ApiResult()
        {
            Code = ResultType.Success;
        }

        public ResultType Code { get; set; }

        public string Msg { get; set; }
    }
}
