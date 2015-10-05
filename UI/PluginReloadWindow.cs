//
// This file is part of the KSPPluginReload plugin for Kerbal Space Program, Copyright Joop Selen
// License: http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KramaxPluginReload.UI
{
    public class PluginReloadWindow : Window
    {
        public override GUILayoutOption[] WindowOptions
        {
            get
            {
                return new[] { GUILayout.MinWidth(100) };
            }
        }

        public Callback ReloadCallback;

        public PluginReloadWindow()
        {
            EnsureSingleton(this);
            Title = "Plugins";
            WindowRect = new Rect(50, 50, 10, 10);
            Contents = new List<IWindowContent>
                {
                    new Button("Reload plugins", ReloadPlugins),
                };
        }

        private void ReloadPlugins()
        {
            ReloadCallback.Invoke();
        }
    }
}
