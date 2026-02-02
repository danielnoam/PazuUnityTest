using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityPazuTest.Tools;

namespace UnityPazuTest.UI
{
    public class ToolButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private ToolSO tool;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
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

        private void OnToolSelected(ToolSO selectedTool)
        {
            if (selectedTool == tool)
            {
                _image.enabled = false;
            }
        }
        
        private void OnToolReleased(ToolSO selectedTool)
        {
            if (selectedTool == tool)
            {
                _image.enabled = true;
            }

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (ToolManager.Instance && tool)
            {
                ToolManager.Instance.SelectTool(tool);
            }
        }
    }
}