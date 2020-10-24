namespace BuildACastle
{
    using System.Collections;
    using UnityEngine;
    using System;

    public class RubeUnit : Unit
    {
        public Action OnBuildFinished;
        private Resource _heldResource;
        private Resource _markedResource;
        public Construct BuildingConstruct { get; private set; }
        private const float BuildingTime = 5;


        public override void NewOrder(Order newOrder)
        {
            if (_markedResource != null)
                Unmark();

            if (_heldResource != null)
                DropResource();

            base.NewOrder(newOrder);
        }

        public void TakeResource(Resource resource)
        {
            _heldResource = resource;
            _heldResource.Taken();
        }

        public void Mark(Resource resource)
        {
            _markedResource = resource;
            _markedResource.Marked();
        }

        private void Unmark()
        {
            _markedResource.UnMarked();
            _markedResource = null;
        }

        public void UseResource(Construct construct)
        {
            construct.AddResource(_heldResource.Type);
            _heldResource.Used();
            _heldResource = null;
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

        private void DropResource()
        {
            _heldResource.Dropped(transform.position);
            _heldResource = null;
        }
    }
}