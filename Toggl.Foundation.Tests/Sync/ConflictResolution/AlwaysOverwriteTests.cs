using System;
using Xunit;
using FluentAssertions;
using Toggl.Foundation.Sync.ConflictResolution;
using Toggl.PrimeRadiant;

namespace Toggl.Foundation.Tests.Sync.ConflictResolution
{
    public sealed class AlwaysOverwriteTests
    {
        [Fact, LogIfTooSlow]
        public void UpdateWhenThereIsAnExistingEntityLocally()
        {
            var existingEntity = new TestModel();
            var incomingEntity = new TestModel();

            var mode = resolver.Resolve(existingEntity, incomingEntity);

            mode.Should().Be(ConflictResolutionMode.Update);
        }

        [Fact, LogIfTooSlow]
        public void CreateNewWhenThereIsNoExistingEntity()
        {
            var incomingEntity = new TestModel();

            var mode = resolver.Resolve(null, incomingEntity);

            mode.Should().Be(ConflictResolutionMode.Create);
        }

        private sealed class TestModel
        {
        }

        private AlwaysOverwrite<TestModel, TestModel> resolver { get; }
            = new AlwaysOverwrite<TestModel, TestModel>();
    }
}
