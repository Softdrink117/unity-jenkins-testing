# unity-jenkins-testing
Simple Jenkins CI build testing with Unity.

Lately, I've been very interested in automating daily builds for some larger personal projects. So I added a Jenkins setup to my home server to act as a Build Server for Unity CI (Continuous Integration). 

There's a fair bit of documentation about how to do this online, but it was useful to have a dummy project like this to mess with.

Basically, this has two parts:
- a BuildScript.cs that needs to go somewhere inside a Unity project in an Editor folder
- a BuildScriptSettings.ini or .txt that needs to go somewhere insie a Unity project in a Resources folder

They've been packaged together into a handy "BuildScript" folder.


####What Went Wrong
I had some **huge** problems getting this to work. The biggest challenges came when I was trying to enable the BuildScriptSettings read from Resources; I thought for the longest time that I was doing something that simply wasn't possible during a Unity Build. It turns out that the error was caused by a very minor thing - I had a FunctionTesting.cs script extending MonoBehaviour that I was using to test out the methods that would actually load the name and filepath. But this script was using some methods from UnityEditor; it wasn't in an Editor folder. 

Logically, I had assumed that it wouldn't make a difference - that the UnityEditor related bits would be compiled out. But instead, it caused build errors, despite never throwing any warnings during normal build and play testing the way that one would expect.

TL;DR: **always put any script with "using UnityEditor" into an Editor folder, or you can have silent errors during the build process!**
