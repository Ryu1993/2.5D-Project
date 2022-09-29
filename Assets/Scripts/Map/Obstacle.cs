using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    SpriteRenderer spriteRenderer;

    private void Awake() => spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];


}
