using System;
using System.Collections.Generic;
using Toggl.Foundation.Models;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Foundation.Sync.ConflictResolution;
using Toggl.Multivac;
using Toggl.Multivac.Extensions;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources
{
    public sealed class ClientsDataSource
        : DataSource<IThreadSafeClient, IDatabaseClient, ClientDto>, IClientsSource
    {
        private readonly IIdProvider idProvider;
        private readonly ITimeService timeService;

        public ClientsDataSource(IIdProvider idProvider, IRepository<IDatabaseClient, ClientDto> repository, ITimeService timeService)
            : base(repository)
        {
            Ensure.Argument.IsNotNull(idProvider, nameof(idProvider));
            Ensure.Argument.IsNotNull(timeService, nameof(timeService));

            this.idProvider = idProvider;
            this.timeService = timeService;
        }

        public IObservable<IThreadSafeClient> Create(string name, long workspaceId)
        {
            var client = new ClientDto(
                id: idProvider.GetNextIdentifier(),
                workspaceId: workspaceId,
                name: name,
                at: timeService.CurrentDateTime,
                syncStatus: SyncStatus.SyncNeeded,
                serverDeletedAt: null,
                isDeleted: false,
                lastSyncErrorMessage: null);
            return Create(client);
        }

        public IObservable<IEnumerable<IThreadSafeClient>> GetAllInWorkspace(long workspaceId)
            => GetAll(c => c.WorkspaceId == workspaceId);

        protected override IThreadSafeClient Convert(IDatabaseClient entity)
            => Client.From(entity);

        protected override ConflictResolutionMode ResolveConflicts(IDatabaseClient first, ClientDto second)
            => Resolver.ForClients.Resolve(first, second);
    }
}
