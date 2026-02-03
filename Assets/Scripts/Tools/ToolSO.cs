using UnityEngine;
using UnityPazuTest.Character;

namespace UnityPazuTest.Tools
{
    public abstract class ToolSO : ScriptableObject
    {
        public abstract Sprite GetCurrentSprite();
        public abstract void Use(Vector2 position, HairPatch[] hairs);
        public abstract Vector2 GetIndicatorPositionOffset(Vector2 localMousePosition);
        public abstract Vector3 GetIndicatorRotation(Vector2 localMousePosition);
        
        
        public virtual void Selected() { }
        public virtual void Released() { }
        public virtual void UpdateTool(float deltaTime) { }
    }
}