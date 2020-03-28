using PandeaGames;
using Terra.SerializedData.World;
using Terra.Services;
using Terra.ViewModels;

namespace Terra.Views.ViewDataStreamers
{
    public class TerraWorldStateStreamer : IDataStreamer
    {
        public const float TickTime = 5;
        
        private TerraDBService _db;
        public static TerraWorldStateSerializer Serializer { get; } = new TerraWorldStateSerializer();
        private TerraWorldStateViewModel _vm;
        private float _timeSinceLastTick;
        
        public void Start()
        {
            _db = Game.Instance.GetService<TerraDBService>();
            _vm = Game.Instance.GetViewModel<TerraWorldStateViewModel>(0);
            string CommandText = $"SELECT * FROM {Serializer.Table} ORDER BY rowId DESC LIMIT 1";
            TerraWorldState[] states = _db.Get<TerraWorldStateSerializer, TerraWorldState>(Serializer, "", CommandText);
            TerraWorldState state = states.Length != 0 ? states[0] : default(TerraWorldState);
            _vm.SetState(state);
        }

        public void Update(float time)
        {
            if (time - _timeSinceLastTick > TickTime)
            {
                _timeSinceLastTick = time;
                _db.WriteNewRecord(_vm.Tick(), Serializer);
            }
        }

        public void Stop()
        {
            
        }
    }
}