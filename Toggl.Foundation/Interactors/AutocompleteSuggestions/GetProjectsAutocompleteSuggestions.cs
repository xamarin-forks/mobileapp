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
        private readonly IQuery<IEnumerable<IThreadSafeProject>> projectsContainingWordsToQuery;

        public GetProjectsAutocompleteSuggestions(
            IQuery<IEnumerable<IThreadSafeProject>> projectsContainingWordsToQuery)
        {
            Ensure.Argument.IsNotNull(projectsContainingWordsToQuery, nameof(projectsContainingWordsToQuery));

            this.projectsContainingWordsToQuery = projectsContainingWordsToQuery;
        }

        public IObservable<IEnumerable<AutocompleteSuggestion>> Execute()
            => projectsContainingWordsToQuery?.Execute()
                .Select(ProjectSuggestion.FromProject)
                .Apply(Observable.Return);
    }

    internal sealed class OldGetProjectsAutocompleteSuggestions : IInteractor<IObservable<IEnumerable<AutocompleteSuggestion>>>
    {
        private readonly IDataSource<IThreadSafeProject, IDatabaseProject> dataSource;

        private readonly IList<string> wordsToQuery;

        public OldGetProjectsAutocompleteSuggestions(IDataSource<IThreadSafeProject, IDatabaseProject> dataSource, IList<string> wordsToQuery)
        {
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));
            Ensure.Argument.IsNotNull(wordsToQuery, nameof(wordsToQuery));

            this.dataSource = dataSource;
            this.wordsToQuery = wordsToQuery;
        }

        public IObservable<IEnumerable<AutocompleteSuggestion>> Execute()
            => getProjectsForSuggestions().Select(ProjectSuggestion.FromProjects);

        private IObservable<IEnumerable<IThreadSafeProject>> getProjectsForSuggestions()
            => wordsToQuery.Count == 0
                ? getAllProjects()
                : getAllProjectsFiltered();

        private IObservable<IEnumerable<IThreadSafeProject>> getAllProjects()
            => dataSource.GetAll(project => project.Active);

        private IObservable<IEnumerable<IThreadSafeProject>> getAllProjectsFiltered()
            => wordsToQuery.Aggregate(getAllProjects(), (obs, word) => obs.Select(filterProjectsByWord(word)));

        private Func<IEnumerable<IThreadSafeProject>, IEnumerable<IThreadSafeProject>> filterProjectsByWord(string word)
            => projects =>
                projects.Where(
                    p => p.Name.ContainsIgnoringCase(word)
                        || (p.Client != null && p.Client.Name.ContainsIgnoringCase(word))
                        || (p.Tasks != null && p.Tasks.Any(task => task.Name.ContainsIgnoringCase(word))));
    }
}
