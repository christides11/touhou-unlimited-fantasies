# Setup

Before you get started modding, there's are a few tools that we need to get.

## Download Tools

The first thing you should get is the modding tools, which come with the game. You can find it [here](https://christides.itch.io/touhou-unlimited-fantasies).

## Install Unity

Next you'll need to download the Specific Unity version used by the toolkit. You can find out which version this is by checking the `info.txt` in the `Modding` folder. 

Once you get that, you should [download Unity Hub here](https://unity3d.com/get-unity/download), and then use it to get the correct version of Unity.

## Creating Project

Now that you have Unity installed, you want to create a new project where you'll be creating mods. 
Using Unity Hub, create a new **Universal Render Pipeline** template project. Name it whatever you like.
Once it opens you can delete all the folders except for the `Settings`, as you won't be needing them.

## Importing Mod.io Package

First we have to import the Mod.io package, which we use to keep track of what mods the user has installed. You can find a link to the proper package in the `info.txt` in the `Modding` folder.

## Finalizing the Setup

Now import the `InitializerTool` package that's in the `Modding` folder. This will handle getting the rest of the files we need.
In Unity, go to `TUF>Initializer` on the top navigation bar. It should open a window like this

The first thing we'll do is install the needed packages by clicking on their buttons one by one. Once you finish that, the `Finalize` button should be available to you. Press it and locate the game's .exe file, and if you did everything right, you should get a `Successfully imported mod tools.` message in the console. Now we're ready to create mods.
