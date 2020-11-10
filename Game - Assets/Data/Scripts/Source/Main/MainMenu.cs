using HexaEngine.Core;
using HexaEngine.Core.Render.Components;
using HexaEngine.Core.Ressources;
using HexaEngine.Core.Scenes;

namespace Main
{
    public class MainMenu : Scene
    {
        public override void LoadRessources()
        {
            Texture.Load("mainmenu.bmp");
            Engine.Current.UIManager.SetUIByType(typeof(MainMenuScreen));
        }

        public override void UnloadRessources()
        {
            Engine.Current.UIManager.SetUIByType(typeof(Screen1));
            RessourceManager.Unload("mainmenu", RessourceType.Texture);
        }
    }
}