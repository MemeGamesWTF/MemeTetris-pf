using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Board board;
    public Tetromino tetromino;
    private Transform pivot;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Ghost";

        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        
        GameObject tmp = GameObject.FindGameObjectWithTag("FallingTetromino");
        if (tmp != null)
        {
            tetromino = tmp.GetComponent<Tetromino>();
        }
        
        pivot = transform.Find("Pivot");

        ChangeColor();
    }

    void LateUpdate()
    {

        if (tetromino == null)
        {
            Destroy(gameObject);
            return;
        }

        Reflect();
        Drop();
    }

    private void Reflect()
    {
        // Move Ghost to same position as Tetromino
        transform.position = tetromino.transform.position;

        // Rotate Ghost to same angle as Tetromino
        float tetrominoRotation = tetromino.transform.Find("Pivot").transform.eulerAngles.z;

        if (tetrominoRotation >= 180) { 
            tetrominoRotation -= 360f; 
        }

        pivot.transform.rotation = Quaternion.Euler(0f, 0f, tetrominoRotation);
    }

    private void Drop()
    {
        while (board.IsValidMovement(pivot))
        {
            transform.position += new Vector3(0, -1, 0);
        }

        transform.position += new Vector3(0, 1, 0);
    }

    private void ChangeColor()
    {
        foreach (Transform child in pivot.transform)
        {
            Color childColor = child.GetComponent<SpriteRenderer>().color;
            childColor.a = 0.15f;
            child.GetComponent<SpriteRenderer>().color = childColor;
        }
    }

    internal void DestroyGhost()
    {
        enabled = false;
        Destroy(gameObject);
    }
}
