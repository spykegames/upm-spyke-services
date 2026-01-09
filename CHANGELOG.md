# Changelog

All notable changes to this project will be documented in this file.

## [1.0.1] - 2025-01-09

### Changed
- Updated CLAUDE.md with PORT-CHECK verification results
- Documented architecture decision: Redesigned vs Direct Port
- Listed files that belong in other packages (upm-spyke-extras-websocket)

### Verified
- Network: 6 files, complete (UnityWebRequest-based REST client)
- Auth: 12 files, complete (multi-provider support)
- Time: 2 files, complete
- Analytics: 5 files, complete (provider-based)
- Cache: 6 files, complete (memory + disk with expiry)
- Push: 4 files, complete (interface + data)
- Localization: 2 files, complete (interface + no-op)
- Crashlytics: 2 files, complete (interface + no-op)
- RemoteConfig: 2 files, complete (interface + no-op)

## [1.0.0] - 2025-01-08

### Added
- Full service implementation (41 files)
- Network/IWebService + WebService (UnityWebRequest)
- Auth/IAuthService + AuthService (Guest, Facebook, Apple, GooglePlay)
- Time/ITimeService + TimeService (server time sync)
- Analytics/IAnalyticsService + AnalyticsService (provider-based)
- Cache/IMemoryCache + IDiskCache implementations
- Push/IPushNotificationService (interface + no-op)
- Localization/ILocalizationService (interface + no-op)
- Crashlytics/ICrashlyticsService (interface + no-op)
- RemoteConfig/IRemoteConfigService (interface + no-op)

## [0.0.1-preview] - 2024-12-24

### Added
- Initial package structure
- CLAUDE.md documentation
- Assembly definitions for Runtime, Editor, Tests
