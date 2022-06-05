using System.Collections.Generic;
using Wing.Persistence.Apm;

namespace Wing.Persistence.APM
{
    public class TracerDto
    {
        public Tracer Tracer { get; set; }

        public HttpTracer HttpTracer { get; set; }

        public SqlTracer SqlTracer { get; set; }

        public List<HttpTracerDetail> HttpTracerDetails { get; set; }

        public List<SqlTracerDetail> SqlTracerDetails { get; set; }

        public bool IsStop { get; set; } = false;
    }
}
