using System;
using UnityEngine;

namespace BlueOakBridge.EasyUnity.Camera
{
    [Obsolete]
    [RequireComponent(typeof(UnityEngine.Camera))]
    public partial class SmartCamera : MonoBehaviour
    {
        public UnityEngine.Camera myCamera;

        protected static SmartCamera main;
        public static SmartCamera Main => main ? main : main = UnityEngine.Camera.main.GetComponent<SmartCamera>();

        public bool IsMain => Main.Equals(this);

        private void Awake()
        {
            myCamera = GetComponent<UnityEngine.Camera>();
            OnWindowAspectChanged();
        
        }

        private void LateUpdate()
        {
            if (windowAspect != ScreenWidth / ScreenHeight)
                OnWindowAspectChanged();

            // GetInput();
            UpdateOrtoTransition();
            DoFollow();
            Shake();
        }
    }
}
