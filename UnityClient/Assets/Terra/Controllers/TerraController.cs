using PandeaGames;
using PandeaGames.Views;
using PandeaGames.Views.ViewControllers;
using Terra.SerializedData.Entities;
using Terra.Services;
using Terra.Views;
using Terra.Views.ViewDataStreamers;

namespace Terra.Controllers
{
    public class TerraController : AbstractViewController
    {
        public TerraController()
        {
            Game.Instance.GetService<TerraDBService>().Setup(
                new IDBSchema[]
                {
                    TerraEntity.Serializer,
                    TerraPosition3D.Serializer,
                    TerraPoint.Serializer,
                    TerraGridPosition.Serializer,
                    TerraWorldStateStreamer.Serializer,
                    TerraPlayerStateService.Serializer
                }
                );
        }

        protected override IView CreateView()
        {
            return new TerraView();
        }
    }
}