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
            order.animator.SetTrigger(order.curAction.ToString());
        }
        public override IEnumerator Middle(Player order)
        {
            while (true)
            {
                DirectionCheck(order);
                order.InputCheck.Invoke();
                if (IsAsyncStateCheck(order)) break;
                if (IsHitCheck(order)) break;
                if (IsAttackCheck(order)) break;
                if (IsMoveCheck(order)) break;
                yield return null;
            }
        }
        public override void Exit(Player order) { }
        protected virtual void DirectionCheck(Player order)
        {
            nextDirection = order.DirectionState();
            if (curDirection != nextDirection)
            {
                order.animator.SetTrigger(nextDirection.ToString());
                curDirection = nextDirection;
            }
        }
        protected virtual bool IsMoveCheck(Player order)
        {
            if (order.Move!=null&&curState!=Player.State.Move)
            {
                order.curAction = Player.State.Move;
                order.ChangeState(Player.State.Move);
                return true;
            }
            else if(order.Idle != null && curState != Player.State.Idle)
            {
                order.curAction = Player.State.Idle;
                order.ChangeState(Player.State.Idle);
                return true;
            }
            return false;
        }
        protected virtual bool IsAttackCheck(Player order)
        {
            if(order.Attack!=null)
            {
                if(order.weaponContainer.superArmor)
                {
                    order.curAction = Player.State.SuperAttack;
                    order.ChangeState(Player.State.SuperAttack);
                    return true;
                }
                else
                {
                    order.curAction = Player.State.Attack;
                    order.ChangeState(Player.State.Attack);
                    return true;
                }
            }
            return false;
        }
        protected virtual bool IsSkillCheck(Player order)
        {
            return false;
        }
        protected virtual bool IsDashCheck(Player order)
        {
            return false;
        }
        protected virtual bool IsHitCheck(Player order)
        {
            if(order.HitInput!=null)
            {
                order.curAction = Player.State.Hit;
                order.ChangeState(Player.State.Hit);
                return true;
            }
            return false;
        }
        protected virtual bool IsAsyncStateCheck(Player order)
        {
            bool isChange = false;
            foreach(AsyncState.Type type in order.curStatusEffect)
            {
                if (type == AsyncState.Type.Sturn) { isChange = true; nextState = Player.State.Sturn; }
            }
            if (isChange) order.ChangeState(nextState);
            return isChange;
        }
    }

    public class NoneReactionHit : BaseState
    {


    }
    public class DirectionState : BaseState
    {
        public override void Enter(Player order)
        {
            base.Enter(order);
            order.Idle?.Invoke();
        }
    }
    public class MoveState : BaseState
    {
        public override IEnumerator Middle(Player order)
        {
            while(true)
            {
                DirectionCheck(order);
                order.Move?.Invoke();
                order.InputCheck.Invoke();
                if (IsAsyncStateCheck(order)) break;
                if (IsHitCheck(order)) break;
                if (IsAttackCheck(order)) break;
                if (IsMoveCheck(order)) break;
                yield return null;
            }
        }
    }

    public class AttackState : BaseState
    {
        public override void Enter(Player order)
        {
            base.Enter (order);
            order.PlayerIdle();
            order.Attack?.Invoke();
        }
        public override IEnumerator Middle(Player order)
        {
            for(int i = 0; i < order.weaponContainer.motionTime; i++)
            {
                if (IsAsyncStateCheck(order)) break;
                if (IsHitCheck(order)) break;
                order.InputCheck?.Invoke();
                if (IsDashCheck(order)) break;
                if (IsSkillCheck(order)) break;
                if (IsAttackCheck(order)) i = 0;
                yield return null;
            }
        }
        protected override bool IsAttackCheck(Player order)
        {
            return order.Attack != null;
        }

    }

    public class SuperAttackState : AttackState
    {
        public override void Enter(Player order)
        {
            base.Enter(order);
            order.PlayerKinematic();
        }
        public override void Exit(Player order)
        {
            order.PlayerKinematic();
        }
    }

    public class SkiilState : BaseState
    {

    }

    public class DashState : BaseState
    {

    }

    public class HitState : BaseState
    {
        public override void Enter(Player order) => order.curAction = Player.State.Hit;
        public override IEnumerator Middle(Player order)
        {
            order.HitInput?.Invoke(order);
            yield return order.hitDelay;
            order.curAction = Player.State.Idle;
            order.ChangeState(order.curAction);
        }
        public override void Exit(Player order)
        {
            order.HitInput = null;
            order.rigi.velocity = Vector3.zero;
        }
           
    }
    
    public class SturnState : NoneReactionHit
    {
  
        public override void Enter(Player order)=> order.curAction = Player.State.Sturn;
        public override IEnumerator Middle(Player order)
        {
            while(true)
            {
                IsHitCheck(order);
                if (!IsAsyncStateCheck(order)) break;
                yield return null;
            }
        }
        protected bool isHitable = true;
        protected override bool IsHitCheck(Player order)
        {
            if (order.HitInput != null && isHitable)
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
                if (type == AsyncState.Type.Sturn) isStop = true;
            }
            if (!isStop) { order.curAction = Player.State.Idle; order.ChangeState(order.curAction); }
            return isStop;
        }
        protected IEnumerator HitDelay(Player order)
        {
            yield return order.hitDelay;
            isHitable = true;
        }



    }





}
