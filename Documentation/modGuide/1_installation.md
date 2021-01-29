# Installation

In this section you will learn how to get all the tools you need setup so that you can start modding. 

## 1. Install Unity
For the tools to work, you have to be on the version of unity that corresponds to your game's modding tools version. You can find this out by opening the game and pressing F1 to open the console window, then typing "+uv".

![Unity Version 2019.4.18f1](../resources/installation/unityversion.png)

Once you find the correct version, download it [here](https://unity3d.com/get-unity/download). This guide will assume you are using Unity Hub.

## 2. Create Project
Next you want to create a Universal Render Pipeline project, which will make sure any models you use will be 1:1 in the game itself. You can create this by clicking the new button in the Projects tab. Name it whatever you like.

![Unity Hub Create Project Window](../resources/installation/unityhub.png)

Once it opens, you can delete all the folders except for Settings, and then create a new scene. You should now have a project that looks like this.

![Unity Empty Project](../resources/installation/unity_initial.png)

## 3. Initializing The Project
In the Modding folder of the game, you should find the InitializerTool. You want to import this into your project. Once imported, you should open the initializer window by going to TUF>Initializer. 

![Unity Initializer Menubar Item](../resources/installation/unity_initializer.png)

Click on each button to install the correct package, and once all of them are installed hit the finalize button. It will ask you for an .exe, so locate your game's executable and select it. Once done you'll have a new folder filled with the necessary dlls for modding.

![Finalize](../resources/installation/unity_finalize.png)

Now that that's done, you are now setup to start creating mods.
