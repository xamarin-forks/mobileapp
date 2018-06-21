
namespace Toggl.PrimeRadiant.Realm
{
    internal interface IUpdatesFrom<TDto>
    {
        void SetPropertiesFrom(TDto entity, Realms.Realm realm);
    }
}
