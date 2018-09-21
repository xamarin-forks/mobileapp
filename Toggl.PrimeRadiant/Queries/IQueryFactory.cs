using System.Collections.Generic;
using Toggl.PrimeRadiant.Models;

namespace Toggl.PrimeRadiant.Queries
{
    public interface IQueryFactory
    {
        IQuery<IDatabaseProject> GetAllProjectsContaining(IEnumerable<string> words);
    }
}
