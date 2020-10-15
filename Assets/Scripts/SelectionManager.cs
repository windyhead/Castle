namespace BuildACastle
{
    using System.Collections.Generic;
    using UnityEngine;

    public class SelectionManager : MonoBehaviour
    {
        private readonly List <Unit> _selected= new List<Unit>();

        public List<Unit> Selected => _selected;

        public void Clear()
        {
            foreach (var unit in _selected)
            {
                unit.Deselected();
            }
            _selected.Clear();
        }

        public void Select(Unit[] units)
        {
            foreach (var unit in units)
            {
                unit.Selected();
                _selected.Add(unit);
            }
        }
    }
}
