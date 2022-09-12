using UnityEngine;

namespace BlueOakBridge.EasyUnity.Camera
{
    public partial class SmartCamera
    {
        protected Vector2 trueMinPos, trueMaxPos;
        protected Vector3 currentVelocity;

        protected bool followObject, followPosition;
        protected GameObject targetObject;
        protected Vector3? targetPosition;

        [Tooltip("Maximum speed the camera can move at")]
        [SerializeField] protected float maxSpeed;

        [Tooltip("An offset to be respected while following a target")]
        [SerializeField] protected Vector3 offset;

        [Tooltip("Minimum position the camera's borders can reach (bottom-left)")]
        [SerializeField] protected Vector2 minPos = new Vector2(-10, -10);

        [Tooltip("Maximum position the camera's borders can reach (top-right)")]
        [SerializeField] protected Vector2 maxPos = new Vector2(10, 10);

        /// <summary>
        /// Gets or sets a target for the camera to follow
        /// </summary>
        public GameObject TargetObject
        {
            get => targetObject;
            set
            {
                targetObject = value;
                followObject = value;
                followPosition = followPosition && !followObject;
            }
        }

    

        /// <summary>
        /// Gets or sets a target for the camera to move to
        /// </summary>
        public Vector3? TargetPosition
        {
            get => targetPosition;
            set
            {
                targetPosition = value;
                followPosition = value.HasValue;
                followObject = followObject && !followPosition;
            }
        }

        /// <summary>
        /// Approximately the time it will take to reach the target. A smaller value will reach the target faster.
        /// </summary>
        public float SmoothTime { get; set; }

        public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
        public Vector2 MinPos { get => minPos; set => minPos = value; }
        public Vector2 MaxPos { get => maxPos; set => maxPos = value; }
        public Vector3 Offset { get => offset; set => offset = value; }

        /// <summary>
        /// Get's the camera's position in world space.
        /// </summary>
        public Vector2 CurrentPosition => transform.position;

        /// <summary>
        /// Gets the camera's position in relation to it's bounds. Return value is 0 at minimumPos, and 1 at maximumPos.
        /// </summary>
        public Vector2 ProportionalPosition => new Vector2((transform.position.x - MinPos.x) / (MaxPos.x - MinPos.x),
            (transform.position.y - MinPos.y) / (MaxPos.y - MinPos.y));

        /// <summary>
        /// Shorthand for setting Target and SmoothTime
        /// </summary>
        /// <param name="target">Gameobject to follow</param>
        /// <param name="smoothTime">Follow time (lower value = more speed)</param>
        public void Follow(GameObject target, float smoothTime = 0)
        {
            // if (MovingManually) return;
            TargetObject = target;
            SmoothTime = smoothTime;
        }

        /// <summary>
        /// Moves the camera by a given distance.
        /// </summary>
        /// <param name="x">Distance to move the camera in the horizontal direction</param>
        /// <param name="y">Distance to move the camera in the vertical direction</param>
        public void MoveBy(Vector2 v) => MoveTo(new Vector3(
            Mathf.Clamp(transform.position.x + v.x, trueMinPos.x, trueMaxPos.x),
            Mathf.Clamp(transform.position.y + v.y, trueMinPos.y, trueMaxPos.y),
            transform.position.z), smooth: false);

        /// <summary>
        /// Moves the camera by a given distance.
        /// </summary>
        /// <param name="x">Distance to move the camera in the horizontal direction</param>
        /// <param name="y">Distance to move the camera in the vertical direction</param>
        public void MoveBy(float x, float y) => MoveTo(new Vector3(
            Mathf.Clamp(transform.position.x + x, trueMinPos.x, trueMaxPos.x),
            Mathf.Clamp(transform.position.y + y, trueMinPos.y, trueMaxPos.y),
            transform.position.z), smooth: false);

        /// <summary>
        /// Moves the camera to a given position
        /// </summary>
        /// <param name="x">Desired final position on the horizontal axis</param>
        /// <param name="y">Desired final position on the vertical axis</param>
        public void MoveTo(float x, float y, bool smooth = true) => MoveTo(new Vector3(x, y, transform.position.z), smooth: smooth);

        /// <summary>
        /// Moves the camera to a given position
        /// </summary>
        /// <param name="pos">Final position of the camera</param>
        /// <param name="ignoreZ">Defines whether the camera should also be moved on the Z axis</param>
        /// <param name="lerp">Defines the point where to lerp. Set to 1 to effectively disable lerping.</param>
        public void MoveTo(Vector2 pos, bool smooth = true)
            => MoveTo(pos, true, smooth);

        /// <summary>
        /// Moves the camera to a given position
        /// </summary>
        /// <param name="pos">Final position of the camera</param>
        /// <param name="ignoreZ">Defines whether the camera should also be moved on the Z axis</param>
        /// <param name="lerp">Defines the point where to lerp. Set to 1 to effectively disable lerping.</param>
        public void MoveTo(Vector3 pos, bool ignoreZ = false, bool smooth = true)
        {
            pos.x = Mathf.Clamp(pos.x, trueMinPos.x, trueMaxPos.x);
            pos.y = Mathf.Clamp(pos.y, trueMinPos.y, trueMaxPos.y);
            if (ignoreZ) pos.z = transform.position.z;
            if (smooth && SmoothTime > 0)
                transform.position = Vector3.SmoothDamp(transform.position, pos, ref currentVelocity, SmoothTime, maxSpeed, Time.deltaTime);
            else transform.position = pos;
        }

        protected void DoFollow()
        {
            if (followObject && TargetObject != null)
                MoveTo(TargetObject.transform.position + offset);
            if (followPosition && targetPosition.HasValue)
                MoveTo(targetPosition.Value + offset);
        }
    }
}
