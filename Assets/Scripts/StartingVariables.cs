namespace BuildACastle
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "StartingUnits", menuName = "StartingUnits", order = 1)]
    public class StartingVariables : ScriptableObject
    {
        public List<UnitsNumber> UnitsNumbers = new List<UnitsNumber>();
        public List<ResourceNumber> ResourcesNumbers = new List<ResourceNumber>();
        public List<ConstructNumber> ConstructNumbers = new List<ConstructNumber>();
    }
}