namespace Wing.Dashboard.Model
{
    public class PageModel<T>
    {
        public PageModel()
        {
            PageSize = 15;
            PageIndex = 1;
        }
        public T Data { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
