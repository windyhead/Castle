namespace BuildACastle
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ResourceManager : MonoBehaviour
    {
        [SerializeField] private RectTransform _ground = default;
        [SerializeField] private StartingVariables _startingVariables = default;
        [SerializeField] private ObjectsLibrary _objectsLibrary = default;
        private readonly List<Resource> _resources = new List<Resource>();
        private readonly List<Vector3> _randomPositions = new List<Vector3>();

        private void Awake()
        {
            GenerateResourcePositions();
            CreateStartingResources();
        }

        private void CreateStartingResources()
        {
            foreach (var startingResource in _startingVariables.ResourcesNumbers)
            {
                foreach (var resource in _objectsLibrary.Resources)
                {
                    if (startingResource.Type == resource.Type)
                    {
                        for (int i = 0; i < startingResource.Number; i++)
                        {
                            CreateResource(resource);
                        }
                    }
                }
            }
        }

        private void GenerateResourcePositions()
        {
            int positionsCount = 0;
            foreach (var resource in _startingVariables.ResourcesNumbers)
                positionsCount = +resource.Number;
            
            for (int i = 0; i < positionsCount; i++)
                _randomPositions.Add(GenerateRandomPosition());
        }

        private void CreateResource(Resource resource)
        {
            Resource newResource = Instantiate(resource);
            newResource.transform.position = _randomPositions[0];
            newResource.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            _randomPositions.RemoveAt(0);
            _resources.Add(newResource);
        }

        public Resource[] GetResources
            (ResourceType type)
        {
            List<Resource> foundedResources = new List<Resource>();
            foreach (var resource in _resources)
            {
                if (resource.Type == type)
                    foundedResources.Add(resource);
            }

            if (foundedResources.Count == 0)
            {
                Debug.Log($"no units of rank {type} was found");
                return null;
            }
            return foundedResources.ToArray();
        }

        private Vector3 GenerateRandomPosition()
        {
            var pos = _ground.position + new Vector3(Random.Range(-_ground.rect.size.x / 2, _ground.rect.size.x / 2),
                          0, Random.Range(-_ground.rect.size.x / 2, _ground.rect.size.x / 2));
            return pos;
        }
    }
}