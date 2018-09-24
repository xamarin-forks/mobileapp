namespace Toggl.PrimeRadiant.Queries
{
    public interface IQuery<T>
    {
        T Execute();
    }

    public interface IQuery
    {
        void Execute();
    }
}
