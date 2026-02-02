using UnityEngine;
using UnityPazuTest.Character;

namespace UnityPazuTest.Tools
{
    public abstract class ToolSO : ScriptableObject
    {
        public abstract Sprite GetIndicatorSprite();
        public abstract void Use(Vector2 position, HairPatch[] hairs);
        public abstract Vector2 GetIndicatorPositionOffset(Vector2 localMousePosition);
        public abstract float GetIndicatorRotation(Vector2 localMousePosition);
        public abstract void Selected();
        public abstract void Released();
    }
}