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

    public class UpState : BaseState
    {
        public override void Enter(Player order)
        {
            curState = Player.State.Up;
            order.animator.SetTrigger("Up");
            order.spriteTransform.localRotation = Quaternion.Euler(45, 0, 0);
        }
        public override IEnumerator Middle(Player order)
        {
            while(true)
            {
                nextState = order.RotateState();
                if (curState != nextState)
                {
                    order.ChangeState(nextState);
                    break;
                }
                order.Move();
                yield return null;
            }
        }
    }

    public class DownState : BaseState
    {
        public override void Enter(Player order)
        {
            curState = Player.State.Down;
            order.animator.SetTrigger("Down");
            order.spriteTransform.localRotation = Quaternion.Euler(45, 0, 0);
        }
        public override IEnumerator Middle(Player order)
        {
            while (true)
            {
                nextState = order.RotateState();
                if (curState != nextState)
                {
                    order.ChangeState(nextState);
                    break;
                }
                order.Move();
                yield return null;
            }
        }

    }

    public class HorizontalState : BaseState
    {
        public override void Enter(Player order)
        {
            curState = Player.State.Horizontal;
            order.animator.SetTrigger("Horizontal");
            order.RotateState();
            order.spriteTransform.localRotation = Quaternion.Euler(45, 0, 0);
        }
        public override IEnumerator Middle(Player order)
        {
            while (true)
            {
                nextState = order.RotateState();
                if (curState != nextState)
                {
                    order.ChangeState(nextState);
                    break;
                }
                order.Move();
                yield return null;
            }
        }
    }


    public class HoUpState : BaseState
    {
        public override void Enter(Player order)
        {
            curState = Player.State.HoUp;
            order.animator.SetTrigger("Up");
            order.spriteTransform.localRotation = Quaternion.Euler(45,15, 0);
        }
        public override IEnumerator Middle(Player order)
        {
            while (true)
            {
                nextState = order.RotateState();
                if (curState != nextState)
                {
                    order.ChangeState(nextState);
                    break;
                }
                order.Move();
                yield return null;
            }
        }
    }

    public class HoDownSate : BaseState
    {
        public override void Enter(Player order)
        {
            curState = Player.State.HoDown;
            order.animator.SetTrigger("Down");
            order.spriteTransform.localRotation = Quaternion.Euler(45,-15, 0);
        }
        public override IEnumerator Middle(Player order)
        {
            while (true)
            {
                nextState = order.RotateState();
                if (curState != nextState)
                {
                    order.ChangeState(nextState);
                    break;
                }
                order.Move();
                yield return null;
            }
        }
    }


}
