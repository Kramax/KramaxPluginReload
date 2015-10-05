//
// This file is part of the KSPPluginReload plugin for Kerbal Space Program, Copyright Joop Selen
// License: http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KramaxPluginReload.Classess;
using UnityEngine;

namespace KramaxPluginReload.Classess
{
    public class PluginMethod : MonoBehaviour
    {
        public KSPAddon.Startup startupType;

        public IEnumerable<PluginClass> PluginClasses
        {
            get
            {
                return PluginReloadModule.PluginClasses.Where(pc => pc.kspAddon.startup == startupType);
            }
        }

        public void OnDestroy()
        {
            foreach (PluginClass p in this.PluginClasses)
            {
                p.alive = false;
                p.DeleteInstance();
            }
        }

        public void Awake()
        {
            foreach (PluginClass p in this.PluginClasses)
            {
                p.CreateInstance();
            }
        }
    }

    [KSPAddon(KSPAddon.Startup.Credits, false)]
    public class Credits : PluginMethod
    {
        public Credits()
        {
            startupType = KSPAddon.Startup.Credits;
        }
    }

    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class EditorAny : PluginMethod
    {
        public EditorAny()
        {
            startupType = KSPAddon.Startup.EditorAny;
        }
    }

    [KSPAddon(KSPAddon.Startup.EditorSPH, false)]
    public class EditorSPH : PluginMethod
    {
        public EditorSPH()
        {
            startupType = KSPAddon.Startup.EditorSPH;
        }
    }

    [KSPAddon(KSPAddon.Startup.EditorVAB, false)]
    public class EditorVAB : PluginMethod
    {
        public EditorVAB()
        {
            startupType = KSPAddon.Startup.EditorVAB;
        }
    }

    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class EveryScene : PluginMethod
    {
        public EveryScene()
        {
            startupType = KSPAddon.Startup.EveryScene;
        }
    }

    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class Flight : PluginMethod
    {
        public Flight()
        {
            startupType = KSPAddon.Startup.Flight;
        }
    }

    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class MainMenu : PluginMethod
    {
        public MainMenu()
        {
            startupType = KSPAddon.Startup.MainMenu;
        }
    }

    [KSPAddon(KSPAddon.Startup.Settings, false)]
    public class PSystemSpawn : PluginMethod
    {
        public PSystemSpawn()
        {
            startupType = KSPAddon.Startup.PSystemSpawn;
        }
    }

    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class SpaceCentre : PluginMethod
    {
        public SpaceCentre()
        {
            startupType = KSPAddon.Startup.SpaceCentre;
        }
    }

    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    public class TrackingStation : PluginMethod
    {
        public TrackingStation()
        {
            startupType = KSPAddon.Startup.TrackingStation;
        }
    }
}
