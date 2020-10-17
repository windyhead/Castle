namespace BuildACastle
{
    using UnityEngine;
    using System;

    public class ConstructionManager : MonoBehaviour
    {
        public Action<UnitsNumber[], Vector3> OnHutConstructed;
        [SerializeField] private StartingVariables _startingVariables = default;
        [SerializeField] private ObjectsLibrary _objectsLibrary = default;
        [SerializeField] private Construct _blueprintPrefab;
       
        private Construct _blueprint;

        private void Start()
        {
            CreateStartingConstructs();
        }

        private void CreateStartingConstructs()
        {
            foreach (var startingConstruct in _startingVariables.ConstructNumbers)
            {
                foreach (var construct in _objectsLibrary.ConstructStats)
                {
                    if (startingConstruct.Type == construct.Type)
                    {
                        for (int i = 0; i < startingConstruct.Number; i++)
                        {
                            CreateConstruct(construct);
                        }
                    }
                }
            }
        }

        private void CreateConstruct(ConstructStats constructStats)
        {
            Construct newConstruct = Instantiate(constructStats.prefab);
            newConstruct.Init(constructStats);
            OnHutConstructed?.Invoke(constructStats.Spawn.ToArray(), this.transform.position);
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

            _blueprint = Instantiate(_blueprintPrefab, this.transform.position, Quaternion.identity);
            _blueprint.Init(constructStats);
            _blueprint.OnConstructionFinished += FinishConstruction;
        }

        public void UpdateBlueprintPosition(Vector3 newPosition)
        {
            _blueprint.transform.position = newPosition;
        }

        public void UpdateBlueprintRotation(Vector3 rotationPoint)
        {
            _blueprint.transform.LookAt(new Vector3(rotationPoint.x, _blueprint.transform.position.y, rotationPoint.z));
        }
        
        private void FinishConstruction(Construct construct)
        {
            Construct newConstruct = Instantiate(construct.Stats.prefab);
            newConstruct.transform.position = construct.transform.position;
            newConstruct.transform.rotation = construct.transform.rotation;
            Destroy(_blueprint.gameObject);
            OnHutConstructed?.Invoke(construct.Stats.Spawn.ToArray(), this.transform.position);
        }
    }
}