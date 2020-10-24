namespace BuildACastle
{
    using UnityEngine.EventSystems;
    using UnityEngine;

    public class InputController : MonoBehaviour
    {
        [SerializeField] private GameHandler _gameHandler = default;
        [SerializeField] private EventSystem _eventSystem = default;
       
        public float _moveCameraSpeed;
        public float _zoomCameraSpeed;
 
        public float minCameraZoom;
        public float maxCameraZoom;
        
        private const string SelectableLayer = "selectable";
        private const string GroundLayer = "ground";

        private Camera _mainCamera;
        private RaycastHit _hit;

        private Vector3 _mouseClickPosition;
        private Vector3 _mouseReleasedPosition;

        private float _lastMousePosX;
        private float _lastMousePosY;
        private float _lastMousePosZ;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            MoveCamera();
            ZoomCamera();
            
            switch (_gameHandler.State)
            {
                case InputState.Selection:
                {
                    if (Input.GetMouseButtonDown(0))
                        SelectionLeftMouseClick();
                    else if (Input.GetMouseButton(0))
                        SelectionLeftMouseHold();
                    else if (Input.GetMouseButtonUp(0))
                        SelectionLeftMouseReleased();
                    else if (Input.GetMouseButtonDown(1))
                        SelectionRightMouseClick();
                    break;
                }
                default:
                {
                    UpdateMousePosition();

                    if (Input.GetMouseButtonDown(0))
                        _gameHandler.ChangeState();
                    break;
                }
            }
        }
        
        private void MoveCamera ()
        {
            var direction = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
            _mainCamera.transform.position += direction * _moveCameraSpeed * Time.deltaTime;
        }
        
        private void ZoomCamera ()
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            float distance = Vector3.Distance(transform.position, _mainCamera.transform.position);
 
            if(distance < minCameraZoom && scrollInput > 0)
                return;
            if(distance > maxCameraZoom && scrollInput < 0)
                return;
            _mainCamera.transform.position += _mainCamera.transform.forward * scrollInput * _zoomCameraSpeed;
        }

        private void SelectionLeftMouseClick()
        {
            if (RayCastLayer(SelectableLayer))
            {
                _mouseClickPosition = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
                _gameHandler.DetectSelection(_hit, _mainCamera.WorldToScreenPoint(_hit.point));
            }

            else if (RayCastLayer(GroundLayer))
            {
                _mouseClickPosition = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
                _gameHandler.DetectGround(_mainCamera.WorldToScreenPoint(_hit.point));
            }
        }

        private void SelectionLeftMouseHold()
        {
            _gameHandler.UpdateFrame(Input.mousePosition);
        }

        private void SelectionLeftMouseReleased()
        {
            _mouseReleasedPosition = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
            _gameHandler.SelectionFinished(_mouseClickPosition, _mouseReleasedPosition);
        }

        private void SelectionRightMouseClick()
        {
            if (RayCastLayer(SelectableLayer))
                _gameHandler.DetectConstructOrder(_hit);
            else if (RayCastLayer(GroundLayer))
                _gameHandler.DetectGroundOrder(_hit);
        }

        private void UpdateMousePosition()
        {
            if (RayCastLayer(GroundLayer) || RayCastLayer(SelectableLayer))
            {
                float PosX = _hit.point.x;
                float PosY = _hit.point.y;
                float PosZ = _hit.point.z;

                if (PosX != _lastMousePosX || PosY != _lastMousePosY || PosZ != _lastMousePosZ)
                {
                    _lastMousePosX = PosX;
                    _lastMousePosY = PosY;
                    _lastMousePosZ = PosZ;
                    _gameHandler.UpdateMousePosition(_hit, new Vector3(_lastMousePosX, _lastMousePosY, _lastMousePosZ));
                }
            }
        }

        private bool RayCastLayer(string layer)
        {
            if (_eventSystem.IsPointerOverGameObject())
                return false;

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask(layer)))
            {
                _hit = hit;
                return true;
            }

            return false;
        }
    }
}