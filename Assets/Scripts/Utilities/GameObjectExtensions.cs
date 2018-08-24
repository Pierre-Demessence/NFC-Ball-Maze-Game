using UnityEngine;

namespace Utilities
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject g) where T : Component
        {
            var component = g.GetComponent<T>();
            if (component == null) component = g.AddComponent<T>();
            return component;
        }
    }
}