using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


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
            if(order.isHit)
            {
                order.HitInput.Invoke(order);
                order.curAction = Player.State.Hit;
                order.animator.SetTrigger(order.curAction.ToString());
                order.ChangeState(order.curAction);
                return true;
            }
            return false;
        }


    }
    public class DirectionState : BaseState
    {
        public override IEnumerator Middle(Player order)
        {
            Debug.Log("Idle¡¯¿‘");
            while (true)
            {
                if (IsHitCheck(order)) break;
                DirectionCheck(order);
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
                order.InputCheck.Invoke();
                order.PlayerMove.Invoke();
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
            curState = order.curAction;
        }
        public override IEnumerator Middle(Player order)
        {
            yield return order.hitDelay;
            order.curAction = Player.State.Idle;
            order.ChangeState(Player.State.Idle);
        }
        public override void Exit(Player order)
        {
            order.HitInput = null;
            order.isHit = false;
        }
            

    }
    





}
