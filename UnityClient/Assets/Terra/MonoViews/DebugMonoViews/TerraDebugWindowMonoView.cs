using System;
using PandeaGames;
using UnityEngine;

namespace Terra.MonoViews.DebugMonoViews
{
    public class TerraDebugWindowMonoView : StateMachine<TerraDebugWindowMonoView.EditorStates>
    {
        public enum EditorStates
        {
            Off, 
            FreeFly, 
            Locked
        }
        
        [SerializeField]
        private Camera _debugCamera;

        private TerraDebugControlViewModel _controlViewModel;
        
        private Camera _mainCamera;

        protected override void Start()
        {
            _controlViewModel = Game.Instance.GetViewModel<TerraDebugControlViewModel>(0);
            _controlViewModel.DebugCamera = _debugCamera;
            //do nothing
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetState(EditorStates.Locked);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetState(EditorStates.FreeFly);
            }
        }

        public void OnEnable()
        {
            TerraPlayerEntityDebugMonoView player = FindObjectOfType<TerraPlayerEntityDebugMonoView>();

            if (player == null)
            {
                Debug.LogWarning("Player not found in scene");
                return;
            }
            
            _mainCamera = Camera.main;
            _debugCamera.transform.position = _mainCamera.transform.position;
            _debugCamera.transform.rotation = _mainCamera.transform.rotation;
            
            SetState(EditorStates.Locked, isInitialState:true);
        }

        public void OnDisable()
        {
            Camera.SetupCurrent(_mainCamera);
            SetState(EditorStates.Off);
        }

        protected override void EnterState(EditorStates state)
        {
            TerraPlayerEntityDebugMonoView player = FindObjectOfType<TerraPlayerEntityDebugMonoView>();
            TerraDebugStatefulMonoView[] statefulViews = FindObjectsOfType<TerraDebugStatefulMonoView>();

            foreach (TerraDebugStatefulMonoView statefulView in statefulViews)
            {
                statefulView.SetState(state);
            }
            
            switch (state)
            {
                case EditorStates.Locked:
                {
                    Camera.SetupCurrent(_debugCamera);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                }
                case EditorStates.Off:
                {
                    Camera.SetupCurrent(_mainCamera);
                    _controlViewModel.SetState(TerraDebugControlViewModel.States.None);
                    break;
                }
                case EditorStates.FreeFly:
                {
                    Camera.SetupCurrent(_debugCamera);
                    break;
                }
            }
        }

        protected override void LeaveState(EditorStates state)
        {
            
        }
    }
}