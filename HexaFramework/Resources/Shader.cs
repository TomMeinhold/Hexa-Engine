using HexaFramework.Scenes;
using HexaFramework.Windows;
using HexaFramework.Windows.Native;
using System.IO;
using System.Text;
using Vortice;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;

namespace HexaFramework.Resources
{
    public abstract class Shader : Resource
    {
#pragma warning disable CA1822 // Member als statisch markieren

        protected void CompileShader(DeviceManager manager, string shaderPath, string entry, string version, out Blob blob)
        {
            _ = Compiler.CompileFromFile(new FileInfo(shaderPath).FullName, entry, version, out blob, out var error);
            if (error is not null)
            {
                var result = manager.Window.ShowMessageBox($"Shader: {version}",
                    Encoding.UTF8.GetString(error.GetBytes()),
                    MessageBoxButtons.OkCancel,
                    MessageBoxIcon.Warning);
                if (result == DialogResult.Cancel)
                    Application.Exit();
            }
        }

        protected void Write<T>(DeviceManager manager, ID3D11Buffer buffer, T t) where T : unmanaged
        {
            var mapped = manager.ID3D11DeviceContext.Map(buffer, MapMode.WriteDiscard);
            UnsafeUtilities.Write(mapped.DataPointer, ref t);
            manager.ID3D11DeviceContext.Unmap(buffer);
        }

        protected void Write<T>(DeviceManager manager, ID3D11Buffer buffer, T[] t) where T : unmanaged
        {
            var mapped = manager.ID3D11DeviceContext.Map(buffer, MapMode.WriteDiscard);
            UnsafeUtilities.Write(mapped.DataPointer, t);
            manager.ID3D11DeviceContext.Unmap(buffer);
        }

#pragma warning restore CA1822 // Member als statisch markieren

        public abstract void Render(SceneObject sceneObject);
    }
}