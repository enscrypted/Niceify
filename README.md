# Niceify

Niceify is a cross-platform AI tone-adjustment utility for rewriting selected text with a chosen tone using a hotkey.

## Setup

1. Install [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
2. Restore packages: `dotnet restore`
3. Add your OpenRouter or OpenAI credentials to `apiSettings.json`
4. Run the app: `dotnet run`

## Features

- Global hotkey (Enter or configurable)
- Tone selection with prompt override
- Cuts selected text, rewrites via AI, and pastes result
- Cross-platform: Windows, macOS, Linux

## Config

- `appSettings.json`: UI and hotkey settings
- `apiSettings.json`: AI provider config (ignored by git)

## Coming Soon

- Tray icon support
- Better native paste/enter simulation per platform
