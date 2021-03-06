Pull sync loop
==============

The pull sync loop is responsible for querying the server and merging the obtained entities into the local database.

Limiting the queries with 'since' parameters
--------------------------------------------

To limit the amount of data downloaded from the server, we use the URL variants which include the `since` parameter. Backend accepts since dates approximately _3 months_ into the past - we don't use dates older than _two months_, just to be sure.

If a `since` date is not available in the database or it is outdated, we fetch all the entities and update our `since` date in the database.

We calculate the `since` date by selecting the latest `ILastChangeDatable.At` value among all the pulled entities  (_note: this might yield a since date which is older than two months if the user didn't edit any of the entities (e.g., projects) for a long period of time so we will ignore it (we don't use since dates older than two months, remember?) and fetch all of the entities even the next time - there is currently nothing we could do about it, using the device's current time is risky because it might be incorrect and we might skip fetching some data if the device was ahead of the server_).

The order of persisting entities
--------------------------------

To make sure that the all the dependencies are already persisted, we process them in a given order:

1. Workspaces
2. User _(depends on workspaces)_
3. Workspaces' features _(depends on workspaces)_
4. Preferences
5. Tags  _(depends on workspaces)_
6. Clients  _(depends on workspaces)_
7. Projects _(depends on workspaces and clients)_
8. Tasks _(depends on workspaces, projects and user)_
9. Time entries _(depends on workspaces, projects, tasks, tags, users)_

Conflict resolution
-------------------

We use conflict resolution and rivals resolution to avoid any data inconsistencies in our database - namely to prevent having two running time entries at one time in the database. There is a [dedicated chapter](conflict-resolution.md) which describes the conflict resolution algorithms.

Ghost entities
--------------

Time entries keep references to the projects even after the projects were _archived_ (and possibly in other scenarios where user loses access to a project), this means that we cannot rely on the projects being in the database even if we respect the order of persisting the entities by types.

To prevent the app from crashing and to increase the UX of the app, we create "ghost entities" 👻 for projects which are referenced by time entries but are not in the database instead of ignoring them or removing these references. We then later try to fetch the details of these projects using the reports API. This way we will be able to update our local data when user gains back the access to a project or if the project becomes active again.

Pruning old data
----------------

After all data is pulled and persisted, we remove unnecessary data as part of the pull-sync loop.

- We remove any _time entry_ which was started more than _two months_ ago.
- We remove any _ghost project_ which is not referenced by any time entry in the database.

_Note: we might move the pruning to a separate loop in the future._

Retry loop
----------

When a `ServerErrorException` or `ClientErrorException` other than `ApiDeprecatedException`, `ClientDeprecatedException` or `UnauthorizedException` is thrown during the processing of the HTTP request, the retry loop kicks in.

The retry loop checks what the `/status` endpoint of the API server returns:
- `200 OK` - exit the retry loop
- `500 Internal server error` - wait for the next "slow delay" and try again
    - slow delay starts with _60 seconds_ and then it is calculated using this formula: `previousDelay * rand(1.5, 2)`
- _otherwise_ wait for the next "fast delay" and try again
    - fast delay starts with _10 seconds_ and then it is calculated using this formula: `previousDelay * rand(1, 1.5)`


Where everything is implemented in the code
-------------------------------------------

The code is located (mostly) under the namespace `Toggl.Foundation.Sync.States.Pull`.

We initiate the HTTP requests in the state class `FetchAllSinceState`.

Individual states are instances of the `PersistSingletonState` (user, preferences) and `PersistListState` (the rest).

This basic logic is then wrapped in `SinceDateUpdatingPersistState` for the entities for which we store the `since` date. All states are wrapped with `ApiExceptionCatchingPersistState` which catches known exceptions and leads into the retry loop.

The logic of creating project ghosts is implemented in the `CreateGhostProjectsState` and fetching the details of these projects using the reports API is done in `TryFetchInaccessibleProjectsState`.

Retry loop uses the `CheckServerStatusState` and the `ResetAPIDelayState`.

The states are instantiated and connected in the `Toggl.Foundation.TogglSyncManagerFactory` class.

---

Previous topic: [Conflict resolution](conflict-resolution.md)
Next topic: [Push sync](push-sync.md)
Go to the [Index](index.md)
