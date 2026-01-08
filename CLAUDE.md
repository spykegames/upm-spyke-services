# CLAUDE.md - Spyke Services Package

## What This Does
Backend services package providing network communication, authentication, analytics tracking, time synchronization, and push notifications for all Spyke games.

## Package Structure

```
upm-spyke-services/
├── Runtime/
│   ├── Network/          ← WebSocket, REST client, request queue
│   ├── Auth/             ← Guest, Facebook, Apple authentication
│   ├── Analytics/        ← Analytics tracking and events
│   ├── Time/             ← Server time synchronization
│   ├── Push/             ← Push notification service interface
│   ├── Cache/            ← Memory and disk caching
│   ├── Localization/     ← Localization service interface
│   └── Spyke.Services.asmdef
├── Editor/
│   └── Spyke.Services.Editor.asmdef
├── Tests/
│   ├── Runtime/
│   └── Editor/
├── package.json
└── CLAUDE.md
```

## Key Files

| Folder | Purpose | Status |
|--------|---------|--------|
| `Runtime/Network/` | REST client (UnityWebRequest) | ✅ Done |
| `Runtime/Auth/` | Multi-provider authentication | ✅ Done |
| `Runtime/Analytics/` | Provider-based analytics | ✅ Done |
| `Runtime/Time/` | Server time sync | ✅ Done |
| `Runtime/Push/` | Push notification interface | ✅ Done |
| `Runtime/Cache/` | Memory and disk caching | ✅ Done |

## How to Use

### Installation
```json
// Packages/manifest.json
{
  "dependencies": {
    "com.spykegames.services": "https://github.com/spykegames/upm-spyke-services.git#v1.0.0"
  }
}
```

### Basic Usage
```csharp
using Spyke.Services.Network;

// Network - Inject the service
[Inject] private readonly IWebService _web;

// Simple GET request
var response = await _web.SendAsync(new WebRequest("https://api.example.com/users").Get());
var users = JsonUtility.FromJson<UserList>(response.Text);

// POST with JSON body
var response = await _web.SendAsync(
    new WebRequest("users")
        .Post()
        .SetBody("{\"name\":\"John\"}")
        .AddHeader("Authorization", "Bearer token")
);

// With error handling callback
await _web.SendAsync(
    new WebRequest("data").Get(),
    onSuccess: response => Debug.Log(response.Text),
    onError: error => Debug.LogError(error.Message)
);
```

## Dependencies
- com.spykegames.core (required)

## Depends On This
- com.spykegames.sdks
- All game projects

## Source Files to Port

From `client-bootstrap`:
| Source | Destination |
|--------|-------------|
| `SpykeLib/.../Core/Network/` | `Runtime/Network/` |
| `Common/Network/Auth/` | `Runtime/Auth/` |
| `Common/Service/TimeService` | `Runtime/Time/` |
| `CubeBusters/LocalNotifications/` | `Runtime/Push/` |
| `CubeBusters/Common/Analytics/` | `Runtime/Analytics/` |

## Status
✅ **COMPLETE** - All service interfaces implemented: Network, Auth, Analytics, Time, Cache, Push

### Completed
- ✅ Network/IWebService + WebService (UnityWebRequest-based REST client)
- ✅ Auth/IAuthService + AuthService (Guest, Facebook, Apple, GooglePlay)
- ✅ Time/ITimeService + TimeService (server time sync with offset)
- ✅ Analytics/IAnalyticsService + AnalyticsService (provider-based)
- ✅ Cache/IMemoryCache + MemoryCache (generic in-memory cache)
- ✅ Cache/IDiskCache + DiskCache (persistent LRU disk cache with expiry)
- ✅ Push/IPushNotificationService (provider-agnostic push interface)
- ✅ Auth/IFacebookAuthProvider (Facebook SDK interface)
- ✅ Auth/IAppleAuthProvider (Sign In with Apple interface)
- ✅ Localization/ILocalizationService (localization interface)

## Cache Usage

```csharp
using Spyke.Services.Cache;

// In-memory cache
var memCache = new MemoryCache<UserData>();
memCache.Put("user123", userData);
var cached = memCache.Get("user123");

// Disk cache (persistent)
var diskCache = new DiskCache(
    maxFiles: 100,
    directory: "response_cache",
    expiry: TimeSpan.FromDays(7)
);
diskCache.Initialize();

// Sync operations
diskCache.Put("key", Encoding.UTF8.GetBytes(jsonData));
var bytes = diskCache.Get("key");

// Async operations
await diskCache.PutAsync("key", bytes);
var data = await diskCache.GetAsync("key");

// No-op implementations for testing
var noCache = new NoMemoryCache<UserData>();
var noDiskCache = new NoDiskCache();
```

## Push Notification Usage

```csharp
using Spyke.Services.Push;

// Inject the service (implementation provided by upm-spyke-sdks)
[Inject] private readonly IPushNotificationService _push;

// Check permission status
if (_push.Status == PushNotificationStatus.Authorized)
{
    Debug.Log($"Push enabled, token: {_push.PushToken}");
}

// Request permission
var granted = await _push.RequestPermissionAsync();

// Set user ID for targeting
_push.SetUserId("user123");

// Add tags for segmentation
_push.SetTag("level", "42");
_push.SetTag("vip", "true");

// Handle notification clicks
_push.OnNotificationClicked += data =>
{
    if (data.TryGetAdditionalData("OpenLocation", out var location))
    {
        // Navigate to location
    }
};

// No-op implementation for testing/editor
var noPush = new NoPushNotificationService();
```
