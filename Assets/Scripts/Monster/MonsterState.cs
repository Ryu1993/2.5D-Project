using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace MonsterState
{
    public class BaseState : State<Monster>
    {
        public override void Enter(Monster order)
        {
            if(this.order==null) this.order = order;
            MonsterBehaviourManager.instance.monsterBehaviour += this.Progress;
        }
        public override void Exit() => MonsterBehaviourManager.instance.monsterBehaviour -= this.Progress;
        public override void Progress() { }

        public void NamedHitCheck()
        {
            if (order.isNamed)
            {
                order.HitInput?.Invoke(order);
                order.HitInput = null;
            }
        }

    }

    public class Idle : BaseState
    {
        public override void Progress()
        {
            NamedHitCheck();
            order.MonsterLookAt();
            if (order.DeadCheck()) order.ChangeState(Monster.MonState.Dead);
            else if(order.MonsterHitCheck() && !order.isNamed) order.ChangeState(Monster.MonState.Hit);
            else if(order.ScanTarget()) order.ChangeState(Monster.MonState.Chase);
        }
    }

    public class Hit : BaseState
    {
        public override void Progress()
        {
        
            order.HitInput?.Invoke(order);
            if(order.HitInput!=null) order.animator.SetTrigger(order.animator_Hit);
            order.HitInput = null;
            if (order.DeadCheck()) order.ChangeState(Monster.MonState.Dead);
            else if (!order.animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            {
                order.ChangeState(Monster.MonState.Idle);
            }
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
            order.MonsterMove();
            NamedHitCheck();
            if (order.DeadCheck()) order.ChangeState(Monster.MonState.Dead);
            else if (order.MonsterHitCheck() && !order.isNamed) order.ChangeState(Monster.MonState.Hit);
            else if (order.AttackableCheck()) order.ChangeState(Monster.MonState.Ready);
        }
        public override void Exit()
        {
            order.MonsterMoveSwitch();
            base.Exit();
        }

    }

    public class Ready : BaseState
    {
        public override void Enter(Monster order)
        {
            base.Enter(order);
            order.animator.SetTrigger(order.animator_Ready);
            order.animator.Update(0);
            order.attackReady?.Invoke();
        }
        public override void Progress()
        {
            NamedHitCheck();
            order.attackReadyProgress?.Invoke();
            if (order.DeadCheck()) order.ChangeState(Monster.MonState.Dead);
            else if (order.MonsterHitCheck()&&!order.isNamed) order.ChangeState(Monster.MonState.Hit);
            else if (order.AttackDelayCount()) order.ChangeState(Monster.MonState.Attack);
        }
        public override void Exit()
        {
            base.Exit();
            order.attackReadyEnd?.Invoke();
            order.AttackDelayCountReset();
        }

    }
    public class Attack : BaseState
    {
        public override void Enter(Monster order)
        {
            base.Enter(order);
            order.animator.SetTrigger(order.animator_Attack);
            order.animator.Update(0);
            order.attackStart?.Invoke();
        }
        public override void Progress()
        {
            order.HitInput?.Invoke(order);
            order.HitInput = null;
            order.MonsterAttack();
            if (order.DeadCheck()) order.ChangeState(Monster.MonState.Dead);
            else if (!order.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) order.ChangeState(Monster.MonState.Idle);
        }
        public override void Exit()
        {
            order.attackEnd?.Invoke();
            base.Exit();
            MonsterBehaviourManager.instance.monsterBehaviour += order.AttackCooltimeCount;
        }

    }
    public class Dead : BaseState
    {
        public override void Enter(Monster order)
        {
            base.Enter(order);
            order.animator.SetTrigger(order.animator_Dead);
            order.animator.Update(0);
        }
        public override void Progress()
        {
            if (!order.animator.GetCurrentAnimatorStateInfo(0).IsName("Dead")) Exit();
        }
        public override void Exit()
        {
            base.Exit();
            order.deadEvent?.Invoke();
        }
    }




}
