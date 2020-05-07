using System;
using System.Collections.Generic;
using System.Text;
using PandeaGames;
using PandeaGames.Data;
using PandeaGames.Services;
using Terra.SerializedData.Entities;
using Terra.SerializedData.GameData;
using Terra.ViewModels;

namespace Terra.Services
{
    public class TerraEntitesService : IService
    {
        private TerraDBService _db;
        private TerraWorldStateViewModel _worldState;
        
        public TerraEntitesService()
        {
            _db = Game.Instance.GetService<TerraDBService>();
            _worldState = Game.Instance.GetViewModel<TerraWorldStateViewModel>(0);
        }
        
        public RuntimeTerraEntity CreateEntity(ITerraEntityType entityType)
        {
            TerraEntity newEntity = new TerraEntity();
            AssembledEntity assembled = new AssembledEntity(newEntity);
            newEntity.EntityID = entityType.EntityID;
            newEntity.TickCreated = _worldState.State.Tick;
            _db.Write(newEntity, TerraEntity.Serializer, TerraEntity.WherePrimaryKey);
            RuntimeTerraEntity runtimeTerraEntity = new RuntimeTerraEntity(assembled, _db);
            return runtimeTerraEntity;
        }

        private string GetColumnSelectors(IDBSchema scheme)
        {
            List<string> inputs = new List<string>();

            for (int i = 0; i < scheme.Columns.Length; i++)
            {
                inputs.Add($"{scheme.Table}.{scheme.Columns[i].ColumnName}");
            }

            return String.Join(",", inputs.ToArray());
        }
        
        public void LoadEntites(Action<RuntimeTerraEntity[]> onComplete, Action<Exception> onError)
        {
            AssembledEntitySerializer serializer = new AssembledEntitySerializer();
            AssembledEntity[] entities = _db.Get<AssembledEntitySerializer, AssembledEntity>(
                serializer,
                "", 
                $"SELECT {GetColumnSelectors(AssembledEntitySerializer.TerraEntitySerializer)},{GetColumnSelectors(AssembledEntitySerializer.TerraPosition3DSerializer)},{GetColumnSelectors(AssembledEntitySerializer.TerraGridPositionSerializer)},{GetColumnSelectors(AssembledEntitySerializer.TerraLivingEntitySerializer)} " +
                $"FROM {AssembledEntitySerializer.TerraEntitySerializer.Table} "+
                $"LEFT OUTER JOIN {AssembledEntitySerializer.TerraPosition3DSerializer.Table} " +
                $"ON {AssembledEntitySerializer.TerraEntitySerializer.Table}.{TerraEntitySerializer.COLUMN_INSTNACE_ID} = {AssembledEntitySerializer.TerraPosition3DSerializer.Table}.{TerraEntitySerializer.COLUMN_INSTNACE_ID} " +
                $"LEFT OUTER JOIN {AssembledEntitySerializer.TerraGridPositionSerializer.Table} " +
                $"ON {AssembledEntitySerializer.TerraEntitySerializer.Table}.{TerraEntitySerializer.COLUMN_INSTNACE_ID} = {AssembledEntitySerializer.TerraGridPositionSerializer.Table}.{TerraEntitySerializer.COLUMN_INSTNACE_ID} " +
                $"LEFT OUTER JOIN {AssembledEntitySerializer.TerraLivingEntitySerializer.Table} " +
                $"ON {AssembledEntitySerializer.TerraEntitySerializer.Table}.{TerraEntitySerializer.COLUMN_INSTNACE_ID} = {AssembledEntitySerializer.TerraLivingEntitySerializer.Table}.{TerraEntitySerializer.COLUMN_INSTNACE_ID}");
            
            RuntimeTerraEntity[] runtimeEntities = new RuntimeTerraEntity[entities.Length];

            for (int i = 0; i < entities.Length; i++)
            {
                runtimeEntities[i] = new RuntimeTerraEntity(entities[i], _db);
            }

            onComplete(runtimeEntities);
        }

        public void LoadEntitesOfType(TerraEntityTypeData type, Action<RuntimeTerraEntity[]> onComplete, Action<Exception> onError)
        {
            AssembledEntitySerializer serializer = new AssembledEntitySerializer();
            //TerraEntity[] entities = _db.Get<TerraEntitySerializer, TerraEntity>(TerraEntity.Serializer);
            
            AssembledEntity[] entities = _db.Get<AssembledEntitySerializer, AssembledEntity>(
                serializer,
                "", 
                $"SELECT {GetColumnSelectors(AssembledEntitySerializer.TerraEntitySerializer)},{GetColumnSelectors(AssembledEntitySerializer.TerraPosition3DSerializer)} " +
                $"FROM {AssembledEntitySerializer.TerraEntitySerializer.Table} "+
                $"LEFT OUTER JOIN {AssembledEntitySerializer.TerraPosition3DSerializer.Table} " +
                $"ON {AssembledEntitySerializer.TerraEntitySerializer.Table}.{TerraEntitySerializer.COLUMN_INSTNACE_ID} = {AssembledEntitySerializer.TerraPosition3DSerializer.Table}.{TerraEntitySerializer.COLUMN_INSTNACE_ID} " +
                $"WHERE {TerraEntitySerializer.TABLE}.{TerraEntitySerializer.COLUMN_ENTITY_ID} = '{type.EntityID}'");
            
            RuntimeTerraEntity[] runtimeEntities = new RuntimeTerraEntity[entities.Length];

            for (int i = 0; i < entities.Length; i++)
            {
                runtimeEntities[i] = new RuntimeTerraEntity(entities[i], _db);
            }

            onComplete(runtimeEntities);
        }

        public void LoadEntities(TerraArea area, Action<RuntimeTerraEntity[]> onComplete, Action onError)
        {
            AssembledEntitySerializer serializer = new AssembledEntitySerializer();
            
            string posTable = AssembledEntitySerializer.TerraPosition3DSerializer.Table;
            
            AssembledEntity[] entities = _db.Get<AssembledEntitySerializer, AssembledEntity>(
                serializer,
                "", 
                $"SELECT {GetColumnSelectors(AssembledEntitySerializer.TerraEntitySerializer)},{GetColumnSelectors(AssembledEntitySerializer.TerraPosition3DSerializer)} " +
                $"FROM {AssembledEntitySerializer.TerraEntitySerializer.Table} "+
                $"LEFT OUTER JOIN {AssembledEntitySerializer.TerraPosition3DSerializer.Table} " +
                $"ON {AssembledEntitySerializer.TerraEntitySerializer.Table}.{TerraEntitySerializer.COLUMN_INSTNACE_ID} = {AssembledEntitySerializer.TerraPosition3DSerializer.Table}.{TerraEntitySerializer.COLUMN_INSTNACE_ID} "+
                $"WHERE {posTable}.x > {area.x} AND {posTable}.x < {area.x + area.width} AND {posTable}.y > {area.y} AND {posTable}.y < {area.y + area.height}");
            
            RuntimeTerraEntity[] runtimeEntities = new RuntimeTerraEntity[entities.Length];

            for (int i = 0; i < entities.Length; i++)
            {
                runtimeEntities[i] = new RuntimeTerraEntity(entities[i], _db);
            }

            onComplete(runtimeEntities);
        }

        public void DeleteEntity(RuntimeTerraEntity entity)
        {
            _db.DeleteRecord(entity.Entity, TerraEntity.Serializer);
            _db.DeleteRecord(entity.Position.Data, TerraPosition3D.Serializer);
            _db.DeleteRecord(entity.GridPosition.Data, TerraGridPosition.Serializer);
        }
    }
}