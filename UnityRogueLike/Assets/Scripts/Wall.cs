using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite DamagedSprite;
    public int HitPoints = 4;

    private SpriteRenderer _spriteRenderer;

    public void DamageWall(int loss)
    {
        _spriteRenderer.sprite = DamagedSprite;
        HitPoints -= loss;

        if (HitPoints <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    void Awake ()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
