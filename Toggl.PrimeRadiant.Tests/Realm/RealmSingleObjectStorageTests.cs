using Toggl.PrimeRadiant.Realm;

namespace Toggl.PrimeRadiant.Tests.Realm
{
    public sealed class RealmSingleObjectStorageTests : SingleObjectStorageTests<TestModel>
    {
        protected override ISingleObjectStorage<TestModel, TestModel> Storage { get; } 
            = new SingleObjectStorage<TestModel, TestModel>(new TestAdapter(_ => __ => true));

        protected override TestModel GetModelWith(int id) => new TestModel { Id = id };
    }
}
