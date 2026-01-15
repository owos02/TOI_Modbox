# Tails of Iron Modbox

A general purpose mod for the game [Tails of Iron (store.steampowered.com)](https://store.steampowered.com/app/1283410/Tails_of_Iron/).

# Requirements

[BepInEx v5.4.23.4 or higher (github.com)](https://github.com/BepInEx/BepInEx/releases)

# Install

## Windows

1. Install BepInEx by following the [official installation guide (docs.bepinex.dev)](https://docs.bepinex.dev/articles/user_guide/installation/index.html#installing-bepinex-1).
2. Run the game once to generate the folders needed for the next step.
3. Download the mod from the [releases page (github.com)](https://github.com/owos02/TOI_Modbox/releases).
4. Extract the zip file and drag the folder into the `Tails of Iron/BepInEx/plugins/` directory.

## Linux

> [!IMPORTANT]
> Follow the BepInEx installation for the Windows version and run the game under proton. The native Linux version of the game is broken.

1. Install BepInEx by following the [official installation guide (docs.bepinex.dev)](https://docs.bepinex.dev/articles/user_guide/installation/index.html#installing-bepinex-1).
2. In Steam, under "Game Properties" -> "Compatibility", enable "Force the use of a specific Steam Play compatibility tool" (If you don't know which to choose use `Proton Hotfix`) 
3. Under "Game Properties" -> "General", set the Launch Options to `WINEDLLOVERRIDES="winhttp.dll=n,b" %command%`
4. Run the game once to generate the folders needed for the next step.
5. Download the mod from the [releases page (github.com)](https://github.com/owos02/TOI_Modbox/releases).
6. Extract the zip file and drag the folder into the `Tails of Iron/BepInEx/plugins/` directory.

## Post Installation - Opening the Menu

The mod window will open, when you go into the settings. To keep it on the screen toggle `Always show GUI`.

# Features

|                Option | Info                                      |
|----------------------:|:------------------------------------------|
|    Splash Screen Skip | Skips the start-up screen                 |
|            Currencies | Add or remove Gold, Iron or Monsterparts  |
|           Infinite HP |                                           |
| Infinite Health Flask | Disable flask consumtion                  |
|         Infinite Ammo |                                           |
|             Add Items | Add any item in the game (some are buggy) |
|  Teleport to Location | Experimental                              |

# Useful links for development

[Unity Docummentation Version 2020.3.x (docs.unity3d.com)](https://docs.unity3d.com/2020.3/Documentation/Manual/index.html)

[BepInEx (docs.bepinex.dev)](https://docs.bepinex.dev/index.html)

[Harmony (harmony.pardeike.net)](https://harmony.pardeike.net/articles/patching-prefix.html)