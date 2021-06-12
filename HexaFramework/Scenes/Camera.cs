﻿using HexaFramework.Extensions;
using HexaFramework.Scripts;
using HexaFramework.Windows;
using System;
using System.Numerics;
using Vortice.XAudio2;

namespace HexaFramework.Scenes
{
    public class Camera : ScriptableElement
    {
        private const float DegToRadFactor = 0.0174532925f;
        private float fov;
        private float near = .0001f;
        private float far = 100f;
        private float positionX;
        private float positionY;
        private float positionZ;
        private float rotationX;
        private float rotationY;
        private float rotationZ;

        public Camera(DeviceManager manager)
        {
            Manager = manager;
            Manager.AspectRatioChanged += (_, _) => UpdateProjection();
        }

        public float PositionX { get => positionX; set { positionX = value; } }

        public float PositionY { get => positionY; set { positionY = value; } }

        public float PositionZ { get => positionZ; set { positionZ = value; } }

        public float RotationX { get => rotationX; set { rotationX = value; } }

        public float RotationY { get => rotationY; set { rotationY = value; } }

        public float RotationZ { get => rotationZ; set { rotationZ = value; } }

        public Vector3 Forward { get; private set; }

        public Vector3 Backward { get; private set; }

        public Vector3 Left { get; private set; }

        public Vector3 Right { get; private set; }

        public Vector3 Up { get; private set; }

        public Vector3 Down { get; private set; }

        public Vector3 Position { get; private set; }

        public Matrix4x4 ViewMatrix { get; private set; }

        public Matrix4x4 ProjectionMatrix { get; private set; }

        public float Fov { get => fov; set { fov = value; UpdateProjection(); } }

        public float NearPlane { get => near; set { near = value; UpdateProjection(); } }

        public float FarPlane { get => far; set { far = value; UpdateProjection(); } }

        public DeviceManager Manager { get; }

        public Listener Listener { get; set; }

        public void UpdateProjection()
        {
            //ProjectionMatrix = OrthoLH(1280, 720, near, far);
            ProjectionMatrix = MatrixExtensions.PerspectiveFovLH(fov * DegToRadFactor, Manager.AspectRatio, near, far);
            //ProjectionMatrix = Matrix4x4.Identity;
        }

        public void UpdateView()
        {
            Position = new(PositionX, PositionY, PositionZ);
            float pitch = RotationX * DegToRadFactor;
            float yaw = RotationY * DegToRadFactor;
            float roll = RotationZ * DegToRadFactor;
            Matrix4x4 rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(yaw, pitch, roll);
            Vector3 up = Vector3.Transform(Vector3.UnitY, rotationMatrix);
            Vector3 target = Vector3.Add(Vector3.Transform(Vector3.UnitZ, rotationMatrix), Position);
            ViewMatrix = MatrixExtensions.LookAtLH(Position, target, up);

            Matrix4x4 vecRotationMatrix = Matrix4x4.CreateFromYawPitchRoll(yaw, pitch, 0f);
            Forward = Vector3.Transform(Vector3.UnitZ, vecRotationMatrix);
            Backward = Vector3.Transform(-Vector3.UnitZ, vecRotationMatrix);
            Right = Vector3.Transform(Vector3.UnitX, vecRotationMatrix);
            Left = Vector3.Transform(-Vector3.UnitX, vecRotationMatrix);
            Up = Vector3.Transform(Vector3.UnitY, vecRotationMatrix);
            Down = Vector3.Transform(-Vector3.UnitY, vecRotationMatrix);
        }

        public void AdjustPosition(Vector3 vector)
        {
            PositionX += vector.X;
            PositionY += vector.Y;
            PositionZ += vector.Z;
        }

        public void AdjustRotation(Vector3 vector)
        {
            RotationX = NormalizeEulerAngle(RotationX += vector.X);
            RotationY = NormalizeEulerAngle(RotationY += vector.Y);
            RotationZ = NormalizeEulerAngle(RotationZ += vector.Z);
        }

        public static float NormalizeEulerAngle(float angle)
        {
            var normalized = angle % 360;
            if (normalized < 0)
                normalized += 360;
            return normalized;
        }
    }
}