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
        public Action<Resource> OnTaken;
        public Action<Resource, Vector3> OnDropped;
        public Action<Resource> OnUsed;
        public ResourceType Type = default;
        public bool IsMarked { get; private set; }

        public void Taken() => OnTaken?.Invoke(this);
        public void Dropped(Vector3 droppedPosition) => OnDropped?.Invoke(this, droppedPosition);
        public void Used() => OnUsed?.Invoke(this);

        public void Marked() => IsMarked = true;
        public void UnMarked() => IsMarked = false;
    }
}