namespace BuildACastle
{
    
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    public class RubeUnit : Unit
    {

        public Action OnBuildFinished;
        public Resource HeldResource { get; private set; }
        public Construct BuildingConstruct { get; private set; }
        private const float BuildingTime = 5;


        public Resource SearchResource(Resource[] resourcesOfType)
        {
            List<float> resourceDistance = new List<float>();
            Dictionary<float, Resource> distanceDictionary = new Dictionary<float, Resource>();

            foreach (var resource in resourcesOfType)
            {
                float distance = Vector3.Distance(transform.position, resource.transform.position);
                resourceDistance.Add(distance);
                distanceDictionary.Add(distance, resource);
            }

            resourceDistance.Sort();
            return distanceDictionary[resourceDistance[0]];
        }
        public void TakeResource(Resource resource)
        {
            Debug.Log("dd");
            HeldResource = resource;
            HeldResource.OnResourceTaken?.Invoke(resource);
        }

        public void DropResource()
        {
            HeldResource.OnResourceDropped?.Invoke(HeldResource,transform.position);
            HeldResource = null;
        }

        public void UseResource(Construct construct)
        {
            construct.AddResource(HeldResource.Type);
            HeldResource.OnResourceUsed?.Invoke(HeldResource);
            HeldResource = null;
            BuildingConstruct = construct;
        }

        public void Build(Construct construct)
        {
            StartCoroutine(WaitForBuild(construct));
            BuildingConstruct = construct;
        }

        private IEnumerator WaitForBuild(Construct construct)
        {
           yield return new WaitForSeconds(BuildingTime); 
           construct.AddConstructionProgress();
           OnBuildFinished?.Invoke();
        }

    }
}
