namespace BuildACastle
{
    using UnityEngine;
    using System;

    public enum ResourceType
    {
        Wood,
        Stone
    }
    [RequireComponent(typeof(Collider))]
    public class Resource : MonoBehaviour
    {
        public Action OnMarked;
        public ResourceType Type  = default;
        private bool _isMarked;
        private Collider _collider;

        public bool IsMarked
        {
            get => _isMarked;
            set
            {
                _isMarked = value;
                OnMarked?.Invoke();
            }
        }

        private void Awake()=>_collider = GetComponent<Collider>();
        
    }
}
