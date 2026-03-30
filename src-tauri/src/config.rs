//! config.rs — Loads and saves user preferences from a JSON file.
use serde::{Deserialize, Serialize};
use std::{fs, path::PathBuf};

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct AppConfig {
    pub hotkey:       String,
    pub save_path:    String,
    pub save_to_disk: bool,
    pub show_toast:   bool,
}

impl Default for AppConfig {
    fn default() -> Self {
        let pictures = dirs::picture_dir()
            .unwrap_or_else(|| PathBuf::from("."))
            .join("Screenshots");
        Self {
            hotkey:       "Ctrl+Shift+S".into(),
            save_path:    pictures.to_string_lossy().into_owned(),
            save_to_disk: true,
            show_toast:   true,
        }
    }
}

impl AppConfig {
    fn config_path() -> PathBuf {
        let dir = dirs::config_dir()
            .unwrap_or_else(|| PathBuf::from("."))
            .join("FastShot");
        fs::create_dir_all(&dir).ok();
        dir.join("settings.json")
    }
    pub fn load() -> Self {
        let path = Self::config_path();
        if path.exists() {
            let json = fs::read_to_string(&path).unwrap_or_default();
            serde_json::from_str(&json).unwrap_or_default()
        } else {
            let default = Self::default();
            default.save().ok();
            default
        }
    }
    pub fn save(&self) -> Result<(), String> {
        let path = Self::config_path();
        let json = serde_json::to_string_pretty(self).map_err(|e| e.to_string())?;
        fs::write(path, json).map_err(|e| e.to_string())
    }
}
