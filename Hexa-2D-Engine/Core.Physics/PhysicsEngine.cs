using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Physics.Gravity;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Physics.Rays;
using HexaEngine.Core.Physics.Collision;
using SharpDX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using HexaEngine.Core.Physics.Components;
using HexaEngine.Core.Objects.Interfaces;

namespace HexaEngine.Core.Physics
{
    public partial class PhysicsEngine : IDisposable
    {
        public PhysicsEngine(Engine engine)
        {
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            Thread = new Thread(ThreadVoid);
            Thread.Start();
        }

        ~PhysicsEngine()
        {
            Dispose(disposing: false);
        }

        public Engine Engine { get; }

        public Thread Thread { get; }

        public bool IsDisposed { get; private set; }

        public bool IsDisposing { get; private set; }

        public TimeSpan ThreadTiming { get; set; }

        private void ThreadVoid()
        {
            Stopwatch stopwatch = new Stopwatch();
            while (!IsDisposing)
            {
                ThreadTiming = new TimeSpan(stopwatch.ElapsedTicks);
                stopwatch.Restart();
                List<IPhysicsObject> physicsObjects;
                if (this.Engine.SceneManager.SelectedScene != null)
                {
                    lock (this.Engine.SceneManager.SelectedScene.Objects)
                    {
                        physicsObjects = GetOnlyIPhysicsObject(this.Engine.SceneManager.SelectedScene.Objects).ToList();
                    }

                    if (ThreadTiming.Milliseconds > 1)
                    {
                        Thread.Sleep(1);
                    }

                    foreach (IPhysicsObject physicsObject in physicsObjects)
                    {
                        var physicsSolver = new PhysicsSolver(this, physicsObjects, physicsObject);
                        physicsSolver.Process();
                    }
                }
            }
        }

        public static IEnumerable<IPhysicsObject> GetOnlyIPhysicsObject(List<IBaseObject> listOfObjects)
        {
            return listOfObjects.OfType<IPhysicsObject>();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    this.IsDisposing = true;
                    while (Thread.IsAlive)
                    {
                        Thread.Sleep(1);
                    }
                }

                this.IsDisposed = true;
            }
        }
    }
}