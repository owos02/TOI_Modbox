# Tails of Iron Modbox

A general purpose mod for the game [Tails of Iron (Steam Link)](https://store.steampowered.com/app/1283410/Tails_of_Iron/).

# Requirements

[BepInEx v5.4.23.4 or higher (Github)](https://github.com/BepInEx/BepInEx/releases)

# Install

## Windows

1. Install BepInEx by following the [official installation guide](https://docs.bepinex.dev/articles/user_guide/installation/index.html#installing-bepinex-1).
2. Run the game once to generate the folders needed for the next step.
3. TODO: Drag the mod into the plugins folder under `Tails of Iron/BepInEx/plugins/`


## Linux

> [!IMPORTANT]
> Follow the BepInEx installation for the windows version. The native linux version of the game is broken.

1. Install BepInEx by following the [official installation guide](https://docs.bepinex.dev/articles/user_guide/installation/index.html#installing-bepinex-1).
2. In Steam, under "Game Properties" -> "Compatibility", enable "Force the use of a specific Steam Play compatibility tool" (If you don't know which to choose use `Proton Hotfix`) 
3. Under "Game Properties" -> "General", set the Launch Options to `WINEDLLOVERRIDES="winhttp.dll=n,b" %command%`
4. Run the game once to generate the folders needed for the next step.
5. TODO: Drag the mod into the plugins folder under `Tails of Iron/BepInEx/plugins/`

# Settings

todo!

# Useful links for development

[Unity Docummentation Version 2020.3.x](https://docs.unity3d.com/2020.3/Documentation/Manual/index.html)

[BepInEx](https://docs.bepinex.dev/index.html)

[Harmony](https://harmony.pardeike.net/articles/patching-prefix.html)