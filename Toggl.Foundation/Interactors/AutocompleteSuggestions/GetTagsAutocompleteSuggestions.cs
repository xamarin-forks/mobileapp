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
    internal sealed class GetTagsAutocompleteSuggestions : IInteractor<IObservable<IEnumerable<AutocompleteSuggestion>>>
    {
        private readonly ITagsSource dataSource;

        private readonly IEnumerable<string> wordsToQuery;

        public GetTagsAutocompleteSuggestions(ITagsSource dataSource, IEnumerable<string> wordsToQuery)
        {
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));
            Ensure.Argument.IsNotNull(wordsToQuery, nameof(wordsToQuery));

            this.dataSource = dataSource;
            this.wordsToQuery = wordsToQuery;
        }

        public IObservable<IEnumerable<AutocompleteSuggestion>> Execute()
            => wordsToQuery
                .Select(word => dataSource.GetAll(filter(word))
                    .Select(TagSuggestion.FromTags))
                .Merge();

        private Func<IDatabaseTag, bool> filter(string word)
        {
            return t => t.Name.ContainsIgnoringCase(word);
        }
    }
}
