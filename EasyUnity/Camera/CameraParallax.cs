using System.Collections.Generic;
using UnityEngine;

namespace BlueOakBridge.EasyUnity.Camera
{
    public partial class SmartCamera
    {
        [SerializeField] protected Vector2 intensity;
        [SerializeField] protected Transform[] backgroundObjects;

        [Header("DEBUG")]
        protected Vector3[] originalPos;

        protected List<MovingObject> movingObjects = new List<MovingObject>();

        public void AddMovingObject(GameObject go) => movingObjects.Add(new MovingObject(go));

        protected void OnEnable() => SaveOriginalPositions();

        protected void SaveOriginalPositions()
        {
            originalPos = new Vector3[backgroundObjects.Length];
            for (int i = 0; i < backgroundObjects.Length; i++)
                originalPos[i] = backgroundObjects[i].transform.position;
        }

        protected void Update()
        {
            Vector2 relativePosition = ProportionalPosition; //Value between 0 and 1
            Vector2 posMultiplier = relativePosition.Add(-0.5f) * 2; //Value between -1 and 1
            posMultiplier *= intensity; //Value between -intensity and intensity

            for (int i = 0; i < backgroundObjects.Length; i++)
                UpdateObject(backgroundObjects[i], originalPos[i], posMultiplier);

            for (int i = 0; i < movingObjects.Count; i++)
            {
                if (movingObjects[i].go == null)
                    movingObjects.RemoveAt(i--);
                else
                    movingObjects[i].Update(posMultiplier);
            }
        }

        protected void UpdateObject(Transform t, Vector3 originalPos, Vector2 rPos)
        {
            if (t == null) return;

            Vector3 pos = t.position;
            pos.x = originalPos.x + rPos.x * (1 / originalPos.z);
            pos.y = originalPos.y + rPos.y * (1 / originalPos.z);
            t.position = pos;
        }

        protected class MovingObject
        {
            public GameObject go;
            public Vector2 lastPosMultiplier;

            public MovingObject(GameObject go)
            {
                this.go = go;
                lastPosMultiplier = Vector2.zero;
            }

            public void Update(Vector2 posMultiplier)
            {
                Vector2 posMultiplierDelta = posMultiplier - lastPosMultiplier;

                Vector3 pos = go.transform.position;
                pos.x += posMultiplierDelta.x * (1 / pos.z);
                pos.y += posMultiplierDelta.y * (1 / pos.z);
                go.transform.position = pos;

                lastPosMultiplier = posMultiplier;
            }
        }
    }
}