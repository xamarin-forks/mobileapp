using System.Collections.Generic;
using Toggl.Multivac.Models;

namespace Toggl.PrimeRadiant.DTOs
{
    public struct WorkspaceFeatureCollectionDto : IWorkspaceFeatureCollection, IIdentifiable
    {
        public long Id => WorkspaceId;
        public long WorkspaceId { get; }
        public IEnumerable<IWorkspaceFeature> Features { get; }

        private WorkspaceFeatureCollectionDto(
            long workspaceId,
            IEnumerable<IWorkspaceFeature> features)
        {
            WorkspaceId = workspaceId;
            Features = features;
        }

        public static WorkspaceFeatureCollectionDto Clean(IWorkspaceFeatureCollection entity)
            => new WorkspaceFeatureCollectionDto(
                workspaceId: entity.WorkspaceId,
                features: entity.Features);
    }
}
