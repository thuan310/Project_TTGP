using UnityEngine;
using UnityEngine.UI;

public class UI_StatBar : MonoBehaviour
{
    private Slider slider;
    // variable to scale bar size depending on stat (higher stat = longer bar across screen)
    // secondary bar behind may bar for polish effect (Yellow bar that shows how much an action/dance takes away from current stat)


    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public virtual void SetStat(int newValue)
    {
        slider.value = newValue;
    }

    public virtual void SetMaxStat( int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }
}
