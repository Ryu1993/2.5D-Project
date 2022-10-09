using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventable
{
    public void EventStart();
    public void EventClear();
    public void EventInteraction(UnityAction action);
}
