using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    public const int Columns = 8;
    public const int Rows = 8;
    public Count WallCount = new Count(5, 9);
    public Count FoodCount = new Count(1, 5);
    public GameObject Exit;
    //I feel like these don't need to be arrays, because we don't need a specific block of sequential memory to be floor tiles. A list 
    //would be beter
    public GameObject[] FloorTiles;
    public GameObject[] WallTiles;
    public GameObject[] FoodTiles;
    public GameObject[] EnemyTiles;
    public GameObject[] OuterWallTiles;

    private Transform _boardHolder;
    private List<Vector3> _gridPositions = new List<Vector3>();
    //I wanted to have a variable for the exit position because maybe I want to change it randomly in the future,
    //also this is more clear
    private readonly Vector3 _exitPosition = new Vector3(Columns - 1, Rows - 1, 0F);

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(WallTiles, WallCount.Minimum, WallCount.Maximum);
        LayoutObjectAtRandom(FoodTiles, FoodCount.Minimum, FoodCount.Maximum);

        int enemyCount = (int) Mathf.Log(level, 2f);
        LayoutObjectAtRandom(EnemyTiles, enemyCount, enemyCount);

        Instantiate(Exit, _exitPosition, Quaternion.identity);
    }

    [Serializable]
    public class Count
    {
        public int Minimum;
        public int Maximum;

        public Count(int min, int max)
        {
            Minimum = min;
            Maximum = max;
        }
    }

    private void InitializeList()
    {
        _gridPositions.Clear();

        for (var x = 1; x < Columns - 1; x++)
        {
            for (var y = 1; y < Rows - 1; y++)
            {
                _gridPositions.Add(new Vector3(x,y,0f));
            }
        }
    }

    private void BoardSetup()
    {
        _boardHolder = new GameObject("Board").transform;

        //rather than checking if the current tile is the wall, you could do four separate for-loops
        //to generate the tiles. This would reduce filling the board from O(n^2) to O(n)
        for (var x = -1; x < Columns + 1; x++)
        {
            for (var y = -1; y < Rows + 1; y++)
            {
                GameObject toInstantiate = FloorTiles[Random.Range(0, FloorTiles.Length)];
                if (x == -1 || x == Columns || y == -1 || y == Rows)
                {
                    //this is not performant... you are instantiating an object above that might be overwritten
                    toInstantiate = OuterWallTiles[Random.Range(0, OuterWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(_boardHolder);
            }
        }
    }

    private Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, _gridPositions.Count);
        Vector3 randomPosition = _gridPositions[randomIndex];
        _gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    private void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);

        for (var i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
}
