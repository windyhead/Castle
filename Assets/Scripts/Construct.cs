namespace BuildACastle
{

    using UnityEngine;
    public enum ConstructType
    {
        Hut,
        Barracks
    }

    [RequireComponent(typeof(Selectable))]
    public class Construct : MonoBehaviour
    {
        public ConstructStats Stats{ get; private set; }
        private Selectable _selectable;

        public void Init(ConstructStats stats)
        {
            _selectable = GetComponent<Selectable>();
            Stats = stats;
        }
        
        public void Selected() => _selectable.Selected();
        public void Deselected() => _selectable.DeSelected();
    }
}
