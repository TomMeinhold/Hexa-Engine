using HexaFramework.Windows;
using HexaFramework.Windows.Input;
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

        internal void AttachMouseAndKeyboardFromWindow(RenderWindow window)
        {
            AttachMouseAndKeyboard(window.Mouse, window.Keyboard, window.Cursor, window.Time);
        }

        internal void AttachMouseAndKeyboard(Mouse mouse, Keyboard keyboard, Cursor cursor, Time time)
        {
            if (Script is null) return;
            Script.Mouse = mouse;
            Script.Keyboard = keyboard;
            Script.Cursor = cursor;
            Script.Time = time;
        }

        private static T CreateInstance<T>()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }
    }
}