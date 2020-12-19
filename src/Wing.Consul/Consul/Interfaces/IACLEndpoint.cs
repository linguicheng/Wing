using System;
using System.Threading;
using System.Threading.Tasks;

namespace Consul
{
    public interface IACLEndpoint
    {
        Task<WriteResult<string>> Clone(string id, CancellationToken ct = default);
        Task<WriteResult<string>> Clone(string id, WriteOptions q, CancellationToken ct = default);
        Task<WriteResult<string>> Create(ACLEntry acl, CancellationToken ct = default);
        Task<WriteResult<string>> Create(ACLEntry acl, WriteOptions q, CancellationToken ct = default);
        Task<WriteResult<bool>> Destroy(string id, CancellationToken ct = default);
        Task<WriteResult<bool>> Destroy(string id, WriteOptions q, CancellationToken ct = default);
        Task<QueryResult<ACLEntry>> Info(string id, CancellationToken ct = default);
        Task<QueryResult<ACLEntry>> Info(string id, QueryOptions q, CancellationToken ct = default);
        Task<QueryResult<ACLEntry[]>> List(CancellationToken ct = default);
        Task<QueryResult<ACLEntry[]>> List(QueryOptions q, CancellationToken ct = default);
        Task<WriteResult> Update(ACLEntry acl, CancellationToken ct = default);
        Task<WriteResult> Update(ACLEntry acl, WriteOptions q, CancellationToken ct = default);
    }
}
