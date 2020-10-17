using System;
using UnityEditorInternal;
using UnityEngine.AI;

namespace BuildACastle
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public enum UnitRank
    {
        Rube,
        Soldier
    }

    [RequireComponent(typeof(Selectable),typeof(NavMeshAgent),typeof(Collider))]
    public class Unit : MonoBehaviour
    {
        public Action <Unit> OnTaskFinished;
        public UnitRank Rank { get; private set; }
        private Selectable _selectable;
        private NavMeshAgent _navMeshAgent;
        private Resource _heldResource;
        public Construct Construct { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            var resource = other.gameObject.GetComponent<Resource>();
            var construct = other.gameObject.GetComponent<Construct>();
            if (resource != null && resource == _heldResource)
            {
                Destroy(resource.gameObject);
                ReturnToConstruct();
            }
            else if (construct != null && construct == Construct)
            {
                if (construct.IsReady && construct.Stats.Type == ConstructType.Barracks)
                {
                    Construct.AddSoldier();
                    Destroy(this.gameObject);
                    return;
                }
                
                construct.AddResource(_heldResource.Type);
                _heldResource = null;
                OnTaskFinished?.Invoke(this);
            }
        }

        public void Init(UnitStats stats)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _selectable = GetComponent<Selectable>();
            Rank = stats.rank;
            _navMeshAgent.speed = stats.speed;
        }

        public void Move(Vector3 destination)
        {
            _navMeshAgent.destination = destination;
        }
        
        public void Move(Resource resource,Construct construct)
        {
            _navMeshAgent.destination = resource.transform.position;
            _heldResource = resource;
            Construct = construct;
        }

        public void ReturnToConstruct()
        {
            _navMeshAgent.destination = Construct.transform.position;
        }

        public void Selected() => _selectable.Selected();
        public void Deselected() => _selectable.DeSelected();

    }
}