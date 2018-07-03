using System;
using Toggl.Foundation.DTOs;
using Toggl.Foundation.Models.Interfaces;
using Toggl.PrimeRadiant.DTOs;

namespace Toggl.Foundation.DataSources
{
    public interface IUserSource : ISingletonDataSource<IThreadSafeUser, UserDto>
    {
        IObservable<IThreadSafeUser> UpdateWorkspace(long workspaceId);

        IObservable<IThreadSafeUser> Update(EditUserDTO dto);
    }
}
