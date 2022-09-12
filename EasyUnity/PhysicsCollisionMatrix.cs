using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlueOakBridge.EasyUnity
{
    public static class PhysicsCollisionMatrix
    {
        private static Dictionary<int, LayerMask> masksByLayer = new Dictionary<int, LayerMask>();

        public static LayerMask GetLayerMask(string layerName) => GetLayerMask(LayerMask.NameToLayer(layerName));
        public static LayerMask GetLayerMask(int layer)
        {
            if (!masksByLayer.TryGetValue(layer, out LayerMask layerMask))
            {
                layerMask.value = Enumerable.Range(0, 32)
                    .Where(i => !Physics.GetIgnoreLayerCollision(layer, i))
                    .Aggregate(0, (mask, i) => mask |= 1 << i);
                masksByLayer.Add(layer, layerMask);
            }
            return layerMask;
        }
    }
}
