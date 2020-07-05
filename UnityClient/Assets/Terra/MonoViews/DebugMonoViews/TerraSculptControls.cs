using System;
using System.Collections.Generic;
using PandeaGames;
using Terra.SerializedData.World;
using Terra.Services;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews.DebugMonoViews
{
    public class TerraSculptControls : MonoBehaviour
    {
        private const double MinFlowTimeMilliseconds = 300;
        
        [SerializeField]
        private Projector _projector;
        
        [SerializeField]
        private Light _light;

        [SerializeField]
        private Material _material;
        
        private TerraSculptViewModel _terraSculptViewModel;
        private TerraDebugControlViewModel _terraDebugControlViewModel;
        private TerraPointerViewModel _terraPointerViewModel;
        private TerraViewModel _terraViewModel;
        private TerraVector _currentGridVector;
        private Vector3 _currentMousePosition;
        private DateTime _lastPaint;
        private void Start()
        {
            _terraPointerViewModel = Game.Instance.GetViewModel<TerraPointerViewModel>(0);
            _terraSculptViewModel = Game.Instance.GetViewModel<TerraSculptViewModel>(0);
            _terraDebugControlViewModel = Game.Instance.GetViewModel<TerraDebugControlViewModel>(0);
            _terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);
        }

        private void Update()
        {
            _projector.material = _material;
            _projector.orthographic = true;
            _projector.orthographicSize = _terraSculptViewModel.Size;
            _projector.transform.position = _terraPointerViewModel.MousePosition + new Vector3(0, _terraSculptViewModel.Size, 0);
            _projector.transform.rotation = Quaternion.Euler(90, 0, 0);
            _light.spotAngle = 45;
            _light.intensity = _terraSculptViewModel.Size * 3;

            if (_terraPointerViewModel.MouseDown)
            {
                UpdateMouseDown();
            }
            else
            {
                UpdateMouseUp();
            }
        }

        private void UpdateMouseDown()
        {
            TimeSpan timeSinceLastPaint = DateTime.UtcNow - _lastPaint;
            bool hasEnoughTimePassed = timeSinceLastPaint.TotalMilliseconds > MinFlowTimeMilliseconds - (MinFlowTimeMilliseconds * _terraSculptViewModel.Flow);

            bool canPaint = hasEnoughTimePassed;
            canPaint &= _terraPointerViewModel.MousePositionTerraVector != _currentGridVector;
            canPaint &= _currentMousePosition != Input.mousePosition;
            
            if (canPaint)
            {
                _currentMousePosition = Input.mousePosition;
                _currentGridVector = _terraPointerViewModel.MousePositionTerraVector;
                _lastPaint = DateTime.UtcNow;
                Paint(_currentGridVector);
            }
        }

        private void UpdateMouseUp()
        {
            _lastPaint = DateTime.MinValue;
            _currentGridVector = default(TerraVector);
            _currentMousePosition = Vector3.zero;
        }

        private void Paint(TerraVector terraVector)
        {
            int size = (int) Math.Ceiling(_terraSculptViewModel.Size);
            int strength =  (int) Math.Ceiling(_terraSculptViewModel.Strength);
            int directionMod = _terraSculptViewModel.CurrentState == TerraSculptViewModel.SculptMode.Push ? -1 : 1;
            List<TerraDataPoint> changes = new List<TerraDataPoint>();
            for (int x = terraVector.x - size;
                x < terraVector.x + size;
                x++)
            {
                for (int y = terraVector.y - size;
                    y < terraVector.y + size;
                    y++)
                {
                    float vx = x -terraVector.x;
                    float vy = y -terraVector.y;
                    float d = Mathf.Sqrt(vx * vx + vy * vy);
                    if (d < size)
                    {
                        float mod = (size - d) / size;
                        int modifiedStrength =  (int) Math.Ceiling(_terraSculptViewModel.Strength * mod);
                        TerraVector vector = new TerraVector(x, y);
                        TerraPoint point = _terraViewModel.Chunk.GetFromWorld(vector);
                        point.Height += modifiedStrength * directionMod;
                        changes.Add(new TerraDataPoint(point, vector));
                    }
                }
            }
            
            _terraViewModel.Chunk.SetFromWorld(changes);
        }
    }
}