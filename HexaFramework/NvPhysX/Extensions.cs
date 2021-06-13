using HexaFramework.Scenes;
using PhysX;
using System.IO;
using System.Linq;
using System.Numerics;

namespace HexaFramework.NvPhysX
{
    public static class Extensions
    {
        public static RigidDynamic CreateConvexMesh(this Scene scene, SceneObject sceneObject, float mass, Material material)
        {
            var convexMeshDesc = new ConvexMeshDesc
            {
                Flags = ConvexFlag.ComputeConvex
            };
            convexMeshDesc.SetPositions(sceneObject.Model.Vertices.ToList().ConvertAll(x => new Vector3(x.Position.X, x.Position.Y, x.Position.Z)).ToArray());
            convexMeshDesc.SetTriangles(sceneObject.Model.Indices);

            var cooking = scene.Physics.CreateCooking();

            var stream = new MemoryStream();
            var cookResult = cooking.CookConvexMesh(convexMeshDesc, stream);

            stream.Position = 0;

            var convexMesh = scene.Physics.CreateConvexMesh(stream);

            var convexMeshGeom = new ConvexMeshGeometry(convexMesh)
            {
                Scale = new MeshScale(new Vector3(1f, 1f, 1f), Quaternion.Identity)
            };

            var rigidActor = scene.Physics.CreateRigidDynamic();
            RigidActorExt.CreateExclusiveShape(rigidActor, convexMeshGeom, material, null);
            rigidActor.SetMassAndUpdateInertia(mass);
            rigidActor.UserData = sceneObject;
            scene.AddActor(rigidActor);
            return rigidActor;
        }

        public static RigidStatic CreateConvexMeshStatic(this Scene scene, SceneObject sceneObject, Material material)
        {
            var convexMeshDesc = new ConvexMeshDesc
            {
                Flags = ConvexFlag.ComputeConvex
            };
            convexMeshDesc.SetPositions(sceneObject.Model.Vertices.ToList().ConvertAll(x => new Vector3(x.Position.X, x.Position.Y, x.Position.Z)).ToArray());
            convexMeshDesc.SetTriangles(sceneObject.Model.Indices);

            var cooking = scene.Physics.CreateCooking();

            var stream = new MemoryStream();
            var cookResult = cooking.CookConvexMesh(convexMeshDesc, stream);

            stream.Position = 0;

            var convexMesh = scene.Physics.CreateConvexMesh(stream);

            var convexMeshGeom = new ConvexMeshGeometry(convexMesh)
            {
                Scale = new MeshScale(new Vector3(1f, 1f, 1f), Quaternion.Identity)
            };

            var rigidActor = scene.Physics.CreateRigidStatic();
            RigidActorExt.CreateExclusiveShape(rigidActor, convexMeshGeom, material, null);
            rigidActor.UserData = sceneObject;
            scene.AddActor(rigidActor);
            return rigidActor;
        }

        public static RigidStatic CreateGroundPlane(this Scene scene, SceneObject sceneObject, Material material)
        {
            var rigidActor = scene.Physics.CreateRigidStatic();
            var planeGeom = new PlaneGeometry();
            RigidActorExt.CreateExclusiveShape(rigidActor, planeGeom, material, null);
            rigidActor.UserData = sceneObject;
            scene.AddActor(rigidActor);
            return rigidActor;
        }

        public static RigidDynamic CreateTriangleMesh(this Scene scene, SceneObject sceneObject, float mass, Material material)
        {
            var rigidActor = scene.Physics.CreateRigidDynamic();

            var triangleMeshDesc = new TriangleMeshDesc()
            {
                Flags = 0,
                Triangles = sceneObject.Model.Indices,
                Points = sceneObject.Model.Vertices.ToList().ConvertAll(x => new Vector3(x.Position.X, x.Position.Y, x.Position.Z)).ToArray()
            };

            var cooking = scene.Physics.CreateCooking();

            var stream = new MemoryStream();
            var cookResult = cooking.CookTriangleMesh(triangleMeshDesc, stream);

            stream.Position = 0;

            var triangleMesh = scene.Physics.CreateTriangleMesh(stream);

            var triangleMeshGeom = new TriangleMeshGeometry(triangleMesh)
            {
                Scale = new MeshScale(new Vector3(1, 1, 1), Quaternion.Identity)
            };

            RigidActorExt.CreateExclusiveShape(rigidActor, triangleMeshGeom, material);
            rigidActor.SetMassAndUpdateInertia(mass);
            rigidActor.UserData = sceneObject;
            scene.AddActor(rigidActor);
            return rigidActor;
        }

        public static RigidStatic CreateTriangleMeshStatic(this Scene scene, SceneObject sceneObject, Material material)
        {
            var rigidActor = scene.Physics.CreateRigidStatic();

            var triangleMeshDesc = new TriangleMeshDesc()
            {
                Flags = 0,
                Triangles = sceneObject.Model.Indices,
                Points = sceneObject.Model.Vertices.ToList().ConvertAll(x => new Vector3(x.Position.X, x.Position.Y, x.Position.Z)).ToArray()
            };

            var cooking = scene.Physics.CreateCooking();

            var stream = new MemoryStream();
            var cookResult = cooking.CookTriangleMesh(triangleMeshDesc, stream);

            stream.Position = 0;

            var triangleMesh = scene.Physics.CreateTriangleMesh(stream);

            var triangleMeshGeom = new TriangleMeshGeometry(triangleMesh)
            {
                Scale = new MeshScale(new Vector3(1, 1, 1), Quaternion.Identity)
            };

            RigidActorExt.CreateExclusiveShape(rigidActor, triangleMeshGeom, material);
            rigidActor.UserData = sceneObject;
            scene.AddActor(rigidActor);
            return rigidActor;
        }
    }
}