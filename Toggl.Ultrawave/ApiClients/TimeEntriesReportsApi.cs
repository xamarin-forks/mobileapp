﻿using System;
using System.Linq;
using Toggl.Ultrawave.ApiClients.Interfaces;
using Toggl.Ultrawave.Models.Reports;
using Toggl.Ultrawave.Network;
using Toggl.Ultrawave.Serialization;
using Toggl.Ultrawave.Network.Reports;
using Toggl.Multivac;
using Newtonsoft.Json;
using System.Reactive.Linq;
using Toggl.Multivac.Models.Reports;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using Toggl.Multivac.Extensions;

namespace Toggl.Ultrawave.ApiClients
{
    internal sealed class TimeEntriesReportsApi : BaseApi, ITimeEntriesReportsApi
    {
        private readonly TimeEntriesEndpoints endPoints;
        private readonly IJsonSerializer serializer;
        private readonly Credentials credentials;
        private readonly TimeSpan maximumRange = TimeSpan.FromDays(365);

        public TimeEntriesReportsApi(Network.Endpoints endPoints, IApiClient apiClient, IJsonSerializer serializer, Credentials credentials)
            : base(apiClient, serializer, credentials, endPoints.LoggedIn)
        {
            this.endPoints = endPoints.ReportsEndpoints.TimeEntries;
            this.serializer = serializer;
            this.credentials = credentials;
        }

        public IObservable<ITimeEntriesTotals> GetTotals(long workspaceId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            if (endDate.Date - startDate.Date > maximumRange)
                throw new ArgumentOutOfRangeException(nameof(endDate));

            var parameters = new TimeEntriesTotalsParameters(startDate, endDate);
            var json = serializer.Serialize(parameters, SerializationReason.Post, null);

            return Observable.Create<ITimeEntriesTotals>(async observer =>
            {
                var response = await CreateObservable<TotalsResponse>(endPoints.Totals(workspaceId), credentials.Header, json);

                var totals = new TimeEntriesTotals
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    Resolution = response.Resolution,
                    Groups = response.Graph.Select(group => new TimeEntriesTotalsGroup
                    {
                        Total = TimeSpan.FromSeconds(group.Seconds),
                        Billable = TimeSpan.FromSeconds(group.BillableSeconds)
                    }).ToArray<ITimeEntriesTotalsGroup>()
                };

                observer.OnNext(totals);
                observer.OnCompleted();

                return () => { };
            });
        }

        [Preserve(AllMembers = true)]
        private sealed class TimeEntriesTotalsParameters
        {
            public string StartDate { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string EndDate { get; set; }

            public bool WithGraph => true;

            public TimeEntriesTotalsParameters() { }

            public TimeEntriesTotalsParameters(DateTimeOffset startDate, DateTimeOffset? endDate)
            {
                StartDate = startDate.ToString("yyyy-MM-dd");
                EndDate = endDate?.ToString("yyyy-MM-dd");
            }
        }

        [Preserve(AllMembers = true)]
        private sealed class TotalsResponse
        {
            public long Seconds { get; set; }

            public TotalsGraphItem[] Graph { get; set; }

            [JsonConverter(typeof(StringEnumConverter), true)]
            public Resolution Resolution { get; set; }
        }

        [Preserve(AllMembers = true)]
        private sealed class TotalsGraphItem
        {
            public long Seconds { get; set; }

            public Dictionary<string, long> ByRate { get; set; }

            [JsonIgnore]
            public long BillableSeconds => ByRate?.Values.Sum() ?? 0;
        }
    }
}
