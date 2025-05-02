using UnityEngine;
using UnityEngine.UI;

public class UI_StatBar : MonoBehaviour
{
    private Slider slider;
    private RectTransform rectTransform;
    // variable to scale bar size depending on stat (higher stat = longer bar across screen)
    [Header("bar options")]
    [SerializeField] protected bool scaleBarLengthWithStats = true;
    [SerializeField] protected float widthScaleMultiplier = 1;

    // secondary bar behind may bar for polish effect (Yellow bar that shows how much an action/dance takes away from current stat)


    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void SetStat(int newValue)
    {
        slider.value = newValue;
    }

    public virtual void SetMaxStat( int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;

        if(scaleBarLengthWithStats)
        {
            // scale the transfoorm of this object
            rectTransform.sizeDelta = new Vector2(maxValue*widthScaleMultiplier, rectTransform.sizeDelta.y);

            //resets the position of the bars based on thier layout group's settings
            PlayerUIManager.instance.playerUIHUDManager.RefreshHUD();
        }
    }
}
