namespace BuildACastle
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    public class UnitManager : MonoBehaviour
    {
        public Action<Unit> OnUnitDestroy;
        public Action<List<Unit>, Construct> OnUnitIdle;

        [SerializeField] private ObjectsLibrary objectsLibrary = default;
        [SerializeField] private UnitData _unitData = default;

        public void CreateUnits(UnitsNumber[] unitsNumbers, Vector3 position)
        {
            foreach (var spawnedUnit in unitsNumbers)
            foreach (var stats in objectsLibrary.UnitStats)
                if (spawnedUnit.type == stats.type)
                    for (int i = 0; i < spawnedUnit.Number; i++)
                        CreateUnit(stats, position);
        }

        public Unit[] GetUnits()
        {
            return _unitData.GetUnits();
        }

        public void Move(Unit unit, Vector3 position)
        {
            unit.NewOrder(new MoveOrder(position));
        }

        public void Move(Unit[] units, Vector3 position)
        {
            foreach (var unit in units)
            {
                unit.NewOrder(new MoveOrder(position));
            }
        }

        public void BuildOrders(RubeUnit rube, Construct construct)
        {
            rube.NewOrder(new MoveOrder(construct.transform.position));
            rube.AddOrder(new BuildOrder(construct));
        }

        public void ResourceOrders(RubeUnit rube, Construct construct, Resource resource)
        {
            rube.NewOrder(new MoveOrder(resource.transform.position));
            rube.Mark(resource);
            rube.AddOrder(new TakeResourceOrder(resource));
            rube.AddOrder(new MoveOrder(construct.transform.position));
            rube.AddOrder(new UseResourceOrder(construct));
        }

        public void EnterOrders(Unit[] units, Construct construct)
        {
            Move(units, construct.transform.position);
            Enter(units, construct);
        }

        public Resource SelectResourceByDistance(RubeUnit rube, Resource[] resourcesOfType)
        {
            List<float> resourceDistance = new List<float>();
            Dictionary<float, Resource> distanceDictionary = new Dictionary<float, Resource>();

            foreach (var resource in resourcesOfType)
            {
                float distance = Vector3.Distance(rube.transform.position, resource.transform.position);
                resourceDistance.Add(distance);
                distanceDictionary.Add(distance, resource);
            }

            resourceDistance.Sort();
            return distanceDictionary[resourceDistance[0]];
        }

        private void CreateUnit(UnitStats unitStats, Vector3 position)
        {
            var direction = UnityEngine.Random.insideUnitCircle.normalized * 6;
            Vector3 generatedPosition = new Vector3(position.x + direction.x, position.y, position.z + direction.y);
            Unit newUnit = Instantiate(unitStats.Prefab);
            newUnit.transform.position = generatedPosition;
            newUnit.Init(unitStats);
            _unitData.AddUnit(newUnit);
            newUnit.OnOrdersFinished += ReturnToWork;
        }

        private void Enter(Unit[] units, Construct construct)
        {
            foreach (var unit in units)
            {
                unit.AddOrder(new EnterOrder(construct as Barracks));
                unit.OnEnter += DestroyUnit;
            }
        }

        private void DestroyUnit(Unit unit)
        {
            Destroy(unit.gameObject);
            OnUnitDestroy?.Invoke(unit);
            _unitData.RemoveUnit(unit);
        }

        private void ReturnToWork(Unit unit)
        {
            if (unit.Type != UnitType.Rube)
                return;
            RubeUnit rube = unit as RubeUnit;
            if (rube.BuildingConstruct == null || rube.BuildingConstruct.IsReady)
                return;
            OnUnitIdle?.Invoke(new List<Unit> {rube}, rube.BuildingConstruct);
        }
    }
}