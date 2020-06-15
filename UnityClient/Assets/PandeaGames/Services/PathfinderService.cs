using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AStar;
using PandeaGames.ViewModels;
using UnityEngine;

namespace PandeaGames.Services
{
    public class PathfinderService : AbstractService<PathfinderService>
    {
        private class PathFinder : AStarPathfinder<TerraVector>
        {
            public PathfinderViewModel m_viewModel;

            public PathFinder(PathfinderViewModel viewModel)
            {
                m_viewModel = viewModel;
            }
            
            protected override void FindNeighbors(TerraVector cell)
            {
                for(int x = Math.Max(0, cell.x - 1); x <= Math.Min(m_viewModel.Width - 1, cell.x + 2); x++)
                {
                    for(int y = Math.Max(0, cell.y - 1); y <= Math.Min(m_viewModel.Height - 1, cell.y + 2); y++)
                    {
                        if (x != cell.x && y != cell.y)
                        {
                            if (m_viewModel[x, y])
                            {
                                AddNeighbor(new TerraVector(x, y));
                            }
                        }
                    }
                }
            }
        }

        private class AsyncPathFind
        {
            private PathFinder m_pathFinder;
            private TerraVector m_start;
            private TerraVector m_end;
            private Action<List<TerraVector>> m_onComplete;
            private List<TerraVector> m_result;
            
            public AsyncPathFind(Action<List<TerraVector>> onComplete, PathfinderViewModel viewModel, TerraVector start, TerraVector end)
            {
                m_pathFinder = new PathFinder(viewModel);
                m_start = start;
                m_end = end;
                m_onComplete = onComplete;
            }

            public async void Begin()
            {
                await Task.Run(() => AsyncBegin());
                if (m_result == null)
                {
                    m_result = new List<TerraVector>();
                }

                m_onComplete.Invoke(m_result);
            }
            
            private void AsyncBegin()
            {
                m_result = m_pathFinder.FindPath(m_start, m_end);
            }
        }

        public void GetPath(Action<List<TerraVector>> onComplete, PathfinderViewModel viewModel, TerraVector start, TerraVector end)
        {
            Debug.Log($"Request Path from {start} to {end}");
            AsyncPathFind pathfinder = new AsyncPathFind(
                onComplete, viewModel, start, end);
            pathfinder.Begin();
        }
    }
}