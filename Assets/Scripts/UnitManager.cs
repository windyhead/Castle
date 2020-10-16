using System;

namespace BuildACastle
 {
     using System.Collections;
     using System.Collections.Generic;
     using UnityEngine;
 
     public class UnitManager : MonoBehaviour
     {
         [SerializeField] private ObjectsLibrary objectsLibrary = default;
         [SerializeField] private UnitData _unitData = default;
         [SerializeField] private StartingVariables startingVariables = default;

         private void Awake()
         {
             CreateStartingUnits();
         }

         private void CreateStartingUnits()
         {
             foreach (var startingUnit in startingVariables.UnitsNumbers)
             {
                 foreach (var unit in objectsLibrary.UnitStats)
                 {
                     if (startingUnit.Rank == unit.rank)
                     {
                         for (int i = 0; i < startingUnit.Number; i++)
                         {
                             CreateUnit(unit);
                         }
                     }
                 }
             }
         }

         private void CreateUnit(UnitStats unitStats)
         {
             Unit newUnit = Instantiate(unitStats.prefab);
             newUnit.Init(unitStats);
             _unitData.AddUnit(newUnit);
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
     }
 }