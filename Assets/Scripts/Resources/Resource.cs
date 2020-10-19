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
        public ResourceType Type  = default;
        private Collider _collider;

        private void Awake()=>_collider = GetComponent<Collider>();
    }
}
