# About
This is an (archived) project called Battlefield 2 HUD Editor. This is a GUI tool I (bergerkiller) wrote back in 2010-2011 to edit HUD (heads up display) modules for Battlefield 2/2142.

![HUD Editor](editor_screenshot.jpg?raw=true "Battlefield 2 HUD Editor")

## Usage
### Installation
You can find the binary you can run under:
<pre>HUD Editor/bin/debug</pre>

The FreeImage.dll installed by default is for 64bit machines. If you run 32bit, rename FreeImage.dll to FreeImage64.dll and rename FreeImage32.dll to FreeImage.dll.

### Custom Textures
Textures are archived in ".texlib" files, stored inside sub-folders inside the "Textures" folder. These are selectable under Settings -> Texture Filter. Here you can also find a creator tool to create new ones. It can also read raw files inside the sub-folder, so you can put your "Ingame" root folder inside such a sub-folder.

### Loading
You can load/save HUD .con files under File.

### Menu Layout
Horizontally at the top are various tools/settings that can be used with the currently selected element. To the left vertically is a pop-up bar where all the HUD elements are listed. Here you can also make it render a specific GUI, as by default it shows all GUIs at the same time.

## Contributing
While not in active development, if people have pull requests for patches I will happily accept them. Do shoot me a message on discord as I probably won't notice such things appearing here

- Discord: bergerkiller#1742
- or join Refractor 2 Modding discord: https://discord.gg/b2ru8j3r8y

## Disclaimer
Back then I was just starting out learning programming in general, so code quality is pretty dreadful. I might supply a few patches here and there.

## License
MIT License. Includes binaries for FreeImage (used for reading .dll/.tga files), for which I claim no ownership rights.