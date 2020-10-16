namespace BuildACastle
{
    using UnityEngine;
    using System.Collections.Generic;
    
    [CreateAssetMenu(fileName = "ConstructStats", menuName = "ConstructStats", order = 1)]
    public class ConstructStats : ScriptableObject
    {
        public ConstructType Type;
        public List<ResourceNumber>  Resources = new List<ResourceNumber>();
        public List<UnitsNumber> Spawn = new List<UnitsNumber>();
        public Construct prefab;
    }
}