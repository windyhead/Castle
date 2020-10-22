using System.Linq;

namespace BuildACastle
{
    using UnityEngine;
    using System.Collections.Generic;
    using System;
    
    public class UnitManager : MonoBehaviour
    {
        public Action <Unit> OnUnitDestroy;
        [SerializeField] private ObjectsLibrary objectsLibrary = default;
        [SerializeField] private UnitData _unitData = default;

        public void CreateUnits(UnitsNumber[] unitsNumbers, Vector3 position)
        {
            foreach (var startingUnit in unitsNumbers)
                foreach (var unit in objectsLibrary.UnitStats)
                    if (startingUnit.Rank == unit.Rank)
                        for (int i = 0; i < startingUnit.Number; i++) 
                            CreateUnit(unit, position);
        }

        private void CreateUnit(UnitStats unitStats, Vector3 position)
        {
            var direction = UnityEngine.Random.insideUnitCircle.normalized*3;
            Vector3 generatedPosition = new Vector3(position.x+direction.x,position.y,position.z+direction.y);
            Unit newUnit = Instantiate(unitStats.Prefab);
            newUnit.transform.position = generatedPosition;
            newUnit.Init(unitStats);
            _unitData.AddUnit(newUnit);
        }
        
        public void Move (Unit[] units,Vector3 position)
        {
            foreach (var unit in units)
                unit.AddOrder(new MoveOrder(position));
        }
        
        public void Enter (Unit[] units,Construct construct)
        {
            foreach (var unit in units)
            {
                unit.AddOrder(new EnterOrder(construct as Barracks));
                unit.OnEnter += RemoveUnit;
            }
        }
        
        
        public Unit[] GetUnits()
        {
            return _unitData.GetUnits();
        }

        private void RemoveUnit(Unit unit)
        {
            Destroy(unit.gameObject);
            OnUnitDestroy?.Invoke(unit);
            _unitData.RemoveUnit(unit);
            
        }

        /*public void SendUnitsForResources(Unit[] selectedUnits, Construct construct, Resource[] foundedResources)
         {
             List<ResourceType> resourcesToFind = new List<ResourceType>();
             foreach (var resource in construct.Resources.ToArray())
                 resourcesToFind.Add(resource);
 
             foreach (var unit in selectedUnits)
             {
                 List<float> resourceDistance = new List<float>();
                 Dictionary<float, Resource> distanceDictionary = new Dictionary<float, Resource>();
 
                 foreach (var resource in foundedResources)
                 {
                     float distance = Vector3.Distance(unit.transform.position, resource.transform.position);
                     resourceDistance.Add(distance);
                     distanceDictionary.Add(distance, resource);
                 }
 
                 resourceDistance.Sort();
                // unit.Move(distanceDictionary[resourceDistance[0]], construct);
                 Debug.Log($"removed {distanceDictionary[resourceDistance[0]].Type}");
                 distanceDictionary[resourceDistance[0]].IsMarked = true;
                 resourcesToFind.Remove(distanceDictionary[resourceDistance[0]].Type);
             }
         }*/
    }
}