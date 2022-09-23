using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MonsterState
{
    public class BaseState : State<Monster>
    {
        
        public override void Enter(Monster order) { isCompleted = false; }
        public override IEnumerator Middle(Monster order) 
        {
            while(true)
            {
                if(IsHitCheck(order))break;
                if(IsTargetCheck(order))break;
                yield return null;
            }
            isCompleted = true;
        }
        public override void Exit(Monster order) { }

        protected virtual bool IsHitCheck(Monster order)
        {
            if (order.HitInput != null)
            {
                Debug.Log("공격당함");
                order.curAction = Character.State.Hit;
                order.ChangeState(Character.State.Hit);
                return true;
            }
            return false;
        }
        protected virtual bool IsTargetCheck(Monster order)
        {
            if(order.isTargetOn)
            {
                order.curAction = Character.State.Trace;
                order.ChangeState(Character.State.Trace);
                return true;
            }
            return false;

        }

    }

    public class IdleState : BaseState
    {
        public override IEnumerator Middle(Monster order)
        {
            while (true)
            {
                order.ScanTarget();
                if (IsHitCheck(order)) break;
                if (IsTargetCheck(order)) break;
                yield return null;
            }
            isCompleted = true;
        }

    }

    public class HitState : BaseState
    {
        public override void Enter(Monster order)
        {
            order.animator.SetTrigger("Hit");
            order.HitInput?.Invoke(order);
            order.HitInput = null;
        }
 
    }

    public class TrasceState : BaseState
    {
        public override IEnumerator Middle(Monster order)
        {
            isCompleted = false;
            while (true)
            {
                order.MonsterMove();
                if (IsHitCheck(order)) break;
                yield return null;     
            }
            isCompleted = true;
        }

    }

    public class ReadyState : BaseState
    {

    }
    public class AttackState : BaseState
    {

    }
    public class DieState : BaseState
    {

    }




}
