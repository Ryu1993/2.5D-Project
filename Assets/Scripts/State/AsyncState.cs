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
        public IEnumerator Progress(int duration,Character order)
        {
            EnterEvent(order);
            int count = 0;
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
        public Burn(int duration, Character order)
        {
            type = Type.Burn;
            order.StartCoroutine(Progress(duration,order));
        }
        protected override void EnterEvent(Character order)
        {
            base.EnterEvent(order);
            Debug.Log(order.name+"화상진입");
        }
        protected override void EffectEvent(Character order)
        {
            Debug.Log(order.name + "화상중");
        }
        protected override void ExitEvent(Character order)
        {
            Debug.Log(order.name + "화상끝");
        }
    }

}
