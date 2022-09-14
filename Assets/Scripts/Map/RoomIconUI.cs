using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomIconUI : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] GameObject up;
    [SerializeField] GameObject down;
    [SerializeField] GameObject left;
    [SerializeField] GameObject right;
    Dictionary<MapManager.GateDirection, GameObject> iconDirection = new Dictionary<MapManager.GateDirection, GameObject>();

    private void Awake()
    {
        SetDirectionIcon();
    }

    public void SetDirectionIcon()
    {
        iconDirection.Add(MapManager.GateDirection.Up, up);
        iconDirection.Add(MapManager.GateDirection.Down, down);
        iconDirection.Add(MapManager.GateDirection.Left, left);
        iconDirection.Add(MapManager.GateDirection.Right, right);
    }

    public void ActiveIcon(MapManager.RoomConnectInfo roominfo)
    {
        image.sprite = roominfo.room.icon;
        foreach (MapManager.GateDirection direction in roominfo.GateCheck())
        {
            iconDirection[direction].SetActive(true);
        }
    }


}
