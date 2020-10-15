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

    [RequireComponent(typeof(Selectable),typeof(NavMeshAgent))]
    public class Unit : MonoBehaviour
    {
        public UnitRank Rank { get; private set; }
        private Selectable _selectable;
        private NavMeshAgent _navMeshAgent;

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

        public void Selected() => _selectable.Selected();
        public void Deselected() => _selectable.DeSelected();

    }
}