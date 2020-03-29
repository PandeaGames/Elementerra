using System;

namespace Terra.SerializedData.Entities
{
    [Flags]
    public enum EntityComponent
    {
        Nothing = 1 << 0,
        Position = 1 << 1,
        Health = 1 << 2,
        LastPlaced = 1 << 3,
        GridPosition = 1 << 4,
        CanPickUp = 1 << 5,
        Spacial = Position & GridPosition
    }
}