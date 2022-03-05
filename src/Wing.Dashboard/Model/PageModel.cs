namespace Wing.Dashboard.Model
{
    public class PageModel
    {
        public PageModel()
        {
            PageSize = 15;
            PageIndex = 1;
        }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
