using System;
using UnityEngine;
using UnityPazuTest.Tools;
using Random = UnityEngine.Random;

namespace UnityPazuTest.Character
{
    [SelectionBase]
    public class HairPatch : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, Min(0)] private float minLength;
        [SerializeField, Min(1)] private float maxLength = 2.75f;
        [SerializeField, Min(0)] private float currentLength = 3;

        [Header("References")]
        [SerializeField] private SpriteRenderer hairSprite;
        [SerializeField] private Transform hairPivot;

        private Vector3 _startRotation;
        private float _flipCooldownTimer;
        
        private Vector2 HairRoot => hairPivot.position;
        private Vector2 HairTip => hairPivot.TransformPoint(Vector3.right * currentLength);

        
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

        private void Start()
        {
            ToolManager.Instance?.AddHairPatch(this);
        }

        private void OnDestroy()
        {
            ToolManager.Instance?.RemoveHairPatch(this);
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
        
        
        public void TryCut(Vector2 scissorFrom, Vector2 scissorTo, float distanceThreshold)
        {
            Vector2 hairRoot = HairRoot;
            Vector2 hairTip = HairTip;

            float closestDist = SegmentToSegmentDistance(
                scissorFrom, scissorTo, 
                hairRoot, hairTip, 
                out float hairT
            );

            if (closestDist > distanceThreshold) return;
            
            float hairWorldLength = Vector2.Distance(hairRoot, hairTip);
            float cutWorldDist = hairT * hairWorldLength;
            float scale = Mathf.Max(hairPivot.lossyScale.x, 0.001f);
            float localProjection = cutWorldDist / scale;

            SetLength(Mathf.Clamp(localProjection, minLength, maxLength));
        }

        public void TryGrow(Vector2 position, float radius, float amount)
        {
            float distanceToHair = DistanceToHairLine(position);
            if (distanceToHair <= radius)
            {
                SetLength(currentLength + amount);
            }
        }

        public void Blow(Vector2 dryerPosition, float flipCooldown)
        {
            Vector2 directionFromDryer = HairRoot - dryerPosition;
            float angle = Vector2.SignedAngle(Vector2.right, directionFromDryer);
            SetRotation(angle);

            _flipCooldownTimer -= Time.deltaTime;
            if (_flipCooldownTimer <= 0f)
            {
                _flipCooldownTimer = Random.Range(flipCooldown * 0.5f, flipCooldown * 1.5f);
                hairSprite.flipY = !hairSprite.flipY;
            }
        }
        
        
                
        /// Finds the shortest distance between two line segments (A and B).
        /// Returns the distance plus hairT (0..1 parameter along segment B, the hair).
        ///
        /// Each segment is parameterized as:
        ///   pointOnA = a1 + tA * (a2 - a1),  tA in [0,1]
        ///   pointOnB = b1 + tB * (b2 - b1),  tB in [0,1]
        ///
        /// We solve for tA and tB that minimize the distance between the two points.
        /// The unclamped solution comes from setting the partial derivatives of 
        /// the squared distance to zero, which gives a 2x2 linear system.
        /// Then we clamp to [0,1] since these are segments, not infinite lines.
        private static float SegmentToSegmentDistance(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out float tB)
        {
            Vector2 dA = a2 - a1; // direction of segment A (scissor sweep)
            Vector2 dB = b2 - b1; // direction of segment B (hair)
            Vector2 r = a1 - b1;  // vector between segment starts

            float a = Vector2.Dot(dA, dA); // squared length of A
            float e = Vector2.Dot(dB, dB); // squared length of B
            float f = Vector2.Dot(dB, r);

            float tA;

            // Degenerate cases: one or both segments are effectively points
            if (a < NearZeroThreshold && e < NearZeroThreshold)
            {
                tA = 0f;
                tB = 0f;
            }
            else if (a < NearZeroThreshold)
            {
                // Segment A is a point, just project it onto B
                tA = 0f;
                tB = Mathf.Clamp01(f / e);
            }
            else
            {
                float c = Vector2.Dot(dA, r);
                
                if (e < NearZeroThreshold)
                {
                    // Segment B is a point, project it onto A
                    tB = 0f;
                    tA = Mathf.Clamp01(-c / a);
                }
                else
                {
                    // General case: both segments have length
                    float b = Vector2.Dot(dA, dB);
                    float denom = a * e - b * b; // zero when segments are parallel

                    // If not parallel, solve for tA on the infinite lines
                    tA = denom > NearZeroThreshold ? Mathf.Clamp01((b * f - c * e) / denom) : 0f;

                    // Compute tB from tA
                    tB = (b * tA + f) / e;

                    // If tB is outside [0,1], clamp it and recompute tA
                    if (tB < 0f)
                    {
                        tB = 0f;
                        tA = Mathf.Clamp01(-c / a);
                    }
                    else if (tB > 1f)
                    {
                        tB = 1f;
                        tA = Mathf.Clamp01((b - c) / a);
                    }
                }
            }

            Vector2 closestOnA = a1 + dA * tA;
            Vector2 closestOnB = b1 + dB * tB;
            return Vector2.Distance(closestOnA, closestOnB);
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(HairRoot, HairTip);
            Gizmos.DrawSphere(HairRoot, 0.05f);
            Gizmos.DrawSphere(HairTip, 0.05f);
        }
    }
}