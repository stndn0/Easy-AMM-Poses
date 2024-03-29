<p align="center">
  <img src="https://i.imgur.com/H6mOAVl.png"/>
</p>

---

![Static Badge](https://img.shields.io/badge/status-active%20development-blue?style=flat&color=%237FFF00)
![Static Badge](https://img.shields.io/badge/platform-windows-blue?style=flat)
![Static Badge](https://img.shields.io/badge/.NET-v8.0%2B-purple)

Photomode to AMM converter for the development of Cyberpunk 2077 mods. 

[Download the latest release.](https://github.com/stndn0/Easy-AMM-Poses/releases)


## What does it do?
A <b> fast and fully automated way of creating Appearance Menu Mod (AMM) pose packs</b>. Simply provide your animation file(s) and my tool will handle the rest. 


<b>Capabilities:</b>
- Automatically reads and extracts all poses from your animation file(s).
- Automatically generates the necessary .workspot, .lua, and .ent files using detected pose data.
- Automatically generates a packed Cyberpunk 2077 mod that you can share with the community.

## Tutorial
#### Requirements
- [.NET framework 8.0](https://dotnet.microsoft.com/en-us/download) to run the application.
- [WolvenKit Console (stable preferred, 8.13+)](https://github.com/WolvenKit/WolvenKit/releases) to be used by the application.

#### Guide
1. Select the path to your WolvenKit Console executable.			
2. Fill out the author, mod name and AMM category. 
3. Load one more more .ANIM files to the corresponding body type.
Optional: To build more advanced packs, click the arrow to reveal more slots.
4. Follow the button prompts: Load -> Build -> Pack.
5. The final packed mod will be saved to the "Packed Folder" in the project directory.

#### Note
If your pose happens to T-Pose in game **this has nothing to do with my tool.** This is an issue with AMM itself and it affects all packs randomly, regardless of whether they're created by hand or automatically via EAP. Simply change your in-game location or restart the game.

## Screenshots
<p align="center">
  <img src="https://i.imgur.com/lUls3ZP.png" height="800"/>
</p>
<p align="center">
  Above: poses successfully loaded from user provided animations. Ready to build.
</p>
<p align="center">
  <img src="https://i.imgur.com/Peio83d.png"/>
</p>
<p align="center">
  Above: Season7 - All Access pose pack built via EAP.
</p>

#### A note for potential contributors
I'm still learning C#, .NET and WPF so excuse some of the mess. Refactoring and code cleanup is a priority for future builds.

## Terms of Use
You are free to modify this software for personal use. If you'd like to help improve it for everyone, i'm happy to collaborate.

Do not redistribute or reupload this software, with or without modification, without my permission. 
