using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Tutorial", menuName = "Tutorial")]
public class Tutorial : ScriptableObject
{
    [Header("Tutorial info")]
    public  string tutorialName;
    [TextArea]
    public List<string> tutorialStrings;
    public List<Sprite> tutorialImages;

}
