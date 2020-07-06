using System;
using System.Collections.Generic;

namespace Terra.Utils
{
    public static class TerraAreaUtils
    {
        public static void CalculateChangeAreas(TerraVector from, TerraVector to, int r, out List<TerraArea> addAreas, out List<TerraArea> removeAreas)
        {
            addAreas = new List<TerraArea>();
            removeAreas = new List<TerraArea>();
            
            int width = r * 2;
            int height = r * 2;
            int deltaX = to.x - from.x;
            int deltaY = to.y - from.y;
            
            //right side add
            addAreas.Add(new TerraArea(
                Math.Max(from.x + r, to.x - r), 
                from.y + r + deltaY, 
                Math.Min(Math.Max(0, deltaX), width), 
                height));
            
            //left side add
            addAreas.Add(new TerraArea(
                to.x - r, 
                from.y + r + deltaY, 
                Math.Min(Math.Max(0, deltaX * -1), width), 
                height));
            
            //top side add
            addAreas.Add(new TerraArea(
                Math.Min(to.x - r + Math.Max(0, deltaX), to.x + r), 
                to.y + r, 
                Math.Max(0, width - Math.Abs(deltaX)), 
                Math.Min(height,Math.Max(0,deltaY))));
            
            //bottom side add
            addAreas.Add(new TerraArea(
                Math.Min(to.x - r + Math.Max(0, deltaX), to.x + r), 
                Math.Min(to.y + r, to.y - r - deltaY), 
                Math.Max(0, width - Math.Abs(deltaX)), 
                Math.Min(height,Math.Max(0,deltaY * -1))));
            //addAreas.Add(new TerraArea(from.x + r + deltaX, from.y + r, Math.Max(0, deltaX), height));
            
            //right side remove
            removeAreas.Add(new TerraArea(
                Math.Max(to.x + r, from.x - r), 
                from.y + r, 
                Math.Min(Math.Max(0, deltaX * -1), width), 
                height));
            
            //left side remove
            removeAreas.Add(new TerraArea(
                from.x - r, 
                from.y + r, 
                Math.Min(width, Math.Max(0, deltaX)), 
                height));
            
            //top side remove
            removeAreas.Add(new TerraArea(
                from.x - r + Math.Max(0, deltaX), 
                from.y + r, 
                Math.Max(0, width - Math.Abs(deltaX)), 
                Math.Min(height, Math.Max(0, deltaY * -1))));
            
            //bottom side remove
            removeAreas.Add(new TerraArea(
                from.x - r + Math.Max(0, deltaX),  
                Math.Min(from.y - r + deltaY, from.y + r), 
                Math.Max(0, width - Math.Abs(deltaX)), 
                Math.Min(height, Math.Max(0, deltaY))));
        }
    }
}