using UnityEngine;

namespace BlueOakBridge.EasyUnity.Camera
{
    public partial class SmartCamera
    {
        [Tooltip("Speed the camera will move at while dragging manually")]
        [SerializeField] protected float dragSpeed;
        public float DragSpeed { get => dragSpeed; set => dragSpeed = value; }

        /// <summary>
        /// A boolean indicating wether the player is manually moving the camera.
        /// </summary>
        public bool MovingManually { get; private set; }

        /// <summary>
        /// Gets user input and moves the camera accordingly
        /// </summary>
        protected void GetInput()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                OrtoSize -= Input.mouseScrollDelta.y * (MaxOrtoSize / 10);
                ortoTransitionTime = 0f;
            }

            if (MovingManually = Input.GetMouseButton(1))
            {
                TargetObject = null;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                ortoTransitionTime = 0f;
                MoveBy(-Input.GetAxis("Mouse X") * DragSpeed * (OrtoSize / 10) * Time.unscaledDeltaTime,
                    -Input.GetAxis("Mouse Y") * DragSpeed * (OrtoSize / 10) * Time.unscaledDeltaTime);
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}