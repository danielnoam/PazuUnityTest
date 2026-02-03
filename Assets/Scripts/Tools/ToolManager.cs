using System;
using UnityEngine;
using UnityPazuTest.Character;

namespace UnityPazuTest.Tools
{
    public class ToolManager : MonoBehaviour
    {
        public static ToolManager Instance { get; private set; }
        public static event Action<ToolSO> OnToolSelected;
        public static event Action<ToolSO> OnToolReleased;
        

        private HairPatch[] _hairPatches;
        private ToolSO _currentTool;
        private Camera _mainCamera;


        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            
            if (Application.isMobilePlatform)
            {
                Application.targetFrameRate = 120;
            }
            
            Instance = this;
            _mainCamera = Camera.main;
            if (!_mainCamera)
            {
                Debug.LogError("Main camera not found");
            }
        }

        private void Start()
        {
            _hairPatches = FindObjectsOfType<HairPatch>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && _currentTool && _mainCamera)
            {
                Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 position = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
                _currentTool.Use(position, _hairPatches);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _currentTool?.Released();
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
    }
}