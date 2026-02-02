using UnityEngine;

namespace UnityPazuTest.Tools
{
    [CreateAssetMenu(fileName = "Scissors", menuName = "Tools/Scissors Tool")]
    public class ScissorsTool : ToolSO
    {
        [SerializeField] private Sprite[] animationFrames;
        [SerializeField] private float animationSpeed = 6f;
        [SerializeField] private Vector2 indicatorOffset = Vector2.zero;
        [SerializeField] private float cutDistanceThreshold = 0.01f;
        
        private float _animationTimer;
        private int _currentFrame;

        public override Vector2 GetIndicatorPositionOffset(Vector2 localMousePosition)
        {
            float xMultiplier = localMousePosition.x > 0 ? 1f : -1f;
            return new Vector2(indicatorOffset.x * xMultiplier, indicatorOffset.y);
        }

        public override float GetIndicatorRotation(Vector2 localMousePosition)
        {
            return localMousePosition.x > 0 ? 90 : -90;
        }

        public override Sprite GetIndicatorSprite()
        {
            if (animationFrames == null || animationFrames.Length == 0)
                return null;

            _animationTimer += Time.deltaTime;
            if (_animationTimer >= 1f / animationSpeed)
            {
                _animationTimer = 0;
                _currentFrame = (_currentFrame + 1) % animationFrames.Length;
            }
            
            return animationFrames[_currentFrame];
        }

        public override void Use(Vector2 position, Character.HairPatch[] hairs)
        {
            foreach (var hair in hairs)
            {
                hair.TryCut(position, cutDistanceThreshold);
            }
        }

        public override void Selected()
        {
            _currentFrame = 0;
            _animationTimer = 0;
        }

        public override void Released()
        {
            _currentFrame = 0;
        }
    }
}