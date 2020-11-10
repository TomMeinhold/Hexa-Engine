using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Physics.Components;
using HexaEngine.Core.Physics.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HexaEngine.Core.Physics
{
    public partial class PhysicsEngine : IDisposable
    {
        public PhysicsEngine()
        {
            Thread = new Thread(ThreadVoid);
            Thread.Start();
        }

        ~PhysicsEngine()
        {
            Dispose(disposing: false);
        }

        public Thread Thread { get; }

        public bool IsDisposed { get; private set; }

        public bool IsDisposing { get; private set; }

        public bool Paused { get; set; }

        public bool DoCycle { get; set; }

        public TimeSpan ThreadTiming { get; set; }

        public ScalingMode ScalingMode { get; set; } = ScalingMode.Meters;

        public Dictionary<IPhysicsObject, PhysicsSolver> Instances { get; } = new Dictionary<IPhysicsObject, PhysicsSolver>();

        private void ThreadVoid()
        {
            Stopwatch stopwatch = new Stopwatch();
            while (!IsDisposing)
            {
                ThreadTiming = new TimeSpan(stopwatch.ElapsedTicks);
                while (Paused && !IsDisposing && !DoCycle)
                {
                    stopwatch.Reset();
                    Thread.Sleep(1);
                }
                DoCycle = false;
                stopwatch.Restart();
                List<IPhysicsObject> physicsObjects;
                if (Engine.Current.SceneManager.SelectedScene != null)
                {
                    lock (Engine.Current.SceneManager.SelectedScene.Objects)
                    {
                        physicsObjects = GetOnlyIPhysicsObject(Engine.Current.SceneManager.SelectedScene.Objects).ToList();
                    }

                    if (ThreadTiming.Milliseconds < 1)
                    {
                        Thread.Sleep(1);
                    }
                    foreach (IPhysicsObject physicsObject in physicsObjects)
                    {
                        if (!Instances.ContainsKey(physicsObject))
                        {
                            Instances[physicsObject] = new PhysicsSolver(this, physicsObjects, physicsObject);
                        }
                    }

                    Parallel.ForEach(physicsObjects, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, (physicsObject) =>
                    {
                        Instances[physicsObject].Process();
                    });
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
            if (!IsDisposed)
            {
                if (disposing)
                {
                    IsDisposing = true;
                    while (Thread.IsAlive)
                    {
                        Thread.Sleep(1);
                    }
                }

                IsDisposed = true;
            }
        }
    }
}