using System;
using System.Collections.Generic;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Multivac;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.DTOs;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.Interactors
{
    internal sealed class GetAllNonDeletedInteractor : IInteractor<IObservable<IEnumerable<IThreadSafeTimeEntry>>>
    {
        private readonly IDataSource<IThreadSafeTimeEntry, IDatabaseTimeEntry, TimeEntryDto> dataSource;

        public GetAllNonDeletedInteractor(IDataSource<IThreadSafeTimeEntry, IDatabaseTimeEntry, TimeEntryDto> dataSource)
        {
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));
            
            this.dataSource = dataSource;
        }
        
        public IObservable<IEnumerable<IThreadSafeTimeEntry>> Execute()
            => dataSource.GetAll(te => !te.IsDeleted);
    }
}
