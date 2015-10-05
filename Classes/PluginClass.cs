//
// This file is part of the KSPPluginReload plugin for Kerbal Space Program, Copyright Joop Selen
// License: http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace KramaxPluginReload.Classess
{
    public class PluginClass
    {
        public Type type; // dynamically created subclass of ReloadableMonoBehavior
        public Type originalType; // static subclass of ReloadableMonoBehavior from assembly
        public Dictionary<Type, Type> typeMapping = new Dictionary<Type, Type>();
        public KSPAddon kspAddon;
        public GameObject go = null;
        public KramaxReloadExtensions.ReloadableMonoBehaviour instance = null;
        public bool fired = false;
        public bool alive = false;
        public PluginSetting pluginSetting;

        public String Name { get { return type.Name; } }
     
        public void CreateInstance()
        {
            if (go != null || instance != null)
            {
                Debug.LogError(String.Format("KramaxPluginReload.PluginClass.CreateInstance object {0} already alive.", Name));
                return;
            }

            Debug.Log(String.Format("KramaxPluginReload.PluginClass.CreateInstance create object {0}.", Name));

            go = new GameObject(type.Name);
            instance = go.AddComponent(type) as KramaxReloadExtensions.ReloadableMonoBehaviour;
            instance.typeMapping = typeMapping;
            
            fired = true;
            alive = true;
        }

        public void DeleteInstance()
        {
            if (go != null)
            {
                Debug.Log(String.Format("KramaxPluginReload.PluginClass.DeleteInstance {0} being destroyed.", Name));
            
                UnityEngine.GameObject.DestroyImmediate(go);
                go = null;
                instance = null;
                fired = false;
                alive = false;
            }
            else
            {
                Debug.Log(String.Format("KramaxPluginReload.PluginClass.DeleteInstance object {0} is not alive.", Name));
            }
        }
    }
}
