using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;

    enum ProgressBarType
    {
        fallDown,
        fallRandom
    }

    [SerializeField] ProgressBarType progressBarType;

    [Header("ProgressBar settings")]
    [SerializeField] float threshHoldUp = 1f;
    [SerializeField] float threshHoldDown = 0.8f;
    [SerializeField] float minusValue = 0.01f;
    [SerializeField] float addValue = 0.3f;
    [SerializeField] RectTransform colorFill;
    [SerializeField] RectTransform treeChopProgressBar;
    private bool valueUp;

    private void Awake()
    {
        slider = this.GetComponent<Slider>();
        colorFill = this.transform.GetChild(1).Find("ColorFill").GetComponent<RectTransform>();
        treeChopProgressBar = this.GetComponent<RectTransform>();
    }
    private void Start()
    {

    }
    private void Update()
    {
        colorFill.offsetMax = new Vector2(0, treeChopProgressBar.rect.height * (threshHoldUp-1));
        colorFill.offsetMin = new Vector2(0, treeChopProgressBar.rect.height * threshHoldDown);
    }
    private void OnEnable()
    {
        //print("bat");
        if (progressBarType == ProgressBarType.fallDown)
        {


            InvokeRepeating("MinusValue", 0f, 0.01f);
        }
        if (progressBarType == ProgressBarType.fallRandom)
        {
            InvokeRepeating("RandomValue", 0f, 0.5f);
            InvokeRepeating("RandomProgress", 0f, 0.01f);
            
        }
    }
    private void OnDisable()
    {
        if (progressBarType == ProgressBarType.fallDown)
        {
            CancelInvoke();
        }
        if (progressBarType == ProgressBarType.fallRandom)
        {
            CancelInvoke();
        }
    }
    private void RandomProgress()
    {
        if (valueUp)
        {
            CancelInvoke("MinusValue");
            InvokeRepeating("AddValue", 0f, 1f);
        }
        else
        {
            CancelInvoke("AddValue");
            InvokeRepeating("MinusValue", 0f, 1f);
        }
    }
    private void RandomValue()
    {
        valueUp = Random.value > 0.5f; // 50/50 true hoặc false
    }
    private void MinusValue()
    {
        //print("tru");
        slider.value -= minusValue;
    }

    public void AddValue()
    {
        slider.value += addValue;
    }
    public void ResetValue()
    {
        if (slider == null)
        {
            return;
        }
        slider.value += 1;
    }
    public bool CheckIfValided()
    {
        return slider.value >= threshHoldDown && slider.value <= threshHoldUp;
    }
}
