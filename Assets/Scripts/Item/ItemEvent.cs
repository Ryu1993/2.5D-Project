using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class ItemEvent : MonoBehaviour, IEventable
{
    [SerializeField]
    private AssetLabelReference label;
    private Item target;
    private SceneItem sceneItem;
    public UnityAction interactionEvent;
    public void EventClear()
    {
        if (sceneItem?.closeCoroutine == null)
        {
            sceneItem?.Close();
        }
    }
    public void EventInteraction(UnityAction action)=>interactionEvent = action;
    public void EventStart()
    {
        target = AddressObject.RandomInstinateScriptable(label) as Item;
        sceneItem = ItemManager.instance.CreateSceneItem(target, transform.position);
        sceneItem.closeEvent = interactionEvent;
    }
}
