using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Multivac;
using Toggl.Multivac.Models;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.Sync.States.Pull
{
    internal sealed class PersistListState<TInterface, TDatabaseInterface, TThreadsafeInterface, TDto>
        : IPersistState
        where TDatabaseInterface : TInterface, IDatabaseModel
        where TThreadsafeInterface : TDatabaseInterface, IThreadSafeModel
    {
        private readonly IDataSource<TThreadsafeInterface, TDatabaseInterface, TDto> dataSource;

        private readonly Func<TInterface, TDto> clean;

        public StateResult<IFetchObservables> FinishedPersisting { get; } = new StateResult<IFetchObservables>();

        public PersistListState(
            IDataSource<TThreadsafeInterface, TDatabaseInterface, TDto> dataSource,
            Func<TInterface, TDto> clean)
        {
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));
            Ensure.Argument.IsNotNull(clean, nameof(clean));

            this.dataSource = dataSource;
            this.clean = clean;
        }

        public IObservable<ITransition> Start(IFetchObservables fetch)
            => fetch.GetList<TInterface>()
                .SingleAsync()
                .Select(toDtos)
                .SelectMany(dataSource.BatchUpdate)
                .Select(_ => FinishedPersisting.Transition(fetch));

        private IList<TDto> toDtos(IEnumerable<TInterface> entities)
            => entities?.Select(clean).ToList() ?? new List<TDto>();
    }
}
