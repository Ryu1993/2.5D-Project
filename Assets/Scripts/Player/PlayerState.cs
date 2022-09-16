using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace PlayerStates
{
    public class BaseState : State<Player>
    {
        protected Player.State curState;
        protected Player.State nextState;
        public override void Enter(Player order)
        {           
        }
        public override IEnumerator Middle(Player order)
        {
            yield return null;
        }
        public override void Exit(Player order)
        {
        }
    }

    #region DirectionMove
    public class DirectionState : BaseState
    {
        public override IEnumerator Middle(Player order)
        {
            while (true)
            {
                nextState = order.RotateState();
                if (curState != nextState)
                {
                    order.animator.SetTrigger(nextState.ToString());
                    order.ChangeState(nextState);
                    break;
                }
                order.Move();
                yield return null;
            }
        }
    }

    public class UpState : DirectionState
    {
        public override void Enter(Player order)
        {
            curState = Player.State.Up;
        }
        public override IEnumerator Middle(Player order)
        {
            return base.Middle(order);
        }
    }

    public class DownState : DirectionState
    {
        public override void Enter(Player order)
        {
            curState = Player.State.Down;
        }
        public override IEnumerator Middle(Player order)
        {
            return base.Middle(order);
        }

    }

    public class HorizontalState : DirectionState
    {
        public override void Enter(Player order)
        {
            curState = Player.State.Horizontal;
        }
        public override IEnumerator Middle(Player order)
        {
            return base.Middle(order);
        }
    }


    public class HoUpState : DirectionState
    {
        public override void Enter(Player order)
        {
            curState = Player.State.HoUp;
        }
        public override IEnumerator Middle(Player order)
        {
            while (true)
            {
                return base.Middle(order);
            }
        }
    }

    public class HoDownSate : DirectionState
    {
        public override void Enter(Player order)
        {
            curState = Player.State.HoDown;
        }
        public override IEnumerator Middle(Player order)
        {
            while (true)
            {
                return base.Middle(order);
            }
            
        }
    }
    #endregion



}
