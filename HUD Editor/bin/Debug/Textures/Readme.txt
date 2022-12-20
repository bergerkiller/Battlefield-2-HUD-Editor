Add your custom textures in here.

Here is the "root" folder in the HUD Editor.
So if a texture is needed:
"Ingame\weapons\general\weapon.dds"

It will search for (if no filter is applied):
"%installdir%\Textures\Ingame\weapons\general\weapon.tga"
"%installdir%\Textures\Ingame\weapons\general\weapon.dds"
"%installdir%\*.texlib

It will search for (if a filter is applied):
"%installdir%\%filter%\Textures\Ingame\weapons\general\weapon.tga"
"%installdir%\%filter%\Textures\Ingame\weapons\general\weapon.dds"
"%installdir%\%filter%\*.texlib

Libraries:
The HUD editor loads all texture library (*.texlib) files.
To find a texture, it searches the header from top to bottom. First path match is used.

To generate libraries for your own mod, simply start the HUD editor and click "Texture Library Creator" under "Settings".
The HUD editor will then hide it's window and you'll be able to add textures, load libraries or load entire folders by dragging it onto the main window.

Extra information about the header:
The header of the library is all on top of the texture library. Every texture persists of a number indicating the size of the texture in bytes, and the path.
If one of the bytes is incorrect, it will fail to load all paths behind it. Hex-editing is not recommended.