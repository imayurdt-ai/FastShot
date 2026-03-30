//! tray.rs — System tray icon and right-click context menu.
use tauri::{
    menu::{Menu, MenuItem},
    tray::{MouseButton, MouseButtonState, TrayIconBuilder, TrayIconEvent},
    Manager,
};
use crate::{screenshot::capture_and_handle, AppState};

pub fn setup_tray(app: &mut tauri::App) -> Result<(), Box<dyn std::error::Error>> {
    let screenshot_item = MenuItem::with_id(app, "screenshot", "📸 Take Screenshot", true, None::<&str>)?;
    let settings_item   = MenuItem::with_id(app, "settings",   "⚙️  Settings",        true, None::<&str>)?;
    let sep             = tauri::menu::PredefinedMenuItem::separator(app)?;
    let quit_item       = MenuItem::with_id(app, "quit",       "❌ Exit",             true, None::<&str>)?;
    let menu = Menu::with_items(app, &[&screenshot_item, &settings_item, &sep, &quit_item])?;

    let _tray = TrayIconBuilder::new()
        .menu(&menu)
        .tooltip("FastShot")
        .on_menu_event(|app, event| match event.id.as_ref() {
            "screenshot" => {
                let state: tauri::State<AppState> = app.state();
                let cfg = state.config.lock().unwrap().clone();
                if let Err(e) = capture_and_handle(&cfg) { eprintln!("Screenshot error: {e}"); }
            }
            "settings" => {
                if let Some(win) = app.get_webview_window("main") { win.show().ok(); win.set_focus().ok(); }
            }
            "quit" => { app.exit(0); }
            _ => {}
        })
        .on_tray_icon_event(|tray, event| {
            if let TrayIconEvent::Click { button: MouseButton::Left, button_state: MouseButtonState::Up, .. } = event {
                let app = tray.app_handle();
                if let Some(win) = app.get_webview_window("main") { win.show().ok(); win.set_focus().ok(); }
            }
        })
        .build(app)?;
    Ok(())
}
