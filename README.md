# Niceify

Niceify is a cross-platform AI tone-adjustment utility for rewriting selected text with a chosen tone using a hotkey.

**Disclaimer:** This application is largely untested, only ran from an IDE, and was made in approximately 30 minutes as a joke after a conversation. User discretion is advised.

## Setup

1.  Install [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
2.  Restore packages: `dotnet restore`
3.  Add your OpenRouter or OpenAI credentials to `apiSettings.json` (see example below)
4.  Run the app: `dotnet run`

## Features

-   Global hotkey (Shift + Ctrl on Windows/Linux, Shift + Cmd on macOS)
-   **Important Note on Usage:** This application simulates `Ctrl/Cmd+A` (select all), `Ctrl/Cmd+X` (cut), processes the text with AI, then `Ctrl/Cmd+V` (paste). Therefore, it requires an active text or chat box (e.g., in Slack or a web browser input field) for proper functionality. Using the hotkey outside of an active text input area may lead to unintended results.
-   Tone selection with prompt override
-   Cuts selected text, rewrites via AI, and pastes result
-   Cross-platform: Windows, macOS, Linux
-   Tray icon support

## Config

-   `appSettings.json`: UI and hotkey settings
-   `apiSettings.json`: AI provider config (ignored by git)
    Example `apiSettings.json`:
    ```json
    {
      "provider": "openrouter",
      "apiKey": "sk-or-v1-YOUR_API_KEY_HERE",
      "model": "meta-llama/llama-3.3-70b-instruct:free"
    }
    ```

## Coming Soon

-   Better native paste/enter simulation per platform