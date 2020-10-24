namespace BuildACastle
 {
     using UnityEngine;
 
     [CreateAssetMenu(fileName = "UnitStats", menuName = "UnitStats", order = 1)]
     public class UnitStats : ScriptableObject
     {
         public UnitType type;
         public string UnitName;
         public float Speed;
         public Unit Prefab;
     }
 }