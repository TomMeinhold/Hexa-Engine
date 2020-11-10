using HexaEngine.Core.Common;
using HexaEngine.Core.Generation;
using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Ressources;
using HexaEngine.Core.Scenes;
using SharpDX;

namespace Main
{
    public class Scene3 : Scene
    {
        public Scene3()
        {
            PhysicsObjectDiscription wall = new PhysicsObjectDiscription
            {
                Mass = 100,
                Static = true
            };

            NoiseRule[] rules = new NoiseRule[]
            {
                new NoiseRule(new Range(0,50),new Range(0,20), new Range(0,1)),
                new NoiseRule(new Range(0,50),new Range(20,50), new Range(2,3))
            };

            ObjectGenerator generator = new ObjectGenerator(50, 50, 4, 0, rules);
            generator.Generate((v, x, y) =>
            {
                Vector3 pos = new Vector3(x * 16, y * 16, 0);
                switch (v)
                {
                    case 0:
                        Add(new Planet(RessourceManager.GetSprite("planet"), pos, wall));
                        break;

                    case 1:
                        Add(new Planet(RessourceManager.GetSprite("planet1"), pos, wall));
                        break;

                    case 2:
                        Add(new Planet(RessourceManager.GetSprite("planet2"), pos, wall));
                        break;
                }
            });
        }
    }
}