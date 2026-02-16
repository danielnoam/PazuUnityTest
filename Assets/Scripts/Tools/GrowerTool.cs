using System.Collections.Generic;
using UnityEngine;
using UnityPazuTest.Character;

namespace UnityPazuTest.Tools
{
    [CreateAssetMenu(fileName = "Grower", menuName = "Tools/Grower Tool")]
    public class GrowerTool : ToolSO
    {
        [Header("Settings")]
        [SerializeField] private Sprite sprite;
        [SerializeField] private Vector2 indicatorOffset = Vector2.zero;
        [SerializeField] private float shakeSpeed = 35f;
        [SerializeField] private float shakeIntensity = 2f;
        [SerializeField] private float growthRadius = 0.5f;
        [SerializeField] private float growthRate = 0.02f;

        public override Vector2 GetIndicatorPositionOffset(Vector2 localMousePosition)
        {
            float shakeX = Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity;
            float shakeY = Mathf.Cos(Time.time * shakeSpeed * 1.3f) * shakeIntensity;
            return indicatorOffset + new Vector2(shakeX, shakeY);
        }

        public override Vector3 GetIndicatorRotation(Vector2 localMousePosition)
        {
            return Vector3.zero;
        }

        public override Sprite GetCurrentSprite()
        {
            return sprite;
        }

        public override void Use(Vector2 from, Vector2 to, List<HairPatch> hairPatches)
        {
            foreach (var patch in hairPatches)
            {
                patch.TryGrow(to, growthRadius, growthRate);
            }
        }
    }
}