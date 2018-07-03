using System.Collections.Generic;
using Toggl.Multivac;
using Toggl.Multivac.Models;

namespace Toggl.PrimeRadiant
{
    public struct WorkspaceFeatureCollectionDto : IWorkspaceFeatureCollection, IIdentifiable
    {
        private WorkspaceFeatureCollectionDto(
            long workspaceId,
            IEnumerable<IWorkspaceFeature> features)
        {
            WorkspaceId = workspaceId;
            Features = features;
        }

        public static WorkspaceFeatureCollectionDto From(
            IWorkspaceFeatureCollection entity,
            New<long> workspaceId = default(New<long>),
            New<IEnumerable<IWorkspaceFeature>> features = default(New<IEnumerable<IWorkspaceFeature>>))
        => new WorkspaceFeatureCollectionDto(
            workspaceId: workspaceId.ValueOr(entity.WorkspaceId),
            features: features.ValueOr(entity.Features));

        public long Id => WorkspaceId;
        public long WorkspaceId { get; }
        public IEnumerable<IWorkspaceFeature> Features { get; }
    }
}
