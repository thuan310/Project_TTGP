using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FunctionSliderSetting : MonoBehaviour
{
    [Header("Name")]
    public string nameFunction;
    public TextMeshProUGUI nameButtonText;

    [Header("Option Function")]
    public TextMeshProUGUI optionText;
    public int maxValue;

    [Header("Slider")]
    public Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nameButtonText.text = nameFunction;

    }

    // Update is called once per frame
    void Update()
    {
        optionText.text = Mathf.RoundToInt(slider.value*maxValue).ToString();
    }
}
