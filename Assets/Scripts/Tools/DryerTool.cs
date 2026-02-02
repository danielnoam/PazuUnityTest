using UnityEngine;

namespace UnityPazuTest.Tools
{
    [CreateAssetMenu(fileName = "Dryer", menuName = "Tools/Dryer Tool")]
    public class DryerTool : ToolSO
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private Vector2 indicatorOffset = Vector2.zero;
        [SerializeField] private float rotationSpeed = 55f;

        private float _currentRotation;

        public override Vector2 GetIndicatorPositionOffset(Vector2 localMousePosition)
        {
            return indicatorOffset;
        }

        public override float GetIndicatorRotation(Vector2 localMousePosition)
        {
            var lookDir = localMousePosition.x > 0 ? 90 : -90;
            
            Vector2 screenCenter = Vector2.zero;
            Vector2 directionToCenter = screenCenter - localMousePosition;
            float targetAngle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
            _currentRotation = Mathf.LerpAngle(_currentRotation, targetAngle, rotationSpeed * Time.deltaTime);

            return _currentRotation;
        }
        

        public override Sprite GetIndicatorSprite()
        {
            return sprite;
        }

        public override void Use(Vector2 position, Character.HairPatch[] hairs)
        {
            foreach (var hair in hairs)
            {
                hair.Blow(position);
            }
        }

        public override void Selected()
        {
            _currentRotation = 0f;
        }

        public override void Released()
        {
        }
    }
}