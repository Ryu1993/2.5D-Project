using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerStates
{
    public class BaseState : State<Player>
    {
        protected Player.State curDirection;
        protected Player.State curState;
        protected Player.State nextDirection;
        protected Player.State nextState;
        public override void Enter(Player order)
        {
            curDirection = order.DirectionState();
            curState = order.curAction;
        }
        public override IEnumerator Middle(Player order)
        {
            yield return null;
        }
        public override void Exit(Player order)
        {


        }

        protected virtual void DirectionCheck(Player order)
        {
            nextDirection = order.DirectionState();
            if (curDirection != nextDirection)
            {
                order.animator.SetTrigger(nextDirection.ToString());
                curDirection = nextDirection;
            }
        }
        protected virtual bool IsNextActionCheck(Player order)
        {
            nextState = order.curAction;
            if (curState != nextState)
            {
                order.animator.SetTrigger(nextState.ToString());
                order.ChangeState(nextState);
                return true;
            }
            return false;
        }
        protected virtual bool IsHitCheck(Player order)
        {
            if(order.HitInput!=null)
            {
                order.animator.SetTrigger(Player.State.Hit.ToString());
                order.ChangeState(Player.State.Hit);
                return true;
            }
            return false;
        }
        protected virtual bool IsAsyncStateCheck(Player order)
        {
            bool isStop = false;
            foreach(AsyncState.Type type in order.curStatusEffect)
            {
                if (type == AsyncState.Type.Sturn) isStop = true;
            }
            if (isStop) order.ChangeState(Player.State.Sturn);
            return isStop;
        }
    }
    public class DirectionState : BaseState
    {
        public override IEnumerator Middle(Player order)
        {
            while (true)
            {
                if (IsAsyncStateCheck(order)) break;
                if (IsHitCheck(order)) break;
                DirectionCheck(order);
                order.rigi.velocity = Vector3.zero;
                order.InputCheck.Invoke();
                if (IsNextActionCheck(order)) break;
                yield return null;
            }
        }
    }

    public class MoveState : BaseState
    {
        public override IEnumerator Middle(Player order)
        {
            while(true)
            {
                if (IsHitCheck(order)) break;
                DirectionCheck(order);
                order.PlayerMove.Invoke();
                order.InputCheck.Invoke();
                if (IsNextActionCheck(order)) break;
                yield return null;
            }
        }
    }

    public class AttackState : BaseState
    {

    }

    public class SkiilState : BaseState
    {

    }

    public class DashState : BaseState
    {

    }

    public class HitState : BaseState
    {
        public override void Enter(Player order)
        {
            order.curAction = Player.State.Hit;
        }
        public override IEnumerator Middle(Player order)
        {
            order.HitInput?.Invoke(order);
            yield return order.hitDelay;
            order.curAction = Player.State.Idle;
            order.ChangeState(Player.State.Idle);
        }
        public override void Exit(Player order)
        {
            order.HitInput = null;
            order.rigi.velocity = Vector3.zero;
        }
           
    }
    
    public class SturnState : BaseState
    {
        bool isHitable = true;
        public override void Enter(Player order)
        {
            order.curAction = Player.State.Sturn;
        }
        public override IEnumerator Middle(Player order)
        {
            while(true)
            {
                IsHitCheck(order);
                if (!IsAsyncStateCheck(order)) break;
                yield return null;
            }
        }
        protected override bool IsHitCheck(Player order)
        {
            if (order.HitInput!=null&&isHitable)
            {
                order.HitInput?.Invoke(order);
                isHitable = false;
                order.StartCoroutine(HitDelay(order));
                return true;
            }
            return false;
        }
        protected override bool IsAsyncStateCheck(Player order)
        {
            bool isStop = false;
            foreach (AsyncState.Type type in order.curStatusEffect)
            {
                if (type==AsyncState.Type.Sturn) isStop = true;
            }
            if (!isStop) order.curAction = Player.State.Idle; order.ChangeState(order.curAction);
            return isStop;
        }
        IEnumerator HitDelay(Player order)
        {
            yield return order.hitDelay;
            isHitable = true;
        }


    }





}
