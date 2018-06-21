using System;
using System.Reactive.Linq;
using Toggl.Foundation.DTOs;
using Toggl.Foundation.Models;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Foundation.Sync.ConflictResolution;
using Toggl.Multivac;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources
{
    public sealed class UserDataSource
        : SingletonDataSource<IThreadSafeUser, IDatabaseUser, UserDto>, IUserSource
    {
        private readonly ITimeService timeService;

        public UserDataSource(ISingleObjectStorage<IDatabaseUser, UserDto> storage, ITimeService timeService)
            : base(storage, null)
        {
            Ensure.Argument.IsNotNull(timeService, nameof(timeService));

            this.timeService = timeService;
        }

        public IObservable<IThreadSafeUser> UpdateWorkspace(long workspaceId)
            => Get()
                .Select(user => UserDto.From(user, defaultWorkspaceId: workspaceId))
                .SelectMany(Update);

        public IObservable<IThreadSafeUser> Update(EditUserDTO dto)
            => Get()
                .Select(user => updatedUser(user, dto))
                .SelectMany(Update);

        public UserDto updatedUser(IThreadSafeUser existing, EditUserDTO dto)
            => UserDto.From<IThreadSafeUser>(
                existing,
                beginningOfWeek: dto.BeginningOfWeek,
                syncStatus: SyncStatus.SyncNeeded,
                at: timeService.CurrentDateTime);

        protected override IThreadSafeUser Convert(IDatabaseUser entity)
            => User.From(entity);

        protected override ConflictResolutionMode ResolveConflicts(IDatabaseUser first, UserDto second)
            => Resolver.ForUser.Resolve(first, second);
    }
}
