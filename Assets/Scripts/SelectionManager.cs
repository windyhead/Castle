using System;

namespace BuildACastle
{
    using System.Collections.Generic;
    using UnityEngine;

    public class SelectionManager : MonoBehaviour
    {
        [SerializeField] private RectTransform _selectionFrame = default;
        
        public List<Unit> Selected { get; } = new List<Unit>();

        private void Awake()
        {
           DisableFrame();
        }

        public void Clear()
        {
            foreach (var unit in Selected)
            {
                unit.Deselected();
            }

            Selected.Clear();
        }

        public void Select(Unit[] units)
        {
            foreach (var unit in units)
            {
                unit.Selected();
                Selected.Add(unit);
            }
        }

        public void Select(Vector3 firstPosition, Vector3 secondPosition,Unit[] units)
        {
            if (firstPosition==secondPosition)
                return;
            
            List<Unit> newUnits = new List<Unit>();
            Rect frameRect = new Rect(firstPosition.x,firstPosition.y,secondPosition.x-firstPosition.x,secondPosition.y-firstPosition.y);

            foreach (var unit in units)
                if (frameRect.Contains(Camera.main.WorldToViewportPoint(unit.transform.position),true))
                    newUnits.Add(unit);

            Select(newUnits.ToArray());
        }

        public void EnableFrame()
        {
            _selectionFrame.gameObject.SetActive(true);
        }

        public void DisableFrame()
        {
            _selectionFrame.gameObject.SetActive(false);
        }

        public void UpdateFrame(Vector3 position, float sizeX, float sizeY)
        {
            _selectionFrame.position = position;
            _selectionFrame.sizeDelta = new Vector2(sizeX, sizeY);
        }
    }
}