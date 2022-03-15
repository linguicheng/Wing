namespace Wing.Result
{
    public class PageResult<T>
    {
        public int TotalCount { get; set; }

        public T Items { get; set; }
    }
}
