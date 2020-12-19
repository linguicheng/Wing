using Wing.Convert;

namespace Wing.Result
{
    public class Response<T> : Response
    {
        public T Data { get; set; }

        public override string ToString()
        {
            return new JsonHelper().Serialize(Data);
        }
    }
}
