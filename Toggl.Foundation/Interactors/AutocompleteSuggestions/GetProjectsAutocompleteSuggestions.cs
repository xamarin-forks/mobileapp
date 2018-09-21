using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using Toggl.Foundation.Autocomplete.Suggestions;
using Toggl.Foundation.DataSources.Interfaces;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Multivac;
using Toggl.Multivac.Extensions;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.Interactors.AutocompleteSuggestions
{
    internal sealed class GetProjectsAutocompleteSuggestions : IInteractor<IObservable<IEnumerable<AutocompleteSuggestion>>>
    {
        private readonly IDataSource<IThreadSafeProject, IDatabaseProject> dataSource;

        private readonly IList<string> wordsToQuery;

        public GetProjectsAutocompleteSuggestions(IDataSource<IThreadSafeProject, IDatabaseProject> dataSource, IList<string> wordsToQuery)
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
            => dataSource.GetAll(activeProjectsFilterByWords(wordsToQuery));

        private Expression<Func<IDatabaseProject, bool>> activeProjectsFilterByWords(IEnumerable<string> words)
        {
            var projectExpr = Expression.Parameter(typeof(IDatabaseProject), "project");
            var projectNameExpr = Expression.Property(projectExpr, nameof(IDatabaseProject.LowerCaseName));
            var clientNameExpr = Expression.Property(projectExpr, nameof(IDatabaseProject.LowerCaseClientName));
            var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
            Expression alwaysTrue = Expression.Constant(true, typeof(bool));

            var wordsExpr = words
                .Select(word => Expression.Constant(projectNameExpr, typeof(string)))
                .Select(wordExpr =>
                    Expression.OrElse(
                        Expression.Call(projectNameExpr, containsMethod, wordExpr),
                        Expression.Call(clientNameExpr, containsMethod, wordExpr)))
                .Aggregate(alwaysTrue, Expression.AndAlso);

            return Expression.Lambda<Func<IDatabaseProject, bool>>(wordsExpr, projectExpr);
        }
    }
}
