namespace BuildACastle
{
    using UnityEngine;
    using System;

    public abstract class Order
    {
        public Action OnFinished;
        protected Unit _unit;

        public virtual void ApplyOrder(Unit unit) => _unit = unit;

        protected virtual void FinishOrder()
        {
            OnFinished?.Invoke();
        }
    }

    public class MoveOrder : Order
    {
        private readonly Vector3 _position;

        public MoveOrder(Vector3 position) => _position = position;

        public override void ApplyOrder(Unit unit)
        {
            base.ApplyOrder(unit);
            _unit.OnMoveFinished += FinishOrder;
            _unit.Move(_position);
        }

        protected override void FinishOrder()
        {
            _unit.OnMoveFinished -= FinishOrder;
            base.FinishOrder();
        }
    }

    public class EnterOrder : Order
    {
        private Barracks _construct;

        public EnterOrder(Barracks construct) => _construct = construct;

        public override void ApplyOrder(Unit unit)
        {
            base.ApplyOrder(unit);
            _construct.AddSoldier(unit);
            unit.OnEnter?.Invoke(unit);
            FinishOrder();
        }
    }

    public class TakeResourceOrder : Order
    {
        private Resource _resource;
        private RubeUnit _rube;

        public TakeResourceOrder(Resource resource)
        {
            _resource = resource;
        }

        public override void ApplyOrder(Unit unit)
        {
            base.ApplyOrder(unit);
            _rube = _unit as RubeUnit;
            _rube.TakeResource(_resource);
            FinishOrder();
        }
    }

    public class UseResourceOrder : Order
    {
        private Construct _costruct;
        private RubeUnit _rube;

        public UseResourceOrder(Construct construct)
        {
            _costruct = construct;
        }

        public override void ApplyOrder(Unit unit)
        {
            base.ApplyOrder(unit);
            _rube = _unit as RubeUnit;
            _rube.UseResource(_costruct);
            FinishOrder();
        }
    }

    public class BuildOrder : Order
    {
        private Construct _costruct;
        private RubeUnit _rube;

        public BuildOrder(Construct construct)
        {
            _costruct = construct;
        }

        public override void ApplyOrder(Unit unit)
        {
            base.ApplyOrder(unit);
            _rube = _unit as RubeUnit;
            _rube.Build(_costruct);
            _rube.OnBuildFinished += FinishOrder;
        }

        protected override void FinishOrder()
        {
            _rube.OnBuildFinished -= FinishOrder;
            base.FinishOrder();
        }
    }
}