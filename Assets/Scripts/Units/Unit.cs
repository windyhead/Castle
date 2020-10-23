namespace BuildACastle
{
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using UnityEngine.AI;
    
    public enum UnitType
    {
        Rube,
        Soldier
    }

    [RequireComponent(typeof(Selectable), typeof(NavMeshAgent), typeof(Collider))]
    public abstract class Unit : MonoBehaviour
    {
        public Action <Unit> OnOrdersFinished;
        public Action OnMoveFinished;
        public Action <Unit> OnEnter;
        
        public UnitType Type { get; private set; }
        private Selectable _selectable;
        private NavMeshAgent _navMeshAgent;
        private bool isMoving;
        private readonly List<Order> orders = new List<Order>();

        public void AddOrder(Order newOrder)
        {
            newOrder.OnFinished += NextOrder;
            orders.Add(newOrder);
            if (orders.Count == 1)
                orders[0].ApplyOrder(this);
        }

        private void NextOrder()
        {
            orders[0].OnFinished -= NextOrder;
            orders.RemoveAt(0);
            if (orders.Count == 0)
                OnOrdersFinished?.Invoke(this);
            else
                orders[0].ApplyOrder(this);
        }

        public void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            if (isMoving && !_navMeshAgent.pathPending && orders.Count>0)
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                    if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        Debug.Log("stopped");
                        isMoving = false;
                        OnMoveFinished?.Invoke();
                    }
        }

        public void Init(UnitStats stats)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _selectable = GetComponent<Selectable>();
            Type = stats.type;
            _navMeshAgent.speed = stats.Speed;
        }

        public void Move(Vector3 destination)
        {
            isMoving = true;
            _navMeshAgent.destination = destination;
        }

        public void Selected() => _selectable.Selected(true);
        
        public void Deselected() => _selectable.Selected(false);
    }
}