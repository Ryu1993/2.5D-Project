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
        protected virtual IEnumerator Progress(int duration,Character order)
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
        protected virtual void EnterEvent(Character order) { order.curStatusEffect.Add(type); }
        protected virtual void EffectEvent(Character order) { }
        protected virtual void ExitEvent(Character order) { order.curStatusEffect.Remove(type); }
    }

    public class Burn : StatusEffect
    {
        StatusIconManager iconManager;
        int startDuration = 0;
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
            iconManager = StatusContainerUi.instance.StatusAdd(type, startDuration);
        }
        protected override void EffectEvent(Character order)
        {
            Debug.Log(order.name + "화상중");
            iconManager.Count(startDuration - count);
        }
        protected override void ExitEvent(Character order)
        {
            Debug.Log(order.name + "화상끝");
            iconManager.Return();
        }
    }

}
