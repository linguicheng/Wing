using System;
using System.Threading;
using System.Threading.Tasks;

namespace Consul
{
    public interface IPreparedQueryEndpoint
    {
        Task<WriteResult<string>> Create(PreparedQueryDefinition query, CancellationToken ct = default);
        Task<WriteResult<string>> Create(PreparedQueryDefinition query, WriteOptions q, CancellationToken ct = default);
        Task<WriteResult> Update(PreparedQueryDefinition query, CancellationToken ct = default);
        Task<WriteResult> Update(PreparedQueryDefinition query, WriteOptions q, CancellationToken ct = default);
        Task<QueryResult<PreparedQueryDefinition[]>> List(CancellationToken ct = default);
        Task<QueryResult<PreparedQueryDefinition[]>> List(QueryOptions q, CancellationToken ct = default);
        Task<QueryResult<PreparedQueryDefinition[]>> Get(string queryID, CancellationToken ct = default);
        Task<QueryResult<PreparedQueryDefinition[]>> Get(string queryID, QueryOptions q, CancellationToken ct = default);
        Task<WriteResult> Delete(string queryID, CancellationToken ct = default);
        Task<WriteResult> Delete(string queryID, WriteOptions q, CancellationToken ct = default);
        Task<QueryResult<PreparedQueryExecuteResponse>> Execute(string queryIDOrName, CancellationToken ct = default);
        Task<QueryResult<PreparedQueryExecuteResponse>> Execute(string queryIDOrName, QueryOptions q, CancellationToken ct = default);
    }
}
