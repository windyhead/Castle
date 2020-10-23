using System.Linq;
using UnityEngine.PlayerLoop;

namespace BuildACastle
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    
    public class UnitManager : MonoBehaviour
    {
        public Action <Unit> OnUnitDestroy;
        public Action<List<Unit>,Construct> OnUnitIdle;
        [SerializeField] private ObjectsLibrary objectsLibrary = default;
        [SerializeField] private UnitData _unitData = default;

        public void CreateUnits(UnitsNumber[] unitsNumbers, Vector3 position)
        {
            foreach (var startingUnit in unitsNumbers)
                foreach (var unit in objectsLibrary.UnitStats)
                    if (startingUnit.type == unit.type)
                        for (int i = 0; i < startingUnit.Number; i++) 
                            CreateUnit(unit, position);
        }
        
        public Unit[] GetUnits()
        {
            return _unitData.GetUnits();
        }
        
        public void Move (Unit unit,Vector3 position)=>unit.AddOrder(new MoveOrder(position));

        public void Move (Unit[] units,Vector3 position)
        {
            foreach (var unit in units)
                unit.AddOrder(new MoveOrder(position));
        }

        public void BuildOrders(Unit unit,Construct construct)
        {
            Move(unit,construct.transform.position);
            Build(unit as RubeUnit, construct);
        }

        public void ResourceOrders(Unit unit,Construct construct,Resource resource)
        {
            Move(unit, resource.transform.position);
            TakeResource(unit as RubeUnit,resource);
            Move(unit,construct.transform.position);
            UseResource(unit as RubeUnit,resource,construct);
        }

        public void EnterOrders(Unit[] units,Construct construct)
        {
            Move(units, construct.transform.position);
            Enter(units, construct);
        }

        private void CreateUnit(UnitStats unitStats, Vector3 position)
        {
            var direction = UnityEngine.Random.insideUnitCircle.normalized*3;
            Vector3 generatedPosition = new Vector3(position.x+direction.x,position.y,position.z+direction.y);
            Unit newUnit = Instantiate(unitStats.Prefab);
            newUnit.transform.position = generatedPosition;
            newUnit.Init(unitStats);
            _unitData.AddUnit(newUnit);
            newUnit.OnOrdersFinished += ReturnToWork;
        }
        
        private void Enter (Unit[] units,Construct construct)
        {
            foreach (var unit in units)
            {
                unit.AddOrder(new EnterOrder(construct as Barracks));
                unit.OnEnter += DestroyUnit;
            }
        }

        private void TakeResource(RubeUnit rube,Resource resource)
        {
            rube.AddOrder(new TakeResourceOrder(resource));
        }

        private void UseResource(RubeUnit rube, Resource resource, Construct construct)
        {
            rube.AddOrder(new UseResourceOrder(resource,construct));
        }

        private void Build(RubeUnit rube, Construct construct)
        {
            rube.AddOrder(new BuildOrder(construct));
        }

        private void DestroyUnit(Unit unit)
        {
            Destroy(unit.gameObject);
            OnUnitDestroy?.Invoke(unit);
            _unitData.RemoveUnit(unit);
        }

        private void ReturnToWork(Unit unit)
        {
            if (!unit is RubeUnit)
                return;
            RubeUnit rube = unit as RubeUnit;
            if (rube.BuildingConstruct == null || rube.BuildingConstruct.IsReady)
                return;
            OnUnitIdle?.Invoke(new List<Unit>{rube},rube.BuildingConstruct);
        }
    }
}