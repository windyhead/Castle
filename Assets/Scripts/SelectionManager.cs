using System;

namespace BuildACastle
{
    using System.Collections.Generic;
    using UnityEngine;

    public class SelectionManager : MonoBehaviour
    {
        [SerializeField] private RectTransform _selectionFrame = default;

        public List<Unit> SelectedUnits { get; } = new List<Unit>();
        public Construct SelectedConstruct { get; private set; }

        private Vector3 _frameStart;
        private Unit selectedUnit;
        private Construct selectedConstruct;

        private void Awake()
        {
            DisableFrame();
        }

        public void Clear()
        {
            foreach (var unit in SelectedUnits)
            {
                unit.Deselected();
            }

            if (SelectedConstruct != null)
            {
                SelectedConstruct.Deselected();
                SelectedConstruct = null;
            }

            SelectedUnits.Clear();
        }

        public void Select(Unit[] units)
        {
            Clear();
            foreach (var unit in units)
            {
                unit.Selected();
                SelectedUnits.Add(unit);
            }
        }

        public void Select(Construct construct)
        {
            Clear();
            SelectedConstruct = construct;
            SelectedConstruct.Selected();
            Debug.Log("Selected");
        }

        public void Select(Vector3 firstPosition, Vector3 secondPosition, Unit[] units)
        {
            if (firstPosition == secondPosition)
                return;

            List<Unit> newUnits = new List<Unit>();
            Rect frameRect = new Rect(firstPosition.x, firstPosition.y, secondPosition.x - firstPosition.x,
                secondPosition.y - firstPosition.y);

            foreach (var unit in units)
                if (frameRect.Contains(Camera.main.WorldToViewportPoint(unit.transform.position), true))
                    newUnits.Add(unit);

            Select(newUnits.ToArray());
        }


        public void EnableFrame(Vector3 frameStart)
        {
            _frameStart = frameStart;
            _selectionFrame.gameObject.SetActive(true);
        }

        public void DisableFrame()
        {
            _selectionFrame.gameObject.SetActive(false);
        }

        public void UpdateFrame(Vector3 mousePosition)
        {
            _frameStart.z = 0f;

            Vector3 centerPosition = (_frameStart + mousePosition) / 2;
            float frameSizeX = Mathf.Abs(_frameStart.x - mousePosition.x);
            float frameSizeY = Mathf.Abs(_frameStart.y - mousePosition.y);
            
            
            _selectionFrame.position = centerPosition;
            _selectionFrame.sizeDelta = new Vector2(frameSizeX, frameSizeY);
        }
    }
}