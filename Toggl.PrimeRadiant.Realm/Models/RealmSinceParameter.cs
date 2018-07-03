using System;
using Realms;
using Toggl.PrimeRadiant.DTOs;
using Toggl.PrimeRadiant.Models;

namespace Toggl.PrimeRadiant.Realm.Models
{
    public sealed class RealmSinceParameter : RealmObject, IDatabaseSinceParameter, IUpdatesFrom<SinceParameterDto>
    {
        [PrimaryKey]
        public long Id { get; set; }

        public DateTimeOffset? Since { get; set; }

        public RealmSinceParameter() { }

        public RealmSinceParameter(SinceParameterDto entity)
        {
            SetPropertiesFrom(entity, null);
        }

        public void SetPropertiesFrom(SinceParameterDto entity, Realms.Realm realm)
        {
            Id = entity.Id;
            Since = entity.Since;
        }
    }
}
