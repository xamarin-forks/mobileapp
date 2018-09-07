using System;
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
    internal sealed class GetTimeEntriesAutocompleteSuggestions : IInteractor<IObservable<IEnumerable<AutocompleteSuggestion>>>
    {
        private readonly ITimeEntriesSource dataSource;

        private readonly IList<string> wordsToQuery;

        public GetTimeEntriesAutocompleteSuggestions(ITimeEntriesSource dataSource, IList<string> wordsToQuery)
        {
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));
            Ensure.Argument.IsNotNull(wordsToQuery, nameof(wordsToQuery));

            this.dataSource = dataSource;
            this.wordsToQuery = wordsToQuery;
        }

        public IObservable<IEnumerable<AutocompleteSuggestion>> Execute()
            => wordsToQuery
                .Select(word => dataSource.GetAll(filter(word))
                .Select(TimeEntrySuggestion.FromTimeEntries))
                .Merge();

        private Func<IDatabaseTimeEntry, bool> filter(string word)
        {
            return te => te.Description.ContainsIgnoringCase(word)
                        || (te.Project != null && te.Project.Name.ContainsIgnoringCase(word) && te.Project.Active)
                        || (te.Project?.Client != null && te.Project.Client.Name.ContainsIgnoringCase(word))
                           || (te.Task != null && te.Task.Name.ContainsIgnoringCase(word));
        }
    }
}
