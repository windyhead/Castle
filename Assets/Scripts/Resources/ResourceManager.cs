namespace BuildACastle
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ResourceManager : MonoBehaviour
    {
        [SerializeField] private Terrain _ground = default;
        [SerializeField] private StartingVariables _startingVariables = default;
        [SerializeField] private ObjectsLibrary _objectsLibrary = default;

        private readonly List<Resource> _unusedResources = new List<Resource>();
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
                positionsCount += resource.Number;

            for (int i = 0; i < positionsCount; i++)
                _randomPositions.Add(GenerateRandomPosition());
        }

        private void CreateResource(Resource resource)
        {
            Resource newResource = Instantiate(resource);
            newResource.transform.position = _randomPositions[0];
            newResource.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            _randomPositions.RemoveAt(0);
            _unusedResources.Add(newResource);
        }

        public Resource[] GetResources(ResourceType type)
        {
            Debug.Log($"Searching for {type}");
            List<Resource> foundedResources = new List<Resource>();
            foreach (var resource in _unusedResources)
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

        public void UseResource(Resource usedResource)
        {
            _unusedResources.Remove(usedResource);
        }

        private Vector3 GenerateRandomPosition()
        {
            var xPosition = _ground.transform.position.x + Random.Range(0, _ground.terrainData.size.x);
            var zPosition = _ground.transform.position.z + Random.Range(0, _ground.terrainData.size.z);
            var pos = new Vector3(xPosition, _ground.SampleHeight(new Vector3(xPosition, 0, zPosition)), +zPosition);
            return pos;
        }
    }
}