using System.Collections.Generic;
using PandeaGames.ViewModels;
using Terra.Utils;

namespace Terra.ViewModels
{
    public class TerraUniversBlobsViewModel : IViewModel
    {
        public IEnumerable<TerraBlob> Blobs;
        private TerraAlterVerseViewModel _terraAlterVerseViewModel;

        public TerraUniversBlobsViewModel()
        {
        }

        public TerraUniversBlobsViewModel(TerraAlterVerseViewModel terraAlterVerseViewModel)
        {
            Blobs = TerraBlobUtil.GetBlobs(terraAlterVerseViewModel.Data);
            _terraAlterVerseViewModel = terraAlterVerseViewModel;
            terraAlterVerseViewModel.OnDataHasChanged += TerraAlterVerseViewModelOnDataHasChanged;
        }

        private void TerraAlterVerseViewModelOnDataHasChanged(IEnumerable<TerraAlterVerseGridPoint> data)
        {
            Blobs = TerraBlobUtil.GetBlobs(_terraAlterVerseViewModel.Data);
        }

        public void Reset()
        {
            
        }
    }
}