﻿using System.Threading;
using System.Threading.Tasks;

namespace Consul
{
    public interface ICoordinateEndpoint
    {
        Task<QueryResult<CoordinateDatacenterMap[]>> Datacenters(CancellationToken ct = default);
        Task<QueryResult<CoordinateEntry[]>> Nodes(CancellationToken ct = default);
        Task<QueryResult<CoordinateEntry[]>> Nodes(QueryOptions q, CancellationToken ct = default);
    }
}