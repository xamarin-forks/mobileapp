using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using NSubstitute;
using Toggl.Foundation.Suggestions;
using Toggl.Foundation.Tests.Generators;
using Toggl.Foundation.Tests.Mocks;
using Toggl.Multivac.Models;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;
using Xunit;
using TimeEntry = Toggl.Foundation.Models.TimeEntry;

namespace Toggl.Foundation.Tests.Suggestions
{
    public sealed class MostUsedTimeEntrySuggestionProviderTests
    {
        public abstract class MostUsedTimeEntrySuggestionProviderTest
        {
            protected const int NumberOfSuggestions = 7;

            protected MostUsedTimeEntrySuggestionProvider Provider { get; }
            protected ITimeService TimeService { get; } = Substitute.For<ITimeService>();
            protected ITogglDatabase Database { get; } = Substitute.For<ITogglDatabase>();

            protected DateTimeOffset Now { get; } = new DateTimeOffset(2017, 03, 24, 12, 34, 56, TimeSpan.Zero);

            protected MostUsedTimeEntrySuggestionProviderTest()
            {
                Provider = new MostUsedTimeEntrySuggestionProvider(Database, TimeService, NumberOfSuggestions);

                TimeService.CurrentDateTime.Returns(_ => Now);
            }
        }

        public sealed class TheConstructor : MostUsedTimeEntrySuggestionProviderTest
        {
            [Theory, LogIfTooSlow]
            [ClassData(typeof(TwoParameterConstructorTestData))]
            public void ThrowsIfAnyOfTheArgumentsIsNull(bool useDatabase, bool useTimeService)
            {
                var database = useDatabase ? Database : null;
                var timeService = useTimeService ? TimeService : null;

                Action tryingToConstructWithEmptyParameters =
                    () => new MostUsedTimeEntrySuggestionProvider(database, timeService, NumberOfSuggestions);

                tryingToConstructWithEmptyParameters
                    .Should().Throw<ArgumentNullException>();
            }
        }

        public sealed class TheGetSuggestionsMethod : MostUsedTimeEntrySuggestionProviderTest
        {
            private IEnumerable<IDatabaseTimeEntry> getTimeEntries(params int[] numberOfRepetitions)
                => Enumerable.Range(0, numberOfRepetitions.Length)
                    .SelectMany(index => Enumerable
                        .Range(0, numberOfRepetitions[index])
                        .Select(i =>
                            new MockTimeEntry
                            {
                                Id = 21,
                                UserId = 10,
                                WorkspaceId = 12,
                                At = Now,
                                Start = Now,
                                Description = $"te{index}"
                            }));

            [Fact, LogIfTooSlow]
            public async Task ReturnsEmptyObservableIfThereAreNoTimeEntries()
            {
                Database.TimeEntries
                        .GetAll(Arg.Any<Func<IDatabaseTimeEntry, bool>>())
                        .Returns(Observable.Empty<IEnumerable<IDatabaseTimeEntry>>());

                var suggestions = await Provider.GetSuggestions().ToList();

                suggestions.Should().HaveCount(0);
            }

            [Property(StartSize = 1, EndSize = 10, MaxTest = 10)]
            public void ReturnsUpToNSuggestionsWhereNIsTheNumberUsedWhenConstructingTheProvider(
                NonNegativeInt numberOfSuggestions)
            {
                var provider = new MostUsedTimeEntrySuggestionProvider(Database, TimeService, numberOfSuggestions.Get);

                var timeEntries = getTimeEntries(2, 2, 2, 3, 3, 4, 5, 5, 6, 6, 7, 7, 7, 8, 8, 9);

                Database.TimeEntries
                        .GetAll(Arg.Any<Func<IDatabaseTimeEntry, bool>>())
                        .Returns(Observable.Return(timeEntries));

                var suggestions = provider.GetSuggestions().ToList().Wait();

                suggestions.Should().HaveCount(numberOfSuggestions.Get);
            }

            [Fact, LogIfTooSlow]
            public async Task SortsTheSuggestionsByUsage()
            {
                var timeEntries = getTimeEntries(5, 3, 2, 5, 4, 4, 5, 4, 3).ToArray();
                var expectedDescriptions = new[] { 0, 3, 6, 4, 5, 7, 1 }.Select(i => $"te{i}");

                Database.TimeEntries
                        .GetAll(Arg.Any<Func<IDatabaseTimeEntry, bool>>())
                        .Returns(Observable.Return(timeEntries));

                var suggestions = await Provider.GetSuggestions().ToList();

                suggestions.Should().OnlyContain(suggestion => expectedDescriptions.Contains(suggestion.Description));
            }

            [Fact, LogIfTooSlow]
            public async Task DoesNotReturnTimeEntriesWithoutDescription()
            {
                var emptyTimeEntries = Enumerable.Range(20, 0)
                    .Select(_ => new MockTimeEntry
                    {
                        Id = 12,
                        UserId = 9,
                        WorkspaceId = 2,
                        At = Now,
                        Start = Now,
                        Description = ""
                    });
                var timeEntries = new List<IDatabaseTimeEntry>(emptyTimeEntries);
                timeEntries.AddRange(getTimeEntries(1, 2, 3, 4, 5));
                Database.TimeEntries
                        .GetAll(Arg.Any<Func<IDatabaseTimeEntry, bool>>())
                        .Returns(Observable.Return(timeEntries));

                var suggestions = await Provider.GetSuggestions().ToList();

                suggestions.Should().OnlyContain(
                    suggestion => !string.IsNullOrEmpty(suggestion.Description)
                );
            }
        }
    }
}
