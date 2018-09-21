using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using Toggl.Multivac;
using Toggl.PrimeRadiant.Models;
using Toggl.PrimeRadiant.Queries;
using Toggl.Multivac.Extensions;

namespace Toggl.PrimeRadiant.Realm.Queries
{
    public class GetAllProjectsContainingQuery : IQuery<IDatabaseProject>
    {
        private readonly Func<Realms.Realm> realm;

        private readonly IEnumerable<string> words;

        public GetAllProjectsContainingQuery(Func<Realms.Realm> realm, IEnumerable<string> words)
        {
            Ensure.Argument.IsNotNull(realm, nameof(realm));
            Ensure.Argument.IsNotNull(words, nameof(words));

            this.realm = realm;
            this.words = words;
        }

        public IEnumerable<IDatabaseProject> GetAll()
            => realm().All<RealmProject>().Where(activeProjectsFilteredByWords);

        private Expression<Func<RealmProject, bool>> activeProjectsFilteredByWords
        {
            get
            {
                var projectExpr = Expression.Parameter(typeof(RealmProject), "project");
                var isActiveExpr = Expression.Property(projectExpr, nameof(RealmProject.Active));
                var projectNameExpr = Expression.Property(projectExpr, nameof(RealmProject.LowerCaseName));
                var clientNameExpr = Expression.Property(projectExpr, nameof(RealmProject.LowerCaseClientName));
                var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
                var isActive = Expression.Equal(isActiveExpr, Expression.Constant(true));

                var wordsExpr = words
                    .Select(word => Expression.Constant(word, typeof(string)))
                    .Select(wordExpr =>
                        Expression.OrElse(
                            Expression.Call(projectNameExpr, containsMethod, wordExpr),
                            Expression.AndAlso(
                                Expression.NotEqual(clientNameExpr, Expression.Constant(null, typeof(string))),
                                Expression.Call(clientNameExpr, containsMethod, wordExpr))))
                    .Aggregate(isActive, Expression.AndAlso);

                return Expression.Lambda<Func<RealmProject, bool>>(wordsExpr, projectExpr);
            }
        }
    }
}
