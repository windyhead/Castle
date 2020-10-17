namespace BuildACastle
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "UnitStats", menuName = "UnitStats", order = 1)]
    public class UnitStats : ScriptableObject
    {
        public UnitRank rank;
        public float speed;
        public Unit prefab;
    }
}