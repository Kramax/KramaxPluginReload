//
// This file is part of the KSPPluginReload plugin for Kerbal Space Program, Copyright Joop Selen
// License: http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KramaxPluginReload.Classess
{
    public class PluginSetting
    {
        public string Name;
        public string Path;
        public bool LoadOnce;
        public bool MethodsAllowedToFail;
    }
}
