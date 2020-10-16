namespace BuildACastle
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ObjectsLibrary", menuName = "ObjectsLibrary", order = 1)]
    public class ObjectsLibrary : ScriptableObject
    {
        public List<UnitStats> UnitStats = new List<UnitStats>();
        public List<Resource> Resources = new List<Resource>();
        public List<ConstructStats> ConstructStats = new List<ConstructStats>();
    }
}