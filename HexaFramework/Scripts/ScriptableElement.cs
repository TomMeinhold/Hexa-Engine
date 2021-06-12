using HexaFramework.Windows;
using HexaFramework.Input;
using System;
using System.Collections.Generic;
using Vortice.XAudio2;

namespace HexaFramework.Scripts
{
    public class ScriptableElement
    {
        public Script Script { get; private set; }

        public Emitter[] Emitter { get; set; }

        public Dictionary<string, object> Resources = new();

        public void AttachScript<T>() where T : Script
        {
            Script = CreateInstance<T>();
            Script.Instance = this;
        }

        public void AddResource(string name, object data)
        {
            Resources.Add(name, data);
        }

        internal void AttachWindow(RenderWindow window)
        {
            if (Script is null) return;
            Script.Mouse = window.Mouse;
            Script.Keyboard = window.Keyboard;
            Script.Cursor = window.Cursor;
            Script.Time = window.Time;
            Script.Window = window;
        }

        private static T CreateInstance<T>()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }
    }
}