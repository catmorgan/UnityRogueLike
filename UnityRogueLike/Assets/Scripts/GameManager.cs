using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public BoardManager BoardScript;
    private int level = 3;

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
	    BoardScript = GetComponent<BoardManager>();
	    InitGame();
	}

    void InitGame()
    {
        BoardScript.SetupScene(level);
    }
}
