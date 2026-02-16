using System.Collections.Generic;
using UnityEngine;
using UnityPazuTest.Character;

namespace UnityPazuTest.Tools
{
    [CreateAssetMenu(fileName = "Dryer", menuName = "Tools/Dryer Tool")]
    public class DryerTool : ToolSO
    {
        [Header("Settings")]
        [SerializeField] private Sprite sprite;
        [SerializeField] private float spriteRotationOffset = -40;
        [SerializeField] private float tiltSensitivity = 0.05f;
        [SerializeField] private float flipCooldown = 0.2f;


        public override Vector2 GetIndicatorPositionOffset(Vector2 localMousePosition)
        {
            return Vector2.zero;
        }

        public override Vector3 GetIndicatorRotation(Vector2 localMousePosition)
        {
            float yOffset = localMousePosition.y * -tiltSensitivity;
            float rotationY = localMousePosition.x > 0 ? 180f : 0f;
    
            return new Vector3(0, rotationY, spriteRotationOffset + yOffset);
        }
        

        public override Sprite GetCurrentSprite()
        {
            return sprite;
        }

        public override void Use(Vector2 from, Vector2 to, List<HairPatch> hairPatches)
        {
            foreach (var patch in hairPatches)
            {
                patch.Blow(to, flipCooldown);
            }
        }
        
    }
}