using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePlayerSet : MonoBehaviour
{
    [SerializeField]
    Player player;

    private void Start()
    {
        player.PlayerSetting(true);
    }
}

