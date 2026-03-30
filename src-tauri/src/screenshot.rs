//! screenshot.rs — Captures the screen, copies to clipboard, saves to disk.
use crate::config::AppConfig;
use arboard::Clipboard;
use screenshots::Screen;
use std::{fs, path::PathBuf};

pub fn capture_and_handle(config: &AppConfig) -> Result<(), String> {
    let screens = Screen::all().map_err(|e| format!("Screen list failed: {e}"))?;
    if screens.is_empty() { return Err("No screens found.".into()); }

    let image  = screens[0].capture().map_err(|e| format!("Capture failed: {e}"))?;
    let width  = image.width();
    let height = image.height();
    let rgba   = image.rgba().to_vec();

    let mut clipboard = Clipboard::new().map_err(|e| format!("Clipboard error: {e}"))?;
    clipboard.set_image(arboard::ImageData {
        width:  width  as usize,
        height: height as usize,
        bytes:  std::borrow::Cow::Owned(rgba),
    }).map_err(|e| format!("Clipboard set failed: {e}"))?;

    if config.save_to_disk {
        let folder = PathBuf::from(&config.save_path);
        fs::create_dir_all(&folder).map_err(|e| format!("Cannot create folder: {e}"))?;
        let now      = chrono::Local::now();
        let filename = now.format("screenshot_%Y%m%d_%H%M%S.png").to_string();
        let filepath = folder.join(&filename);
        image.save(&filepath).map_err(|e| format!("Save failed: {e}"))?;
    }
    Ok(())
}
