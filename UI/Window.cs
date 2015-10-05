//
// This file is part of the KSPPluginReload plugin for Kerbal Space Program, Copyright Joop Selen
// License: http://creativecommons.org/licenses/by-nc-sa/3.0/
// Part of this file is based of HyperEdit (http://www.kerbaltekaerospace.com/)
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KramaxPluginReload.UI
{
    public class Window
    {
        private static GameObject _gameObject;
        private static GameObject GameObject
        {
            get
            {
                if (_gameObject == null)
                {
                    _gameObject = new GameObject("PluginReloadWindowManager");
                    UnityEngine.Object.DontDestroyOnLoad(_gameObject);
                }
                return _gameObject;
            }
        }

        private static IEnumerable<WindowDrawer> WindowDrawers
        {
            get { return GameObject.GetComponents<WindowDrawer>(); }
        }

        public static void EnsureSingleton<T>(T window) where T : Window
        {
            foreach (var windowDrawer in WindowDrawers.Where(windowDrawer => windowDrawer.Window is T && windowDrawer.Window != window))
                windowDrawer.Window = null;
        }

        public Rect WindowRect { get; set; }
        public string Title { get; set; }
        public List<IWindowContent> Contents { get; set; }
        public Boolean Visible = false;

        public void OnGUI(int windowId)
        {
            GUI.skin = HighLogic.Skin;
            WindowRect = string.IsNullOrEmpty(Title)
                                        ? GUILayout.Window(windowId, WindowRect, DrawWindow, GUIContent.none, WindowOptions)
                                        : GUILayout.Window(windowId, WindowRect, DrawWindow, Title, WindowOptions);
        }

        public void OpenWindow()
        {
            if (WindowDrawers.Any(w => w.Window == this))
                return;
            var windowDrawer = WindowDrawers.FirstOrDefault(w => w.Window == null) ?? GameObject.AddComponent<WindowDrawer>();
            windowDrawer.Window = this;
            Visible = true;
        }

        public void CloseWindow()
        {
            var thisDrawer = WindowDrawers.FirstOrDefault(w => w.Window == this);
            if (thisDrawer != null)
                thisDrawer.Window = null;
            Visible = false;
        }

        private void DrawWindow(int id)
        {
            if (Contents != null)
            {
                GUILayout.BeginVertical();
                foreach (var content in Contents)
                    content.Draw();
                GUILayout.EndVertical();
            }
            GUI.DragWindow();
        }

        public virtual GUILayoutOption[] WindowOptions
        {
            get { return new[] { GUILayout.ExpandHeight(true) }; }
        }
    }

    class WindowDrawer : MonoBehaviour
    {
        public Window Window;
        private int _instanceId = -1;

        public void OnGUI()
        {
            if (_instanceId == -1)
                _instanceId = GetInstanceID();
            if (Window == null)
                return;
            Window.OnGUI(_instanceId);
        }
    }

    public class Selector<T> : Window
    {
        public Selector(string title, IEnumerable<T> elements, Func<T, string> nameSelector, Action<T> onSelect)
        {
            EnsureSingleton(this);
            Title = title;
            WindowRect = new Rect(Screen.width * 3 / 4 - 125, Screen.height / 2 - 200, 250, 400);
            Contents = new List<IWindowContent>
                {
                    new Button("Cancel", CloseWindow)
                };
        }
    }

    public interface IWindowContent
    {
        void Draw();
    }

    public class Button : IWindowContent
    {
        public string Text { get; set; }
        public Action OnClick { get; set; }

        public Button(string text, Action onClick)
        {
            Text = text;
            OnClick = onClick;

        }

        public void Draw()
        {
            if (GUILayout.Button(Text, GUILayout.ExpandWidth(true)))
                OnClick();
        }
    }
}
