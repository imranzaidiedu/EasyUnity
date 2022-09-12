using NoxLibrary;
using UnityEngine;

namespace BlueOakBridge.EasyUnity.Camera
{
    public partial class SmartCamera
    {
        protected Vector2 extent, aspect;
        protected float aspectRatio, windowAspect, cameraAspect;
        protected float targetOrtoSize, ortoTransitionTime, currentOrtoVelocity;

        [Tooltip("Minimum ortographic size the camera may have.")]
        [SerializeField] protected float minOrtoSize = 1f;

        [Tooltip("Maximum ortographic size the camera may have.")]
        [SerializeField] protected float maxOrtoSize = 3f;

        [Tooltip("Maximum ortographic change per second.")]
        [SerializeField] protected float maxOrtoSpeed = 1f;

        protected Vector2 MaxExtent => (MaxPos - minPos) / 2;
        protected float ScreenWidth => Screen.width;
        protected float ScreenHeight => Screen.height;
        protected float CameraWidth => ScreenWidth * myCamera.rect.width;
        protected float CameraHeight => ScreenHeight * myCamera.rect.height;

        public float OrtoSize
        {
            get => myCamera.orthographicSize;
            set
            {
                myCamera.orthographicSize = value.Clamp(MinOrtoSize, MaxOrtoSize);
                RecalculateExtent();
            }
        }
        public float MinOrtoSize
        {
            get => minOrtoSize;
            set
            {
                minOrtoSize = value.Clamp(float.Epsilon, MaxAllowedOrtoSize);
                maxOrtoSize = maxOrtoSize.Clamp(minOrtoSize, MaxAllowedOrtoSize);
                OrtoSize = OrtoSize;
            }
        }
        public float MaxOrtoSize
        {
            get => maxOrtoSize;
            set
            {
                maxOrtoSize = value.Clamp(float.Epsilon, MaxAllowedOrtoSize);
                minOrtoSize = minOrtoSize.Clamp(float.Epsilon, maxOrtoSize);
                OrtoSize = OrtoSize;
            }
        }
        public float MaxOrtoSpeed { get => maxOrtoSpeed; set => maxOrtoSpeed = value; }
        public float MaxAllowedOrtoSize => Mathf.Min(MaxExtent.x / cameraAspect, MaxExtent.y);

        /// <summary>
        /// Gets or sets the camera's Aspect Ratio
        /// </summary>
        public Vector2 Aspect { get => aspect; set { aspect = value; aspectRatio = ValidateAspectRatio(value); RecalculateExtent(); } }

        public void Zoom(float targetOrtoSize, float transitionTime)
        {
            this.targetOrtoSize = targetOrtoSize.Clamp(MinOrtoSize, MaxOrtoSize);
            this.ortoTransitionTime = transitionTime;
        }

        protected void OnWindowAspectChanged()
        {
            windowAspect = ScreenWidth / ScreenHeight;
            cameraAspect = CameraWidth / CameraHeight;
            MaxOrtoSize = MaxOrtoSize;
        }

        protected void RecalculateExtent()
        {
            extent = new Vector2(cameraAspect * OrtoSize, OrtoSize);
            trueMinPos = MinPos + extent;
            trueMaxPos = MaxPos - extent;
            MoveBy(0, 0);//Ensures the camera stays within it's bounds
        }

        /// <summary>
        /// Validates a given aspect ratio
        /// </summary>
        /// <param name="aspectRatio">Desired aspect ratio</param>
        /// <returns>Validated aspect ratio</returns>
        protected float ValidateAspectRatio(Vector2 aspectRatio) => (aspectRatio.x == 0 || aspectRatio.y == 0) ? (16f / 9f) : (aspectRatio.x / aspectRatio.y);

        protected void UpdateOrtoTransition()
        {
            if (ortoTransitionTime > 0f)
            {
                if (OrtoSize.Approximately(targetOrtoSize, 0.01f))
                {
                    OrtoSize = targetOrtoSize;
                    ortoTransitionTime = 0f;
                }
                else
                {
                    OrtoSize = Mathf.SmoothDamp(OrtoSize, targetOrtoSize, ref currentOrtoVelocity, ortoTransitionTime, maxOrtoSpeed, Time.deltaTime);
                }
            }
        }
    }
}