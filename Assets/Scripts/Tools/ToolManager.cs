using System;
using System.Collections.Generic;
using UnityEngine;
using UnityPazuTest.Character;

namespace UnityPazuTest.Tools
{
    public class ToolManager : MonoBehaviour
    {
        public static ToolManager Instance { get; private set; }
        public static event Action<ToolSO> OnToolSelected;
        public static event Action<ToolSO> OnToolReleased;
        
        private ToolSO _currentTool;
        private Camera _mainCamera;
        
        private readonly List<HairPatch> _hairPatches = new List<HairPatch>();
        private Vector2 _lastMouseWorldPos;
        private bool _isDragging;


        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            
            _mainCamera = Camera.main;
            if (!_mainCamera)
            {
                Debug.LogError("Main camera not found");
                enabled = false;
                return;
            }
            
            if (Application.isMobilePlatform)
            {
                Application.targetFrameRate = 120;
            }
        }
        

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _currentTool)
            {
                StartTool();
            }
            else if (Input.GetMouseButton(0) && _currentTool && _isDragging)
            {
                UpdateTool();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ReleaseTool();
            }
        }

        private void StartTool()
        {
            _isDragging = true;
            Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _lastMouseWorldPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
            _currentTool.Use(_lastMouseWorldPos, _lastMouseWorldPos, _hairPatches);
        }

        private void UpdateTool()
        {
            Vector3 mousePos3D = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 currentMousePos = new Vector2(mousePos3D.x, mousePos3D.y);

            _currentTool.Use(_lastMouseWorldPos, currentMousePos, _hairPatches);
            _lastMouseWorldPos = currentMousePos;
        }

        private void ReleaseTool()
        {
            _isDragging = false;
            if (_currentTool)
            {
                _currentTool.Released();
                OnToolReleased?.Invoke(_currentTool);
                _currentTool = null;
            }
        }

        public void SelectTool(ToolSO tool)
        {
            if (!tool) return;

            _currentTool = tool;
            _currentTool.Selected();
            OnToolSelected?.Invoke(_currentTool);
        }


        public void AddHairPatch(HairPatch hairPatch)
        {
            if (!hairPatch || _hairPatches.Contains(hairPatch)) return;
            
            _hairPatches.Add(hairPatch);
        }

        public void RemoveHairPatch(HairPatch hairPatch)
        {
            if (!hairPatch || !_hairPatches.Contains(hairPatch)) return;
            
            _hairPatches.Remove(hairPatch);
        }
    }
}