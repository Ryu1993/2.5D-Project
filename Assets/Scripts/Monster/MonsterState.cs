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

        }

        public override void Exit(Monster order)
        {
            throw new System.NotImplementedException();        }

        public override IEnumerator Middle(Monster order)
        {
            yield return null;
        }
    }

    public class IdleState : BaseState
    {
  

    }

    public class HitState : BaseState
    {

    }

    public class TrasceState : BaseState
    {


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
