using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public int score = 0;
    internal int level = 1;
    private int lines = 0;

    public TextMeshProUGUI scoreGUI;
    public TextMeshProUGUI levelGUI;
    public TextMeshProUGUI linesGUI;

    // Update score
    internal void UpdateScore(int value)
    {
        score += value;
        scoreGUI.text = score.ToString();
    }

    // Update level
    private void UpdateLevel()
    {
        level++;
        levelGUI.text = level.ToString();
    }

    // Update lines counter
    internal void UpdateLines()
    {
        lines++;
        linesGUI.text = lines.ToString();
        
        UpdateScore(200);

        if (lines == 1)
        {
            UpdateLevel();
        }

        if ((lines % 10) == 0)
        {
            UpdateLevel();
        }
    }

    // Clean all values
    internal void RestartValues()
    {
        score = 0;
        level = 1;
        lines = 0;

        scoreGUI.text = score.ToString();
        levelGUI.text = level.ToString();
        linesGUI.text = lines.ToString();
    }
}
