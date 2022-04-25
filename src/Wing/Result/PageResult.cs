namespace Wing.Result
{
    public class PageResult<T>
    {
        public long TotalCount { get; set; }

        public T Items { get; set; }
    }
}
