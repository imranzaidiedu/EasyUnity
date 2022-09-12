using System;
using UnityEngine;

namespace BlueOakBridge.EasyUnity.Camera
{
    [Obsolete]
    [ExecuteAlways]
    [RequireComponent(typeof(SmartCamera))]
    public class CameraMinMaxSetter : MonoBehaviour
    {
        [SerializeField] Transform bottomLeft = null;
        [SerializeField] Transform topRight = null;

        SmartCamera _cam;
        SmartCamera Cam => _cam ? _cam : _cam = GetComponent<SmartCamera>();

        protected void Start() => SetMinMax();
        protected void OnValidate() => SetMinMax();

        private void SetMinMax()
        {
            if (bottomLeft) Cam.MinPos = bottomLeft.position;
            if (topRight) Cam.MaxPos = topRight.position;
        }

    }
}