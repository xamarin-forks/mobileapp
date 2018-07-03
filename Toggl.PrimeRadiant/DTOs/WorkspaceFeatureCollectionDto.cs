using System.Collections.Generic;
using System.Collections.ObjectModel;
using Toggl.Multivac;
using Toggl.Multivac.Models;

namespace Toggl.PrimeRadiant.DTOs
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

        public static WorkspaceFeatureCollectionDto Clean(IWorkspaceFeatureCollection entity)
            => new WorkspaceFeatureCollectionDto(
                workspaceId: entity.WorkspaceId,
                features: entity.Features);

        public long Id => WorkspaceId;
        public long WorkspaceId { get; }
        public IEnumerable<IWorkspaceFeature> Features { get; }
    }
}
