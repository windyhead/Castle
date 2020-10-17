namespace BuildACastle
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "UnitsData", menuName = "UnitsData", order = 1)]
    public class UnitData : ScriptableObject
    {
        private readonly List<Unit> _units = new List<Unit>();

        public void AddUnit(Unit newUnit)
        {
            _units.Add(newUnit);
        }

        public void RemoveUnit(Unit unit)
        {
            _units.Remove(unit);
        }

        public Unit[] GetUnits()
        {
            return _units.ToArray();
        }

        public Unit[] GetUnits(UnitRank rank)
        {
            List<Unit> foundedUnits = new List<Unit>();
            foreach (var unit in _units)
            {
                if (unit.Rank == rank)
                    foundedUnits.Add(unit);
            }

            if (foundedUnits.Count == 0)
            {
                Debug.Log($"no units of rank {rank} was found");
                return null;
            }

            return foundedUnits.ToArray();
        }
    }
}