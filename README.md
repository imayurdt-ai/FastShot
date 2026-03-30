# ⚡ FastShot

> A lightweight, fast, and minimal desktop screenshot application for Windows.

![.NET 8](https://img.shields.io/badge/.NET-8.0-blue) ![WPF](https://img.shields.io/badge/UI-WPF-purple) ![Windows](https://img.shields.io/badge/Platform-Windows-0078D4)

## Features
- 📸 **Global Hotkey** (`Ctrl+Shift+S` by default) — works system-wide
- 🖥️ **Full Screen Capture** — all monitors combined
- 📋 **Instant Clipboard Copy** — paste anywhere immediately
- 💾 **Auto File Save** — `screenshot_YYYYMMDD_HHMMSS.png`
- 🔔 **Toast Notification** — subtle balloon tip
- ⚙️ **Settings UI** — change hotkey, folder, toggles
- 🗂️ **System Tray** — runs silently in background

## Requirements
- Windows 10 / 11
- [.NET 8 SDK](https://dot.net/download)

## Build & Run
```bash
dotnet run
```

## Publish single-file EXE
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
```

## Configuration
Stored at `%LOCALAPPDATA%\FastShot\settings.json`
```json
{
  "Hotkey": "Ctrl+Shift+S",
  "SavePath": "C:/Users/<You>/Pictures/Screenshots",
  "SaveToDisk": true,
  "ShowToast": true
}
```

## Project Structure
```
FastShot/
├── Models/
│   └── Config.cs
├── Services/
│   ├── ClipboardService.cs
│   ├── ConfigService.cs
│   ├── FileStorageService.cs
│   ├── HotkeyParser.cs
│   ├── HotkeyService.cs
│   ├── ScreenshotService.cs
│   └── ToastService.cs
├── Views/
│   ├── SettingsWindow.xaml
│   └── SettingsWindow.xaml.cs
├── App.xaml
├── App.xaml.cs
├── HiddenWindow.cs
├── TrayIconHost.cs
└── FastShot.csproj
```

## Roadmap
- [ ] Region/partial screen capture
- [ ] Annotation tools
- [ ] Multi-monitor mode selector
- [ ] Scroll capture
- [ ] Cloud sharing
