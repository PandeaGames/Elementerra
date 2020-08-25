using System;

namespace PandeaGames.ViewModels
{
    public class SerializedStatefullViewModel<T> : AbstractStatefulViewModel<T>
        where T : struct, IConvertible
    {
        
    }
}