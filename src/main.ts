/**
 * main.ts — FastShot Settings Window
 * Handles UI interactions and communicates with Rust backend via Tauri commands.
 */
import { invoke } from "@tauri-apps/api/core";
import { open   } from "@tauri-apps/plugin-dialog";

interface Config {
  hotkey:       string;
  save_path:    string;
  save_to_disk: boolean;
  show_toast:   boolean;
}

const hotkeyInput   = document.getElementById("hotkey")        as HTMLInputElement;
const savePathInput = document.getElementById("savePath")      as HTMLInputElement;
const saveToDiskChk = document.getElementById("saveToDisk")    as HTMLInputElement;
const showToastChk  = document.getElementById("showToast")     as HTMLInputElement;
const browseBtn     = document.getElementById("browseBtn")     as HTMLButtonElement;
const saveBtn       = document.getElementById("saveBtn")       as HTMLButtonElement;
const screenshotBtn = document.getElementById("screenshotBtn") as HTMLButtonElement;
const statusBar     = document.getElementById("status")        as HTMLDivElement;

function showStatus(message: string, type: "success" | "error") {
  statusBar.textContent = message;
  statusBar.className   = `status ${type}`;
  setTimeout(() => { statusBar.className = "status hidden"; }, 3000);
}

async function loadConfig() {
  try {
    const config = await invoke<Config>("get_config");
    hotkeyInput.value     = config.hotkey;
    savePathInput.value   = config.save_path;
    saveToDiskChk.checked = config.save_to_disk;
    showToastChk.checked  = config.show_toast;
  } catch (err) {
    showStatus(`Failed to load config: ${err}`, "error");
  }
}

browseBtn.addEventListener("click", async () => {
  const selected = await open({ directory: true, multiple: false });
  if (selected && typeof selected === "string") savePathInput.value = selected;
});

saveBtn.addEventListener("click", async () => {
  const config: Config = {
    hotkey:       hotkeyInput.value.trim(),
    save_path:    savePathInput.value.trim(),
    save_to_disk: saveToDiskChk.checked,
    show_toast:   showToastChk.checked,
  };
  if (!config.hotkey) { showStatus("⚠️ Hotkey cannot be empty.", "error"); return; }
  try {
    await invoke("save_config", { config });
    showStatus("✅ Settings saved! Hotkey re-registered.", "success");
  } catch (err) {
    showStatus(`❌ Save failed: ${err}`, "error");
  }
});

screenshotBtn.addEventListener("click", async () => {
  try {
    await invoke("take_screenshot");
    showStatus("✅ Screenshot taken and copied to clipboard!", "success");
  } catch (err) {
    showStatus(`❌ Screenshot failed: ${err}`, "error");
  }
});

loadConfig();
