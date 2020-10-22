namespace BuildACastle
{
    using UnityEngine;
    using System;

    public class ConstructionManager : MonoBehaviour
    {
        public Action<UnitsNumber[], Vector3> OnHutConstructed;
        [SerializeField] private ObjectsLibrary _objectsLibrary = default;
        
        private Construct _newConstruct;
        
        public void CreateStartingConstructs(ConstructNumber[] constructNumbers)
        {
            foreach (var startingConstruct in constructNumbers)
                foreach (var construct in _objectsLibrary.ConstructStats)
                    if (startingConstruct.Type == construct.Type)
                        for (int i = 0; i < startingConstruct.Number; i++)
                            CreateConstruct(construct);
        }

        private void CreateConstruct(ConstructStats constructStats)
        {
            Construct newConstruct = Instantiate(constructStats.prefab);
            newConstruct.Init(constructStats);
            newConstruct.FinishConstruction();
            FinishConstruction(newConstruct);
        }

        public void CreateBlueprint(ConstructType type)
        {
            ConstructStats constructStats = null;
            foreach (var construct in _objectsLibrary.ConstructStats)
            {
                if (construct.Type == type)
                    constructStats = construct;
            }

            if (constructStats == null)
            {
                Debug.Log($" construct of type {type} was not found in library");
                return;
            }

            _newConstruct = Instantiate(constructStats.prefab, this.transform.position, Quaternion.identity);
            _newConstruct.Init(constructStats);
            _newConstruct.OnConstructionFinished += FinishConstruction;
        }

        public void UpdateBlueprintPosition(Vector3 newPosition)=>
            _newConstruct.transform.position = newPosition;
        

        public void UpdateBlueprintRotation(Vector3 rotationPoint)=>_newConstruct.transform.LookAt
            (new Vector3(rotationPoint.x, _newConstruct.transform.position.y, rotationPoint.z));
        
        
        private void FinishConstruction(Construct construct)
        {
            OnHutConstructed?.Invoke(construct.Stats.Spawn.ToArray(), construct.transform.position); 
            construct.OnConstructionFinished -= FinishConstruction;
        }
    }
}