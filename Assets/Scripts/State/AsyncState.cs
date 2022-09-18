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
        protected int duration;
        protected Character order;
        protected WaitForSeconds wait = new WaitForSeconds(1);
        protected Type type;
        public StatusEffect(int duration, Character order)
        {
            this.order = order;
            this.duration = duration;
        }  // duration 0으로 설정시 effect 무시
        public IEnumerator Progress()
        {
            EnterEvent();
            int count = 0;
            while(count < duration)
            {
                count++;
                EffectEvent();
                yield return wait;
            }
            EnterEvent();
        }
        protected virtual void EnterEvent()
        {
            order.curStatusEffect.Add(type);
        }
        protected virtual void EffectEvent()
        {

        }
        protected virtual void ExitEvent()
        {
            order.curStatusEffect.Remove(type);
        }
    }

    public class Burn : StatusEffect
    {
        public Burn(int duration, Character order) : base(duration, order)
        {
            type = Type.Burn;
            order.StartCoroutine(Progress());
        }
        protected override void EffectEvent()
        {
            
        }


    }

}
