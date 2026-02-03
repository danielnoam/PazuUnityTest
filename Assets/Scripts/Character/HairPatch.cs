using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityPazuTest.Character
{
    [SelectionBase]
    public class HairPatch : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float lengthChangeInterval = 0.02f;
        [SerializeField] private float spriteFlipInterval = 0.05f;
        [SerializeField, Min(0)] private float minLength;
        [SerializeField, Min(1)] private float maxLength = 2.75f;
        [SerializeField, Min(0)] private float currentLength = 3;

        [Header("References")]
        [SerializeField] private SpriteRenderer hairSprite;
        [SerializeField] private Transform hairPivot;

        private Vector3 _startRotation;
        private float _lengthChangeCooldown;
        private float _spriteFlipCooldown;
        
        private Vector2 HairRoot => hairPivot.position;
        private Vector2 HairTip => HairRoot + (Vector2)(hairPivot.right * currentLength);

        
        private const float NearZeroThreshold = 0.01f;
        
        
        private void OnValidate()
        {
            if (maxLength < minLength)
            {
                maxLength = minLength;
            }
            
            SetLength(currentLength);
        }

        private void Awake()
        {
            _startRotation = hairPivot.localEulerAngles;
            SetLength(currentLength);
        }

        private void Update()
        {
            if (_lengthChangeCooldown > 0)
            {
                _lengthChangeCooldown -= Time.deltaTime;
            }

            if (_spriteFlipCooldown > 0)
            {
                _spriteFlipCooldown -= Time.deltaTime;
            }
        }
        

        private void SetLength(float length)
        {
            currentLength = Mathf.Clamp(length, minLength, maxLength);
            hairSprite.transform.localScale = new Vector2(currentLength, hairSprite.transform.localScale.y);
        }
        
        private void SetRotation(float rotation)
        {
            hairPivot.localEulerAngles = new Vector3(_startRotation.x, _startRotation.y, rotation);
        }

        private float DistanceToHairLine(Vector2 point)
        {
            Vector2 line = HairTip - HairRoot;
            float lineLength = line.magnitude;
            

            if (lineLength < NearZeroThreshold)
            {
                return Vector2.Distance(point, HairRoot);
            }
    
            Vector2 lineDirection = line / lineLength;
            Vector2 toPoint = point - HairRoot;
            float projection = Vector2.Dot(toPoint, lineDirection);
    
            projection = Mathf.Clamp(projection, 0, lineLength);
            Vector2 closestPoint = HairRoot + lineDirection * projection;
    
            return Vector2.Distance(point, closestPoint);
        }
        
        
        public void TryCut(Vector2 scissorPosition, float distanceThreshold)
        {
            if (_lengthChangeCooldown > 0) return;
    
            Vector2 line = HairTip - HairRoot;
            float lineLength = line.magnitude;
            Vector2 lineDirection = line / lineLength;
            Vector2 toScissor = scissorPosition - HairRoot;
            float projection = Vector2.Dot(toScissor, lineDirection);

            if (projection < 0 || projection > lineLength) return;
    
            Vector2 closestPoint = HairRoot + lineDirection * projection;
            float distanceToLine = Vector2.Distance(scissorPosition, closestPoint);
    
            if (distanceToLine <= distanceThreshold)
            {
                float newLength = Mathf.Clamp(projection, minLength, maxLength);
                SetLength(newLength);
                _lengthChangeCooldown = lengthChangeInterval;
            }
        }

        public void TryGrow(Vector2 position, float radius, float amount)
        {
            if (_lengthChangeCooldown > 0) return;

            float distanceToHair = DistanceToHairLine(position);
            if (distanceToHair <= radius)
            {
                SetLength(currentLength + amount);
                _lengthChangeCooldown = lengthChangeInterval;
            }
        }

        public void Blow(Vector2 dryerPosition)
        {
            Vector2 directionFromDryer = HairRoot - dryerPosition;
            float angle = Vector2.SignedAngle(Vector2.right, directionFromDryer);
            SetRotation(angle);

            if (_spriteFlipCooldown <= 0)
            {
                hairSprite.flipY = !hairSprite.flipY;
                _spriteFlipCooldown = spriteFlipInterval + Random.Range(0f, spriteFlipInterval); 
            }
        }

    }
}