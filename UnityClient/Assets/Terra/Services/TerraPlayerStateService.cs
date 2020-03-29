using PandeaGames;
using PandeaGames.Services;
using Terra.SerializedData.GameState;

namespace Terra.Services
{
    public class TerraPlayerStateService : IService
    {
        public static TerraPlayerStateSerializer Serializer { get; } = new TerraPlayerStateSerializer();
        public TerraPlayerState GetPlayerState()
        {
            string CommandText = $"SELECT * FROM {Serializer.Table} ORDER BY rowId DESC LIMIT 1";

            TerraPlayerState[] states = Game.Instance.GetService<TerraDBService>().Get<TerraPlayerStateSerializer, TerraPlayerState>(
                Serializer, string.Empty, CommandText
                );
            TerraPlayerState state = states.Length != 0 ? states[0] : default(TerraPlayerState);
            return state;
        }
        
        public void WriteNewRecord(TerraPlayerState state)
        {
            Game.Instance.GetService<TerraDBService>().WriteNewRecord(state, Serializer);
        }
    }
}