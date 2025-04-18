using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweening : MonoBehaviour
{
    [SerializeField] GameObject playButton, htpButton, quitButton;
    [SerializeField] GameObject playAgainButton, options2Button, quit2Button;
    [SerializeField] GameObject resumeButton, options3Button, quit3Button;
    [SerializeField] GameObject score, level, lines;
    [SerializeField] GameObject title, subtitle, gameOver, pause;

    private void Start()
    {
        LeanTween.scale(title, new Vector3(0.8f, 0.8f, 0.8f), 2f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.moveLocal(title, new Vector3(0f, 62.5f, 0f), .7f).setDelay(2f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(subtitle, new Vector3(1f, 1f, 1f), 1f).setDelay(2f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(title, new Vector3(1f, 1f, 1f), 2f).setDelay(1.7f).setEase(LeanTweenType.easeInOutCubic);

        LeanTween.scale(playButton, new Vector3(.5f, .5f, 1f), 1f).setDelay(2.3f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(htpButton, new Vector3(.5f, .5f, 1f), 1f).setDelay(2.5f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(quitButton, new Vector3(.5f, .5f, 1f), 1f).setDelay(2.5f).setEase(LeanTweenType.easeInOutCubic);

        LeanTween.scale(score, new Vector3(1f, 1f, 1f), 1f).setDelay(2.5f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(level, new Vector3(1f, 1f, 1f), 1f).setDelay(2.5f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(lines, new Vector3(1f, 1f, 1f), 1f).setDelay(2.5f).setEase(LeanTweenType.easeInOutCubic);
    }

    internal void PulsatingTitle(GameObject title)
    {
        LeanTween.scale(title, new Vector3(.9f, .9f, 1f), .7f).setLoopPingPong();
    }

    public void OnPointerEnter(GameObject button) 
    {
        LeanTween.scale(button, new Vector3(.55f, .55f, 1f), .5f);
    }

    public void OnPointerExit(GameObject button)
    {
        LeanTween.scale(button, new Vector3(.5f, .5f, 1f), .5f);
    }
}
