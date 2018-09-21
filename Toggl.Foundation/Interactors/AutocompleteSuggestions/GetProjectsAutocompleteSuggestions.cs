using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Toggl.Foundation.Autocomplete.Suggestions;
using Toggl.Foundation.DataSources.Interfaces;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Multivac;
using Toggl.Multivac.Extensions;
using Toggl.PrimeRadiant.Models;
using Toggl.PrimeRadiant.Queries;

namespace Toggl.Foundation.Interactors.AutocompleteSuggestions
{
    internal sealed class GetProjectsAutocompleteSuggestions : IInteractor<IObservable<IEnumerable<AutocompleteSuggestion>>>
    {
        private readonly IDataSource<IThreadSafeProject, IDatabaseProject> dataSource;

        private readonly IQuery<IThreadSafeProject> projectsContainingWordsToQuery;

        public GetProjectsAutocompleteSuggestions(
            IDataSource<IThreadSafeProject, IDatabaseProject> dataSource,
            IQuery<IThreadSafeProject> projectsContainingWordsToQuery)
        {
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));

            this.dataSource = dataSource;
            this.projectsContainingWordsToQuery = projectsContainingWordsToQuery;
        }

        public IObservable<IEnumerable<AutocompleteSuggestion>> Execute()
            => getFilteredProjectsForSuggestions()
               ?? getAllProjects().Select(ProjectSuggestion.FromProjects);

        private IObservable<IEnumerable<ProjectSuggestion>> getFilteredProjectsForSuggestions()
            => projectsContainingWordsToQuery?.GetAll()
                .Select(ProjectSuggestion.FromProject)
                .Apply(Observable.Return);

        private IObservable<IEnumerable<IThreadSafeProject>> getAllProjects()
            => dataSource.GetAll(project => project.Active);
    }
}
