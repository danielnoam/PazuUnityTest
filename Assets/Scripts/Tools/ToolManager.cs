using System;
using UnityEngine;
using UnityPazuTest.Tool;

namespace UnityPazuTest
{
    public class ToolManager : MonoBehaviour
    {
        public static ToolManager Instance { get; private set; }
        
        public static event Action<ToolType> OnToolSelected;
        public static event Action<ToolType> OnToolReleased;
        public static event Action<ToolType, Vector2> OnToolUsing;

        [SerializeField] private ToolType currentTool = ToolType.None;
        [SerializeField] private float dryerBlowRadius = 2f;
        [SerializeField] private float dryerBlowStrength = 5f;
        [SerializeField] private float scissorsCutRadius = 0.5f;
        [SerializeField] private float growerRadius = 2f;
        [SerializeField] private float growerAmount = 0.1f;

        public ToolType CurrentTool => currentTool;

        private Camera _mainCamera;
        private bool _isMouseHeld;

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isMouseHeld = true;
            }

            if (Input.GetMouseButton(0) && _isMouseHeld && currentTool != ToolType.None)
            {
                Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 position = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
                UseTool(position);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isMouseHeld = false;
                OnToolReleased?.Invoke(currentTool);
                currentTool = ToolType.None;
            }
        }

        public void SelectTool(ToolType tool)
        {
            currentTool = tool;
            OnToolSelected?.Invoke(tool);
        }

        private void UseTool(Vector2 position)
        {
            switch (currentTool)
            {
                case ToolType.Dryer:
                    UseDryer(position);
                    break;
                case ToolType.Scissors:
                    UseScissors(position);
                    break;
                case ToolType.Grower:
                    UseGrower(position);
                    break;
            }
            
            OnToolUsing?.Invoke(currentTool, position);
        }

        private void UseDryer(Vector2 position)
        {
            
        }

        private void UseScissors(Vector2 position)
        {
            
        }

        private void UseGrower(Vector2 position)
        {
            
        }
    }
}