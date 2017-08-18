using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int WallDamage = 1;
    public int PointsPerFood = 10;
    public int PointsPerSoda = 20;
    public float RestartLevelDelay = 1f;

    private Animator animator;
    private int _food;

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        _food -= loss;
        CheckIfGameOver();
    }

	protected override void Start ()
	{
	    animator = GetComponent<Animator>();
	    PointsPerFood = GameManager.Instance.PlayerFoodPoints;
        base.Start();
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        _food--;
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;
        CheckIfGameOver();
        GameManager.Instance.IsPlayersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        //but this is assuming that this function is going to be used with a wall...so you might as well
        //change the signature to be a wall ?
        Wall hitWall = component as Wall;
        hitWall.DamageWall(WallDamage);
        animator.SetTrigger("playerChop");
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlayersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int) Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            //may encounter a wall
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", RestartLevelDelay);
            enabled = false;
        } else if (other.tag == "Food")
            //I feel like checking a string is so fragile, should you not check what type of object you hit? such is if other is Food where Food is a model ?
        {
            _food += PointsPerFood;
            other.gameObject.SetActive(false);
        } else if (other.tag == "Soda")
        {
            _food += PointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerFoodPoints = _food;
    }

    private void CheckIfGameOver()
    {
        if (_food <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }
}
