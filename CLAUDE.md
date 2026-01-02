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
│   ├── Push/             ← Local notifications
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
| `Runtime/Analytics/` | Analytics manager | To port |
| `Runtime/Time/` | Server time sync | To port |
| `Runtime/Push/` | Local notifications | To port |

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
IN DEVELOPMENT - Network service complete, other services pending

### Completed
- ✅ Network/IWebService + WebService (UnityWebRequest-based REST client)
- ✅ Auth/IAuthService + AuthService (Guest, Facebook, Apple, GooglePlay)

### Pending
- TimeService (server time sync)
- Analytics
- Push notifications
