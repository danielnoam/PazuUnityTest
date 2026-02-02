using UnityEngine;
using UnityEngine.UI;
using UnityPazuTest.Tool;

namespace UnityPazuTest.UI
{
    public class InteractionIndicator : MonoBehaviour
    {
        [SerializeField, Min(1)] private float followSpeed = 20f;
        [SerializeField] private Vector3 offset = Vector3.zero;
        [SerializeField] private Image image;
        
        [SerializeField] private Sprite dryerSprite;
        [SerializeField] private Sprite scissorsSprite;
        [SerializeField] private Sprite growerSprite;

        private RectTransform _rectTransform;
        private Canvas _canvas;

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
            FollowMouse();
        }

        private void OnToolSelected(ToolType toolType)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                Input.mousePosition,
                _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                out var localPoint
            );

            Vector3 targetPosition = localPoint + (Vector2)offset;
            _rectTransform.localPosition = targetPosition;
            
            
            switch (toolType)
            {
                case ToolType.Dryer:
                    image.sprite = dryerSprite;
                    image.enabled = true;
                    break;

                case ToolType.Scissors:
                    image.sprite = scissorsSprite;
                    image.enabled = true;
                    break;

                case ToolType.Grower:
                    image.sprite = growerSprite;
                    image.enabled = true;
                    break;

                case ToolType.None:
                    image.enabled = false;
                    break;
            }
        }

        private void OnToolReleased(ToolType toolType)
        {
            image.enabled = false;
        }

        private void FollowMouse()
        {
            if (!image.enabled) return;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                Input.mousePosition,
                _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                out var localPoint
            );

            Vector3 targetPosition = localPoint + (Vector2)offset;

            _rectTransform.localPosition = Vector3.Lerp(_rectTransform.localPosition, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}