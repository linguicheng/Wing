using System.Collections.Generic;

namespace Wing.Dashboard.Result
{
    public class PageResult<T>
    {
        public int TotalCount { get; set; }
        public T Items { get; set; }
    }
}
