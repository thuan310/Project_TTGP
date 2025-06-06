using UnityEngine;
using UnityEngine.UI;

public class CheckBox : MonoBehaviour
{
    [SerializeField] Image[] images = new Image[3];
    [SerializeField] Color color= Color.red;
    int state = 0;


    public void TickColor()
    {
        if (state == 3)
        {
            return;
        }
        images[state].color = color;
        state++;
        if (state == 3)
        {
            OnFail();
        }
    }
    public void ResetColor()
    {
        foreach (Image image in images)
        {
            image.color = Color.white;
        }
        state = 0;
    }
    public void OnFail()
    {
        MinigameInputManager.instance.Quit();
    }
}
