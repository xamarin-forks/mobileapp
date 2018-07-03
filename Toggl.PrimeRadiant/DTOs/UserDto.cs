using System;
using Toggl.Multivac;
using Toggl.Multivac.Models;

namespace Toggl.PrimeRadiant.DTOs
{
    public struct UserDto : IUser, IDatabaseSyncable
    {
        private UserDto(
            long id,
            string apiToken,
            DateTimeOffset at,
            long? defaultWorkspaceId,
            Email email,
            string fullname,
            BeginningOfWeek beginningOfWeek,
            string language,
            string imageUrl,
            SyncStatus syncStatus,
            bool isDeleted,
            string lastSyncErrorMessage)
        {
            Id = id;
            ApiToken = apiToken;
            At = at;
            DefaultWorkspaceId = defaultWorkspaceId;
            Email = email;
            Fullname = fullname;
            BeginningOfWeek = beginningOfWeek;
            Language = language;
            ImageUrl = imageUrl;
            SyncStatus = syncStatus;
            IsDeleted = isDeleted;
            LastSyncErrorMessage = lastSyncErrorMessage;
        }

        public static UserDto From<T>(
            T entity,
            New<long> id = default(New<long>),
            New<string> apiToken = default(New<string>),
            New<DateTimeOffset> at = default(New<DateTimeOffset>),
            New<long?> defaultWorkspaceId = default(New<long?>),
            New<Email> email = default(New<Email>),
            New<string> fullname = default(New<string>),
            New<BeginningOfWeek> beginningOfWeek = default(New<BeginningOfWeek>),
            New<string> language = default(New<string>),
            New<string> imageUrl = default(New<string>),
            New<SyncStatus> syncStatus = default(New<SyncStatus>),
            New<bool> isDeleted = default(New<bool>),
            New<string> lastSyncErrorMessage = default(New<string>))
            where T : IUser, IDatabaseSyncable
        => new UserDto(
            id: id.ValueOr(entity.Id),
            apiToken: apiToken.ValueOr(entity.ApiToken),
            at: at.ValueOr(entity.At),
            defaultWorkspaceId: defaultWorkspaceId.ValueOr(entity.DefaultWorkspaceId),
            email: email.ValueOr(entity.Email),
            fullname: fullname.ValueOr(entity.Fullname),
            beginningOfWeek: beginningOfWeek.ValueOr(entity.BeginningOfWeek),
            language: language.ValueOr(entity.Language),
            imageUrl: imageUrl.ValueOr(entity.ImageUrl),
            syncStatus: syncStatus.ValueOr(entity.SyncStatus),
            isDeleted: isDeleted.ValueOr(entity.IsDeleted),
            lastSyncErrorMessage: lastSyncErrorMessage.ValueOr(entity.LastSyncErrorMessage));

        public static UserDto Clean(IUser entity)
            => createFrom(entity, SyncStatus.InSync);

        public static UserDto Unsyncable(IUser entity, string errorMessage)
            => createFrom(entity, SyncStatus.SyncFailed, lastSyncErrorMessage: errorMessage);

        private static UserDto createFrom(
            IUser entity,
            SyncStatus syncStatus,
            bool isDeleted = false,
            string lastSyncErrorMessage = null)
            => new UserDto(
                id: entity.Id,
                apiToken: entity.ApiToken,
                at: entity.At,
                defaultWorkspaceId: entity.DefaultWorkspaceId,
                email: entity.Email,
                fullname: entity.Fullname,
                beginningOfWeek: entity.BeginningOfWeek,
                language: entity.Language,
                imageUrl: entity.ImageUrl,
                syncStatus: syncStatus,
                isDeleted: isDeleted,
                lastSyncErrorMessage: lastSyncErrorMessage);

        public long Id { get; }
        public string ApiToken { get; }
        public DateTimeOffset At { get; }
        public long? DefaultWorkspaceId { get; }
        public Email Email { get; }
        public string Fullname { get; }
        public BeginningOfWeek BeginningOfWeek { get; }
        public string Language { get; }
        public string ImageUrl { get; }
        public SyncStatus SyncStatus { get; }
        public bool IsDeleted { get; }
        public string LastSyncErrorMessage { get; }
    }
}
