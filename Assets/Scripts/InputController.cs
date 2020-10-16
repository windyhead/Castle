namespace BuildACastle
{
    using UnityEngine;

    public enum InputState
    {
        Selection,
        Construction,
        Rotation
    }

    public class InputController : MonoBehaviour
    {
        [SerializeField] private SelectionManager _selectionManager = default;
        [SerializeField] private UnitManager _unitManager = default;
        [SerializeField] private ConstructionManager _constructionManager = default;

        private const string GroundTag = "ground";
        private const string SelectableLayer = "selectable";

        private InputState _state;

        private Camera _mainCamera;

        private Vector3 _clickPosition;
        private Vector3 _releasedPosition;
        private Vector3 _mouseClickPosition;
        private Vector3 _mouseReleasedPosition;
        private Vector3 _mosePosition;

        private float LastPosX;
        private float LastPosY;
        private float LastPosZ;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _state = InputState.Selection;
        }

        private void Update()
        {
            switch (_state)
            {
                case InputState.Selection:
                {
                    if (Input.GetMouseButtonDown(0))
                        SelectionLeftMouseClick();
                    if (Input.GetMouseButton(0))
                        SelectionLeftMouseHold();
                    if (Input.GetMouseButtonUp(0))
                        SelectionLeftMouseReleased();
                    if (Input.GetMouseButtonDown(1))
                        SelectionRightMouseClick();
                    break;
                }
                case InputState.Construction:
                {
                    UpdateConstructionPosition();
                    if (Input.GetMouseButtonDown(0))
                       ConstructionLeftMouseClick();
                    if (Input.GetMouseButtonUp(0))
                        ConstructionLeftMouseRelease();
                    break;
                }
                case InputState.Rotation:
                {
                    UpdateConstructionPosition();
                    if (Input.GetMouseButtonDown(0))
                        RotatingLeftMouseClick();
                    break;
                }
                    
            }
        }

        private void SelectionLeftMouseClick()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask(SelectableLayer)))
            {
                _selectionManager.EnableFrame();
                _clickPosition = hit.point;
                _mouseClickPosition = _mainCamera.ScreenToViewportPoint(Input.mousePosition);

                Unit selectedUnit = hit.collider.gameObject.GetComponent<Unit>();

                if (hit.collider.tag == GroundTag)
                    _selectionManager.Clear();

                else if (selectedUnit != null)
                {
                    _selectionManager.Clear();
                    _selectionManager.Select(new Unit[] {selectedUnit});
                }
            }
        }

        private void SelectionRightMouseClick()
        {
            if (_selectionManager.Selected.Count == 0)
                return;
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask(SelectableLayer)))
            {
                if (hit.collider.tag == GroundTag)
                    _unitManager.MoveUnits(_selectionManager.Selected.ToArray(), hit.point);
            }
        }

        private void SelectionLeftMouseHold()
        {
            _releasedPosition = Input.mousePosition;

            Vector3 frameStart = _mainCamera.WorldToScreenPoint(_clickPosition);
            frameStart.z = 0f;

            Vector3 centerPosition = (frameStart + _releasedPosition) / 2;
            float frameSizeX = Mathf.Abs(frameStart.x - _releasedPosition.x);
            float frameSizeY = Mathf.Abs(frameStart.y - _releasedPosition.y);
            _selectionManager.UpdateFrame(centerPosition, frameSizeX, frameSizeY);
        }

        private void SelectionLeftMouseReleased()
        {
            _selectionManager.DisableFrame();
            _mouseReleasedPosition = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
            _selectionManager.Select(_mouseClickPosition, _mouseReleasedPosition, _unitManager.GetUnits());
        }

        private void ConstructionLeftMouseClick()
        {
            _constructionManager.StartConstruction();
            _state = InputState.Rotation;
        }

        private void RotatingLeftMouseClick()
        {
            _constructionManager.UpdateBlueprintRotation(new Vector3(LastPosX,0,LastPosZ));
        }

        private void ConstructionLeftMouseRelease()
        {
             
              _state = InputState.Construction;
        }

        private void UpdateConstructionPosition()
        {
            _mosePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(_mosePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask(SelectableLayer)))
            {
                float PosX = hit.point.x;
                float PosY = hit.point.y;
                float PosZ = hit.point.z;

                if (PosX != LastPosX || PosY != LastPosY || PosZ != LastPosZ)
                {
                    LastPosX = PosX;
                    LastPosY = PosY;
                    LastPosZ = PosZ;
                    _constructionManager.UpdateBlueprintPosition(new Vector3(LastPosX, LastPosY + .5f, LastPosZ));
                }
            }
        }
    }
}