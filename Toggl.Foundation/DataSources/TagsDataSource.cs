using System;
using Toggl.Foundation.Models;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Foundation.Sync.ConflictResolution;
using Toggl.Multivac;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources
{
    public sealed class TagsDataSource
        : DataSource<IThreadSafeTag, IDatabaseTag, TagDto>, ITagsSource
    {
        private readonly IIdProvider idProvider;
        private readonly ITimeService timeService;

        public TagsDataSource(IIdProvider idProvider, IRepository<IDatabaseTag, TagDto> repository, ITimeService timeService)
            : base(repository)
        {
            Ensure.Argument.IsNotNull(idProvider, nameof(idProvider));
            Ensure.Argument.IsNotNull(timeService, nameof(timeService));

            this.idProvider = idProvider;
            this.timeService = timeService;
        }

        public IObservable<IThreadSafeTag> Create(string name, long workspaceId)
        {
            var id = idProvider.GetNextIdentifier();
            var dto = new TagDto(
                id: id,
                workspaceId: workspaceId,
                name: name,
                at: timeService.CurrentDateTime);

            return Create(dto);
        }

        protected override IThreadSafeTag Convert(IDatabaseTag entity)
            => Tag.From(entity);

        protected override ConflictResolutionMode ResolveConflicts(IDatabaseTag first, TagDto second)
            => Resolver.ForTags.Resolve(first, second);
    }
}
