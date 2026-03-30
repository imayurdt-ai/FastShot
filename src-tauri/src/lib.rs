//! FastShot — Tauri application entry point
mod config;
mod screenshot;
mod tray;

use config::AppConfig;
use screenshot::capture_and_handle;
use std::sync::Mutex;
use tauri::{Manager, State};
use tauri_plugin_global_shortcut::{GlobalShortcutExt, ShortcutState};

pub struct AppState {
    pub config: Mutex<AppConfig>,
}

#[tauri::command]
fn get_config(state: State<AppState>) -> AppConfig {
    state.config.lock().unwrap().clone()
}

#[tauri::command]
fn save_config(config: AppConfig, state: State<AppState>, app: tauri::AppHandle) -> Result<(), String> {
    config.save().map_err(|e| e.to_string())?;
    *state.config.lock().unwrap() = config.clone();
    register_hotkey(&app, &config.hotkey)?;
    Ok(())
}

#[tauri::command]
fn take_screenshot(state: State<AppState>) -> Result<(), String> {
    let cfg = state.config.lock().unwrap().clone();
    capture_and_handle(&cfg)
}

fn register_hotkey(app: &tauri::AppHandle, hotkey: &str) -> Result<(), String> {
    let manager = app.global_shortcut();
    manager.unregister_all().map_err(|e| e.to_string())?;
    let app_handle = app.clone();
    manager.on_shortcut(hotkey, move |_app, _shortcut, event| {
        if event.state == ShortcutState::Pressed {
            let state: State<AppState> = app_handle.state();
            let cfg = state.config.lock().unwrap().clone();
            let _ = capture_and_handle(&cfg);
        }
    }).map_err(|e| e.to_string())?;
    Ok(())
}

pub fn run() {
    let config = AppConfig::load();
    let hotkey = config.hotkey.clone();
    tauri::Builder::default()
        .plugin(tauri_plugin_shell::init())
        .plugin(tauri_plugin_dialog::init())
        .plugin(tauri_plugin_notification::init())
        .plugin(tauri_plugin_global_shortcut::Builder::new().build())
        .manage(AppState { config: Mutex::new(config) })
        .invoke_handler(tauri::generate_handler![get_config, save_config, take_screenshot])
        .setup(move |app| {
            tray::setup_tray(app)?;
            if let Err(e) = register_hotkey(&app.handle(), &hotkey) {
                eprintln!("Hotkey registration failed: {e}");
            }
            if let Some(window) = app.get_webview_window("main") {
                window.hide().ok();
            }
            Ok(())
        })
        .run(tauri::generate_context!())
        .expect("error while running FastShot");
}
