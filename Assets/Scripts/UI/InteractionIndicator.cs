using UnityEngine;
using UnityEngine.UI;
using UnityPazuTest.Tools;

namespace UnityPazuTest.UI
{
    public class InteractionIndicator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image image;

        private RectTransform _rectTransform;
        private Canvas _canvas;
        private ToolSO _currentTool;
        

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
        }

        private void OnEnable()
        {
            ToolManager.OnToolSelected += OnToolSelected;
            ToolManager.OnToolReleased += OnToolReleased;
        }

        private void OnDisable()
        {
            ToolManager.OnToolSelected -= OnToolSelected;
            ToolManager.OnToolReleased -= OnToolReleased;
        }

        private void Update()
        {
            if (!_currentTool || !image.enabled) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                Input.mousePosition,
                _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                out var localPoint
            );
            
            _rectTransform.localPosition = localPoint + _currentTool.GetIndicatorPositionOffset(localPoint);
            _rectTransform.localEulerAngles = _currentTool.GetIndicatorRotation(localPoint);
            _currentTool.UpdateTool(Time.deltaTime);
            image.sprite = _currentTool.GetCurrentSprite();
        }

        private void OnToolSelected(ToolSO tool)
        {
            if (!tool)  return;
            
            _currentTool = tool;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                Input.mousePosition,
                _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                out var localPoint
            );

            _rectTransform.localPosition = localPoint + tool.GetIndicatorPositionOffset(localPoint);
            _rectTransform.localEulerAngles = _currentTool.GetIndicatorRotation(localPoint);
            
            image.sprite = tool.GetCurrentSprite();
            image.enabled = true;
        }

        private void OnToolReleased(ToolSO tool)
        {
            image.enabled = false;
            _currentTool = null;
        }
    }
}