using TMPro;
using UnityEngine;

public class FunctionSetting : MonoBehaviour
{
    [Header("Name")]
    public string nameFunction;
    public TextMeshProUGUI nameButtonText;

    [Header("Option Function")]
    public string[] option;
    int optionIndex;
    public TextMeshProUGUI optionButtonText;

    [Header("Icons")]
    public Sprite moveRight;
    public Sprite moveLeft;

    
    public void OptionLeft()
    {
        if(optionIndex == 0)
        {
            optionIndex = option.Length;
        }
        optionIndex -= 1;
        optionButtonText.text = option[optionIndex];
    }

    public void OptionRight()
    {
        if (optionIndex == option.Length -1)
        {
            optionIndex = -1;
        }
        optionIndex += 1;
        optionButtonText.text = option[optionIndex];
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nameButtonText.text = nameFunction;

        optionIndex = 0;
        optionButtonText.text = option[optionIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
