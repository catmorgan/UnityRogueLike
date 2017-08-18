using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {
    public int PlayerDamage;
    public AudioClip EnemyAttack1;
    public AudioClip EnemyAttack2;

    private Animator _animator;
    private Transform _target;
    private bool _skipMove;

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        //the tutorial says float.Epsilon, but my values were never close enough to 
        //get any y coordinate action
        if (Mathf.Abs(_target.position.x - transform.position.x) < 1)
        {
            yDir = _target.position.y > transform.position.y ? 1 : -1;
        } else
        {
            xDir = _target.position.x > transform.position.x ? 1 : -1;
        }

        //I feel like this is confusing because it makes me feel like I'm checking
        //to attempt to move the player, when really i'm checking if I hit the player
        AttemptMove<Player>(xDir, yDir);
    }
    
	// Use this for initialization
	protected override void Start () {
        GameManager.Instance.AddEnemyToList(this);
        _animator = GetComponent<Animator>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
	}

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        hitPlayer.LoseFood(PlayerDamage);
        _animator.SetTrigger("enemyAttack");
        SoundManager.Instance.RandomFx(EnemyAttack1, EnemyAttack2);
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (_skipMove)
        {
            _skipMove = false;
            return;
        }
        base.AttemptMove<T>(xDir, yDir);

        _skipMove = true;
    }
}
