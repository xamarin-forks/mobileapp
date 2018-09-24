using System.Collections.Generic;
using Toggl.PrimeRadiant.Models;

namespace Toggl.PrimeRadiant.Queries
{
    public interface IQueryFactory
    {
        IQuery<IEnumerable<IDatabaseProject>> GetAllProjectsContaining(IEnumerable<string> words);

        IQuery CacheProperties();
    }
}
