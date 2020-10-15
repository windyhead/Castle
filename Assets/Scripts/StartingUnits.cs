namespace BuildACastle
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "StartingUnits", menuName = "StartingUnits", order = 1)]
    public class StartingUnits : ScriptableObject
    {
        public List<UnitsNumber> UnitsNumbers = new List<UnitsNumber>();
    }
}