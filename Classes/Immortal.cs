//
// This file is part of the KSPPluginReload plugin for Kerbal Space Program, Copyright Joop Selen
// License: http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KramaxPluginReload.Classe
{
    public static class Immortal
    {
        private static GameObject _gameObject;
        public static T AddImmortal<T>() where T : Component
        {
            if (_gameObject == null)
            {
                _gameObject = new GameObject("PluginReloadModuleImmortal", typeof(T));
                UnityEngine.Object.DontDestroyOnLoad(_gameObject);
            }
            return _gameObject.GetComponent<T>() ?? _gameObject.AddComponent<T>();
        }

    }
}
