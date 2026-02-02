using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityPazuTest.Tool;

namespace UnityPazuTest.UI
{
    public class ToolButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private ToolType toolType;
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


        private void OnToolSelected(ToolType toolType)
        {
            if (toolType == this.toolType)
            {
                _image.enabled = false;
            }
        }
        
        private void OnToolReleased(ToolType toolType)
        {
            if (toolType == this.toolType)
            {
                _image.enabled = true;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (ToolManager.Instance)
            {
                ToolManager.Instance.SelectTool(toolType);
            }
        }
    }
}