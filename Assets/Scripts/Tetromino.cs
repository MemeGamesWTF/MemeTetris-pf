using System;
using UnityEngine;
using UnityEngine.UI;

public class Tetromino : MonoBehaviour
{
    private float fallDelay = 1f;
    private const float moveDelay = 0.1f;
    private float fallTime, moveTime;

    private Board board;
    private Game game;
    private ScoreBoard scoreboard;
    private AudioManager audioManager;
    private Transform pivot;
    
    // Button references
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button softDropButton;
    [SerializeField] private Button rotateButton;
    [SerializeField] private Button hardDropButton;
    
    void Start()
    {
        Debug.Log(gameObject.name);
        gameObject.tag = "FallingTetromino";

        moveTime = Time.time + moveDelay;
        fallTime = Time.time + fallDelay;

        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        scoreboard = GameObject.FindGameObjectWithTag("ScoreBoard").GetComponent<ScoreBoard>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        // Get Tetromino's pivot
        pivot = transform.Find("Pivot");

        // Add button listeners
        if (moveLeftButton != null)
            moveLeftButton.onClick.AddListener(MoveLeft);
        
        if (moveRightButton != null)
            moveRightButton.onClick.AddListener(MoveRight);
        
        if (softDropButton != null)
            softDropButton.onClick.AddListener(SoftDrop);
        
        if (rotateButton != null)
            rotateButton.onClick.AddListener(Rotate);
        
        if (hardDropButton != null)
            hardDropButton.onClick.AddListener(HardDrop);

        Falling();
    }

    void Update()
    {
        // Original keyboard inputs remain for desktop/keyboard support
        if (Time.time > moveTime && game.isPaused == false)
        {
            HandleKeyboardInputs();
        }

        // Move Tetromino down every x seconds
        if (Time.time > fallTime && game.isPaused == false)
        {
            Falling();
        }
    }

    private void HandleKeyboardInputs()
    {
        Vector3 currentPos = transform.position;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }
        else if (Input.GetKey(KeyCode.DownArrow)) 
        {
            SoftDrop();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Rotate();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop(); 
        }
    }

    // Button-specific movement methods
    public void MoveLeft()
    {
        audioManager.PlaySound("move");
        transform.position += new Vector3(-1, 0, 0);
        moveTime = Time.time + moveDelay;

        // Validate movement
        if (!board.IsValidMovement(pivot))
        {
            transform.position -= new Vector3(-1, 0, 0);
        }
    }

    public void MoveRight()
    {
        audioManager.PlaySound("move");
        transform.position += new Vector3(1, 0, 0);
        moveTime = Time.time + moveDelay;

        // Validate movement
        if (!board.IsValidMovement(pivot))
        {
            transform.position -= new Vector3(1, 0, 0);
        }
    }

    public void SoftDrop()
    {
        audioManager.PlaySound("soft drop");
        Vector3 currentPos = transform.position;
        transform.position += new Vector3(0, -1, 0);
        scoreboard.UpdateScore(1);
        moveTime = Time.time + moveDelay / 2;

        if (!board.IsValidMovement(pivot))
        {
            scoreboard.UpdateScore(-1);
            transform.position = currentPos;
        }
    }

    public void Rotate()
    {
        audioManager.PlaySound("rotate");
        pivot.transform.Rotate(0, 0, -90);

        if (!board.IsValidMovement(pivot))
        {
            // Attempt wall kick by moving right
            transform.position += new Vector3(1, 0, 0);

            if (!board.IsValidMovement(pivot))
            {
                //Attempt wall kick by moving left
                transform.position += new Vector3(-2, 0, 0);

                if (!board.IsValidMovement(pivot))
                {
                    // Place tetromino back in its original place
                    transform.position += new Vector3(1, 0, 0);
                    pivot.transform.Rotate(0, 0, 90);
                }
            }       
        }
    }

    public void HardDrop()
    {
        while (board.IsValidMovement(pivot))
        {
            transform.position += new Vector3(0, -1, 0);
            scoreboard.UpdateScore(2);
        }

        transform.position += new Vector3(0, 1, 0);
        scoreboard.UpdateScore(-2);
        fallTime = Time.time + moveDelay;
    }

    private void Falling()
    {
        Vector3 currentPos = transform.position;

        transform.position += new Vector3(0, -1, 0);

        float level = scoreboard.level;
        fallDelay = (float)Math.Pow(0.8f - ((level - 1f) * 0.007f), level - 1f);
        fallTime = Time.time + fallDelay;

        if (!board.IsValidMovement(pivot))
        {
            transform.position = currentPos;
            Lock();
        }
    }

    private void Lock()
    {
        audioManager.PlaySound("locking");

        // Update grid and check for lines
        board.AddToGrid(pivot);
        board.CheckForLines();

        gameObject.tag = "Tetromino";

        // Destroy ghost tetromino 
        GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>().DestroyGhost();

        enabled = false;

        // Spawn the next piece
        board.RemoveNext();
        board.Spawn();
    }
}