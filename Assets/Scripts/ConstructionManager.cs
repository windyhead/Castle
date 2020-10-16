namespace BuildACastle
{
    using UnityEngine;

    public class ConstructionManager : MonoBehaviour
    {
        
        [SerializeField] private StartingVariables startingVariables = default;
        [SerializeField] private ObjectsLibrary objectsLibrary = default;
        
        private Transform ObjToMove;
        private Construct ObjToPlace;
        private Construct _builtObject;
        
        private void Awake()
        {
            CreateStartingConstructs();
        }

        private void CreateStartingConstructs()
        {
            foreach (var startingConstruct in startingVariables.ConstructNumbers)
            {
                foreach (var construct in objectsLibrary.ConstructStats)
                {
                    if (startingConstruct.Type == construct.Type)
                    {
                        for (int i = 0; i < startingConstruct.Number; i++)
                        {
                            CreateUnit(construct);
                        }
                    }
                }
            }
        }

        private void CreateUnit(ConstructStats constructStats)
        {
            Construct newConstruct = Instantiate(constructStats.prefab);
            newConstruct.Init(constructStats);
        }
        
        public void StartConstruction()
        {
            Construct building = Instantiate(ObjToPlace,ObjToMove.position,Quaternion.identity);
            _builtObject = building;
        }

        public void UpdateBlueprintPosition(Vector3 newPosition)
        {
            ObjToMove.position = newPosition;
        }

        public void UpdateBlueprintRotation(Vector3 rotationPoint)
        {
            _builtObject.transform.LookAt(new Vector3(rotationPoint.x,_builtObject.transform.position.y,rotationPoint.z));
        }

        public void CreateConstruction()
        {
            
        }

        public void CreateBluePrint()
        {
            
        }

        public void FinishConstruction()
        {
            
        }
    }
}
