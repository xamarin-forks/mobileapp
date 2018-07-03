using System;
using System.Collections.Generic;
using Toggl.Multivac;
using Toggl.PrimeRadiant.DTOs;
using Toggl.PrimeRadiant.Models;

namespace Toggl.PrimeRadiant.Realm
{
    internal sealed class SinceParameterStorage : ISinceParameterRepository
    {
        private readonly IRealmAdapter<IDatabaseSinceParameter, SinceParameterDto> realmAdapter;

        private readonly object storageAccess = new object();

        private static readonly Dictionary<Type, long> typesToIdsMapping
            = new Dictionary<Type, long>
            {
                [typeof(IDatabaseClient)] = 0,
                [typeof(IDatabaseProject)] = 1,
                [typeof(IDatabaseTag)] = 2,
                [typeof(IDatabaseTask)] = 3,
                [typeof(IDatabaseTimeEntry)] = 4,
                [typeof(IDatabaseWorkspace)] = 5
            };

        public SinceParameterStorage(IRealmAdapter<IDatabaseSinceParameter, SinceParameterDto> realmAdapter)
        {
            Ensure.Argument.IsNotNull(realmAdapter, nameof(realmAdapter));

            this.realmAdapter = realmAdapter;
        }

        public DateTimeOffset? Get<T>()
            where T : IDatabaseSyncable
        {
            var id = getId<T>();

            lock (storageAccess)
            {
                try
                {
                    var record = realmAdapter.Get(id);
                    return record.Since;
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }
        }

        public void Set<T>(DateTimeOffset? since)
        {
            var id = getId<T>();
            var record = new SinceParameterDto(
                id: id,
                since: since);

            lock (storageAccess)
            {
                try
                {
                    realmAdapter.Update(id, record);
                }
                catch (InvalidOperationException)
                {
                    realmAdapter.Create(record);
                }
            }
        }

        public bool Supports<T>()
            => typesToIdsMapping.TryGetValue(typeof(T), out _);

        private static long getId<T>()
        {
            if (typesToIdsMapping.TryGetValue(typeof(T), out var id))
                return id;

            throw new ArgumentException($"Since parameters for the type {typeof(T).FullName} cannot be stored.");
        }
    }
}
