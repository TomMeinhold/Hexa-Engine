using HexaFramework.Input;
using HexaFramework.Resources;
using HexaFramework.Scenes;
using HexaFramework.Scripts;
using System.Numerics;

namespace App.Scripts
{
    public class CameraScript : Script
    {
        private bool state;

        public float Speed = 20F;
        public float AngluarSpeed = 2F;
        private const float RadToDeg = 57.2958F;

        public override void Update()
        {
            var camera = GetComponent<Camera>();

            if (!state & Keyboard.IsDown(Keys.L))
            {
                state = true;
                GetResource<Sound>().Play(1);
                //Cursor.Lock(!Cursor.IsLocked);
            }
            if (!Keyboard.IsDown(Keys.L))
                state = false;

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

            if (Keyboard.IsDown(Keys.N))
            {
                camera.Manager.SwitchWireframe(false);
            }
            if (Keyboard.IsDown(Keys.M))
            {
                camera.Manager.SwitchWireframe(true);
            }

            if (Mouse.Buttons[MouseButton.LButton] == MouseButtonState.Pressed)
            {
                if (Mouse.Delta.X != 0)
                {
                    camera.AdjustRotation(new Vector3(0, Mouse.Delta.X * AngluarSpeed * Time.Delta * RadToDeg, 0));
                }
                if (Mouse.Delta.Y != 0)
                {
                    camera.AdjustRotation(new Vector3(Mouse.Delta.Y * AngluarSpeed * Time.Delta * RadToDeg, 0, 0));
                }
                Mouse.ClearDelta();
            }
        }
    }
}