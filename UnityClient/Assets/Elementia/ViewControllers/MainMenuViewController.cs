using PandeaGames.Views;
using PandeaGames.Views.ViewControllers;
using Views;

namespace ViewControllers
{
    public class MainMenuViewController : AbstractViewController
    {
        protected override IView CreateView()
        {
            return new MainMenuView();
        }
    }
}