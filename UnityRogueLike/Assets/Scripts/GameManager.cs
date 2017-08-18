using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public int PlayerFoodPoints = 100;
    public float TurnDelay = 0.1f;

    [HideInInspector] public bool IsPlayersTurn = true;
    private BoardManager BoardScript;
    private int level = 3;
    private List<Enemy> Enemies;
    private bool _isEnemiesMoving;

    public void GameOver()
    {
        enabled = false;
    }

    public void AddEnemyToList(Enemy script)
    {
        Enemies.Add(script);
    }

	void Awake ()
	{
	    if (Instance == null)
	    {
	        Instance = this;
	    } else if (Instance != null) //why is this an else if and not just an else?
	    {
	        Destroy(gameObject);
	    }

        DontDestroyOnLoad(gameObject);
        Enemies = new List<Enemy>();
	    BoardScript = GetComponent<BoardManager>();
	    InitGame();
	}

    void InitGame()
    {
        Enemies.Clear();
        BoardScript.SetupScene(level);
    }

    void Update()
    {
        if (IsPlayersTurn || _isEnemiesMoving) return;

        StartCoroutine(MoveEnemies());
        IsPlayersTurn = true;
    }

    IEnumerator MoveEnemies()
    {
        _isEnemiesMoving = true;
        yield return new WaitForSeconds(TurnDelay);
        if (Enemies.Count == 0)
        {
            yield return new WaitForSeconds(TurnDelay);
        }

        for (var i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].MoveEnemy();
            yield return new WaitForSeconds(Enemies[i].MoveTime);
        }

        IsPlayersTurn = true;
        _isEnemiesMoving = false;
    }
}
