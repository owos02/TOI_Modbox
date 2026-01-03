using BepInEx.Configuration;
using UnityEngine;

namespace com.owos02.toi_modbox;

public class ImmediateModeGUI {
    private Settings? _settings;
    private bool isGUIactive;
    private static Rect window = new Rect(50, 50, 300, 200);
    internal bool active {
        get => isGUIactive;
        set => isGUIactive = value;
    }

    internal ImmediateModeGUI( ) {
        isGUIactive = false;
    }
    
    internal void Run(ref Settings s) {
        if (_settings == null)
            _settings = s;
        if (!isGUIactive && !s.configAlwaysShowModBox.Value)
            return;
        
        var color = Color.black;
        color.a = 1.0f;
        GUI.backgroundColor = color;
        GUI.color = Color.white;
        window = GUILayout.Window(0, window, MainWindow, "Tails of Iron Modbox");
    }
    
    private void GeneralOptions() {
        GUILayout.Label("General Settings");
        ButtonToggle("Always show GUI", Plugin.settings.configAlwaysShowModBox);
        ButtonToggle("Splashscreen Skip", Plugin.settings.configSplashScreenSkip);
        
    }
    
    private void PlayerOptions() {
        GUILayout.Label("Player Settings");
        ButtonToggle("Infinite HP", Plugin.settings.configInfiniteHealth);
        ButtonToggle("Infinite Health Flask", Plugin.settings.configInfiniteHealthFlask);
    }

    private void MainWindow(int windowID) {
        GeneralOptions();
        PlayerOptions();
        GUILayout.Label("Developed by owos02(github.com/owos02)");
        GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }

    #region Control Elements

    void ButtonToggle(string ButtonName, ConfigEntry<bool> toggle) {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(ButtonName)) {
            toggle.Value = !toggle.Value;
            _settings.SaveConfig();
        }

        var originalColor = GUI.color;
        if (toggle.Value)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;
        GUILayout.Label(toggle.Value ? "Active" : "Inactive");
        GUI.color = originalColor;
        GUILayout.EndHorizontal();
    }

    #endregion
}