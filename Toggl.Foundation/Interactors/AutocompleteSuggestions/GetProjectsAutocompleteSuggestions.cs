﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Toggl.Foundation.Autocomplete.Suggestions;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Multivac;
using Toggl.Multivac.Extensions;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.Interactors.AutocompleteSuggestions
{
    internal sealed class GetProjectsAutocompleteSuggestions : IInteractor<IObservable<IEnumerable<AutocompleteSuggestion>>>
    {
        private readonly IProjectsSource dataSource;

        private readonly IList<string> wordsToQuery;

        public GetProjectsAutocompleteSuggestions(IProjectsSource dataSource, IList<string> wordsToQuery)
        {
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));
            Ensure.Argument.IsNotNull(wordsToQuery, nameof(wordsToQuery));

            this.dataSource = dataSource;
            this.wordsToQuery = wordsToQuery;
        }

        public IObservable<IEnumerable<AutocompleteSuggestion>> Execute()
            => getProjectsForSuggestions();

        private IObservable<IEnumerable<AutocompleteSuggestion>> getProjectsForSuggestions()
            => wordsToQuery.Count == 0
                ? getAllProjects()
                : getAllProjectsFiltered();

        private IObservable<IEnumerable<AutocompleteSuggestion>> getAllProjects()
            => dataSource.GetAll(project => project.Active).Select(ProjectSuggestion.FromProjects);

        private IObservable<IEnumerable<AutocompleteSuggestion>> getAllProjectsFiltered()
            => wordsToQuery
                .Select(word => dataSource.GetAll(filter(word))
                    .Select(ProjectSuggestion.FromProjects))
                .Merge();

        private Func<IDatabaseProject, bool> filter(string word)
        {
            return p => p.Active
                    || p.Name.ContainsIgnoringCase(word)
                    || (p.Client != null && p.Client.Name.ContainsIgnoringCase(word))
                    || (p.Tasks != null && p.Tasks.Any(task => task.Name.ContainsIgnoringCase(word)));
        }
    }
}
