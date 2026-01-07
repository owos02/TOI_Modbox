using System;
using System.Linq;
using BepInEx.Configuration;
using UnityEngine;

namespace com.owos02.toi_modbox;

internal static class PluginColors {
    internal static Color red = new(f(237), f(135), f(150));
    internal static Color importantInfoRed = new(f(210), f(15), f(57));
    internal static Color green = new(f(166), f(218), f(149));

    /// <summary>
    ///     To Float
    ///     Converts the color from 0-255 to 0f-1f
    ///     Name is shortened for shortening
    /// </summary>
    /// <param name="strength"> 0-255 </param>
    /// <returns></returns>
    private static float f(int strength) {
        if (strength < 0) return 0f;
        if (strength > 255) return 1f;
        return strength / 255f;
    }
}
#nullable enable
public class ImmediateModeGUI {
    private static Rect mainWindow = new(Screen.width - 50 - 300, 150, 300, 600);
    private static Rect itemsWindow = new(Screen.width - 900 - 300, 150, 850, 600);
    private Settings? _settings;
    private bool foldGeneral;
    private bool foldPlayer;
    private bool runOnce = true;
    private bool unfoldAll = true;

    internal ImmediateModeGUI() {
        active = false;
    }

    internal bool active { get; set; }
    private bool isItemsWindowOpen = false;

    internal void Run(ref Settings s) {
        _settings ??= s;
        if (!active && !s.configAlwaysShowModBox.Value) return;
        mainWindow = GUILayout.Window(0, mainWindow, MainWindow, $"Tails of Iron Modbox v{MyPluginInfo.PLUGIN_VERSION}");
        if (isItemsWindowOpen) itemsWindow = GUILayout.Window(1, itemsWindow, ItemsWindow, $"Items");
    }

    private void MainWindow(int windowID) {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(unfoldAll ? "Show All" : "Close All", GUILayout.ExpandWidth(false)) || runOnce) {
            unfoldAll = !unfoldAll;
            foldGeneral = !unfoldAll;
            foldPlayer = !unfoldAll;
            runOnce = false;
        }

        GUI.skin.label.alignment = TextAnchor.MiddleRight;
        if (Settings.saveFile != 5) GUILayout.Label($"Save File: {Settings.saveFile + 1}");
        GUI.skin.label.alignment = default;
        GUILayout.EndHorizontal();
        GeneralOptions();
        PlayerOptions();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Developed by owos02");
        GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }

    internal enum SelectedCategory : int {
        KeyItem,
        Weapon1H,
        Weapon2H,
        WeaponRanged,
        Shield,
        Helmet,
        Armour
    }

    internal int selectedCategory = 1;
    internal int selectedItem = 0;
    internal string[ ][ ] allItems;
    internal string[ ] categoryNames = Enum.GetValues(typeof(SelectedCategory)).Cast<SelectedCategory>().Select((category) => category.ToString()).ToArray();
    internal bool namesAreLoaded = false;
    internal int previousSelected = 1;
    internal bool addItemEvent = false;

    private void ItemsWindow(int windowID) {
        if (namesAreLoaded) {
            if (Settings.saveFile != Settings.NO_SAVE_FILE_SELECTED) {
                GUILayout.Label("Category");
                selectedCategory = GUILayout.SelectionGrid(selectedCategory, categoryNames, 4);
                GUILayout.Space(10f);
                if (selectedCategory == (int)SelectedCategory.KeyItem) {
                    GUILayout.FlexibleSpace();
                    GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                    GUI.color = PluginColors.importantInfoRed;
                    GUILayout.Label("Key Items not implemented!");
                    GUI.color = default;
                    GUI.skin.label.alignment = default;
                    GUILayout.FlexibleSpace();
                }

                GUILayout.Label("Item");
                selectedItem = GUILayout.SelectionGrid(selectedItem, allItems[selectedCategory], 5);
                if (selectedCategory != previousSelected) selectedItem = 0;
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Equip")) addItemEvent = true;
                GUILayout.Space(5f);
            }
            else {
                GUILayout.FlexibleSpace();
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                GUILayout.Label("No save file selected!");
                GUI.skin.label.alignment = default;
                GUILayout.FlexibleSpace();
            }
        }

        if (GUILayout.Button("Close")) isItemsWindowOpen = false;
        GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
        previousSelected = selectedCategory;
    }

    #region Options Pages

    private void GeneralOptions() {
        foldGeneral = GUILayout.Toggle(foldGeneral, "General Settings", "button");
        if (!foldGeneral) return;
        ButtonToggle("Always show GUI", Plugin.settings.configAlwaysShowModBox);
        ButtonToggle("Splashscreen Skip", Plugin.settings.configSplashScreenSkip);
        ButtonToggleOnly((!isItemsWindowOpen ? "Open" : "Close") + " Item Window", ref isItemsWindowOpen);
    }

    private void PlayerOptions() {
        foldPlayer = GUILayout.Toggle(foldPlayer, "Player Settings", "button");
        if (!foldPlayer) return;
        ButtonToggle("Infinite HP", Plugin.settings.configInfiniteHealth);
        ButtonToggle("Infinite Health Flask", Plugin.settings.configInfiniteHealthFlask);
        ButtonToggle("Infinite Ammo", Plugin.settings.configInfiniteAmmo);
        CurrenciesInput();
    }

    #endregion

    #region Control Elements

    private void ButtonToggle(string ButtonName, ConfigEntry<bool> toggle) {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(ButtonName, GUILayout.Width(200f))) {
            toggle.Value = !toggle.Value;
            _settings!.SaveConfig();
        }

        var originalColor = GUI.color;
        if (toggle.Value)
            GUI.color = PluginColors.green;
        else
            GUI.color = PluginColors.red;
        GUILayout.Label(toggle.Value ? "Active" : "Inactive");
        GUI.color = originalColor;
        GUILayout.EndHorizontal();
    }

    private void ButtonToggleOnly(string ButtonName, ref bool toggle) {
        if (GUILayout.Button(ButtonName, GUILayout.Width(200f))) {
            toggle = !toggle;
            _settings!.SaveConfig();
        }

        var originalColor = GUI.color;
        if (toggle)
            GUI.color = PluginColors.green;
        else
            GUI.color = PluginColors.red;
        GUI.color = originalColor;
    }

    private void CurrenciesInput() {
        GUI.skin.textField.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Currencies");
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        NumberInput("Gold", Settings.dataGold);
        StepwiseButton('+', (_) => Settings.incrementCoin = 1);
        StepwiseButton('-', (_) => Settings.incrementCoin = -1);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        NumberInput("Iron", Settings.dataIron);
        StepwiseButton('+', (_) => Settings.incrementIron = 1);
        StepwiseButton('-', (_) => Settings.incrementIron = -1);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        NumberInput("Monster Parts", Settings.dataMonsterParts);
        StepwiseButton('+', (_) => Settings.incrementMonster = 1);
        StepwiseButton('-', (_) => Settings.incrementMonster = -1);
        GUILayout.EndHorizontal();
        GUI.skin.textField.alignment = default;
        GUILayout.EndVertical();
    }

    private void NumberInput(string label, string data) {
        GUILayout.BeginHorizontal();
        data = GUILayout.TextField(data, GUILayout.MaxWidth(40f));
        GUILayout.Label(label);
        GUILayout.EndHorizontal();
    }

    internal delegate void Method(params object[ ] args);

    private void StepwiseButton(char symbol, Method onClick, params object[ ] onClickArgs) {
        if (GUILayout.Button(symbol.ToString(), GUILayout.MaxWidth(20f))) {
            onClick.Invoke(onClickArgs);
        }
    }

    #endregion
}