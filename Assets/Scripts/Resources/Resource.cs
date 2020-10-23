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
        public Action <Resource> OnResourceTaken;
        public Action <Resource,Vector3 > OnResourceDropped;
        public Action <Resource> OnResourceUsed;
        public ResourceType Type  = default;
        private Collider _collider;

        public bool IsMarked { get; set; }

        private void Awake()=>_collider = GetComponent<Collider>();
    }
}
