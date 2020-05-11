using PandeaGames;
using Terra.MonoViews;
using Terra.SerializedData.Entities;
using Terra.Services;
using Terra.ViewModels;
using UnityEngine;

public class EntityBuildComponentMonoView : AbstractTerraMonoComponent
{
    [SerializeField]
    private GameObject[] _levelViews;

    protected override void Initialize(RuntimeTerraEntity Entity)
    {
        base.Initialize(Entity);
        UpdateViewState();
    }
    
    public void LevelUp()
    {
        Entity.TerraLivingEntity.State++;

        if (Entity.TerraLivingEntity.State >= Entity.EntityTypeData.NumberOfLevels)
        {
            Game.Instance.GetViewModel<TerraEntitiesViewModel>(0).RemoveEntity(Entity);

            if (Entity.EntityTypeData.EntityToSpawnAfterUpgraded != null)
            {
                RuntimeTerraEntity entity = Game.Instance.GetService<TerraEntitesService>().CreateEntity(Entity.EntityTypeData.EntityToSpawnAfterUpgraded.Data);
                TerraEntitiesViewModel vm = Game.Instance.GetViewModel<TerraEntitiesViewModel>(0);

                if (Entity.EntityTypeData.EntityToSpawnAfterUpgraded.Data.Component.HasFlag(EntityComponent.Position))
                {
                    entity.Position.Set(
                        new Vector3(
                            transform.position.x,
                            transform.position.y,
                            transform.position.z));
                }
            
                vm.AddEntity(entity);
            }
        }

        UpdateViewState();
    }

    private void UpdateViewState()
    {
        for (int i = 0; i < _levelViews.Length; i++)
        {
            _levelViews[i].SetActive(i == Entity.TerraLivingEntity.State);
        }
    }
}
