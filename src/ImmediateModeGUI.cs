using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using UnityEngine;

namespace com.owos02.toi_modbox;

internal static class PluginColors {
    internal static Color red = new(f(237), f(135), f(150));
    internal static Color importantInfoRed = new(f(210), f(15), f(57));
    internal static Color green = new(f(166), f(218), f(149));
    internal static Color linkBlue = new(f(140), f(170), f(238));

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
    private static Rect mainWindow = new(Screen.width - 50 - 300, 250, 300, 600);
    private static Rect itemsWindow = new(Screen.width - 900 - 300, 150, 850, 600);
    private static Rect teleportWindow = new(50, 150, 850, 600);
    private Settings? _settings;
    private bool _foldGeneral;
    private bool _foldPlayer;
    private bool _runOnce = true;
    private bool _unfoldAll = true;

    internal ImmediateModeGUI() {
        Active = false;
    }

    internal bool Active { get; set; }
    private bool _isItemsWindowOpen = false;
    private bool _isTeleportWindowOpen = false;

    internal void Run(ref Settings s) {
        _settings ??= s;
        if (!Active && !s.configAlwaysShowModBox.Value) return;
        Cursor.visible = Active | s.configAlwaysShowModBox.Value;
        Cursor.lockState = CursorLockMode.None;
        mainWindow = GUILayout.Window(0, mainWindow, MainWindow, $"Tails of Iron Modbox v{MyPluginInfo.PLUGIN_VERSION}-alpha");
        if (_isItemsWindowOpen) itemsWindow = GUILayout.Window(1, itemsWindow, ItemsWindow, $"Items");
        if (_isTeleportWindowOpen) teleportWindow = GUILayout.Window(2, teleportWindow, TeleportWindow, $"Teleport");
    }

    private void MainWindow(int windowID) {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(_unfoldAll ? "Show All" : "Close All", GUILayout.ExpandWidth(false)) || _runOnce) {
            _unfoldAll = !_unfoldAll;
            _foldGeneral = !_unfoldAll;
            _foldPlayer = !_unfoldAll;
            _runOnce = false;
        }

        GUI.skin.label.alignment = TextAnchor.MiddleRight;
        if (Settings.saveFile != 5) GUILayout.Label($"Save File: {Settings.saveFile + 1}");
        GUI.skin.label.alignment = default;
        GUILayout.EndHorizontal();
        GeneralOptions();
        PlayerOptions();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Developed by owos02");
        GUI.color = PluginColors.linkBlue;
        GUI.skin.label.alignment = TextAnchor.MiddleRight;
        if (GUILayout.Button("See Project", "label")) System.Diagnostics.Process.Start("https://github.com/owos02/TOI_Modbox");
        GUILayout.EndHorizontal();
        GUI.color = default;
        GUI.skin.label.alignment = default;
        GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }

    /// <summary>
    /// Changing the order of the categories inside the enum will change the display order of buttons in the UI.
    /// </summary>
    internal enum ItemCategory : int {
        Weapon1H,
        Helmet,
        WeaponRanged,
        KeyItem,
        Weapon2H,
        Armour,
        Shield,
    }

    private const int StartCategory = (int)ItemCategory.Weapon1H;
    internal int SelectedCategory = StartCategory;
    internal int PreviousSelectedCategory = StartCategory;
    internal int SelectedItem;
    internal Dictionary<string, string[ ]>? AllItems = null;
    internal bool NamesAreLoaded = false;
    internal bool AddItemEvent;

    private void ItemsWindow(int windowID) {
        if (NamesAreLoaded) {
            if (Settings.saveFile != Settings.NO_SAVE_FILE_SELECTED) {
                GUILayout.Label("Category");
                SelectedCategory = GUILayout.SelectionGrid(SelectedCategory, Enum.GetNames(typeof(ItemCategory)), 4);
                GUILayout.Space(10f);
                GUILayout.Label("Item");
                SelectedItem = GUILayout.SelectionGrid(SelectedItem, AllItems![((ItemCategory)SelectedCategory).ToString()], 5);
                if (SelectedCategory != PreviousSelectedCategory) SelectedItem = 0;
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                var addEquip = Plugin.settings.autoEquipAddedItems.Value ? "Add & Equip" : "Add";
                ButtonToggle("Auto Equip", Plugin.settings.autoEquipAddedItems, "ON", "OFF");
                if (GUILayout.Button((ItemCategory)SelectedCategory == ItemCategory.KeyItem ? "Add" : addEquip, GUILayout.Width(200f))) AddItemEvent = true;
                GUILayout.EndHorizontal();
                GUILayout.Space(5f);
            }
            else
                NoSaveFileSelected();
        }

        if (GUILayout.Button("Close")) _isItemsWindowOpen = false;
        GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
        PreviousSelectedCategory = SelectedCategory;
    }

    internal int SelectedMap;
    internal int SelectedLocation;
    internal MapManager.Maps? CurrentMap = null;
    internal MapManager.LevelSectionLocation? CurrentLocation = null;
    internal bool TeleportToLocationEvent;
    internal string[ ] Maps = Enum.GetNames(typeof(MapManager.Maps));
    internal string[ ] Entrances = Enum.GetNames(typeof(RatVillageLocation));

    private void TeleportWindow(int windowID) {
        if (NamesAreLoaded) {
            if (Settings.saveFile != Settings.NO_SAVE_FILE_SELECTED) {
                // The names of the locations are unintuitive. Will be fixed later.
                // Hopefully with community made up names for areas.
                // Refactoring this and other similar problems with dictionaries would be ideal.
                string MapString = CurrentMap.ToString() ?? string.Empty;
                string LocationString = CurrentLocation.ToString() ?? string.Empty;
                GUILayout.Label($"Current Location: {MapString} - {LocationString}");
                // GUILayout.Label("Maps");
                // SelectedMap = GUILayout.SelectionGrid(SelectedMap, Enum.GetNames(typeof(MapManager.Maps)), 3);
                GUILayout.Label("Locations");
                // SelectedLocation = GUILayout.SelectionGrid(SelectedLocation, AllLocations, 5);
                SelectedLocation = GUILayout.SelectionGrid(SelectedLocation, Entrances, 4);
                GUILayout.Space(10f);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Teleport")) TeleportToLocationEvent = true;
                GUILayout.Space(5f);
            }
            else
                NoSaveFileSelected();
        }

        if (GUILayout.Button("Close")) _isTeleportWindowOpen = false;
        GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
        PreviousSelectedCategory = SelectedCategory;
    }

    #region Options Pages

    private void GeneralOptions() {
        _foldGeneral = GUILayout.Toggle(_foldGeneral, "General Settings", "button");
        if (!_foldGeneral) return;
        ButtonToggle("Always show GUI", Plugin.settings.configAlwaysShowModBox);
        ButtonToggle("Splashscreen Skip", Plugin.settings.configSplashScreenSkip);
        ButtonToggleOnly((!_isItemsWindowOpen ? "Open" : "Close") + " Item Window", ref _isItemsWindowOpen);
        ButtonToggleOnly((!_isTeleportWindowOpen ? "Open" : "Close") + " Teleport Window", ref _isTeleportWindowOpen);
    }

    private void PlayerOptions() {
        _foldPlayer = GUILayout.Toggle(_foldPlayer, "Player Settings", "button");
        if (!_foldPlayer) return;
        ButtonToggle("Infinite HP", Plugin.settings.configInfiniteHealth);
        ButtonToggle("Infinite Health Flask", Plugin.settings.configInfiniteHealthFlask);
        ButtonToggle("Infinite Ammo", Plugin.settings.configInfiniteAmmo);
        CurrenciesInput();
    }

    #endregion

    #region Control Elements

    private void NoSaveFileSelected() {
        GUILayout.FlexibleSpace();
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("No save file selected!");
        GUI.skin.label.alignment = default;
        GUILayout.FlexibleSpace();
    }

    private void ButtonToggle(string buttonName, ConfigEntry<bool> toggle, string activeText = "Active", string inactiveText = "Inactive") {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(buttonName, GUILayout.Width(200f))) {
            toggle.Value = !toggle.Value;
            _settings!.SaveConfig();
        }

        var originalColor = GUI.color;
        GUI.color = toggle.Value ? PluginColors.green : PluginColors.red;
        GUILayout.Label(toggle.Value ? activeText : inactiveText);
        GUI.color = originalColor;
        GUILayout.EndHorizontal();
    }

    private void ButtonToggleOnly(string buttonName, ref bool toggle) {
        if (GUILayout.Button(buttonName, GUILayout.Width(200f))) {
            toggle = !toggle;
            _settings!.SaveConfig();
        }

        var originalColor = GUI.color;
        GUI.color = toggle ? PluginColors.green : PluginColors.red;
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

    private static void NumberInput(string label, string data) {
        GUILayout.BeginHorizontal();
        data = GUILayout.TextField(data, GUILayout.MaxWidth(40f));
        GUILayout.Label(label);
        GUILayout.EndHorizontal();
    }

    private delegate void Method(params object[ ] args);

    private static void StepwiseButton(char symbol, Method onClick, params object[ ] onClickArgs) {
        if (GUILayout.Button(symbol.ToString(), GUILayout.MaxWidth(20f))) {
            onClick.Invoke(onClickArgs);
        }
    }

    #endregion
}