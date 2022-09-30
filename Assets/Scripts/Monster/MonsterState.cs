using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MonsterState
{
    public class BaseState : State<Monster>
    {
        public override void Enter(Monster order)
        {
            if(this.order==null) this.order = order;
            MonsterBehaviourManager.instance.monsterBehaviour += Progress;
        }
        public override void Exit() => MonsterBehaviourManager.instance.monsterBehaviour -= Progress;
        public override void Progress() { }
   
    }

    public class Idle : BaseState
    {
        public override void Progress()
        {
            Debug.Log("대기");
            if (order.DeadCheck()) order.ChangeState(Monster.MonState.Dead);
            else if(order.MonsterHitCheck()) order.ChangeState(Monster.MonState.Hit);
            else if(order.ScanTarget()) order.ChangeState(Monster.MonState.Chase);
        }
    }

    public class Hit : BaseState
    {
        public override void Enter(Monster order)
        {
            base.Enter(order);
            order.MonsterKinematicSwitch();
        }
        public override void Progress()
        {
            Debug.Log("Hit");
            order.HitInput?.Invoke(order);
            if(order.HitInput!= null) order.animator.SetTrigger("Hit");
            order.HitInput = null;
            if (order.DeadCheck()) order.ChangeState(Monster.MonState.Dead);
            else if (!order.animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) order.ChangeState(Monster.MonState.Idle);
        }
        public override void Exit()
        {
            base.Exit();
            order.MonsterKinematicSwitch();
        }
    }

    public class Chase : BaseState
    {
        public override void Enter(Monster order)
        {
            base.Enter(order);
            order.MonsterMoveSwitch();
        }
        public override void Progress()
        {
            Debug.Log("추적");
            order.MonsterMove();
            if (order.DeadCheck()) order.ChangeState(Monster.MonState.Dead);
            else if (order.MonsterHitCheck()) order.ChangeState(Monster.MonState.Hit);
            else if (order.AttackableCheck()) order.ChangeState(Monster.MonState.Ready);
        }
        public override void Exit()
        {
            base.Exit();
            order.MonsterMoveSwitch();
        }

    }

    public class Ready : BaseState
    {
        public override void Enter(Monster order)
        {
            base.Enter(order);
            order.animator.SetTrigger("Ready");
        }
        public override void Progress()
        {

            if (order.DeadCheck()) order.ChangeState(Monster.MonState.Dead);
            else if (order.MonsterHitCheck()) order.ChangeState(Monster.MonState.Hit);
            else if (order.AttackDelayCount()) order.ChangeState(Monster.MonState.Attack);
        }
        public override void Exit()
        {
            base.Exit();
            order.AttackDelayCountReset();
        }

    }
    public class Attack : BaseState
    {
        public override void Enter(Monster order)
        {
            base.Enter(order);
            order.animator.SetTrigger("Attack");
            order.animator.Update(0);
        }
        public override void Progress()
        {
            order.HitInput?.Invoke(order);
            order.HitInput = null;
            if (order.DeadCheck()) order.ChangeState(Monster.MonState.Dead);
            else if (!order.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) order.ChangeState(Monster.MonState.Idle);
        }
    }
    public class Dead : BaseState
    {
        public override void Enter(Monster order)
        {
            base.Enter(order);
            order.animator.SetTrigger("Dead");
        }
        public override void Progress()
        {
            if (!order.isAnimation)Exit();
        }
        public override void Exit()
        {
            base.Exit();
            order.deadEvent?.Invoke();
        }
    }




}
