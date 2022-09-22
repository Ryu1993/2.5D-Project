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
            isCompleted = false;
            curDirection = order.DirectionState();
            curState = order.curAction;
            order.animator.SetTrigger(order.curAction.ToString());
        }
        public override IEnumerator Middle(Player order)
        {
            while (true)
            {
                if (IsAsyncStateCheck(order)) break;
                if (IsHitCheck(order)) break;
                DirectionCheck(order);
                order.InputCheck.Invoke();
                if (IsDashCheck(order)) break;
                if (IsSkillCheck(order)) break;
                if (IsAttackCheck(order)) break;
                if (IsMoveCheck(order)) break;
                yield return null;
            }
            isCompleted = true;
        }
        public override void Exit(Player order) { }

        #region BaseCheck
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
            if (order.MoveBehavior != null&&curState!=Player.State.Move)
            {
                order.curAction = Player.State.Move;
                order.ChangeState(Player.State.Move);
                return true;
            }
            else if(order.IdleBehavior != null && curState != Player.State.Idle)
            {
                order.curAction = Player.State.Idle;
                order.ChangeState(Player.State.Idle);
                return true;
            }
            return false;
        }
        protected virtual bool IsAttackCheck(Player order)
        {
            if(order.AttackBehavior != null)
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
            if (order.SkillBehavior != null)
            {
                order.curAction = Player.State.Skill;
                order.ChangeState(Player.State.Skill);
                return true;
            }
            return false;
        }
        protected virtual bool IsDashCheck(Player order)
        {
            if(order.DashBehavior!=null)
            {
                order.curAction = Player.State.Dash;
                order.ChangeState(Player.State.Dash);
                return true;
            }
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
        #endregion
    }
    public class DirectionState : BaseState
    {
        public override void Enter(Player order)
        {
            base.Enter(order);
            order.IdleBehavior?.Invoke();
        }
    }
    public class MoveState : BaseState
    {
        public override IEnumerator Middle(Player order)
        {
            while(true)
            {
                DirectionCheck(order);
                order.MoveBehavior?.Invoke();
                order.InputCheck.Invoke();
                if (IsAsyncStateCheck(order)) break;
                if (IsHitCheck(order)) break;
                if (IsDashCheck(order)) break;
                if (IsSkillCheck(order)) break;
                if (IsAttackCheck(order)) break;
                if (IsMoveCheck(order)) break;
                yield return null;
            }
            isCompleted = true;
        }


    }
    public class AttackState : BaseState
    {
        public override void Enter(Player order)
        {
            base.Enter(order);
            Debug.Log("АјАн");
            order.PlayerIdle();
            order.directionCircle.isStop = true;
        }
        public override IEnumerator Middle(Player order)
        {
            order.AttackBehavior?.Invoke();
            yield return null;
            while (order.weaponContainer.isProgress)
            {
                if (IsAsyncStateCheck(order)) break;
                if (IsHitCheck(order)) break;
                order.InputCheck?.Invoke();
                order.AttackBehavior?.Invoke();
                if (IsDashCheck(order)) break;
                if (IsSkillCheck(order)) break;
                yield return null;
            }
            order.directionCircle.isStop = false;
            order.rigi.velocity = Vector3.zero;
            order.curAction = Player.State.Idle;
            order.ChangeState(Player.State.Idle);
            isCompleted = true;
        }
        public override void Exit(Player order)=> order.weaponContainer.animator.SetTrigger("Stop");
    }
    public class SuperAttackState : AttackState
    {
        public override void Enter(Player order)
        {
            base.Enter(order);
            order.PlayerKinematic();
            order.directionCircle.isStop = true;
        }
        public override void Exit(Player order)
        {
            order.weaponContainer.animator.SetTrigger("HitStop");
            order.directionCircle.isStop = false;
            order.PlayerKinematic();
        }
    }
    public class SkiilState : BaseState
    {

    }
    public class DashState : BaseState
    {
        float baseSpeed = 0;
        public override void Enter(Player order)
        {
            base.Enter(order);
            baseSpeed = order.moveSpeed;
            order.moveSpeed = order.moveSpeed * 2;
            order.MoveInput();
        }
        public override IEnumerator Middle(Player order)
        {
            order.illusionCreator.Play();
            if (order.MoveBehavior != null) order.PlayerMove();
            else order.rigi.velocity = order.directionCircle.transform.forward * order.moveSpeed;
            yield return order.dashDelay;
            order.HitReset();
            order.illusionCreator.Stop();
            order.rigi.velocity = Vector3.zero;
            order.moveSpeed = baseSpeed;
            order.curAction = Player.State.Idle;
            order.ChangeState(order.curAction);
            isCompleted = true;
        }
    }
    public class HitState : BaseState
    {
        public override void Enter(Player order)
        {
            order.curAction = Player.State.Hit;
            isCompleted = false;
        }
        public override IEnumerator Middle(Player order)
        {
            order.HitInput?.Invoke(order);
            yield return order.hitDelay;
            order.curAction = Player.State.Idle;
            order.ChangeState(order.curAction);
            isCompleted = true;
        }
        public override void Exit(Player order)
        {
            order.HitInput = null;
            order.rigi.velocity = Vector3.zero;
        }         
    }
    
    public class SturnState : BaseState
    {

        public override void Enter(Player order)
        {
            order.curAction = Player.State.Sturn;
            isCompleted = false;
        }
        public override IEnumerator Middle(Player order)
        {
            while(true)
            {
                IsHitCheck(order);
                if (!IsAsyncStateCheck(order)) break;
                yield return null;
            }
            isCompleted = true;
        }
        #region SturnCustomCheck
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
        #endregion
    }





}
