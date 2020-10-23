namespace BuildACastle
{
    using UnityEngine;
    using System.Collections.Generic;

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
        [SerializeField] private ResourceManager _resourceManager = default;
        [SerializeField] private StartingVariables _startingVariables = default;

        public InputState State { get; private set; }

        private void Awake()
        {
            State = InputState.Selection;
            _constructionManager.OnHutConstructed += _unitManager.CreateUnits;
            _unitManager.OnUnitDestroy += _selectionManager.Deselect;
            _unitManager.OnUnitIdle += SendUnitsForConstruction;
        }

        private void Start()
        {
            _resourceManager.CreateStartingResources(_startingVariables.ResourcesNumbers.ToArray());
            _constructionManager.CreateStartingConstructs(_startingVariables.ConstructNumbers.ToArray());
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

            if (orderedConstruct == null || _selectionManager.SelectedUnits.Count == 0)
                return;
            
            if (orderedConstruct.Stats.Type == ConstructType.Barracks && orderedConstruct.IsReady)
                _unitManager.EnterOrders(_selectionManager.SelectedUnits.ToArray(), orderedConstruct);
            
            else if (!orderedConstruct.IsReady)
                SendUnitsForConstruction(_selectionManager.SelectedUnits, orderedConstruct);
            
            else
                _unitManager.Move(_selectionManager.SelectedUnits.ToArray(), orderedConstruct.transform.position);
        }

        public void DetectGroundOrder(RaycastHit hit)
        {
            if (_selectionManager.SelectedUnits.Count == 0)
                return;
            _unitManager.Move(_selectionManager.SelectedUnits.ToArray(), hit.point);
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

        private void SendUnitsForConstruction(List<Unit> selectedUnits,Construct construct)
        {
            int resourceCollected = construct.ResourcesCollected;
            List<ResourceType> resourcesToFind = new List<ResourceType>();
            
            foreach (var resource in construct.ResourcesNeeded.ToArray())
                resourcesToFind.Add(resource);
            
            if (resourcesToFind.Count == 0)
                return;
            
            foreach (var unit in selectedUnits)
            {
                if (unit.Type!=UnitType.Rube)
                    _unitManager.Move(unit, construct.transform.position);

                else if (resourceCollected > 0)
                {
                    _unitManager.BuildOrders(unit, construct);
                    resourceCollected--;
                }

                else
                {
                    var rube = unit as RubeUnit;
                    var selectedResource = rube.SearchResource(_resourceManager.GetResources(construct.ResourcesNeeded[0]));
                    
                    selectedResource.IsMarked = true;
                    resourcesToFind.Remove(selectedResource.Type);
                    _unitManager.ResourceOrders(unit,construct,selectedResource);
                }
            }
        }
    }
}