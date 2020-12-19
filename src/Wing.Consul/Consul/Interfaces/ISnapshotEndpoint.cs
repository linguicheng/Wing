using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Consul
{
    public interface ISnapshotEndpoint
    {
        Task<QueryResult<Stream>> Save(CancellationToken ct = default);
        Task<QueryResult<Stream>> Save(QueryOptions q, CancellationToken ct = default);
        Task<WriteResult> Restore(Stream s, CancellationToken ct = default);
        Task<WriteResult> Restore(Stream s, WriteOptions q, CancellationToken ct = default);
    }
}
