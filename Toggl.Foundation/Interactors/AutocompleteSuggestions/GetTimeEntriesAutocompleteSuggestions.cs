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
            Console.WriteLine("timeentries  " + wordsToQuery);
        }

        public IObservable<IEnumerable<AutocompleteSuggestion>> Execute()
        {
            return wordsToQuery
                .Select(word => dataSource.GetAll(filter(word))
                .Select(TimeEntrySuggestion.FromTimeEntries))
                .Merge();
            //return wordsToQuery
            //.Aggregate(dataSource.GetAll(), (obs, word) => obs.Select(filterByWord(word)))
            //.Select(TimeEntrySuggestion.FromTimeEntries);
        }

        private Func<IDatabaseTimeEntry, bool> filter(string word)
        {
            Console.WriteLine("filter: " + word);
            return te => te.Description.ContainsIgnoringCase(word)
                        || (te.Project != null && te.Project.Name.ContainsIgnoringCase(word) && te.Project.Active)
                        || (te.Project?.Client != null && te.Project.Client.Name.ContainsIgnoringCase(word))
                           || (te.Task != null && te.Task.Name.ContainsIgnoringCase(word));
        }

        private Func<IEnumerable<IThreadSafeTimeEntry>, IEnumerable<IThreadSafeTimeEntry>> filterByWord(string word)
        {

            return timeEntries =>
                timeEntries.Where(te =>
                {
                Console.WriteLine("filterByWord: " + te.Description + "==" + word);
                    return te.Description.ContainsIgnoringCase(word)
                            || (te.Project != null && te.Project.Name.ContainsIgnoringCase(word) && te.Project.Active)
                            || (te.Project?.Client != null && te.Project.Client.Name.ContainsIgnoringCase(word))
                               || (te.Task != null && te.Task.Name.ContainsIgnoringCase(word));
                });
                    
                    
                    //.Where(
                    //te => te.Description.ContainsIgnoringCase(word)
                        //|| (te.Project != null && te.Project.Name.ContainsIgnoringCase(word) && te.Project.Active)
                        //|| (te.Project?.Client != null && te.Project.Client.Name.ContainsIgnoringCase(word))
                        //|| (te.Task != null && te.Task.Name.ContainsIgnoringCase(word)));
        }
    }
}
