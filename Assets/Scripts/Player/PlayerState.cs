//using System.Collections;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.Events;

//namespace PlayerStates
//{
//    public class BaseState : State<Player>
//    {
//        protected Player.State curDirection;
//        protected Player.State curState;
//        protected Player.State nextDirection;
//        protected Player.State nextState;
//        public override void Enter(Player order)
//        {
//            isCompleted = false;
//            curDirection = order.DirectionState();
//            curState = order.curAction;
//            order.animator.SetTrigger(order.curAction.ToString());
//        }
//        public override IEnumerator Middle(Player order)
//        {
//            while (true)
//            {
 
//                if (IsAsyncStateCheck(order)) break;
//                if (IsHitCheck(order)) break;
//                DirectionCheck(order);
//                if (IsDashCheck(order)) break;
//                if (IsSkillCheck(order)) break;
//                if (IsAttackCheck(order)) break;
//                if (IsMoveCheck(order)) break;
//                yield return order.fixedUpdateDelay;
//            }
//            isCompleted = true;
//        }
//        public override void Exit(Player order) { }

//        #region BaseCheck
//        protected virtual void DirectionCheck(Player order)
//        {
//            nextDirection = order.DirectionState();
//            if (curDirection != nextDirection)
//            {
//                order.animator.SetTrigger(nextDirection.ToString());
//                curDirection = nextDirection;
//            }
//        }
//        protected virtual bool IsMoveCheck(Player order)
//        {
//            if (order.isMoveBehavior&&curState!=Player.State.Move)
//            {
//                order.curAction = Player.State.Move;
//                order.ChangeState(Player.State.Move);
//                return true;
//            }
//            else if(order.isIdleBehavior && curState != Player.State.Idle)
//            {
//                order.curAction = Player.State.Idle;
//                order.ChangeState(Player.State.Idle);
//                return true;
//            }
//            return false;
//        }
//        protected virtual bool IsAttackCheck(Player order)
//        {
//            if (order.isAttackBehavior &&curState!=Player.State.Attack)
//            {
//                if(order.weaponContainer.superArmor)
//                {
//                    order.curAction = Player.State.SuperAttack;
//                    order.ChangeState(Player.State.SuperAttack);
//                    return true;
//                }
//                else
//                {
//                    order.curAction = Player.State.Attack;
//                    order.ChangeState(Player.State.Attack);
//                    return true;
//                }
//            }
//            return false;
//        }
//        protected virtual bool IsSkillCheck(Player order)
//        {
//            if (order.isSkillBehavior&&curState!= Player.State.Skill)
//            {
//                order.curAction = Player.State.Skill;
//                order.ChangeState(Player.State.Skill);
//                return true;
//            }
//            return false;
//        }
//        protected virtual bool IsDashCheck(Player order)
//        {
//            if(order.isDashBehavior&& curState!=Player.State.Dash)
//            {
//                order.curAction = Player.State.Dash;
//                order.ChangeState(Player.State.Dash);
//                return true;
//            }
//            return false;
//        }
//        protected virtual bool IsHitCheck(Player order)
//        {
//            if(order.HitInput!=null)
//            {
//                order.curAction = Player.State.Hit;
//                order.ChangeState(Player.State.Hit);
//                return true;
//            }
//            return false;
//        }
//        protected virtual bool IsAsyncStateCheck(Player order)
//        {
//            bool isChange = false;
//            foreach(AsyncState.Type type in order.curStatusEffect)
//            {
//                if (type == AsyncState.Type.Sturn) { isChange = true; nextState = Player.State.Sturn; }
//            }
//            if (isChange) order.ChangeState(nextState);
//            return isChange;
//        }

//        protected virtual bool IsDeadCheck(Player order)
//        {
//            if(order.isDeadBehavior)
//            {
//                return true;
//            }
//            return false;
//        }


//        #endregion
//    }
//    public class DirectionState : BaseState
//    {
//        public override void Enter(Player order)
//        {
//            base.Enter(order);
//            order.PlayerIdle();
//            GameManager.instance.PlayerPosition = order.transform.position;
//        }
//    }
//    public class MoveState : BaseState
//    {
//        public override IEnumerator Middle(Player order)
//        {
//            while (true)
//            {
//                order.PlayerMove();
//                GameManager.instance.PlayerPosition = order.transform.position;
//                DirectionCheck(order);
//                if (IsAsyncStateCheck(order)) break;
//                if (IsHitCheck(order)) break;
//                if (IsDashCheck(order)) break;
//                if (IsSkillCheck(order)) break;
//                if (IsAttackCheck(order)) break;
//                if (IsMoveCheck(order)) break;
//                yield return order.fixedUpdateDelay;
//            }
//            isCompleted = true;
//        }
//    }
//    public class AttackState : BaseState
//    {
//        public override IEnumerator Middle(Player order)
//        {
//            order.PlayerIdle();
//            order.directionCircle.isStop = true;
//            order.AttackBehavior?.Invoke();
//            yield return order.fixedUpdateDelay;
//            while (order.weaponContainer.isProgress)
//            {
//                if (IsAsyncStateCheck(order)) break;
//                if (IsHitCheck(order)) break;
//                if(order.isAttackBehavior) order.AttackBehavior?.Invoke();
//                if (IsDashCheck(order)) break;
//                if (IsSkillCheck(order)) break;
//                yield return order.fixedUpdateDelay;
//            }
//            order.directionCircle.isStop = false;
//            order.curAction = Player.State.Idle;
//            order.ChangeState(Player.State.Idle);
//            isCompleted = true;
//        }
//        public override void Exit(Player order)
//        {
//            order.weaponContainer.animator.SetTrigger("Stop");
//            Debug.Log("°ø°Ý³¡");
//        }
//    }
//    public class SuperAttackState : AttackState
//    {
//        public override void Enter(Player order)
//        {
//            base.Enter(order);
//            order.PlayerKinematic();
//            order.directionCircle.isStop = true;
//        }
//        public override void Exit(Player order)
//        {
//            order.weaponContainer.animator.SetTrigger("HitStop");
//            order.directionCircle.isStop = false;
//            order.PlayerKinematic();
//        }
//    }
//    public class SkiilState : BaseState
//    {

//    }
//    public class DashState : BaseState
//    {
//        float dashSpeed;
//        public override void Enter(Player order)
//        {
//            base.Enter(order);
//            dashSpeed = order.moveSpeed * 2;

//        }
//        //public override IEnumerator Middle(Player order)
//        //{
//        //    order.illusionCreator.Play();
//        //    if (order.isMoveBehavior) order.PlayerDashMove(dashSpeed);
//        //    else order.PlayerDash(dashSpeed);
//        //    yield return order.dashDelay;
//        //    order.CooltimeCounter += order.DashCoolCount;
//        //    order.illusionCreator.Stop();
//        //    order.rigi.velocity = Vector3.zero;
//        //    order.curAction = Player.State.Idle;
//        //    order.ChangeState(order.curAction);
//        //    isCompleted = true;
//        //}
//        public override void Exit(Player order)
//        {
//            base.Exit(order);
//            order.HitReset();
//        }

//    }
//    public class HitState : BaseState
//    {
//        public override void Enter(Player order)
//        {
//            order.curAction = Player.State.Hit;
//            isCompleted = false;
//        }
//        public override IEnumerator Middle(Player order)
//        {
//            order.HitInput?.Invoke(order);
//            yield return order.hitDelay;
//            order.curAction = Player.State.Idle;
//            order.ChangeState(order.curAction);
//            isCompleted = true;
//        }
//        public override void Exit(Player order)
//        {
//            order.HitReset();
//        }         
//    }
    
//    public class SturnState : BaseState
//    {

//        public override void Enter(Player order)
//        {
//            order.curAction = Player.State.Sturn;
//            isCompleted = false;
//        }
//        public override IEnumerator Middle(Player order)
//        {
//            while(true)
//            {
//                yield return order.fixedUpdateDelay;
//                IsHitCheck(order);
//                if (!IsAsyncStateCheck(order)) break;
//            }
//            isCompleted = true;
//        }
//        #region SturnCustomCheck
//        protected bool isHitable = true;
//        protected override bool IsHitCheck(Player order)
//        {
//            if (order.HitInput != null && isHitable)
//            {
//                order.HitInput?.Invoke(order);
//                isHitable = false;
//                order.StartCoroutine(HitDelay(order));
//                return true;
//            }
//            return false;
//        }
//        protected override bool IsAsyncStateCheck(Player order)
//        {
//            bool isStop = false;
//            foreach (AsyncState.Type type in order.curStatusEffect)
//            {
//                if (type == AsyncState.Type.Sturn) isStop = true;
//            }
//            if (!isStop) { order.curAction = Player.State.Idle; order.ChangeState(order.curAction); }
//            return isStop;
//        }
//        protected IEnumerator HitDelay(Player order)
//        {
//            yield return order.hitDelay;
//            isHitable = true;
//        }
//        #endregion
//    }





//}
