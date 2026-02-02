using UnityEngine;

namespace UnityPazuTest.Tools
{
    [CreateAssetMenu(fileName = "Grower", menuName = "Tools/Grower Tool")]
    public class GrowerTool : ToolSO
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private Vector2 indicatorOffset = Vector2.zero;
        [SerializeField] private float shakeSpeed = 35f;
        [SerializeField] private float shakeIntensity = 0.5f;
        [SerializeField] private float growerDistanceThreshold = 0.5f;
        [SerializeField] private float growerAmount = 0.02f;

        public override Vector2 GetIndicatorPositionOffset(Vector2 localMousePosition)
        {
            float shakeX = Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity;
            float shakeY = Mathf.Cos(Time.time * shakeSpeed * 1.3f) * shakeIntensity;
            return indicatorOffset + new Vector2(shakeX, shakeY);
        }

        public override float GetIndicatorRotation(Vector2 localMousePosition)
        {
            return 0f;
        }

        public override Sprite GetIndicatorSprite()
        {
            return sprite;
        }

        public override void Use(Vector2 position, Character.HairPatch[] hairs)
        {
            foreach (var hair in hairs)
            {
                hair.TryGrow(position, growerDistanceThreshold, growerAmount);
            }
        }

        public override void Selected()
        {
        }

        public override void Released()
        {
        }
    }
}