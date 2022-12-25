namespace Wing.Persistence.Saga
{
    public class ResponseData
    {
        public ResponseData()
        {
            Success = true;
        }

        public bool Success { get; set; }

        public string Msg { get; set; }
    }
}
