using System.Collections.Generic;

namespace BuildACastle
 {
     using UnityEngine;
 
     public class UnitManager : MonoBehaviour
     {
         [SerializeField] private ConstructionManager _constructionManager = default;
         [SerializeField] private ObjectsLibrary objectsLibrary = default;
         [SerializeField] private UnitData _unitData = default;
         [SerializeField] private ResourceManager _resourceData = default;
         [SerializeField] private StartingVariables startingVariables = default;

         private void Awake()
         {
             CreateUnits(startingVariables.UnitsNumbers.ToArray(),this.transform.position);
             _constructionManager.OnHutConstructed += CreateUnits;
         }

         private void CreateUnits(UnitsNumber[] unitsNumbers,Vector3 position)
         {
             foreach (var startingUnit in unitsNumbers)
             {
                 foreach (var unit in objectsLibrary.UnitStats)
                 {
                     if (startingUnit.Rank == unit.rank)
                     {
                         for (int i = 0; i < startingUnit.Number; i++)
                         {
                             CreateUnit(unit,position);
                         }
                     }
                 }
             }
         }

         private void CreateUnit(UnitStats unitStats,Vector3 position)
         {
             Unit newUnit = Instantiate(unitStats.prefab);
             newUnit.transform.position = position;
             newUnit.Init(unitStats);
             _unitData.AddUnit(newUnit);
             newUnit.OnTaskFinished += SetNewTask;
         }
         
         public void  MoveUnits(Unit[] units,Vector3 destination)
         {
             foreach (var unit in units)
                 unit.Move(destination);
         }

         public Unit[] GetUnits()
         {
             return _unitData.GetUnits();
         }
         
         public void SendUnitsForResources( Unit[] selectedUnits,Construct construct)
         {
             List<ResourceType> resourcesToFind = new List<ResourceType>();
             foreach (var resource in construct.Resources.ToArray()) 
                 resourcesToFind.Add(resource);
             
             foreach (var unit in selectedUnits)
             {
                 List<float> resourceDistance = new List<float>();
                 Dictionary<float,Resource> distanceDictionary = new Dictionary<float, Resource>();
                 Debug.Log($"left {resourcesToFind.Count}");
                 Debug.Log($"selested {resourcesToFind[0]}");
                 var foundedResources = _resourceData.GetResources(resourcesToFind[0]);

                 foreach (var resource in foundedResources)
                 {
                     float distance = Vector3.Distance(unit.transform.position, resource.transform.position);
                     resourceDistance.Add(distance);
                     distanceDictionary.Add(distance,resource);
                 }
                 
                 resourceDistance.Sort();
                 unit.Move(distanceDictionary[resourceDistance[0]],construct);
                 Debug.Log($"removed {distanceDictionary[resourceDistance[0]].Type}");
                 _resourceData.UseResource(distanceDictionary[resourceDistance[0]]);
                 resourcesToFind.Remove(distanceDictionary[resourceDistance[0]].Type);
             }
         }

         private void SetNewTask(Unit unit)
         {
             SendUnitsForResources(new Unit[]{unit},unit.Construct );
         }
     }
 }