namespace Terra.MonoViews
{
    public class TerraEntityLifespanMonoView : AbstractTerraMonoComponent
    {
        private void Update()
        {
            if (Initialized && _entityMonoView.Entity.IsPastLifetime())
            {
                _entityMonoView.Entity.ExpireEntity();
            }
        }
    }
}