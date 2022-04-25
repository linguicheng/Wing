using System;
using System.Collections.Generic;

namespace Wing.APM
{
    public interface IDiagnosticListener : IObserver<KeyValuePair<string, object>>
    {
        string Name { get; }

    }
}
