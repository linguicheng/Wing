using Wing.Convert;

namespace Wing.Result
{
    public class Response
    {
        public Response()
        {
            Ret = ResultType.Success;
        }

        public ResultType Ret { get; set; }

        public string Msg { get; set; }
    }
}
