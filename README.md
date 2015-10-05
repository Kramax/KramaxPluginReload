Kramax Plugin Reload
====================
I started developing an autopilot mode (Kramax Autopilot) and I normally develop software using a very quick
modify/test cycle. I quickly realized that waiting for KSP to restart every time I wanted to try out a change
I had made would make it take so long to develop that I would probably give up first. I looked around for a
solution that would allow a plugin developer to dynamically reload the compiled plugin without restarting KSP.

I found "KSPPluginReload" by m1nd0 (joopselen@gmail.com). Unfortunately I could not get it to work properly
with the way my other mod was structured. The mod used a MonoBehavior sub-class and it seems that there is
a bug in Unity that made it instantiate the old version of my objects even when I loaded a new DLL.
The only way I was able to get it to work was to actually dynamically change the class names of my components
everytime the mod was loaded. This works for a plugin that uses KSPAddon to start and stop. It will NOT work
with a plugin that uses sub-classes of VesselModule to work. Your sub-classes MUST be direct sub-classes
of MonoBehavior.

Basic Instructions
==================
First you need to setup your project. I setup the "Debug" build of the project to use plugin reload and
the "Release" build to NOT use it. This way you can debug with the reloadable mod and release your mod
to the world without it.

To do this you need to first add the assembly "KramaxReloadExtensions" that is in this mod as a referece assembly.
This assembly is used in your debug build to do the DLL reloading.
Next you need to copy the file "ReleaseReloadableMonoBehaviour.cs" into your project. This file is used
in release mode when you want to release your mod to the world.

Sadly in order to make this work you will have to hand edit your "csproj" to make the use
of the KramaxReloadExtensions assembly conditional based on build configuration. You need to find
the line the csproj file that is like this:

    <Reference Include="KramaxReloadExtensions">
      <HintPath>..\..\KramaxPluginReload\bin\Debug\KramaxReloadExtensions.dll</HintPath>
    </Reference>
and change it to this:

    <Reference Condition="'$(Configuration)' == 'Debug'" Include="KramaxReloadExtensions">
      <HintPath>..\..\KramaxPluginReload\bin\Debug\KramaxReloadExtensions.dll</HintPath>
    </Reference>

Using ReloadableMonoBehaviour
-----------------------------
Your sub-classes which inherit from MonoBehavior need to inherit from this mods class ReloadableMonoBehaviour
instead. ReloadableMonoBehaviour is a direct sub-class of MonoBehavior that adds a "type mapping" property
that is used to ensure the correct class types are used for the DLL that is reloaded.

Next, you need to change any calls to GameObject.AddComponent<type> to use the method provided by
ReloadableMonoBehaviour ReloadableMonoBehaviour.AddComponent(type). 

For example, in my autopilot, my main top-level object is a MonoBehavior. It looked like this:

	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class KramaxAutoPilot : MonoBehaviour
    {
        public void Start()
        {
            mainPilot = gameObject.AddComponent<George>();
        }
    	...
	}
This needed to be changed to this:  
 
	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class KramaxAutoPilot : ReloadableMonoBehaviour /* note changed baseclass here */
    {
        public void Start()
        {
            mainPilot = AddComponent<type(George)> as George; /* note AddComponent call and cast */
        }
    	...
	}

If you use any other varient of AddComponent they also need to be changed.

Installation
------------
This mod needs to be installed in your KSP GameData directory as a normal mod would be installed.
Normally it would go into GameData/GramaxPluginReload. The DLL files will be in 
GameData/GramaxPluginReload/Plugins including both KramaxPluginReload.dll and KramaxReloadExtensions.dll.
The Settings.cfg file in GameData/GramaxPluginReload is used to configure where the plugin you are
developing gets loaded from. Here is a sample entry for my autopilot mod:

	PluginSetting
	{
        name = KramaxAutoPilot
        path = C:\root\DevKSP\KramaxAutoPilot\Source\bin\Debug\KramaxAutoPilot.dll
        loadOnce = false
        methodsAllowedToFail = false
	}

You should change the name and path to match your plugin. The other settings should be left alone.
You can have multiple PluginSetting objects in the config file for more than one plugin.
Note the path can be outside of your KSP install directories--in fact using the place where
your compiled DLL gets put works best.

Reloading
---------
The plugin reload mod will create a UI window with a title "Plugins" and one button "Reload plugins".
Simply recompile your mod and press the button. It should load the new version of your DLL.
If you do not actually create a new DLL the reload will not work.

How It Works
------------
When it reloads (or loads the first time) your plugin DLL it does the following:
+ First it goes through all the loaded plugins it is managing and DELETES the
instances of objects created with the old DLL.
+ It loads your (new version) DLL (assembly) by loading the file as "bytes" and then using
Assembly.Load(bytes). It does not load directly from the DLL file or it would not
reload properly.
+ It then creates a "dynamic assembly" that it names using a scheme of "KramaxPIRLAsmb_#"
where # is replaced by a sequential count.
+ It scans all the class types in your mods assembly for sub-classes of MonoBehavior and 
creates a dynamic sub-class of each type with a unique class name such as
"KramaxAutoPilot_43" (it adds the number which increments every time). 
The dynamic sub-class simply creates a unique name for the class
and adds a default constructor.
+ It keeps track of these unique class names in a "type mapping" dictionary. This type
mapping is used to instantiate new components. It never instantiates the original class,
always the unique sub-class.
+ It looks at the KSPAddon attributes of the classes and starts them if they are
supposed to be started based on the current scene.
+ When a ReloadableMonoBehaviour sub-class is instantiated it uses the type mapping
to create the write type of object and propigates that type map to any sub-components.

Example
-------
I know this all sounds pretty confusing. You can look at my autopilot mod for a real-world example.
The source is found at: <https://github.com/Kramax/KramaxAutoPilot>.

Attributions
============
This plugin is a modified version of "KSPPluginReload" by m1nd0 (joopselen@gmail.com).
Many thanks for an excellent starting point for this mod.

License
=======
This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/ or send a letter to
Creative Commons, PO Box 1866, Mountain View, CA 94042, USA.

The file "ReleaseReloadableMonoBehaviour.cs" is excepted from this license and is released into the public domain.


This work is a derivative of "KSPPluginReload" by m1nd0 (joopselen@gmail.com) that was distributed under the same license.
The original work was found at https://github.com/m1ndst0rm/KSPPluginReload.
