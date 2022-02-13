using System.Collections.Generic;

namespace Wing.Dashboard.Result
{
    public class PageResult<T>
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; }
    }
}
