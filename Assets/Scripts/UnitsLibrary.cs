namespace BuildACastle
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "UnitsLibrary", menuName = "UnitsLibrary", order = 1)]
    public class UnitsLibrary : ScriptableObject
    {
        public List<UnitStats> UnitStats = new List<UnitStats>();
    }
}