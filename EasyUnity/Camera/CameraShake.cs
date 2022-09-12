using UnityEngine;

namespace BlueOakBridge.EasyUnity.Camera
{
    public partial class SmartCamera
    {
        /// <summary>
        /// Time in seconds until the screen stops shaking completly
        /// </summary>
        public float ShakeDuration { get => ShakeMagnitude / ShakeFade; set => ShakeFade = ShakeMagnitude / value; }
        /// <summary>
        /// How much magnitude is lost per second during the shake
        /// </summary>
        public float ShakeFade { get; set; }
        /// <summary>
        /// How far the camera can go from the origin
        /// </summary>
        public float ShakeMagnitude { get; set; }

        /// <summary>
        /// The center position for the camera shake
        /// </summary>
        public Vector2 ShakeOrigin { get; set; }

        /// <summary>
        /// Makes the camera shake for a while.
        /// </summary>
        /// <param name="duration">The duration in seconds.</param>
        /// <param name="magnitude">The variation in position.</param>
        /// <param name="relativeRoughness">The rooughness of the shake.</param>
        public void Shake(float magnitude = 1f, float duration = 1f)
        {
            ShakeOrigin = CurrentPosition;
            ShakeMagnitude = magnitude;
            ShakeDuration = duration;
        }

        protected void Shake()
        {
            if (ShakeDuration > 0)
            {
                if (followObject && TargetObject != null)
                    ShakeOrigin = TargetObject.transform.position;
                if (followPosition && targetPosition.HasValue)
                    ShakeOrigin = targetPosition.Value;

                Vector2 frameShake = new Vector2((Random.Range(- ShakeMagnitude,  ShakeMagnitude)),
                    (Random.Range(- ShakeMagnitude,  ShakeMagnitude)));

                ShakeMagnitude -= ShakeFade * Time.deltaTime;

                MoveTo(ShakeOrigin + frameShake, false);
            }
        }
    }
}
