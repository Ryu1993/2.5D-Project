using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Experimental.Rendering;
using UnityEngine;

namespace AsyncState
{
    public enum Type { Burn,Poison,Sturn};
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
            if(order as Player != null) iconManager = StatusContainerUi.instance.StatusAdd(type, startDuration);
        }
        protected virtual void EffectEvent(Character order) { iconManager.Count(startDuration - count); }
        protected virtual void ExitEvent(Character order) { order.curStatusEffect.Remove(type); iconManager.Return(); }
    }

    public class Burn : StatusEffect
    {
        public Burn(int duration, Character order)
        {
            type = Type.Burn;
            startDuration = duration;
            order.StartCoroutine(Progress(duration,order));
        }
        protected override void EnterEvent(Character order)
        {
            base.EnterEvent(order);
            Debug.Log(order.name+"화상진입");      
        }
        protected override void EffectEvent(Character order)
        {
            base.EffectEvent(order);
        }
        protected override void ExitEvent(Character order)
        {
            base.ExitEvent(order);
            Debug.Log(order.name + "화상끝");
        }
    }

}
