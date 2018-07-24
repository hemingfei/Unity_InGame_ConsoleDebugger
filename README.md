# Debugger-For-Unity
debugger at runtime

How To Use

Just drag the prefab into the hierachy, or add the script named "Debugger" in any GameObject.

Custom Setting
In inspector of the "Debugger" Script, "Drag I You Want" concluds the log images, GuiSkin and MaskCanvs.
The log image will show at the small icon. Skin will chagned all the gui skin of the debugger. The Mask Canvas is used after click the icon and main window shows, it decides the click will work only in the debugger or both debugger and your game.
The "Settings" in inspector of "Debugger" Script let you changed some parameters of each window.

Invoke Build-In Game Methods
Just add your functions in the script named "Debugger.Debug.CustomMethods". And the most important is adding some of the below attributes above your functions.
[DebuggerButtonDebug(string descrition, int priority = 0)]
[DebuggerSelectDebug(string descrition, int priority = 0)]
[DebuggerCodeDebug(string customCode, string descrition, int priority = 0)]
Also, this class is a partial class of "Debugger" script which you draged into inspector before, so it is a monobehaviour. You can use awake, start and whatever Unity Engion Methods just make sure it is not duplicated defined because it is a partial class.
