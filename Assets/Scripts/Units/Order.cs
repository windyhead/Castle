namespace BuildACastle
{
    using UnityEngine;
    using System;
    public abstract class Order 
    {
        public Action OnFinished;
        protected Unit _unit;

        public virtual void ApplyOrder(Unit unit) => _unit = unit; 

        protected void FinishOrder() => OnFinished?.Invoke();
    }

    public class MoveOrder : Order
    {
        private readonly Vector3 _position;
        
        public MoveOrder(Vector3 position)=>_position = position;
        public override void ApplyOrder(Unit unit)
        {
            base.ApplyOrder(unit);
            _unit.Move(_position); 
            _unit.OnMoveFinished += FinishOrder;
        }
    }
    
    public class EnterOrder : Order
    {
        private readonly Barracks _construct;
        
        public EnterOrder(Barracks construct)=>_construct = construct;
        
        public override void ApplyOrder(Unit unit)
        {
            base.ApplyOrder(unit);
            _construct.AddSoldier(unit);
            unit.OnEnter?.Invoke(unit);
        }
    }
}
