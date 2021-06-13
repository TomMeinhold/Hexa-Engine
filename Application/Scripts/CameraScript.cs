using HexaFramework.Input;
using HexaFramework.Input.Events;
using HexaFramework.Scenes;
using HexaFramework.Scripts;
using PhysX;
using System.Numerics;

namespace App.Scripts
{
    public class CameraScript : Script
    {
        public float Speed = 20F;
        public float AngluarSpeed = 0.5F;

        public override void Initialize()
        {
            Window.MouseMove += Window_MouseMove;
            Window.KeyUp += Window_KeyUp;
        }

        private void Window_KeyUp(object sender, KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.L:
                    Cursor.Lock(!Cursor.IsLocked);
                    break;

                case Keys.N:
                    Window.DeviceManager.SwitchWireframe(false);
                    break;

                case Keys.M:
                    Window.DeviceManager.SwitchWireframe(true);
                    break;
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var camera = GetComponent<Camera>();
            if (Mouse.IsDown(MouseButton.LButton))
            {
                if (Mouse.Delta.X != 0)
                {
                    camera.AdjustRotation(new Vector3(0, Mouse.Delta.X * AngluarSpeed, 0));
                }
                if (Mouse.Delta.Y != 0)
                {
                    camera.AdjustRotation(new Vector3(Mouse.Delta.Y * AngluarSpeed, 0, 0));
                }
            }
        }

        public override void Update()
        {
            var camera = GetComponent<Camera>();
            Window.Scene.Raycast(camera.Position, camera.Forward, 1000, 2,
                hits =>
            {
                foreach (var hit in hits)
                {
                    if (hit.Actor.Name == "Cube")
                    {
                        if (Mouse.IsDown(MouseButton.MButton))
                        {
                            if (!Keyboard.IsDown(Keys.F))
                                (hit.Actor as RigidDynamic).AddForce(camera.Forward * 100, ForceMode.Impulse, true);
                            else
                                (hit.Actor as RigidDynamic).AddForce(camera.Backward * 100, ForceMode.Impulse, true);
                        }

                        return true;
                    }
                }
                return false;
            });
            if (Keyboard.IsDown(Keys.W))
            {
                camera.AdjustPosition(camera.Forward * Speed * Time.Delta);
            }
            if (Keyboard.IsDown(Keys.S))
            {
                camera.AdjustPosition(camera.Backward * Speed * Time.Delta);
            }
            if (Keyboard.IsDown(Keys.A))
            {
                camera.AdjustPosition(camera.Left * Speed * Time.Delta);
            }
            if (Keyboard.IsDown(Keys.D))
            {
                camera.AdjustPosition(camera.Right * Speed * Time.Delta);
            }
            if (Keyboard.IsDown(Keys.Space))
            {
                camera.AdjustPosition(camera.Up * Speed * Time.Delta);
            }
            if (Keyboard.IsDown(Keys.C))
            {
                camera.AdjustPosition(camera.Down * Speed * Time.Delta);
            }
        }
    }
}