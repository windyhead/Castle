﻿namespace BuildACastle
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ResourceManager : MonoBehaviour
    {
        [SerializeField] private Terrain _ground = default;
        [SerializeField] private ObjectsLibrary _objectsLibrary = default;

        private readonly List<Resource> resources = new List<Resource>();
        private readonly List<Vector3> _randomPositions = new List<Vector3>();

        public void CreateStartingResources(ResourceNumber[] resourceNumbers)
        {
            GenerateResourcePositions(resourceNumbers);

            foreach (var startingResource in resourceNumbers)
            foreach (var resource in _objectsLibrary.Resources)
                if (startingResource.Type == resource.Type)
                    for (int i = 0; i < startingResource.Number; i++)
                        CreateResource(resource);
        }

        private void GenerateResourcePositions(ResourceNumber[] resourceNumbers)
        {
            int positionsCount = 0;
            foreach (var resource in resourceNumbers)
                positionsCount += resource.Number;

            for (int i = 0; i < positionsCount; i++)
                _randomPositions.Add(GenerateRandomPosition());
        }
        
        public Resource[] GetResources(ResourceType type)
        {
            List<Resource> foundedResources = new List<Resource>();
            foreach (var resource in resources)
                if (resource.Type == type && !resource.IsMarked)
                    foundedResources.Add(resource);

            if (foundedResources.Count == 0)
            {
                Debug.Log($"no units of rank {type} was found");
                return null;
            }

            return foundedResources.ToArray();
        }

        private void CreateResource(Resource resource)
        {
            Resource newResource = Instantiate(resource);
            newResource.transform.position = _randomPositions[0];
            newResource.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            newResource.OnTaken += ResourceTaken;
            newResource.OnDropped += ResourceDropped;
            newResource.OnUsed += ResourceUsed;

            _randomPositions.RemoveAt(0);
            resources.Add(newResource);
        }

        private Vector3 GenerateRandomPosition()
        {
            var xPosition = _ground.transform.position.x + Random.Range(0, _ground.terrainData.size.x);
            var zPosition = _ground.transform.position.z + Random.Range(0, _ground.terrainData.size.z);
            var pos = new Vector3(xPosition, _ground.SampleHeight(new Vector3(xPosition, 0, zPosition)), +zPosition);
            return pos;
        }

        private void ResourceTaken(Resource resource) => resource.gameObject.SetActive(false);

        private void ResourceDropped(Resource resource, Vector3 position)
        {
            resource.gameObject.SetActive(true);
            resource.transform.position = position;
            resource.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }

        private void ResourceUsed(Resource resource)
        {
            resources.Remove(resource);
            Destroy(resource.gameObject);
        }
    }
}