using System;
using System.Collections.Generic;
using System.Linq;
using PandeaGames;
using PandeaGames.Data;
using Terra.MonoViews.AI;
using Terra.SerializedData.GameData;
using Terra.Services;
using Terra.ViewModels;
using Terra.Views.ViewDataStreamers;
using UnityEngine;

namespace Terra.SerializedData.Entities
{
    public partial class RuntimeTerraEntity : ITerraEntity
    {
        public event Action<RuntimeTerraEntity, string> OnLabelAdded;
        public event Action<RuntimeTerraEntity, string> OnLabelRemoved;
            
        event Action<TerraEntity, string> ITerraEntity.OnLabelRemoved
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event Action<TerraEntity, string> ITerraEntity.OnLabelAdded
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }
        
        public TerraEntity Entity { private set; get; }
        public TerraDBService DB { private set; get; }

        public TerraPosition3DComponent Position;
        public TerraGridPositionComponent GridPosition;
        public TerraLivingEntityComponent TerraLivingEntity;
        public TerraEntityTypeData EntityTypeData { get; private set; }
        private TerraWorldStateViewModel _worldStateViewModel;
        private PlayerEntitySlaveViewModel _slaveViewModel;
        private AssembledEntity _assembledEntity;

        public override int GetHashCode()
        {
            return Entity.InstanceId;
        }

        public override bool Equals(object obj)
        {
            return obj.GetHashCode() == this.GetHashCode();
        }

        public RuntimeTerraEntity(AssembledEntity entity, TerraDBService db)
        {
            _assembledEntity = entity;
            entity.TerraPosition3D.InstanceId = entity.TerraEntity.InstanceId;
            entity.TerraGridPosition.InstanceId = entity.TerraEntity.InstanceId;
            Position = new TerraPosition3DComponent(db,entity.TerraPosition3D);
            GridPosition = new TerraGridPositionComponent(db, entity.TerraGridPosition);
            TerraLivingEntity = new TerraLivingEntityComponent(db, entity.TerraLivingEntity);

            TerraLivingEntity.Data.InstanceId = entity.TerraEntity.InstanceId;
            
            Entity = entity.TerraEntity;
            
            Entity.OnLabelAdded += (labelEntity, label) => OnLabelAdded?.Invoke(this, label);
            Entity.OnLabelRemoved += (labelEntity, label) => OnLabelRemoved?.Invoke(this, label);
            
            EntityTypeData = TerraGameResources.Instance.TerraEntityPrefabConfig.GetEntityConfig(this);
            DB = db;
            _worldStateViewModel = Game.Instance.GetViewModel<TerraWorldStateViewModel>(0);
            _slaveViewModel = Game.Instance.GetViewModel<PlayerEntitySlaveViewModel>(0);
        }

        public int InstanceId
        {
            get => Entity.InstanceId;
            set => Entity.InstanceId = value;
        }
        public HashSet<string> Labels
        {
            get => Entity.Labels;
            set => Entity.Labels = value;
        }

        public string EntityID
        {
            get => Entity.EntityID;
            set => Entity.EntityID = value;
        }
        
        public bool IsRipe()
        {
            if (EntityTypeData.Component.HasFlag(EntityComponent.Harvestable))
            {
                if ((_worldStateViewModel.State.Tick - Entity.TickCreated) * TerraWorldStateStreamer.TickTimeSeconds >
                    EntityTypeData.RipeTimeSeconds)
                {
                    return true;
                }
            }

            return false;
        }
        
        public bool IsPastLifetime()
        {
            if (EntityTypeData.LifespanSeconds > 0)
            {
                if ((_worldStateViewModel.State.Tick - Entity.TickCreated) * TerraWorldStateStreamer.TickTimeSeconds >
                    EntityTypeData.LifespanSeconds)
                {
                    return true;
                }
            }

            return false;
        }

        public void ExpireEntity()
        {
            if (_slaveViewModel.CurrentSlave == this)
            {
                _slaveViewModel.ClearSlave();
            }
            
            Game.Instance.GetViewModel<TerraEntitiesViewModel>(0).RemoveEntity(this);
            TerraViewModel tViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);
            if (EntityTypeData.EntityToSpawnAfterDeath != null)
            {
                RuntimeTerraEntity newEntity = Game.Instance.GetService<TerraEntitesService>().CreateEntity(EntityTypeData.EntityToSpawnAfterDeath.Data);
                if (EntityTypeData.Component.HasFlag(EntityComponent.GridPosition))
                {
                    newEntity.Position.Set(tViewModel.Geometry[
                        tViewModel.Chunk.WorldToLocal(GridPosition.Data)
                    ]);
                }
                else
                {
                    newEntity.Position.Set(Position.Data);
                }
                
                Game.Instance.GetViewModel<TerraEntitiesViewModel>(0).AddEntity(newEntity);
            }
        }

        public bool IsDead()
        {
            return TerraLivingEntity.Data.HP > EntityTypeData.TotalHealth;
        }

        public void Attack(AttackDef def)
        {
            TerraLivingEntity.Attack(def);
        }

        public bool IsSlavable => EntityTypeData.IsSlavable;
    }
    
    

    public struct AssembledEntity : IDBSerializable
    {
        public TerraEntity TerraEntity;
        public TerraPosition3D TerraPosition3D;
        public TerraGridPosition TerraGridPosition;
        public TerraLivingEntity TerraLivingEntity;

        public AssembledEntity(TerraEntity entity)
        {
            TerraEntity = entity;
            TerraPosition3D = new TerraPosition3D();
            TerraGridPosition = new TerraGridPosition();
            TerraLivingEntity = new TerraLivingEntity();
        }
    }

    //Use LEFT OUTER JOIN to GET this
    public class AssembledEntitySerializer : IDBSerializer<AssembledEntity>
    {
        public static TerraEntitySerializer TerraEntitySerializer = new TerraEntitySerializer();
        public static TerraPosition3DSerializer TerraPosition3DSerializer = new TerraPosition3DSerializer();
        public static TerraGridPositionSerializer TerraGridPositionSerializer = new TerraGridPositionSerializer();
        public static TerraLivingEntitySerializer TerraLivingEntitySerializer = TerraLivingEntitySerializer.Instance;

        private IDBSchema[] Schemas { get; } = new IDBSchema[]
        {
            TerraEntitySerializer,
            TerraPosition3DSerializer,
            TerraGridPositionSerializer,
            TerraLivingEntitySerializer
        };

        public string Table => TerraEntitySerializer.Table;
        public IDBColumn[] Columns => 
            TerraEntitySerializer.Columns.Concat(TerraPosition3DSerializer.Columns).
                Concat(TerraGridPositionSerializer.Columns).
                Concat(TerraLivingEntitySerializer.Columns).ToArray();

        public int PrimaryKeyColumnIndex => TerraEntitySerializer.PrimaryKeyColumnIndex;
        
        public AssembledEntity Instantiate()
        {
            return new AssembledEntity()
            {
                TerraEntity = new TerraEntity(), TerraPosition3D = new TerraPosition3D(), TerraGridPosition = new TerraGridPosition(), TerraLivingEntity = new TerraLivingEntity()
            };
        }

        private struct SerializerScrubber
        {
            public int SerializerIndex;
            public int SerializerColumnIndex;
        }
        
        private SerializerScrubber GetSerializerIndex(int columnIndex)
        {
            SerializerScrubber scrubber = new SerializerScrubber();
            int totalSerializerColumnIndex = 0;
            for (scrubber.SerializerIndex = 0; scrubber.SerializerIndex < Schemas.Length; scrubber.SerializerIndex++)
            {
                scrubber.SerializerColumnIndex = columnIndex - totalSerializerColumnIndex;
                if (columnIndex < totalSerializerColumnIndex + Schemas[scrubber.SerializerIndex].Columns.Length)
                {
                    break;
                }
                
                totalSerializerColumnIndex += Schemas[scrubber.SerializerIndex].Columns.Length;
            }

            return scrubber;
        }

        public void ParseStringResult(ref AssembledEntity serializable, int columnIndex, string value)
        {
            SerializerScrubber scrubber = GetSerializerIndex(columnIndex);

            if (scrubber.SerializerIndex == 0)
            {
                TerraEntitySerializer.ParseStringResult(ref serializable.TerraEntity, scrubber.SerializerColumnIndex, value);
            } 
            else if (scrubber.SerializerIndex == 1)
            {
                TerraPosition3DSerializer.ParseStringResult(ref serializable.TerraPosition3D, scrubber.SerializerColumnIndex, value);
            }
            else if (scrubber.SerializerIndex == 2)
            {
                TerraGridPositionSerializer.ParseStringResult(ref serializable.TerraGridPosition, scrubber.SerializerColumnIndex, value);
            }
            else if (scrubber.SerializerIndex == 3)
            {
                TerraLivingEntitySerializer.ParseStringResult(ref serializable.TerraLivingEntity, scrubber.SerializerColumnIndex, value);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void ParseIntegerResult(ref AssembledEntity serializable, int columnIndex, int value)
        {
            SerializerScrubber scrubber = GetSerializerIndex(columnIndex);

            if (scrubber.SerializerIndex == 0)
            {
                TerraEntitySerializer.ParseIntegerResult(ref  serializable.TerraEntity, scrubber.SerializerColumnIndex, value);
            } 
            else if (scrubber.SerializerIndex == 1)
            {
                TerraPosition3DSerializer.ParseIntegerResult(ref  serializable.TerraPosition3D, scrubber.SerializerColumnIndex, value);
            }
            else if (scrubber.SerializerIndex == 2)
            {
                TerraGridPositionSerializer.ParseIntegerResult(ref  serializable.TerraGridPosition, scrubber.SerializerColumnIndex, value);
            }
            else if (scrubber.SerializerIndex == 3)
            {
                TerraLivingEntitySerializer.ParseIntegerResult(ref  serializable.TerraLivingEntity, scrubber.SerializerColumnIndex, value);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void ParseNumericResult(ref AssembledEntity serializable, int columnIndex, float value)
        {
            SerializerScrubber scrubber = GetSerializerIndex(columnIndex);

            if (scrubber.SerializerIndex == 0)
            {
                TerraEntitySerializer.ParseNumericResult(ref serializable.TerraEntity, scrubber.SerializerColumnIndex, value);
            } 
            else if (scrubber.SerializerIndex == 1)
            {
                TerraPosition3DSerializer.ParseNumericResult(ref serializable.TerraPosition3D, scrubber.SerializerColumnIndex, value);
            }
            else if (scrubber.SerializerIndex == 2)
            {
                TerraGridPositionSerializer.ParseNumericResult(ref serializable.TerraGridPosition, scrubber.SerializerColumnIndex, value);
            }
            else if (scrubber.SerializerIndex == 3)
            {
                TerraLivingEntitySerializer.ParseNumericResult(ref serializable.TerraLivingEntity, scrubber.SerializerColumnIndex, value);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public string GetValue(AssembledEntity serializable, int columnIndex)
        {
            SerializerScrubber scrubber = GetSerializerIndex(columnIndex);

            if (scrubber.SerializerIndex == 0)
            {
                return TerraEntitySerializer.GetValue(serializable.TerraEntity, scrubber.SerializerColumnIndex);
            } 
            else if (scrubber.SerializerIndex == 1)
            {
                return TerraPosition3DSerializer.GetValue(serializable.TerraPosition3D, scrubber.SerializerColumnIndex);
            }
            else if (scrubber.SerializerIndex == 2)
            {
                return TerraGridPositionSerializer.GetValue(serializable.TerraGridPosition, scrubber.SerializerColumnIndex);
            }
            else if (scrubber.SerializerIndex == 3)
            {
                return TerraLivingEntitySerializer.GetValue(serializable.TerraLivingEntity, scrubber.SerializerColumnIndex);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

    }
}