using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Experimental.Rendering;
using UnityEngine;

namespace AsyncState
{
    public enum Type { Burn,Poison,Sturn,Heal,MoveUP};
    public class StatusEffect
    { 
        protected WaitForSeconds wait = new WaitForSeconds(1);
        protected Type type;
        protected int count = 0;
        protected StatusIconManager iconManager;
        protected int startDuration = 0;
        protected IEnumerator Progress(int duration,Character order)
        {
            EnterEvent(order);
            while(count < duration)
            {
                count++;
                EffectEvent(order);
                yield return wait;
            }
            ExitEvent(order);
        }
        protected virtual void EnterEvent(Character order) 
        { 
            order.curStatusEffect.Add(type);
            if(order as Player != null) iconManager = StatusManager.instance.playerStatusUI.StatusAdd(type, startDuration);
        }
        protected virtual void EffectEvent(Character order) { iconManager?.Count(startDuration - count); }
        protected virtual void ExitEvent(Character order) { order.curStatusEffect.Remove(type); iconManager?.Return(); }
    }

    public class Burn : StatusEffect
    {
        int strength;
        public Burn(int duration, Character order)
        {
            type = Type.Burn;
            startDuration = duration;
            strength = duration;
            order.StartCoroutine(Progress(duration,order));
        }

        protected override void ExitEvent(Character order)
        {
            base.ExitEvent(order);
            order.curHp -= strength / 3;
        }
    }

    public class Heal : StatusEffect
    {
        public Heal(int duration, Character order)
        {
            type = Type.Heal;
            startDuration = duration;
            EnterEvent(order);   
        }
        protected override void EnterEvent(Character order)
        {
            base.EnterEvent(order);
            order.curHp += startDuration;
        }


    }

    public class Sturn : StatusEffect
    {
        Player target;
        bool isPlayer;
        public Sturn(int duration,Character order)
        {
            type = Type.Sturn;
            startDuration = duration;
            order.StartCoroutine(Progress(duration, order));
        }
        protected override void EnterEvent(Character order)
        {
            base.EnterEvent(order);
            target = order as Player;
            isPlayer = target != null;
            if(isPlayer) target.InputCheckRemove();
        }
        protected override void ExitEvent(Character order)
        {
            base.ExitEvent(order);
            if (isPlayer)
            {
                target.InputCheckRemove();
                target.InputCheckSet();
            }
        }
    }

    public class MoveUP : StatusEffect
    {
        public MoveUP(int duration,Character order)
        {
            type = Type.MoveUP;
            startDuration = duration;
            order.StartCoroutine(Progress(duration, order));
        }
        protected override void EnterEvent(Character order)
        {
            base.EnterEvent(order);
            order.moveSpeed += 2f;
        }
        protected override void ExitEvent(Character order)
        {
            order.moveSpeed -= 2f;
            base.ExitEvent(order);
        }
    }


}
