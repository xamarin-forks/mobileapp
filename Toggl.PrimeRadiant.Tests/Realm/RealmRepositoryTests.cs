using Toggl.PrimeRadiant.Realm;

namespace Toggl.PrimeRadiant.Tests.Realm
{
    public sealed class RealmRepositoryTests : RepositoryTests<TestModel>
    {
        protected override IRepository<TestModel, TestModel> Repository { get; } = new Repository<TestModel, TestModel>(new TestAdapter());

        protected override TestModel GetModelWith(int id) => new TestModel { Id = id };
    }
}
