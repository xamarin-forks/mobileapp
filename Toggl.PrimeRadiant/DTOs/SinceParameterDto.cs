using System;
using Toggl.PrimeRadiant.Models;

namespace Toggl.PrimeRadiant.DTOs
{
    public struct SinceParameterDto : IDatabaseSinceParameter
    {
        public long Id { get; }
        public DateTimeOffset? Since { get; }

        public SinceParameterDto(long id, DateTimeOffset? since)
        {
            Id = id;
            Since = since;
        }
    }
}
