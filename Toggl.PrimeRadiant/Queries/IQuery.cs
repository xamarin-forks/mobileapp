using System.Collections.Generic;

namespace Toggl.PrimeRadiant.Queries
{
    public interface IQuery<T>
    {
        IEnumerable<T> GetAll();
    }
}
