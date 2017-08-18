using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    public int WallDamage = 1;
    public int PointsPerFood = 10;
    public int PointsPerSoda = 20;
    public float RestartLevelDelay = 1f;
    public Text FoodText;
    public AudioClip MoveSound1;
    public AudioClip MoveSound2;
    public AudioClip DrinkSound1;
    public AudioClip DrinkSound2;
    public AudioClip EatSound1;
    public AudioClip EatSound2;
    public AudioClip GameOver;

    private Animator animator;
    private int _food;

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        _food -= loss;
        FoodText.text = "-" + loss + " Food: " + _food;
        CheckIfGameOver();
    }

	protected override void Start ()
	{
	    animator = GetComponent<Animator>();
        _food = GameManager.Instance.PlayerFoodPoints;
        FoodText.text = "Food: " + _food;
        base.Start();
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        _food--;
        FoodText.text = "Food: " + _food;
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;
        if (Move (xDir, yDir, out hit))
        {
            SoundManager.Instance.RandomFx(MoveSound1, MoveSound2);
        }
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

    private void Update()
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
            FoodText.text = "+" + PointsPerFood + " Food: " + _food;
            SoundManager.Instance.RandomFx(EatSound1, EatSound2);
            other.gameObject.SetActive(false);
        } else if (other.tag == "Soda")
        {
            _food += PointsPerSoda;
            FoodText.text = "+" + PointsPerSoda + " Food: " + _food;
            SoundManager.Instance.RandomFx(DrinkSound1, DrinkSound2);
            other.gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerFoodPoints = _food;
    }

    private void CheckIfGameOver()
    {
        if (_food <= 0)
        {
            SoundManager.Instance.PlaySingle(GameOver);
            SoundManager.Instance.MusicSource.Stop();
            GameManager.Instance.GameOver();
        }
    }
}
