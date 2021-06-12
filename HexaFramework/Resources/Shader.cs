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

        protected static void Write<T>(T t, DeviceManager manager, ID3D11Buffer buffer) where T : unmanaged
        {
            var mapped = manager.ID3D11DeviceContext.Map(buffer, MapMode.WriteDiscard);
            DataStream stream = new(mapped.DataPointer, mapped.DepthPitch * mapped.RowPitch, false, true);
            stream.Write(t);
            stream.Close();
            manager.ID3D11DeviceContext.Unmap(buffer);
        }

        public abstract void Render(SceneObject sceneObject);
    }
}