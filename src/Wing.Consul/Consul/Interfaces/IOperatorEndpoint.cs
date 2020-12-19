using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Consul
{
    public interface IOperatorEndpoint
    {
        Task<QueryResult<RaftConfiguration>> RaftGetConfiguration(CancellationToken ct = default);
        Task<QueryResult<RaftConfiguration>> RaftGetConfiguration(QueryOptions q, CancellationToken ct = default);
        Task<WriteResult> RaftRemovePeerByAddress(string address, CancellationToken ct = default);
        Task<WriteResult> RaftRemovePeerByAddress(string address, WriteOptions q, CancellationToken ct = default);
        Task<WriteResult> KeyringInstall(string key, CancellationToken ct = default);
        Task<WriteResult> KeyringInstall(string key, WriteOptions q, CancellationToken ct = default);
        Task<QueryResult<KeyringResponse[]>> KeyringList(CancellationToken ct = default);
        Task<QueryResult<KeyringResponse[]>> KeyringList(QueryOptions q, CancellationToken ct = default);
        Task<WriteResult> KeyringRemove(string key, CancellationToken ct = default);
        Task<WriteResult> KeyringRemove(string key, WriteOptions q, CancellationToken ct = default);
        Task<WriteResult> KeyringUse(string key, CancellationToken ct = default);
        Task<WriteResult> KeyringUse(string key, WriteOptions q, CancellationToken ct = default);
    }
}
