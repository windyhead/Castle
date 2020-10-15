namespace BuildACastle
{
    using UnityEngine;

    public class InputController : MonoBehaviour
    {
        [SerializeField] private Collider ground = default;
        [SerializeField] private SelectionManager _selectionManager = default;
        [SerializeField] private UnitManager _unitManager = default;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                LeftMouseClick();
            if (Input.GetMouseButtonDown(1))
                RightMouseClick();
        }

        private void LeftMouseClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit,100))
            {
                Unit selectedUnit = hit.collider.gameObject.GetComponent<Unit>();
                
                if (hit.collider == ground)
                    _selectionManager.Clear();

                else if (selectedUnit != null)
                {
                    _selectionManager.Clear();
                    _selectionManager.Select(new Unit[]{selectedUnit});
                    
                }
            }
        }
        
        private void RightMouseClick()
        {
            if(_selectionManager.Selected.Count == 0)
                return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit,100))
            {
                if (hit.collider == ground)
                    _unitManager.MoveUnits(_selectionManager.Selected.ToArray(), hit.point);
            }
        }

    }
}
