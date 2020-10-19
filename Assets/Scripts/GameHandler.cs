namespace BuildACastle
{
    using UnityEngine;

    public enum InputState
    {
        Selection,
        Construction,
        Rotation
    }

    public class GameHandler : MonoBehaviour
    {
        [SerializeField] private SelectionManager _selectionManager = default;
        [SerializeField] private UnitManager _unitManager = default;
        [SerializeField] private ConstructionManager _constructionManager = default;
        
        public InputState State { get; private set; }

        private void Awake()
        {
            State = InputState.Selection;
        }

        public void DetectSelection(RaycastHit hit, Vector3 cameraPoint)
        {
            Unit selectedUnit = hit.collider.gameObject.GetComponent<Unit>();
            Construct selectedConstruct = hit.collider.gameObject.GetComponent<Construct>();
            
            _selectionManager.EnableFrame(cameraPoint);
            
            if (selectedUnit != null)
                _selectionManager.Select(new Unit[] {selectedUnit});
            
            else if (selectedConstruct != null)
                _selectionManager.Select(selectedConstruct);
        }

        public void DetectGround(Vector3 cameraPoint)
        {
            _selectionManager.Clear();
            _selectionManager.EnableFrame(cameraPoint);
        }

        public void UpdateFrame(Vector3 mousePosition)
        {
            _selectionManager.UpdateFrame(mousePosition);
        }


        public void SelectionFinished(Vector3 mouseStart, Vector3 mouseReleased)
        {
            _selectionManager.DisableFrame();
            _selectionManager.Select(mouseStart, mouseReleased, _unitManager.GetUnits());
        }

        public void StartConstruction(ConstructType type)
        {
            State = InputState.Construction;
            _constructionManager.CreateBlueprint(type);
        }

        public void DetectConstructOrder(RaycastHit hit)
        {
            Construct orderedConstruct = hit.collider.GetComponent<Construct>();
            
            if (orderedConstruct != null)
            {
                if (orderedConstruct.IsReady && orderedConstruct.Stats.Type == ConstructType.Barracks)
                    _unitManager.MoveUnits(_selectionManager.SelectedUnits.ToArray(),
                        orderedConstruct.transform.position);
                else
                {
                    if (_selectionManager.SelectedUnits.Count == 0)
                        return;

                    _unitManager.SendUnitsForResources(_selectionManager.SelectedUnits.ToArray(),
                        orderedConstruct);
                }
            }
        }

        public void DetectGroundOrder(RaycastHit hit)
        {
            if (_selectionManager.SelectedUnits.Count == 0)
                    return;
                _unitManager.MoveUnits(_selectionManager.SelectedUnits.ToArray(), hit.point);
        }

        public void ChangeState()
        {
            switch (State)
            {
                case InputState.Selection:
                    State = InputState.Construction;
                    break;
                case InputState.Construction:
                    State = InputState.Rotation;
                    break;
                default:
                    State = InputState.Selection;
                    break;
            }
        }

        public void UpdateMousePosition(RaycastHit hit, Vector3 mousePosition)
        {
            
            if (State == InputState.Construction)
                _constructionManager.UpdateBlueprintPosition(mousePosition);
            
            else if (State == InputState.Rotation)
                _constructionManager.UpdateBlueprintRotation(hit.point);
        }
    }
}