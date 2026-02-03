using UnityEngine;
using UnityPazuTest.Character;

namespace UnityPazuTest.Tools
{
    [CreateAssetMenu(fileName = "Scissors", menuName = "Tools/Scissors Tool")]
    public class ScissorsTool : ToolSO
    {
        [Header("Settings")]
        [SerializeField] private Sprite[] animationFrames;
        [SerializeField] private float animationSpeed = 6f;
        [SerializeField] private Vector2 indicatorOffset = Vector2.zero;
        [SerializeField] private float cutDistanceThreshold = 0.02f;
        
        private float _animationTimer;
        private int _currentFrame;


        public override Vector2 GetIndicatorPositionOffset(Vector2 localMousePosition)
        {
            float xMultiplier = localMousePosition.x > 0 ? 1f : -1f;
            return new Vector2(indicatorOffset.x * xMultiplier, indicatorOffset.y);
        }

        public override Vector3 GetIndicatorRotation(Vector2 localMousePosition)
        {
            return new Vector3(0, 0 ,localMousePosition.x > 0 ? 90 : -90);
        }

        public override Sprite GetCurrentSprite()
        {
            if (animationFrames == null || animationFrames.Length == 0) return null;
            return animationFrames[_currentFrame];
        }

        public override void Use(Vector2 position, HairPatch[] hairs)
        {
            foreach (var hair in hairs)
            {
                hair.TryCut(position, cutDistanceThreshold);
            }
        }

        public override void UpdateTool(float deltaTime)
        {
            if (animationFrames is not { Length: > 1 }) return;

            _animationTimer += deltaTime;
            if (_animationTimer >= 1f / animationSpeed)
            {
                _animationTimer = 0f;
                _currentFrame = (_currentFrame + 1) % animationFrames.Length;
            }
        }

        public override void Selected()
        {
            _animationTimer = 0f;
            _currentFrame = 0;
        }
    }
}