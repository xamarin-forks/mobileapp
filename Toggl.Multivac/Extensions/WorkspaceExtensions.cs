using Toggl.Multivac.Models;

namespace Toggl.Multivac.Extensions
{
    public static class WorkspaceExtensions
    {
        public static bool IsEligibleForProjectCreation(this IWorkspace self)
        {
            return self.Admin || !self.OnlyAdminsMayCreateProjects;
        }
    }
}