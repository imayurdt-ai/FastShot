# ⚡ FastShot — Tauri Screenshot App

> Lightweight, fast, minimal desktop screenshot app. Windows first, Mac + Linux coming soon.

![Tauri](https://img.shields.io/badge/Tauri-2.0-blue) ![Rust](https://img.shields.io/badge/Rust-1.77+-orange) ![TypeScript](https://img.shields.io/badge/TypeScript-5.4-blue) ![Platform](https://img.shields.io/badge/Platform-Windows-0078D4)

## Features
- 📸 Global Hotkey (`Ctrl+Shift+S`) — system-wide
- 🖥️ Full Screen Capture (all monitors)
- 📋 Instant Clipboard Copy
- 💾 Auto File Save `screenshot_YYYYMMDD_HHMMSS.png`
- 🔔 Toast Notifications
- ⚙️ Settings UI (hotkey, folder, toggles)
- 🗂️ System Tray
- 📦 NSIS `.exe` Installer

## Prerequisites
| Tool | Version | Install |
|------|---------|---------|
| Node.js | 20+ | https://nodejs.org |
| Rust | 1.77+ | https://rustup.rs |
| VS C++ Build Tools | Latest | https://aka.ms/buildtools |

## Quick Start
```bash
npm install
npm run tauri dev
```

## Build Installer
```bash
npm run tauri build
# Output: src-tauri/target/release/bundle/nsis/FastShot_1.0.0_x64-setup.exe
```

## Config
`%LOCALAPPDATA%\\FastShot\\settings.json`
```json
{
  "hotkey": "Ctrl+Shift+S",
  "save_path": "C:/Users/You/Pictures/Screenshots",
  "save_to_disk": true,
  "show_toast": true
}
```

## Roadmap
- [ ] Region capture
- [ ] Multi-monitor stitching
- [ ] Annotation tools
- [ ] macOS + Linux
- [ ] Cloud sharing
