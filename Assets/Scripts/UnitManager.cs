using System;

namespace BuildACastle
 {
     using System.Collections;
     using System.Collections.Generic;
     using UnityEngine;
 
     public class UnitManager : MonoBehaviour
     {
         [SerializeField] private UnitsLibrary _unitsLibrary = default;
         [SerializeField] private UnitData _unitData = default;
         [SerializeField] private StartingUnits _startingUnits = default;

         private void Awake()
         {
             CreateStartingUnits();
         }

         private void CreateStartingUnits()
         {
             foreach (var startingUnit in _startingUnits.UnitsNumbers)
             {
                 foreach (var unit in _unitsLibrary.UnitStats)
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
     }
 }