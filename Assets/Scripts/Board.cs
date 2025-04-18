using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject[] tetrominoList;
    [SerializeField] Game game;
    [SerializeField] ScoreBoard scoreboard;
    [SerializeField] AudioManager audioManager;

    private static readonly int width = 10, height = 21;
    private static Transform[,] grid = new Transform[width, height];
    private GameObject currTetromino, nextTetromino, ghostTetromino;
    private List<int> bag, shuffledBag;
    private Vector3 spawnPos = new(4f, 19f, 0f), nextPos = new(13.5f, 16.5f, 0f);

    internal void BeginGame()
    {
        // Shuffle bag to generate a random 7 piece sequence
        bag = Enumerable.Range(0, tetrominoList.Length).ToList();
        ShuffleTetrominos(bag);

        Spawn();
    }
    
    internal void Spawn()
    {

        if (shuffledBag.Count <= 0)
        {
            ShuffleTetrominos(bag);
        }

        // Pull Tetromino from shuffled bag   
        currTetromino = Instantiate(tetrominoList[shuffledBag[0]], spawnPos, Quaternion.identity);
        currTetromino.AddComponent<Tetromino>();

        CreateGhost();
        shuffledBag.RemoveAt(0);

        if (shuffledBag.Count <= 0)
        {
            ShuffleTetrominos(bag);
        }

        nextTetromino = Instantiate(tetrominoList[shuffledBag[0]]);
        nextTetromino.transform.Find("Pivot").position = nextPos;

        // Check if Tetromino spawned overlapping other pieces 
        // or spawned outside the play zone
        if (!IsValidMovement(currTetromino.transform.Find("Pivot")))
        {
            ToppedOut();
        }
    }

    private void ShuffleTetrominos(List<int> values)
    {
        Random rand = new Random();
        shuffledBag = values.OrderBy(_ => rand.Next()).ToList();
    }

    private void CreateGhost()
    {
        ghostTetromino = Instantiate(tetrominoList[shuffledBag[0]], spawnPos, Quaternion.identity);
        ghostTetromino.AddComponent<Ghost>();
    }

    private void ToppedOut()
    {
        game.isPaused = true;

        // Clean board
        CleanBoard();

        // Open game over menu
        game.GameOver();

    }

    internal void RemoveNext()
    {
        Destroy(nextTetromino);
    }

    internal void AddToGrid(Transform pivot)
    {
        foreach (Transform children in pivot.transform)
        {
            int x = Mathf.RoundToInt(children.transform.position.x);
            int y = Mathf.RoundToInt(children.transform.position.y);

            grid[x, y] = children;
        }
    }

    internal void CheckForLines()
    {
        bool _foundLines = false;

        for (int y = 0; y < height; y++)
        {
            if (HasLine(y))
            {
                _foundLines = true;

                // Delete line and move the rows above down one line
                DeleteLine(y);
                RowDown(y); 
            }
        }

        // If lines were found, check again
        if (_foundLines) CheckForLines();
    }

    private bool HasLine(int y)
    {
        // Check if all x pos of a y line have pieces the grid
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    private void DeleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }

        scoreboard.UpdateLines();
        audioManager.PlaySound("clean line");
    }

    private void RowDown(int line)
    {
        for (int y = line; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;
                    grid[x, y - 1].transform.position += new Vector3(0, -1, 0);
                }
            }
        }
    }

    internal bool IsValidMovement(Transform pivot)
    {
        foreach (Transform children in pivot.transform)
        {
            int x = Mathf.RoundToInt(children.transform.position.x);
            int y = Mathf.RoundToInt(children.transform.position.y);

            // If a piece is outside of grid, dont allow movement
            if (x < 0 || x >= width || y < 0)
            {
                return false;
            }

            // If there's another piece it this spot, dont allow movement
            if (grid[x, y] != null)
            {
                return false;
            }
        }

        return true;
    }

    internal void CleanBoard()
    {
        // Clean grid
        grid = new Transform[width, height];

        // Destroy all existing tetrominos
        GameObject[] tetrominosToDelete = GameObject.FindGameObjectsWithTag("Tetromino");
        foreach (GameObject tetromino in tetrominosToDelete)
        {
            Destroy(tetromino);
        }

        Destroy(currTetromino);
        Destroy(nextTetromino);
        Destroy(ghostTetromino);
    }
}
