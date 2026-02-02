using System;
using TMPro;
using UnityEngine;
using UnityPazuTest.Character;

namespace UnityPazuTest.Tools
{
    public class ToolManager : MonoBehaviour
    {
        public static ToolManager Instance { get; private set; }
        public static event Action<ToolSO> OnToolSelected;
        public static event Action<ToolSO> OnToolReleased;
        
        [Header("Debug")]
        [SerializeField] private TextMeshProUGUI debugText;

        private HairPatch[] _allHairs;
        private ToolSO _currentTool;
        private Camera _mainCamera;
        private bool _isMouseHeld;


        private void Awake()
        {
            if (Instance && Instance != this)
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
        }

        private void Start()
        {
            _allHairs = FindObjectsOfType<HairPatch>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isMouseHeld = true;
            }

            if (Input.GetMouseButton(0) && _isMouseHeld && _currentTool)
            {
                Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 position = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
                _currentTool.Use(position, _allHairs);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isMouseHeld = false;
                _currentTool?.Released();
                OnToolReleased?.Invoke(_currentTool);
                _currentTool = null;
            }

            if (debugText)
            {
                var text = "";
                text += $"FPS: {Mathf.RoundToInt(1 / Time.smoothDeltaTime)}\n";
                text += $"Hairs: {_allHairs.Length}";
                debugText.text = text;
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