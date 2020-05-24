using System;
using System.Collections.Generic;
using Terra.SerializedData.GameData;

namespace Terra.SerializedData.Entities
{
    public interface ITerraEntity : ITerraEntityComponent, ITerraEntityType
    {
        string EntityID { get; set; }
        int InstanceId { get; set; }
    }
}