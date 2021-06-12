using HexaFramework.Windows;
using HexaFramework.Windows.Input;
using System.Linq;

namespace HexaFramework.Scripts
{
    public abstract class Script
    {
        public virtual void Initialize()
        {
        }

        public virtual void Uninitialize()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void UpdateFixed()
        {
        }

        public Keyboard Keyboard { get; internal set; }

        public Cursor Cursor { get; internal set; }

        public Mouse Mouse { get; internal set; }

        public Time Time { get; internal set; }

        public ScriptableElement Instance { get; internal set; }

        public T GetInstance<T>() where T : ScriptableElement
        {
            if (Instance is T t)
            {
                return t;
            }
            else
            {
                return null;
            }
        }

        public T GetResource<T>() where T : class
        {
            return Instance.Resources.FirstOrDefault(x => x.Value is T).Value as T;
        }

        public T GetResourceByName<T>(string name) where T : class
        {
            return Instance.Resources[name] as T;
        }
    }
}