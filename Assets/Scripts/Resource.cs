using System;

namespace BuildACastle
{
    using UnityEngine;
    public enum ResourceType
    {
        Wood,
        Stone
    }
    [RequireComponent(typeof(Collider))]
    public class Resource : MonoBehaviour
    {
        public ResourceType Type { get; } = default;
        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponents<Collider>()[0];
        }
    }
}
