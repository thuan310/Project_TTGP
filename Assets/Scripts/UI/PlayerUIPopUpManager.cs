using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerUIPopUpManager : MonoBehaviour
{
    [Header("You died Pop up")]
    [SerializeField] GameObject youDiedPopUpGameObject;
    [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI youDiedPopUpText;
    [SerializeField] CanvasGroup youDiedPopUpCanvasGroup; // allows us to set the alpha to fade over time

    [Header("Boss Defeated Pop up")]
    [SerializeField] GameObject bossDefeatedPopUpGameObject;
    [SerializeField] TextMeshProUGUI bossDefeatedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI bossDefeatedPopUpText;
    [SerializeField] CanvasGroup bossDefeatedPopUpCanvasGroup; // allows us to set the alpha to fade over time

    [SerializeField] bool trigger;

    private void Update()
    {
        SendYouDiedPopUp();
    }

    public void SendYouDiedPopUp()
    {
        if (!trigger)
        { 
            return;
        }

        trigger = false;
        // active post processing effects

        youDiedPopUpGameObject.SetActive(true);
        youDiedPopUpBackgroundText.characterSpacing = 0;

        //stretch out the pop up
        StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8, 19f));
        // fade in the pop up
        StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5));
        // wait, then fade out the pop up
        StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup,2, 5));
    }

    public void SendBossDefeatedPopUp(string bossDefeatedMessage)
    {
        // active post processing effects
        bossDefeatedPopUpText.text = bossDefeatedMessage;
        bossDefeatedPopUpBackgroundText.text = bossDefeatedMessage;
        bossDefeatedPopUpGameObject.SetActive(true);
        bossDefeatedPopUpBackgroundText.characterSpacing = 0;

        //stretch out the pop up
        StartCoroutine(StretchPopUpTextOverTime(bossDefeatedPopUpBackgroundText, 8, 19f));
        // fade in the pop up
        StartCoroutine(FadeInPopUpOverTime(bossDefeatedPopUpCanvasGroup, 5));
        // wait, then fade out the pop up
        StartCoroutine(WaitThenFadeOutPopUpOverTime(bossDefeatedPopUpCanvasGroup, 2, 5));
    }

    private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
    {
        if(duration > 0f)
        {
            text.characterSpacing = 0; // reset our character spacing
            float timer = 0;
            yield return null;
            while (timer < duration)
            {
                timer = timer + Time.deltaTime;
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                yield return null;
            }
        }
    }

    private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
    {
        if(duration > 0f)
        {
            canvas.alpha = 0;

            float timer = 0;
            yield return null;
            while (timer<duration)
            {
                timer = timer + Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha,1,duration * Time.deltaTime);
                yield return null;
            }

        }
        canvas.alpha = 1;

        yield return null;
    }

    private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
    {
        if (duration > 0f)
        {
            while(delay > 0f)
            {
                delay=  delay -Time.deltaTime;
                yield return null;
            }
            canvas.alpha = 1;

            float timer = 0;
            yield return null;
            while (timer < duration)
            {
                timer = timer + Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                yield return null;
            }

        }
        canvas.alpha = 0;

        yield return null;
    }

    private IEnumerator DisplayGameOver(CanvasGroup canvas)
    {
        yield return null;
    }
}
