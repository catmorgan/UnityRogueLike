using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite DamagedSprite;
    public int HitPoints = 4;
    public AudioClip Chop1;
    public AudioClip Chop2;

    private SpriteRenderer _spriteRenderer;

    public void DamageWall(int loss)
    {
        SoundManager.Instance.RandomFx(Chop1, Chop2);
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
