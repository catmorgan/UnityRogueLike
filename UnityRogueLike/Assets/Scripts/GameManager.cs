using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public int PlayerFoodPoints = 100;
    public float TurnDelay = 0.1f;
    public float LevelStartDelay = 2f;

    [HideInInspector] public bool IsPlayersTurn = true;
    private Text _levelText;
    private GameObject _levelImage;
    private BoardManager BoardScript;
    private int level = 1;
    private List<Enemy> Enemies;
    private bool _isEnemiesMoving;
    private bool _doingSetup;

    public void GameOver()
    {
        _levelText.text = "After " + level + " days, you starved.";
        _levelImage.SetActive(true);
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
        _doingSetup = true;
        _levelImage = GameObject.Find("LevelImage");
        _levelText = GameObject.Find("LevelText").GetComponent<Text>();
        _levelText.text = "Day " + level;
        _levelImage.SetActive(true);
        Invoke("HideLevelImage", LevelStartDelay);

        Enemies.Clear();
        BoardScript.SetupScene(level);
    }

    void Update()
    {
        if (IsPlayersTurn || _isEnemiesMoving || _doingSetup) return;

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

    private void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();
    }

    private void HideLevelImage()
    {
        _levelImage.SetActive(false);
        _doingSetup = false;
    }
}
